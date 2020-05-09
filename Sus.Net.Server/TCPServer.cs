using log4net;
using Newtonsoft.Json;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Message;
using Sus.Net.Common.Utils;
using Sus.Net.Server.Sockets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Sus.Net.Server
{
    public class TCPServer
    {
        private readonly AsyncTcpServer server;
        private readonly static ILog log = LogManager.GetLogger(typeof(TCPServer));
        public TCPServer(int port)
        {
            server = new AsyncTcpServer(port) { Encoding = Encoding.UTF8 };
            server.ClientConnected += new EventHandler<TcpClientConnectedEventArgs>(server_ClientConnected);
            server.ClientDisconnected += new EventHandler<TcpClientDisconnectedEventArgs>(server_ClientDisconnected);
            //server.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(server_PlaintextReceived);
            server.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(server_DatagramReceived);
            MessageFactory.Instance.Init(new ClientUserInfo("0","0"));
        }

        //启动
        public void Start()
        {
            server.Start();
            log.InfoFormat("服务器 {0} 已经启动",server.ToString());
        }

        public void Stop()
        {
            server.Stop();
            ClientManager.Instance.Clear();
            log.InfoFormat("服务器 {0} 已经停止",server.ToString());
        }

        //public void SendMessageToAll(string strMessage)
        //{
        //    try
        //    {
        //        Message message = MessageFactory.Instance.CreateMessage(strMessage);
        //        log.InfoFormat("已经给所有终端发送消息:{0}",message.Describe());
        //        server.SendToAll(message.Encode());
        //    }
        //    catch ( Exception ex )
        //    {
        //        log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
        //    }
        //}

        //public void SendMessageWithCorps(string strMessage,List<string> toCorpIds)
        //{
        //    try
        //    {
        //        if ( null != toCorpIds )
        //        {

        //            SendMessageWithClients(strMessage,ClientManager.Instance.ClientInfoWithCorpIds(toCorpIds));
        //        }
        //        else
        //        {
        //            log.WarnFormat("客户端列表为空，取消发送！！！！！");
        //        }
        //    }
        //    catch ( Exception ex )
        //    {
        //        log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
        //    }
        //}

        public void SendMessageWithClients(string strMessage,List<ClientUserInfo> clientInfos)
        {
            try
            {
                if ( null != clientInfos )
                {
                    log.InfoFormat("将要发送消息:{0}",strMessage);
                    log.InfoFormat("到客户端:");
                    log.InfoFormat("[");
                    foreach ( var info in clientInfos )
                    {
                        log.InfoFormat("{0}",info.ToString());
                    }
                    log.InfoFormat("]");
                    SendMessage(MessageFactory.Instance.CreateMessage(strMessage),ClientManager.Instance.ClientKeyWithUserInfos(clientInfos));
                }
                else
                {
                    log.Warn("客户端列表为空，取消发送！！！！！");
                }
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }

        private void SendMessage(Message message,List<string> tcpClientEndPointKeys)
        {
            try
            {
                if ( null != tcpClientEndPointKeys )
                {
                    log.InfoFormat("已经发送消息:{0}",message.Describe());
                    log.InfoFormat("到客户端:");
                    log.InfoFormat("[");
                    foreach ( string clientKey in tcpClientEndPointKeys )
                    {
                        try
                        {
                            log.InfoFormat("{0}",ClientManager.Instance.ClientUserInfoDesc(clientKey));
                            server.Send(clientKey,message.Encode());
                        }
                        catch ( Exception ex )
                        {
                            log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                        }
                    }
                    log.InfoFormat("]");
                }
                else
                {
                    log.WarnFormat("客户端列表为空，取消发送！！！！！");
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

        private void Ack(MessageBody recvMessage,TcpClient tcpClient)
        {
            Message message = MessageFactory.Instance.CreateMessage(string.Empty, MessageType.ACK, recvMessage.id);
            SendMessage(message,new List<string> { ClientKey(tcpClient) });
        }

        private string ClientKey(TcpClient tcpClient)
        {
            if ( tcpClient != null )
            {
                return tcpClient.Client.RemoteEndPoint.ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_ClientConnected(object sender,TcpClientConnectedEventArgs e)
        {
            log.InfoFormat("客户端:{0} 已连接",ClientKey(e.TcpClient));
        }

        private void server_ClientDisconnected(object sender,TcpClientDisconnectedEventArgs e)
        {
            try
            {
                log.InfoFormat("客户端:{0} 已断开",ClientKey(e.TcpClient));
                ClientManager.Instance.RemoveTcpClient(e.TcpClient);
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }

        private void server_DatagramReceived(object sender,TcpDatagramReceivedEventArgs<byte[]> e)
        {
            try
            {
                TcpClient tcpClient = e.TcpClient;
                log.InfoFormat("收到消息:【客户端: {0}】-->【消息:{1}】",ClientKey(tcpClient),BufferUtils.ByteToHexStr(e.Datagram));
                List<Message> messageList = MessageProcesser.Instance.ProcessRecvData(ClientKey(tcpClient),e.Datagram);
                foreach ( var rMessage in messageList )
                {
                    log.InfoFormat("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】",ClientKey(tcpClient),rMessage.Describe());
                    if ( rMessage.body != null )
                    {
                        log.InfoFormat("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】", ClientKey(tcpClient), Encoding.UTF8.GetString(rMessage.body));
                        ClientManager.Instance.AddOrUpdateTcpClient("1111","1111", tcpClient);
                        //MessageBody messageBody = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(rMessage.body));
                        //if ( messageBody != null )
                        //{
                        //    ClientManager.Instance.AddOrUpdateTcpClient(messageBody.gid,messageBody.uid,tcpClient);
                        //    Log.Info("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】",ClientKey(tcpClient),messageBody.Describe());
                        //    switch ( rMessage.type )
                        //    {
                        //        case MessageType.ACK:
                        //            break;
                        //        case MessageType.Heartbeat:
                        //        case MessageType.Common:
                        //            Ack(messageBody,tcpClient);
                        //            break;
                        //        default:
                        //            break;
                        //    }
                        //}
                    }
                }
            }
            catch ( Exception ex )
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
            }
        }

        public void TestDataProcess(string strMessage,List<string> toCorpIds)
        {
            try
            {
                List<ClientUserInfo> clientInfos = ClientManager.Instance.ClientInfoWithCorpIds(toCorpIds);
                log.InfoFormat("将要发送消息:{0}",strMessage);
                log.InfoFormat("到客户端:");
                log.InfoFormat("[");
                foreach ( var info in clientInfos )
                {
                    log.InfoFormat("{0}",info.ToString());
                }
                log.Info("]");

                Message message = MessageFactory.Instance.CreateMessage(strMessage);
                List<string> tcpClientEndPointKeys = ClientManager.Instance.ClientKeyWithUserInfos(clientInfos);
                byte[] messageData = message.Encode();
                log.InfoFormat("已经发送消息:{0}",message.Describe());
                log.InfoFormat("到客户端:");
                log.InfoFormat("[");
                foreach ( string clientKey in tcpClientEndPointKeys )
                {
                    try
                    {
                        server.Send(clientKey,messageData);
                        log.InfoFormat("{0}",ClientManager.Instance.ClientUserInfoDesc(clientKey));
                    }
                    catch ( Exception ex )
                    {
                        log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                    }
                }
                log.InfoFormat("]");

                List<Message> messageList = MessageProcesser.Instance.ProcessRecvData("0",messageData);
                foreach ( var rMessage in messageList )
                {
                    log.InfoFormat("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】","0",rMessage.Describe());
                    if ( rMessage.body != null )
                    {
                        MessageBody messageBody = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(rMessage.body));
                        if ( messageBody != null )
                        {
                            log.InfoFormat("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】","0",messageBody.Describe());
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
