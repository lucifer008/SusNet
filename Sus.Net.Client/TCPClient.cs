using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using Sus.Net.Client.Sockets;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Utils;
using Sus.Net.Common.Message;
using Sus.Net.Common.Event;
using Newtonsoft.Json;
using log4net;

namespace Sus.Net.Client
{
    public class TCPClient
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(TCPClient));

        private readonly AsyncTcpClient client;

        private readonly ClientUserInfo clientUserInfo;
        /// <summary>
        /// 服务器地址
        /// </summary>
        private readonly string serverIP;
        /// <summary>
        /// 服务器端口
        /// </summary>
        private readonly int serverPort;

        /// <summary>
        /// 心跳定时器
        /// </summary>
        private System.Timers.Timer heartbeatTimer = new System.Timers.Timer();

        /// <summary>
        /// 与服务器的连接已建立事件
        /// </summary>
        public event EventHandler<TcpServerConnectedEventArgs> ServerConnected;
        /// <summary>
        /// 与服务器的连接已断开事件
        /// </summary>
        public event EventHandler<TcpServerDisconnectedEventArgs> ServerDisconnected;
        /// <summary>
        /// 与服务器的连接发生异常事件
        /// </summary>
        public event EventHandler<TcpServerExceptionOccurredEventArgs> ServerExceptionOccurred;

        /// <summary>
        /// 接收到消息
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        private TCPClient() { }
        public TCPClient(ClientUserInfo info,string serverIP,int serverPort)
        {
            this.clientUserInfo = info;
            this.serverIP = serverIP;
            this.serverPort = serverPort;

            MessageFactory.Instance.Init(clientUserInfo);

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(serverIP),serverPort);
            client = new AsyncTcpClient(remoteEP) { Encoding = Encoding.UTF8 };
            client.ServerExceptionOccurred += new EventHandler<TcpServerExceptionOccurredEventArgs>(client_ServerExceptionOccurred);
            client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
            client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
            //client.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(client_PlaintextReceived);
            client.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(client_DatagramReceived);
            //无限制重连
            client.Retries = AsyncTcpClient.UnlimitedRetry;
            client.RetryInterval = 3;
        }

        public void Connect()
        {
            if ( !client.Connected )
            {
                client.Connect();
                log.InfoFormat("开始连接服务器:({0}:{1})",serverIP,serverPort);
            }
            else
            {
            }
        }

        public void Disconnect()
        {
            client.Close(true);
        }

        private void SendMessage(Message message)
        {
            try
            {
                if ( client.Connected )
                {
                    client.Send(message.Encode());
                    log.InfoFormat("已经发送消息:{0}",message.Describe());
                }
                else
                {
                    log.ErrorFormat("未连接，无法发送消息:{0}",message);
                }
            }
            catch ( InvalidProgramException ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }


        private void Login()
        {
            SendMessage(MessageFactory.Instance.CreateMessage(string.Empty,MessageType.Login));
        }

        private void Ack(MessageBody recvMessage)
        {
            SendMessage(MessageFactory.Instance.CreateMessage(string.Empty,MessageType.ACK,recvMessage.id));
        }

        private void Heartbeat(object o,EventArgs e)
        {
            SendMessage(MessageFactory.Instance.CreateMessage(string.Empty,MessageType.Heartbeat));
        }

        private void startHeartbeatTimer()
        {
            if ( heartbeatTimer.Enabled )
            {
                return;
            }
            heartbeatTimer = new System.Timers.Timer
            {
                Interval = 120 * 1000,
                AutoReset = true
            };
            heartbeatTimer.Elapsed += Heartbeat;
            heartbeatTimer.Start();
        }

        private void stopHeartbeatTimer()
        {
            heartbeatTimer.Stop();
        }

        private void client_ServerConnected(object sender,TcpServerConnectedEventArgs e)
        {
            try
            {
                log.InfoFormat("已连接->服务器:{0}",e.ToString());
                Login();
                ServerConnected?.Invoke(this,e);
                startHeartbeatTimer();
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }

        private void client_ServerDisconnected(object sender,TcpServerDisconnectedEventArgs e)
        {
            try
            {
                log.WarnFormat("已经断开->服务器 {0} .",e.ToString());
                ServerDisconnected?.Invoke(this,e);
                stopHeartbeatTimer();
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }

        private void client_ServerExceptionOccurred(object sender,TcpServerExceptionOccurredEventArgs e)
        {
            try
            {
                log.ErrorFormat("服务器:{0} 发生异常:{1}.",e.ToString(),e.Exception.Message);
                ServerExceptionOccurred?.Invoke(this,e);
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }

        private void client_DatagramReceived(object sender,TcpDatagramReceivedEventArgs<byte[]> e)
        {
            try
            {
                log.InfoFormat("【客户端:{0}】收到消息:【服务器: {1}】-->【消息:{2}】",e.TcpClient.Client.LocalEndPoint.ToString(),e.TcpClient.Client.RemoteEndPoint.ToString(),BufferUtils.ByteToHexStr(e.Datagram));
                List<Message> messageList = MessageProcesser.Instance.ProcessRecvData(e.TcpClient.Client.RemoteEndPoint.ToString(),e.Datagram);
                foreach ( var rMessage in messageList )
                {
                    if ( rMessage.body != null )
                    {
                        //Log.Info("【客户端:{0}】收到消息完整解析消息:【服务器: {1}】-->【消息:{2}】",e.TcpClient.Client.LocalEndPoint.ToString(),e.TcpClient.Client.RemoteEndPoint.ToString(),rMessage.Describe());
                        log.InfoFormat("【客户端:{0}】收到消息完整解析消息:【服务器: {1}】-->【消息:{2}】", e.TcpClient.Client.LocalEndPoint.ToString(), e.TcpClient.Client.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(rMessage.body));
                        MessageBody messageBody = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(rMessage.body));
                        if ( messageBody != null )
                        {
                            log.InfoFormat("【客户端:{0}】收到消息业务消息:【客户端: {1}】-->【消息:{2}】",e.TcpClient.Client.LocalEndPoint.ToString(),e.TcpClient.Client.RemoteEndPoint.ToString(),messageBody.Describe());
                            switch ( rMessage.type )
                            {
                                case MessageType.ACK:
                                    break;
                                case MessageType.Heartbeat:
                                    Ack(messageBody);
                                    break;
                                case MessageType.Common:
                                    if ( MessageReceived != null )
                                    {
                                        MessageEventArgs args = new MessageEventArgs(messageBody.DATA);
                                        try
                                        {
                                            MessageReceived(this,args);
                                        }
                                        catch ( Exception ex )
                                        {
                                            log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                                        }
                                    }
                                    Ack(messageBody);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }
    }
}
