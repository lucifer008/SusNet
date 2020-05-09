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
using SusNet2.Common.BusModel;
using SusNet2.Common.Utils;
using SusNet2.Common.SusBusMessage;

namespace Sus.Net.Client
{
    public class SusTCPClient2
    {
        private ILog log = LogManager.GetLogger(typeof(SusTCPClient2));
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

        /// <summary>
        /// 衣架进站：终端向pc的通知消息时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> HangerArrivalStatingMessageReceived;

        /// <summary>
        /// 启动主轨响应【终端向pc通知触发】
        /// </summary>
        public event EventHandler<MessageEventArgs> StartMainTrackResponseMessageReceived;

        /// <summary>
        /// 停止主轨响应【终端向pc通知时触发】
        /// </summary>
        public event EventHandler<MessageEventArgs> StopMainTrackResponseMessageReceived;

        /// <summary>
        /// 急停主轨响应【终端向pc通知时触发】
        /// </summary>
        public event EventHandler<MessageEventArgs> EmergencyStopMainTrackResponseMessageReceived;

        /// <summary>
        /// 硬件按【暂停键】终端向pc发送请求时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> StopReceiveHangerRequestMessageReceived;

        /// <summary>
        /// 衣架落入读卡器发送的请求，硬件发pc端时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> HangerDropCardRequestMessageReceived;

        /// <summary>
        /// 衣架出战，硬件发pc端时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> HangerOutStatingRequestMessageReceived;

        /// <summary>
        /// 软件给硬件分配工序到衣架成功时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> AllocationHangerResponseMessageReceived;
        /// <summary>
        /// 制品界面上线来自硬件的响应
        /// </summary>
        public event EventHandler<MessageEventArgs> ClientMachineResponseMessageReceived;

        /// <summary>
        /// 挂片站上线时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> HangingPieceStatingOnlineMessageReceived;

        private SusTCPClient2() { }
        public SusTCPClient2(ClientUserInfo info, string serverIP, int serverPort)
        {
            this.clientUserInfo = info;
            this.serverIP = serverIP;
            this.serverPort = serverPort;

            MessageFactory.Instance.Init(clientUserInfo);

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
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
            if (!client.Connected)
            {
                client.Connect();
                // Log.Info("开始连接服务器:({0}:{1})",serverIP,serverPort);
                log.Info(string.Format("开始连接服务器:({0}:{1})", serverIP, serverPort));
            }
            else
            {
            }
        }

        public void Disconnect()
        {
            client.Close(true);
        }
        public void SendData(byte[] data) {
            client.Send(data);
            log.Info(string.Format("已经发送消息:{0}", HexHelper.byteToHexStr(data)));
        }
        public void SendMessage(SusNet2.Common.Message.MessageBody message)
        {
            try
            {
                if (client.Connected)
                {
                    client.Send(message.GetBytes());
                    //Log.Info("已经发送消息:{0}",message.Describe());
                    log.Info(string.Format("已经发送消息:{0}", message.GetHexStr()));
                }

                else
                {
                    // Log.Error("未连接，无法发送消息:{0}",message);
                    log.Error(string.Format("未连接，无法发送消息:{0}", message.GetHexStr()));
                }
            }
            catch (InvalidProgramException ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }
        private void SendMessage(Message message)
        {
            try
            {
                if (client.Connected)
                {
                    client.Send(message.Encode());
                    //Log.Info("已经发送消息:{0}",message.Describe());
                    log.Info(string.Format("已经发送消息:{0}", message.Describe()));
                }

                else
                {
                    // Log.Error("未连接，无法发送消息:{0}",message);
                    log.Error(string.Format("未连接，无法发送消息:{0}", message));
                }
            }
            catch (InvalidProgramException ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }


        private void Login()
        {
            SendMessage(MessageFactory.Instance.CreateMessage(string.Empty, Common.Message.MessageType.Login));
        }

        private void Ack(MessageBody recvMessage)
        {
            SendMessage(MessageFactory.Instance.CreateMessage(string.Empty, Common.Message.MessageType.ACK, recvMessage.id));
        }

        private void Heartbeat(object o, EventArgs e)
        {
            SendMessage(MessageFactory.Instance.CreateMessage(string.Empty, Common.Message.MessageType.Heartbeat));
        }

        private void startHeartbeatTimer()
        {
            if (heartbeatTimer.Enabled)
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

        private void client_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            try
            {
                log.InfoFormat("已连接->服务器:{0}", e.ToString());
                //  Login();
                ServerConnected?.Invoke(this, e);
               // startHeartbeatTimer();
            }
            catch (Exception ex)
            {
                // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        private void client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            try
            {
                //Log.Warn("已经断开->服务器 {0} .",e.ToString());
                log.Warn(string.Format("已经断开->服务器 {0} .", e.ToString()));
                ServerDisconnected?.Invoke(this, e);
                stopHeartbeatTimer();
            }
            catch (Exception ex)
            {
                //  Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        private void client_ServerExceptionOccurred(object sender, TcpServerExceptionOccurredEventArgs e)
        {
            try
            {
                //Log.Error("服务器:{0} 发生异常:{1}.",e.ToString(),e.Exception.Message);
                log.Error(string.Format("服务器:{0} 发生异常:{1}.", e.ToString(), e.Exception.Message));
                ServerExceptionOccurred?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        private void client_DatagramReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            try
            {
                //Log.Info("【客户端:{0}】收到消息:【服务器: {1}】-->【消息:{2}】",e.TcpClient.Client.LocalEndPoint.ToString(),e.TcpClient.Client.RemoteEndPoint.ToString(),BufferUtils.ByteToHexStr(e.Datagram));
                log.Info(string.Format("【客户端:{0}】收到消息:【服务器: {1}】-->【消息:{2}】", e.TcpClient.Client.LocalEndPoint.ToString(), e.TcpClient.Client.RemoteEndPoint.ToString(), BufferUtils.ByteToHexStr(e.Datagram)));
                List<SusNet2.Common.Message.MessageBody> messageList = SusNet2.Common.SusBusMessage.MessageProcesser.Instance.ProcessRecvData(e.TcpClient.Client.RemoteEndPoint.ToString(), e.Datagram);
                foreach (var rMessage in messageList)
                {
                    log.Info(string.Format("【客户端:{0}】收到消息业务消息:【客户端: {1}】-->【消息:{2}】", e.TcpClient.Client.LocalEndPoint.ToString(), e.TcpClient.Client.RemoteEndPoint.ToString(), rMessage.GetHexStr()));

                    if (MessageReceived != null)
                    {
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        try
                        {
                            MessageReceived(this, args);
                        }
                        catch (Exception ex)
                        {
                            //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
                            log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                        }
                    }

                    //【启动主轨后响应】
                    var startmtMessage = SusNet2.Common.SusBusMessage.StartMainTrackResponseMessage.isEqual(rMessage.GetBytes());
                    if (null != StartMainTrackResponseMessageReceived && null != startmtMessage)
                    {
                        log.Info(string.Format("【启动主轨后响应】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = startmtMessage;
                        StartMainTrackResponseMessageReceived(this, args);
                    }
                    //【停止主轨后响应】
                    var smtMessage = SusNet2.Common.SusBusMessage.StopMainTrackResponseMessage.isEqual(rMessage.GetBytes());
                    if (null != StopMainTrackResponseMessageReceived && null != smtMessage)
                    {
                        log.Info(string.Format("【停止主轨后响应】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = smtMessage;
                        StopMainTrackResponseMessageReceived(this, args);
                    }
                    //【急停主轨后响应】
                    var esmtMessage = SusNet2.Common.SusBusMessage.EmergencyStopMainTrackResponseMessage.isEqual(rMessage.GetBytes());
                    if (null != EmergencyStopMainTrackResponseMessageReceived && null != esmtMessage)
                    {
                        log.Info(string.Format("【急停主轨后响应】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = esmtMessage;
                        EmergencyStopMainTrackResponseMessageReceived(this, args);
                    }
                    //【【停止接收衣架】终端按下【暂停键时】硬件通知pc】
                    var srhMessage = SusNet2.Common.SusBusMessage.StopReceiveHangerRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != StopReceiveHangerRequestMessageReceived && null != srhMessage)
                    {
                        log.Info(string.Format("【停止接收衣架】【终端按下【暂停键时】硬件通知pc消息】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = srhMessage;
                        StopReceiveHangerRequestMessageReceived(this, args);
                    }

                    //【衣架落入读卡器发送的请求，硬件发pc端时触发】
                    var hdcMessage = SusNet2.Common.SusBusMessage.HangerDropCardRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != HangerDropCardRequestMessageReceived && null != hdcMessage)
                    {
                        log.Info(string.Format("【衣架落入读卡器 硬件通知pc消息】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = hdcMessage;
                        HangerDropCardRequestMessageReceived(this, args);
                    }

                    //【衣架进站】
                    var hasMessage = SusNet2.Common.SusBusMessage.HangerArrivalStatingRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != HangerArrivalStatingMessageReceived && null != hasMessage)
                    {
                        log.Info(string.Format("【衣架进站】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = hasMessage;
                        HangerArrivalStatingMessageReceived(this, args);
                    }
                    //【衣架出战】
                    var hoMessage = SusNet2.Common.SusBusMessage.HangerOutStatingRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != HangerOutStatingRequestMessageReceived && null != hoMessage)
                    {
                        log.Info(string.Format("【衣架出战】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = hoMessage;
                        HangerOutStatingRequestMessageReceived(this, args);
                    }
                    //【分配工序到衣架成功回应】
                    var allMessage = SusNet2.Common.SusBusMessage.AllocationHangerResponseMessage.isEqual(rMessage.GetBytes());
                    if (null != AllocationHangerResponseMessageReceived && null != allMessage)
                    {
                        log.Info(string.Format("【分配工序到衣架成功回应】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = allMessage;
                        AllocationHangerResponseMessageReceived(this, args);
                    }
                    //【制品界面上线来自硬件的响应】
                    var cmsResMessage = SusNet2.Common.SusBusMessage.ClientMachineResponseMessage.isEqual(rMessage.GetBytes());
                    if (null != ClientMachineResponseMessageReceived && null != cmsResMessage)
                    {
                        log.Info(string.Format("【制品界面上线来自硬件的响应】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = cmsResMessage;
                        ClientMachineResponseMessageReceived(this, args);
                    }
                    //【挂片站上线】
                    var hpsOnlineResMessage = SusNet2.Common.SusBusMessage.HangingPieceStatingOnlineRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != HangingPieceStatingOnlineMessageReceived && null != hpsOnlineResMessage)
                    {
                        log.Info(string.Format("【挂片站上线】客户端收到服务器端消息:{0}", rMessage.GetHexStr()));
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = allMessage;
                        HangingPieceStatingOnlineMessageReceived(this, args);
                    }

                    //if ( rMessage.body != null )
                    //{
                    //    //Log.Info("【客户端:{0}】收到消息完整解析消息:【服务器: {1}】-->【消息:{2}】",e.TcpClient.Client.LocalEndPoint.ToString(),e.TcpClient.Client.RemoteEndPoint.ToString(),rMessage.Describe());
                    //    Log.Info("【客户端:{0}】收到消息完整解析消息:【服务器: {1}】-->【消息:{2}】", e.TcpClient.Client.LocalEndPoint.ToString(), e.TcpClient.Client.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(rMessage.body));
                    //    MessageBody messageBody = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(rMessage.body));
                    //    if ( messageBody != null )
                    //    {
                    //        Log.Info("【客户端:{0}】收到消息业务消息:【客户端: {1}】-->【消息:{2}】",e.TcpClient.Client.LocalEndPoint.ToString(),e.TcpClient.Client.RemoteEndPoint.ToString(),messageBody.Describe());
                    //        switch ( rMessage.type )
                    //        {
                    //            case MessageType.ACK:
                    //                break;
                    //            case MessageType.Heartbeat:
                    //                Ack(messageBody);
                    //                break;
                    //            case MessageType.Common:
                    //                if ( MessageReceived != null )
                    //                {
                    //                    MessageEventArgs args = new MessageEventArgs(messageBody.DATA);
                    //                    try
                    //                    {
                    //                        MessageReceived(this,args);
                    //                    }
                    //                    catch ( Exception ex )
                    //                    {
                    //                        Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                    //                    }
                    //                }
                    //                Ack(messageBody);
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        /// <summary>
        /// 启动主轨
        /// </summary>
        public void StartMainTrack(string mainTrackNo, string xor = null)
        {
            //01 00 03 XX 00 06 00 00 00 00 00 10
            var message = new SusNet2.Common.SusBusMessage.StartMainTrackRequestMessage(mainTrackNo, xor);
            log.Info(string.Format("【启动主轨】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【启动主轨】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        /// <summary>
        /// 停止主轨
        /// </summary>
        public void StopMainTrack(string mainTrackNo, string xor = null)
        {
            //01 00 03 XX 00 06 00 00 00 00 00 11
            var message = new SusNet2.Common.SusBusMessage.StopMainTrackRequestMessage(mainTrackNo, xor);
            log.Info(string.Format("【停止主轨】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【停止主轨】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        /// <summary>
        /// 急停主轨
        /// </summary>
        public void EmergencyStopMainTrack(string mainTrackNo, string xor = null)
        {
            //01 00 03 XX 00 06 00 00 00 00 00 12
            var message = new SusNet2.Common.SusBusMessage.EmergencyStopMainTrackRequestMessage(mainTrackNo, xor);
            log.Info(string.Format("【急停主轨】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【急停主轨】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }

        /// <summary>
        /// 出站【对硬件是否出战的回应】
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="isAuto"></param>
        /// <param name="hangerNo"></param>
        /// <param name="xor"></param>
        public void AutoHangerOutStating(string mainTrackNo, string statingNo, bool isAuto, string hangerNo, string xor = null)
        {
            //01 44 05 XX 00 55 00 AA BB CC DD EE 允许出站
            var message = new SusNet2.Common.SusBusMessage.HangerOutStatingResponseMessage(mainTrackNo, statingNo, isAuto, hangerNo, "00", "55", xor);
            log.Info(string.Format("【出站】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【出站】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }

        /// <summary>
        /// 分配衣架下一工序站点回应
        /// </summary>
        public void AllocationHanger(string mainTrackNo, string statingNo, string hangerNo, string xor = null)
        {
            //01 46 04 XX 00 51 00 AA BB CC DD EE 将衣架分配到下一个46工位成功
            var message = new SusNet2.Common.SusBusMessage.AllocationHangerResponseMessage(mainTrackNo, hangerNo, "00", "51", xor);
            log.Info(string.Format("【分配衣架下一工序站点回应】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【分配衣架下一工序站点回应】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        /// <summary>
        /// 衣架进站软件回应
        /// </summary>
        public void HangerArrivalStating(string mainTrackNo, string statingNo, string hangerNo, string xor = null)
        {
            //上位机回复：01 44 05 XX 00 50 00 AA BB CC DD EE 回复
            var message = new SusNet2.Common.SusBusMessage.HangerArrivalStatingResponeMessage(mainTrackNo, statingNo, "00", "50", hangerNo, xor);
            log.Info(string.Format("【衣架进站软件回应】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【衣架进站软件回应】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        ///// <summary>
        ///// 工序检查回应
        ///// </summary>
        //public void CheckFlowResponse(string mainTrackNo, string statingNo, string hangerNo, string xor = null)
        //{
        //    //上位机回复：01 44 05 XX 00 54 01 AA BB CC DD EE 不同工序

        //    var message = new SusNet2.Common.SusBusMessage.HangerDropCardResponseMessage(mainTrackNo, hangerNo, "00", "54", xor);
        //    log.Info(string.Format("【工序检查回应】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        //    SendMessage(message);
        //    log.Info(string.Format("【工序检查回应】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        //}
        /// <summary>
        /// 制品界面上线
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="productNumber"></param>
        /// <param name="xor"></param>
        public void ClientMancheOnLine(string mainTrackNo, string statingNo, string productNumber, string xor = null)
        {
            var message = new SusNet2.Common.SusBusMessage.ClientMachineRequestMessage(mainTrackNo, statingNo, "00", "35", productNumber, xor);
            log.Info(string.Format("【制品界面上线请求】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【制品界面上线请求】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }

        /// <summary>
        /// 挂片站上线请求响应pc---->硬件
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="productNumber"></param>
        /// <param name="xor"></param>
        public void HangingPieceOnLine(string mainTrackNo, string statingNo, string productNumber, string xor = null)
        {
            var message = new SusNet2.Common.SusBusMessage.HangingPieceStatingOnlineResponseMessage(mainTrackNo, statingNo, "00", "35", productNumber, xor);
            log.Info(string.Format("【挂片站上线请求响应pc---->硬件】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【挂片站上线请求响应pc---->硬件】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        /// <summary>
        /// 【衣架进站pc---->硬件】
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="hangerNo"></param>
        public void HangerArriveStating(string mainTrackNo, string statingNo, string hangerNo, string xor = null)
        {
            //01 04 05 XX 00 50 00 AA BB CC DD EE
            var message = new SusNet2.Common.SusBusMessage.HangerArrivalStatingResponeMessage(mainTrackNo, statingNo, "00", "50", hangerNo, xor);
            log.Info(string.Format("【衣架进站pc---->硬件】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【衣架进站pc---->硬件】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        /// <summary>
        /// 给衣架分配下一站点
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="hangerNo"></param>
        /// <param name="xor"></param>
        public void AllocationHangerToNextStating(string mainTrackNo, string statingNo, string hangerNo, string xor = null)
        {
            //01 04 03 XX 00 51 00 AA BB CC DD EE
            var message = new SusNet2.Common.SusBusMessage.AllocationHangerRequestMessage(mainTrackNo, statingNo, "00", "51", hangerNo, xor);
            log.Info(string.Format("【给衣架分配下一站点pc---->硬件】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【给衣架分配下一站点pc---->硬件】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        /// <summary>
        /// 衣架落入读卡器工序比较 pc---->硬件
        /// </summary>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="hangerNo"></param>
        /// <param name="tag">
        /// 0:相同工序
        /// 1:工序不同
        /// 2:返工衣架
        /// </param>
        /// <param name="xor"></param>
        public void HangerDropCardProcessFlowCompare(string mainTrackNo, string statingNo, string hangerNo, int tag, string xor = null)
        {
            //01 04 05 XX 00 54 00 AA BB CC DD EE

            var message = new SusNet2.Common.SusBusMessage.HangerDropCardResponseMessage(mainTrackNo, statingNo, hangerNo, tag, "00", "54", xor);
            log.Info(string.Format("【衣架落入读卡器工序比较 pc---->硬件】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendMessage(message);
            log.Info(string.Format("【衣架落入读卡器工序比较 pc---->硬件】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        //public void Test()
        //{
        //    var productsModel = new ProductsModel();
        //    var data = string.Format("933304-9BUY,010,28,任务1867件,单位1件,累计出10000件,今日出213件");
        //    var hexData = SusNet2.Common.Utils.HexHelper.ToHex(data, "utf-8", false);
        //}

        /// <summary>
        /// 向挂片站发送制品信息
        /// </summary>
        /// <param name="products"></param>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="productNumber"></param>
        /// <param name="xor"></param>
        public void BindProudctsToHangingPiece(List<byte> products, string mainTrackNo, string statingNo, int productNumber, string xor = null)
        {
            if (productNumber > 255)
            {
                var ex = new ApplicationException("排产号超过最大值255!不能挂片!");
                log.Error("【向挂片站发送制品信息】", ex);
                throw ex;
            }
            if (products.Count > 46)
            {
                var ex = new ApplicationException("制品信息超过46个字节!不能挂片!");
                log.Error("【向挂片站发送制品信息】", ex);
                throw ex;
            }
            if (string.IsNullOrEmpty(xor))
            {
                xor = "00";
            }
            var endData = string.Format("{0} {1} 03 {2} 60 0F FF FF FF FF FF FF", mainTrackNo, statingNo, xor);
            //var data = string.Format("{0} {1} {2} {3} {60}", mainTrackNo, statingNo, "03", xor, "60");
            //var index = 0;

            var j = 0;
            for (var i = 0; i < 8; i++)
            {
                var sendDataList = new List<byte>();
                var sData = BindProcessOrderHangingPieceRequestMessage.GetHeaderBytes(mainTrackNo, statingNo, "60", string.Format("0{0}", i), productNumber, i, xor);
                sendDataList.AddRange(sData);
                if (j < products.Count)
                {
                    for (int b = j; j < products.Count; j++)
                    {
                        if (sendDataList.Count == 12)
                        {
                            break;
                        }
                        sendDataList.Add(products[j]);
                    }
                }
                var teLen = sendDataList.Count;
                for (var ii = 0; ii < 12 - teLen; ii++)
                {
                    if (sendDataList.Count == 12)
                    {
                        break;
                    }
                    sendDataList.AddRange(HexHelper.strToToHexByte("FF"));
                }
                log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送开始,【序号:{0}】 消息:--->{1}", i, HexHelper.byteToHexStr(sendDataList.ToArray())));
                client.Send(sendDataList.ToArray());

                log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送完成，【序号:{0}】 消息:--->{1}", i, SusNet2.Common.Utils.HexHelper.byteToHexStr(sendDataList.ToArray())));

            }
            client.Send(HexHelper.strToToHexByte(endData));
            log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送完成，【主轨号+站号+排产号:{0}】 消息:--->{1}", string.Format("{0},{1},{2}", mainTrackNo, statingNo, productNumber), endData));


            //for (var b= 0;b<products.Count;b++) {
            //    if (index == 0) {
            //        var fisrtData = string.Format(data+" 0{0} {1}", index, productNumber);
            //        sendData.AddRange(HexHelper.strToToHexByte(fisrtData));
            //        var bTemp = new byte[5];
            //        products.CopyTo(bTemp, b);
            //        sendData.AddRange(bTemp);
            //        log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送开始,【序号:{0}】 消息:--->{1}", index, HexHelper.byteToHexStr(sendData.ToArray())));
            //        b = 4;
            //        index++;
            //        client.Send(sendData.ToArray());
            //        log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送完成，【序号:{0}】 消息:--->{1}",index, SusNet2.Common.Utils.HexHelper.byteToHexStr(sendData.ToArray())));
            //        continue;
            //    }
            //    if (sendData.Count==12) {
            //        sendData = new List<byte>();
            //        var fisrtData = string.Format(data + " 0{0} {1}", index, productNumber);
            //        sendData.AddRange(HexHelper.strToToHexByte(fisrtData));
            //        var bTemp = new byte[6];
            //        products.CopyTo(bTemp, b+1);
            //        sendData.AddRange(bTemp);
            //        log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送开始,【序号:{0}】 消息:--->{1}", index, HexHelper.byteToHexStr(sendData.ToArray())));
            //        b = b+6;
            //        client.Send(sendData.ToArray());
            //        log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送完成，【序号:{0}】 消息:--->{1}",index, SusNet2.Common.Utils.HexHelper.byteToHexStr(sendData.ToArray())));
            //        index++;
            //    }

            //}
        }

        /// <summary>
        /// 制品界面直接上线：上线信息【制单号，颜色，尺码，任务数量，单位，累计完成，今日上线】发送
        /// </summary>
        /// <param name="onlineInfo"></param>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="xor"></param>
        public void SendDataByProductsDirectOnline(List<byte> onlineInfo, string mainTrackNo, string statingNo, string xor = null)
        {
            if (null == onlineInfo || onlineInfo.Count == 0)
            {
                var ex = new ApplicationException("发送内容不能为空!");
                log.Error("【制品界面直接上线】", ex);
                throw ex;
            }
            if (string.IsNullOrEmpty(xor))
            {
                xor = "00";
            }
            var j = 0;
            var times = 0;
            if (onlineInfo.Count % 6 == 0)
            {
                times = onlineInfo.Count / 6;
            }
            else {
                times =1+ onlineInfo.Count / 6;
            }
            for (var i = 0; i < times; i++)
            {
                var sendDataList = new List<byte>();
                var sData = ProductsDirectOnlineRequestMessage.GetHeaderBytes(mainTrackNo, statingNo, "01", string.Format("{0:00}", i), i, xor);
                sendDataList.AddRange(sData);
                if (j < onlineInfo.Count)
                {
                    for (int b = j; j < onlineInfo.Count; j++)
                    {
                        if (sendDataList.Count == 12)
                        {
                            break;
                        }
                        sendDataList.Add(onlineInfo[j]);
                    }
                }
                var teLen = sendDataList.Count;
                for (var ii = 0; ii < 12 - teLen; ii++)
                {
                    if (sendDataList.Count == 12)
                    {
                        break;
                    }
                    sendDataList.AddRange(HexHelper.strToToHexByte("00"));
                }
                log.Info(string.Format("【制品界面直接上线 pc---->硬件】发送开始,【序号:{0}】 消息:--->{1}", i, HexHelper.byteToHexStr(sendDataList.ToArray())));
                client.Send(sendDataList.ToArray());

                log.Info(string.Format("【制品界面直接上线pc---->硬件】发送完成，【序号:{0}】 消息:--->{1}", i, SusNet2.Common.Utils.HexHelper.byteToHexStr(sendDataList.ToArray())));

            }
            var endData = string.Format("{0} {1} 05 {2} 01 {3} 00 00 00 00 00 00", mainTrackNo, statingNo, xor, string.Format("{0:00}", times));
            client.Send(HexHelper.strToToHexByte(endData));
            log.Info(string.Format("【向挂片站发送制品信息pc---->硬件】发送完成，【主轨号+站号】 消息:--->{0}", string.Format("{0},{1}", mainTrackNo, statingNo), endData));

        }


        /// <summary>
        /// 挂片站上线，制品信息回写到硬件：上线信息【制单号，颜色，尺码，任务数量，单位，累计完成，今日上线】发送
        /// </summary>
        /// <param name="onlineInfo"></param>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="xor"></param>
        public void SendDataByHangingPieceOnline(List<byte> onlineInfo, string mainTrackNo, string statingNo, string xor = null)
        {
            if (null == onlineInfo || onlineInfo.Count == 0)
            {
                var ex = new ApplicationException("发送内容不能为空!");
                log.Error("【挂片站上线】", ex);
                throw ex;
            }
            if (string.IsNullOrEmpty(xor))
            {
                xor = "00";
            }
            var j = 0;
            var times = 0;
            if (onlineInfo.Count % 6 == 0)
            {
                times = onlineInfo.Count / 6;
            }
            else
            {
                times = 1 + onlineInfo.Count / 6;
            }
            for (var i = 0; i < times; i++)
            {
                var sendDataList = new List<byte>();
                var sData = HangingPieceStatingOnlineResponseMessage.GetHeaderBytes(mainTrackNo, statingNo, "01", string.Format("{0:00}", i), i, xor);
                sendDataList.AddRange(sData);
                if (j < onlineInfo.Count)
                {
                    for (int b = j; j < onlineInfo.Count; j++)
                    {
                        if (sendDataList.Count == 12)
                        {
                            break;
                        }
                        sendDataList.Add(onlineInfo[j]);
                    }
                }
                var teLen = sendDataList.Count;
                for (var ii = 0; ii < 12 - teLen; ii++)
                {
                    if (sendDataList.Count == 12)
                    {
                        break;
                    }
                    sendDataList.AddRange(HexHelper.strToToHexByte("00"));
                }
                log.Info(string.Format("【挂片站上线 pc---->硬件】发送开始,【序号:{0}】 消息:--->{1}", i, HexHelper.byteToHexStr(sendDataList.ToArray())));
                client.Send(sendDataList.ToArray());

                log.Info(string.Format("【挂片站上线 pc---->硬件】发送完成，【序号:{0}】 消息:--->{1}", i, SusNet2.Common.Utils.HexHelper.byteToHexStr(sendDataList.ToArray())));

            }
            var endData = string.Format("{0} {1} 05 {2} 01 {3} 00 00 00 00 00 00", mainTrackNo, statingNo, xor, string.Format("{0:00}", times));
            client.Send(HexHelper.strToToHexByte(endData));
            log.Info(string.Format("【挂片站上线 ，发送制品信息pc---->硬件】发送完成，【主轨号+站号】 消息:--->{0}", string.Format("{0},{1}", mainTrackNo, statingNo), endData));

        }

        /// <summary>
        /// 衣架落入读卡器,衣架携带制品信息推送：制品信息【产品及工艺信息，制单号，颜色，尺码，单位，工序：工序号，工艺信息】发送
        /// </summary>
        /// <param name="onlineInfo"></param>
        /// <param name="mainTrackNo"></param>
        /// <param name="statingNo"></param>
        /// <param name="xor"></param>
        public void SendDataByHangerDropCardCompare(List<byte> onlineInfo, string mainTrackNo, string statingNo, string xor = null)
        {
            if (null == onlineInfo || onlineInfo.Count == 0)
            {
                var ex = new ApplicationException("发送内容不能为空!");
                log.Error("【衣架落入读卡器,衣架携带制品信息推送】", ex);
                throw ex;
            }
            if (string.IsNullOrEmpty(xor))
            {
                xor = "00";
            }
            var j = 0;
            var times = 0;
            if (onlineInfo.Count % 6 == 0)
            {
                times = onlineInfo.Count / 6;
            }
            else
            {
                times = 1 + onlineInfo.Count / 6;
            }
            for (var i = 0; i < times; i++)
            {
                var sendDataList = new List<byte>();
                var sData = HangerDropCardResponseMessage.GetHeaderBytes(mainTrackNo, statingNo, "01", string.Format("{0:00}", i), i, xor);
                sendDataList.AddRange(sData);
                if (j < onlineInfo.Count)
                {
                    for (int b = j; j < onlineInfo.Count; j++)
                    {
                        if (sendDataList.Count == 12)
                        {
                            break;
                        }
                        sendDataList.Add(onlineInfo[j]);
                    }
                }
                var teLen = sendDataList.Count;
                for (var ii = 0; ii < 12 - teLen; ii++)
                {
                    if (sendDataList.Count == 12)
                    {
                        break;
                    }
                    sendDataList.AddRange(HexHelper.strToToHexByte("00"));
                }
                log.Info(string.Format("【衣架落入读卡器,衣架携带制品信息推送 pc---->硬件】发送开始,【序号:{0}】 消息:--->{1}", i, HexHelper.byteToHexStr(sendDataList.ToArray())));
                client.Send(sendDataList.ToArray());

                log.Info(string.Format("【衣架落入读卡器,衣架携带制品信息推送 pc---->硬件】发送完成，【序号:{0}】 消息:--->{1}", i, SusNet2.Common.Utils.HexHelper.byteToHexStr(sendDataList.ToArray())));

            }
            var endData = string.Format("{0} {1} 05 {2} 01 {3} 00 00 00 00 00 00", mainTrackNo, statingNo, xor, string.Format("{0:00}", times));
            client.Send(HexHelper.strToToHexByte(endData));
            log.Info(string.Format("【衣架落入读卡器,衣架携带制品信息推送 ，发送制品信息pc---->硬件】发送完成，【主轨号+站号】 消息:--->{0}", string.Format("{0},{1}", mainTrackNo, statingNo), endData));

        }

    }
}
