using log4net;
using Newtonsoft.Json;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Event;
using Sus.Net.Common.Message;
using Sus.Net.Common.Utils;
using Sus.Net.Server.Sockets;
using SusNet2.Common.Model;
using SusNet2.Common.SusBusMessage;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;


namespace Sus.Net.Server
{
    public class SusTCPServer
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(SusTCPServer));
        private readonly AsyncTcpServer server;
        /// <summary>
        /// 接收到消息
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        public event EventHandler<MessageEventArgs> HangingPieceBindProcessMessageReceived;

        public event EventHandler<MessageEventArgs> ProductsDirectOnlineMessageReceived;

        /// <summary>
        /// 与客户端的连接已建立事件
        /// </summary>
        public event EventHandler<TcpClientConnectedEventArgs> ClientConnected;
        /// <summary>
        /// 与客户端的连接已断开事件
        /// </summary>
        public event EventHandler<TcpClientDisconnectedEventArgs> ClientDisconnected;

        public static Dictionary<string, StatingInfo> DictStatingInfo = new Dictionary<string, StatingInfo>();

        #region 业务事件
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
        /// 衣架出站，硬件发pc端时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> HangerOutStatingRequestMessageReceived;
        /// <summary>
        /// 【协议2.0】 挂片站上传衣架号信息事件
        /// </summary>

        public event EventHandler<MessageEventArgs> HangingPieceHangerUploadRequestMessageReceived;

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

        /// <summary>
        /// 衣架返工时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> HangerReworkMessageReceived;
        /// <summary>
        /// 衣架返工收到疵点代码时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> ReworkDefectMessageReceived;

        /// <summary>
        /// 卡片请求时触发
        /// </summary>
        public event EventHandler<MessageEventArgs> CardRequestMessageReceived;

        /// <summary>
        /// 衣架在监测点读卡时触发消息
        /// </summary>
        public event EventHandler<MessageEventArgs> MonitorMessageReceived;

        /// <summary>
        /// 衣架进站时读卡发现满站触发
        /// </summary>
        public event EventHandler<MessageEventArgs> FullSiteMessageReceived;

        /// <summary>
        /// 清除衣架缓存 硬件的响应
        /// </summary>
        public event EventHandler<MessageEventArgs> ClearHangerCacheResponseMessageReceived;

        /// <summary>
        /// 返工疵点代码
        /// </summary>
        public event EventHandler<MessageEventArgs> ReworkFlowDefectRequestMessageReceived;

        /// <summary>
        /// 修改站点容量
        /// </summary>
        public event EventHandler<MessageEventArgs> StatingCapacityResponseMessageReceived;

        /// <summary>
        /// 修改或者新增站点类型
        /// </summary>
        public event EventHandler<MessageEventArgs> StatingTypeResponseMessageReceived;

        /// <summary>
        /// 上电初始化
        /// </summary>

        public event EventHandler<MessageEventArgs> PowerSupplyInitMessageReceived;
        /// <summary>
        /// 主板版本
        /// </summary>
        public event EventHandler<MessageEventArgs> MainboardVersionMessageReceived;
        /// <summary>
        /// SN序列号
        /// </summary>
        public event EventHandler<MessageEventArgs> SNSerialNumberMessageReceived;

        /// <summary>
        /// 下位机暂停或接收衣架
        /// </summary>
        public event EventHandler<MessageEventArgs> LowerMachineSuspendOrReceiveMessageReceived;


        /// <summary>
        /// 上位机发起的上电初始化请求后的下位机的响应
        /// </summary>
        public event EventHandler<MessageEventArgs> UpperComputerInitResponseMessageReceived;

        /// <summary>
        /// 上位机发起满站查询修正
        /// </summary>
        public event EventHandler<MessageEventArgs> FullSiteQueryResponseMessageReceived;
        #endregion

        public SusTCPServer(int port)
        {
            server = new AsyncTcpServer(port) { Encoding = Encoding.UTF8 };
            server.ClientConnected += new EventHandler<TcpClientConnectedEventArgs>(server_ClientConnected);
            server.ClientDisconnected += new EventHandler<TcpClientDisconnectedEventArgs>(server_ClientDisconnected);
            //server.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(server_PlaintextReceived);
            server.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(server_DatagramReceived);
            MessageFactory.Instance.Init(new ClientUserInfo("0", "0"));
        }

        //启动
        public void Start()
        {
            InitProductsCMD();
            server.Start();
            //Log.Info("服务器 {0} 已经启动",server.ToString());
            log.Info(string.Format("服务器 {0} 已经启动", server.ToString()));
        }

        public void Stop()
        {
            server.Stop();
            ClientManager.Instance.Clear();
            //Log.Info("服务器 {0} 已经停止",server.ToString());
            log.Info(string.Format("服务器 {0} 已经停止", server.ToString()));
        }

        //public void SendMessageToAll(string strMessage)
        //{
        //    try
        //    {
        //        Message message = MessageFactory.Instance.CreateMessage(strMessage);
        //        // Log.Info("已经给所有终端发送消息:{0}",message.Describe());
        //        log.InfoFormat(string.Format("已经给所有终端发送消息:{0}", message.Describe()));
        //        server.SendToAll(message.Encode());
        //    }
        //    catch (Exception ex)
        //    {
        //        log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
        //    }
        //}
        public void SendMessageToAll(byte[] data)
        {
            try
            {
                //Message message = MessageFactory.Instance.CreateMessage(strMessage);
                // Log.Info("已经给所有终端发送消息:{0}",message.Describe());
                log.Info(string.Format("已经给所有终端发送消息:{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(data)));
                server.SendToAll(data);
            }
            catch (Exception ex)
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }
        //public void SendMessageWithCorps(string strMessage, List<string> toCorpIds)
        //{
        //    try
        //    {
        //        if (null != toCorpIds)
        //        {

        //            SendMessageWithClients(strMessage, ClientManager.Instance.ClientInfoWithCorpIds(toCorpIds));
        //        }
        //        else
        //        {
        //            //Log.Warn("客户端列表为空，取消发送！！！！！");
        //            log.Warn(string.Format("客户端列表为空，取消发送！！！！！"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
        //        log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
        //    }
        //}

        //public void SendMessageWithClients(string strMessage, List<ClientUserInfo> clientInfos)
        //{
        //    try
        //    {
        //        if (null != clientInfos)
        //        {
        //            //Log.Info("将要发送消息:{0}",strMessage);
        //            //Log.Info("到客户端:");
        //            //Log.Info("[");
        //            log.Info(string.Format("将要发送消息:{0}", strMessage));
        //            log.Info(string.Format("到客户端:"));
        //            log.Info(string.Format("["));
        //            foreach (var info in clientInfos)
        //            {
        //                //Log.Info("{0}",info.ToString());
        //                log.Info(string.Format("{0}", info.ToString()));
        //            }
        //            //Log.Info("]");
        //            log.Info("]");
        //            SendMessage(MessageFactory.Instance.CreateMessage(strMessage), ClientManager.Instance.ClientKeyWithUserInfos(clientInfos));
        //        }
        //        else
        //        {
        //            //Log.Warn("客户端列表为空，取消发送！！！！！");
        //            log.Warn("客户端列表为空，取消发送！！！！！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
        //        log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
        //    }
        //}
        //public void SendMessage()
        //{

        //}
        //private void SendMessage(Message message, List<string> tcpClientEndPointKeys)
        //{
        //    try
        //    {
        //        if (null != tcpClientEndPointKeys)
        //        {
        //            //Log.Info("已经发送消息:{0}",message.Describe());
        //            //Log.Info("到客户端:");
        //            //Log.Info("[");
        //            log.Info(string.Format("已经发送消息:{0}", message.Describe()));
        //            log.Info(string.Format("到客户端:"));
        //            log.Info(string.Format("["));
        //            foreach (string clientKey in tcpClientEndPointKeys)
        //            {
        //                try
        //                {
        //                    //Log.Info("{0}",ClientManager.Instance.ClientUserInfoDesc(clientKey));
        //                    log.Info(string.Format("{0}", ClientManager.Instance.ClientUserInfoDesc(clientKey)));
        //                    server.Send(clientKey, message.Encode());
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
        //                    log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
        //                }
        //            }
        //            //  Log.Info("]");
        //            log.Info("]");
        //        }
        //        else
        //        {
        //            // Log.Warn("客户端列表为空，取消发送！！！！！");
        //            log.Warn("客户端列表为空，取消发送！！！！！");
        //        }
        //    }
        //    catch (InvalidProgramException ex)
        //    {
        //        //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
        //        log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
        //        log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
        //    }
        //}
        private void SendMessages(SusNet2.Common.Message.MessageBody message, List<string> tcpClientEndPointKeys)
        {
            try
            {
                if (null != tcpClientEndPointKeys)
                {
                    //Log.Info("已经发送消息:{0}",message.Describe());
                    //Log.Info("到客户端:");
                    //Log.Info("[");
                    log.Info(string.Format("已经发送消息:{0}", message.Describe()));
                    log.Info(string.Format("到客户端:"));
                    log.Info(string.Format("["));
                    foreach (string clientKey in tcpClientEndPointKeys)
                    {
                        try
                        {
                            //Log.Info("{0}",ClientManager.Instance.ClientUserInfoDesc(clientKey));
                            log.Info(string.Format("{0}", ClientManager.Instance.ClientUserInfoDesc(clientKey)));
                            server.Send(clientKey, message.Encode());
                        }
                        catch (Exception ex)
                        {
                            // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                            log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                        }
                    }
                    //  Log.Info("]");
                    log.Info("]");
                }
                else
                {
                    // Log.Warn("客户端列表为空，取消发送！！！！！");
                    log.Warn("客户端列表为空，取消发送！！！！！");
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
        private void SendMessage(SusNet2.Common.Message.MessageBody message, string clientKey)
        {
            try
            {
                if (null != clientKey)
                {
                    //Log.Info("已经发送消息:{0}",message.Describe());
                    //Log.Info("到客户端:");
                    //Log.Info("[");
                    log.Info(string.Format("已经发送消息:{0}", message.Describe()));
                    log.Info(string.Format("到客户端:"));
                    log.Info(string.Format("["));
                    try
                    {
                        //Log.Info("{0}",ClientManager.Instance.ClientUserInfoDesc(clientKey));
                        log.Info(string.Format("{0}", ClientManager.Instance.ClientUserInfoDesc(clientKey)));
                        server.Send(clientKey, message.GetBytes());
                    }
                    catch (Exception ex)
                    {
                        // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                        log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                    }
                    //  Log.Info("]");
                    log.Info("]");
                }
                else
                {
                    // Log.Warn("客户端列表为空，取消发送！！！！！");
                    log.Warn("客户端列表为空，取消发送！！！！！");
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
        private void AckInfo(SusNet2.Common.Message.MessageBody recvMessage, TcpClient tcpClient)
        {
            //Message message = MessageFactory.Instance.CreateMessage(string.Empty, Common.Message.MessageType.ACK, recvMessage.id);
            //SendMessage(message, new List<string> { ClientKey(tcpClient) });
            log.Info(string.Format("Ack消息--->{0} 客户端{1}", recvMessage.GetHexStr(), ClientManager.Instance.ClientUserInfoDesc(ClientKey(tcpClient))));
        }
        private void Ack(SusNet2.Common.Message.MessageBody rMessage, TcpClient tcpClient)
        {
            //SendMessage(rMessage, new List<string> { ClientKey(tcpClient) });
        }
        private string ClientKey(TcpClient tcpClient)
        {
            if (tcpClient != null)
            {
                return tcpClient.Client.RemoteEndPoint.ToString();
            }
            return string.Empty;
        }

        private void server_ClientConnected(object sender, TcpClientConnectedEventArgs e)
        {
            // Log.Info("客户端:{0} 已连接",ClientKey(e.TcpClient));
            if (ClientConnected != null)
                ClientConnected(null, e);
            log.Info(string.Format("客户端:{0} 已连接", ClientKey(e.TcpClient)));
        }

        private void server_ClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            try
            {
                if (ClientDisconnected != null)
                    ClientDisconnected(null, e);
                //Log.Info("客户端:{0} 已断开",ClientKey(e.TcpClient));
                log.Info(string.Format("客户端:{0} 已断开", ClientKey(e.TcpClient)));
                ClientManager.Instance.RemoveTcpClient(e.TcpClient);
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }
        static List<byte> hangingPieceProductList = new List<byte>();
        static List<byte> g_productDataList = new List<byte>();
        static List<byte> g_productsInfo = new List<byte>();

        static List<byte> g_HangingPieceOnlineProductsInfo = new List<byte>();
        static List<byte> g_HangerDropCardCamporesInfo = new List<byte>();

        public static Dictionary<string, List<Hanger>> dicHangingPiece = new Dictionary<string, List<Hanger>>();
        public static Dictionary<string, List<Hanger>> dicCommonPiece = new Dictionary<string, List<Hanger>>();
        private void server_DatagramReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            try
            {
                TcpClient tcpClient = e.TcpClient;
                log.InfoFormat("收到消息:【客户端: {0}】-->【消息:{1}】", ClientKey(tcpClient), BufferUtils.ByteToHexStr(e.Datagram));
                //List<Message> messageList = MessageProcesser.Instance.ProcessRecvData(ClientKey(tcpClient),e.Datagram);
                List<SusNet2.Common.Message.MessageBody> messageList = SusNet2.Common.SusBusMessage.MessageProcesser.Instance.ProcessRecvData(e.TcpClient.Client.RemoteEndPoint.ToString(), e.Datagram);
                foreach (var rMessage in messageList)
                {
                    //Log.Info("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】",ClientKey(tcpClient),rMessage.Describe());
                    //心跳包特殊处理，不更新keys
                    if ("00 00 00 00 00 00 00 00 00 00 00 00".Equals(rMessage.GetHexStr()))
                    {
                        AckInfo(rMessage, e.TcpClient);
                    }
                    else
                    {
                        ClientManager.Instance.AddOrUpdateTcpClient(rMessage.gid, rMessage.XID, tcpClient);
                    }
                    log.Info(string.Format("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】", ClientKey(tcpClient), rMessage.GetHexStr()));


                    if (null != MessageReceived)
                    {
                        MessageEventArgs args = new MessageEventArgs(rMessage.GetHexStr());
                        args.Tag = rMessage;
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

                    #region 主轨相关
                    //【启动主轨后响应】
                    var startmtMessage = SusNet2.Common.SusBusMessage.StartMainTrackRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != startmtMessage)
                    {
                        log.Info(string.Format("【启动主轨后响应】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));

                        var startmtResponseMessage = "01 44 04 FF 00 37 00 00 00 00 00 10";
                        log.Info(string.Format("【启动主轨后响应】---->服务器端发送开始--->客户端消息为:{0}", startmtResponseMessage));
                        SendMessageToAll(HexHelper.strToToHexByte(startmtResponseMessage));
                        log.Info(string.Format("【启动主轨后响应】---->服务器端发送完成--->客户端消息为:{0}", startmtResponseMessage));

                    }
                    //【停止主轨后响应】
                    var stopmtMessage = SusNet2.Common.SusBusMessage.StopMainTrackRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != stopmtMessage)
                    {
                        log.Info(string.Format("【停止主轨后响应】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));

                        var startmtResponseMessage = "01 44 04 FF 00 37 00 00 00 00 00 11";
                        log.Info(string.Format("【停止主轨后响应】---->服务器端发送开始--->客户端消息为:{0}", startmtResponseMessage));
                        SendMessageToAll(HexHelper.strToToHexByte(startmtResponseMessage));
                        log.Info(string.Format("【停止主轨后响应】---->服务器端发送完成--->客户端消息为:{0}", startmtResponseMessage));

                    }

                    //【急停主轨后响应】
                    var emercMessage = SusNet2.Common.SusBusMessage.EmergencyStopMainTrackRequestMessage.isEqual(rMessage.GetBytes());
                    if (null != emercMessage)
                    {
                        log.Info(string.Format("【急停主轨后响应】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));

                        var startmtResponseMessage = "01 44 04 FF 00 37 00 00 00 00 00 12";
                        log.Info(string.Format("【急停主轨后响应】---->服务器端发送开始--->客户端消息为:【{0}】", startmtResponseMessage));
                        SendMessageToAll(HexHelper.strToToHexByte(startmtResponseMessage));
                        log.Info(string.Format("【急停主轨后响应】---->服务器端发送完成--->客户端消息为:【{0}】", startmtResponseMessage));

                    }
                    #endregion

                    #region 挂片站上线
                    var hPieceNo = "04";
                    var hexMainTrackNum = rMessage.XID;
                    var cmd = rMessage.CMD;
                    var address = string.Format("{0}{1}", rMessage.ADDH, rMessage.ADDL);
                    var id = rMessage.ID;
                    int productNum = GetProductNumber(address);
                    if (0 != productNum && id.Equals(hPieceNo))
                    {
                        var hg = new Hanger()
                        {
                            ProductNumber = productNum.ToString(),
                            MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                            StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                            Content = "制品发送挂片站确认",
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                        };
                        if (!dicHangingPiece.ContainsKey(hPieceNo))
                        {
                            dicHangingPiece.Add(rMessage.ID, new List<Hanger>() { hg });
                        }
                        else
                        {
                            dicHangingPiece[hPieceNo].Add(hg);
                        }
                    }
                    // var hexSta
                    //if (!dicHangingPiece.ContainsKey("04") && rMessage.CMD.Equals("03") && rMessage.ADDH.Equals("60") && rMessage.ADDL.Equals("00"))
                    //{
                    //    var hg = new Hanger()
                    //    {
                    //        ProductNumber = HexHelper.HexToTen(rMessage.DATA1).ToString(),
                    //        Index = dicHangingPiece.Count,
                    //        MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                    //        StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                    //        Content = "制品发送挂片站确认",
                    //        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                    //    };
                    //    //首次制品上线-->挂片
                    //    if (!dicHangingPiece.ContainsKey("04"))
                    //    {
                    //        dicHangingPiece.Add(rMessage.ID, new List<Hanger>() { hg });
                    //    }
                    //    else
                    //    {
                    //        dicHangingPiece["04"].Add(hg);
                    //    }
                    //}
                    //else if (dicHangingPiece.ContainsKey("04") && rMessage.CMD.Equals("03") && rMessage.ADDH.Equals("60") && rMessage.ADDL.Equals("00"))
                    //{
                    //    var hg = new Hanger()
                    //    {
                    //        ProductNumber = HexHelper.HexToTen(rMessage.DATA1).ToString(),
                    //        Index = dicHangingPiece.Count,
                    //        MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                    //        StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                    //        Content = "制品发送挂片站确认",
                    //        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                    //    };
                    //    //首次制品上线-->挂片
                    //    if (!dicHangingPiece.ContainsKey("04"))
                    //    {
                    //        dicHangingPiece.Add(rMessage.ID, new List<Hanger>() { hg });
                    //    }
                    //    else
                    //    {
                    //        dicHangingPiece["04"].Add(hg);
                    //    }
                    //}
                    #endregion

                    #region 制品界面直接上线
                    var statingNo = rMessage.ID;
                    if (!dicHangingPiece.ContainsKey("04") && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("35"))
                    {//首次制品上线-->挂片
                        if (!dicHangingPiece.ContainsKey("04"))
                        {
                            dicHangingPiece.Add(rMessage.ID, new List<Hanger>() {
                            new Hanger() {
                                ProductNumber=HexHelper.HexToTen(rMessage.DATA6).ToString(),
                                Index=dicHangingPiece.Count,
                                MainTrackNo=HexHelper.HexToTen(rMessage.XID).ToString(),
                                StatingNo=HexHelper.HexToTen(rMessage.ID).ToString(),
                                Content="制品发送挂片站确认",
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            }
                        });
                        }
                        else
                        {
                            dicHangingPiece["04"].Add(new Hanger()
                            {
                                ProductNumber = HexHelper.HexToTen(rMessage.DATA6).ToString(),
                                Index = dicHangingPiece.Count,
                                MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                Content = "制品发送挂片站确认"
                            });
                        }
                    }
                    else if (dicHangingPiece.ContainsKey("04") && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("35"))
                    {//多次  制品上线-->挂片
                        if (!dicHangingPiece.ContainsKey("04"))
                        {
                            dicHangingPiece[rMessage.ID].Add(new Hanger()
                            {
                                ProductNumber = HexHelper.HexToTen(rMessage.DATA6).ToString(),
                                Index = dicHangingPiece.Count,
                                MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                Content = "制品发送挂片站确认",
                                DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            });
                        }
                        else
                        {
                            dicHangingPiece["04"].Add(new Hanger()
                            {
                                ProductNumber = HexHelper.HexToTen(rMessage.DATA6).ToString(),
                                Index = dicHangingPiece.Count,
                                MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                Content = "制品发送挂片站确认",
                                DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            });
                        }
                    }
                    //制品界面显示信息
                    else if (dicHangingPiece.ContainsKey("04") && rMessage.CMD.Equals("05") && rMessage.ADDH.Equals("01"))
                    {

                    }
                    #endregion

                    #region 普通站
                    if (!statingNo.Equals("04"))
                    {
                        //站1
                        if (!dicCommonPiece.ContainsKey(statingNo) && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("51"))
                        {//首次制品上线-->挂片
                            if (!dicCommonPiece.ContainsKey(statingNo))
                            {
                                dicCommonPiece.Add(rMessage.ID, new List<Hanger>() {
                            new Hanger() {
                                //ProductNumber=rMessage.DATA6,
                                Index=dicCommonPiece.Count,
                                MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                HangerNo=StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2+rMessage.DATA3+rMessage.DATA4+rMessage.DATA5+rMessage.DATA6)).ToString()),
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            }
                        });
                            }
                            else
                            {
                                dicCommonPiece[statingNo].Add(new Hanger()
                                {
                                    //ProductNumber=rMessage.DATA6,
                                    Index = dicHangingPiece.Count,
                                    MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                    StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                    Content = "普通站消息",
                                    HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                                    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                                });
                            }
                            //衣架进站回应
                            var data = string.Format("{0} {1} 02 FF 00 50 00 {2}", rMessage.XID, rMessage.ID, string.Format("{0}{1}{2}{3}{4}", rMessage.DATA2, rMessage.DATA3, rMessage.DATA4, rMessage.DATA5, rMessage.DATA6));
                            var dBytes = HexHelper.strToToHexByte(data);
                            SendMessageToAll(dBytes);

                            ////衣架工序对比
                            //var dataCompare = string.Format("{0} {1} 06 FF 00 54 00 {2}", rMessage.XID, rMessage.ID, string.Format("{0}{1}{2}{3}{4}", rMessage.DATA2, rMessage.DATA3, rMessage.DATA4, rMessage.DATA5, rMessage.DATA6));
                            //var dataCompareBytes = HexHelper.strToToHexByte(dataCompare);
                            //SendMessageToAll(dataCompareBytes);

                        }
                        else if (dicCommonPiece.ContainsKey(statingNo) && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("51"))
                        {//多次  制品上线-->挂片
                            if (!dicHangingPiece.ContainsKey(statingNo))
                            {
                                dicCommonPiece[rMessage.ID].Add(new Hanger()
                                {
                                    //  ProductNumber = rMessage.DATA6,
                                    Index = dicCommonPiece.Count,
                                    MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                    StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                    Content = "普通站消息",
                                    HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                                    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                                });
                            }
                            else
                            {
                                dicCommonPiece[statingNo].Add(new Hanger()
                                {
                                    //  ProductNumber = rMessage.DATA6,
                                    Index = dicCommonPiece.Count,
                                    MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                    StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                    Content = "普通站消息",
                                    HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                                    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                                });
                            }
                            //衣架进站回应
                            var data = string.Format("{0} {1} 02 FF 00 50 00 {2}", rMessage.XID, rMessage.ID, string.Format("{0}{1}{2}{3}{4}", rMessage.DATA2, rMessage.DATA3, rMessage.DATA4, rMessage.DATA5, rMessage.DATA6));
                            var dBytes = HexHelper.strToToHexByte(data);
                            SendMessageToAll(dBytes);
                        }

                        //站2
                    }
                    #endregion

                    //【制品界面上线来自硬件的响应】
                    //var cmRsMessage = SusNet2.Common.SusBusMessage.ClientMachineRequestMessage.isEqual(rMessage.GetBytes());
                    //if (null != cmRsMessage)
                    //{
                    //    log.Info(string.Format("【制品界面上线来自硬件的响应】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));
                    //    cmRsMessage.CMD = "04";
                    //    var startmtResponseMessage = cmRsMessage.ToString(); //"01 01 04 FF 00 35 00 00 00 00 00 05";

                    //    log.Info(string.Format("【制品界面上线来自硬件的响应】---->服务器端发送开始--->客户端消息为:【{0}】", startmtResponseMessage));
                    //    SendMessageToAll(cmRsMessage.GetBytes());
                    //    log.Info(string.Format("【制品界面上线来自硬件的响应】---->服务器端发送完成--->客户端消息为:【{0}】", startmtResponseMessage));

                    //}

                    ////【分配工序到衣架成功回应】
                    //var allHangerMessage = SusNet2.Common.SusBusMessage.AllocationHangerRequestMessage.isEqual(rMessage.GetBytes());
                    //if (null != allHangerMessage)
                    //{
                    //    log.Info(string.Format("【分配工序到衣架成功回应】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));

                    //    var startmtResponseMessage = string.Format("{0} {1} 04 FF 00 51 00 {2}", allHangerMessage.XID, allHangerMessage.ID, allHangerMessage.HangerNo);//AA BB CC DD EE";
                    //    log.Info(string.Format("【分配工序到衣架成功回应】---->服务器端发送开始--->客户端消息为:【{0}】", startmtResponseMessage));
                    //    SendMessageToAll(HexHelper.strToToHexByte(startmtResponseMessage));
                    //    log.Info(string.Format("【分配工序到衣架成功回应】---->服务器端发送完成--->客户端消息为:【{0}】", startmtResponseMessage));

                    //}
                    //【衣架出站状态】
                    //var hSOutangerMessage = SusNet2.Common.SusBusMessage.HangerOutStatingResponseMessage.isEqual(rMessage.GetBytes());
                    //if (null != hSOutangerMessage)
                    //{
                    //    log.Info(string.Format("【衣架出站状态】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));

                    //    //var startmtResponseMessage = "01 04 04 FF 00 51 00 AA BB CC DD EE";
                    //    //log.Info(string.Format("【衣架出站状态】---->服务器端发送开始--->客户端消息为:【{0}】", startmtResponseMessage));
                    //    //SendMessageToAll(HexHelper.strToToHexByte(startmtResponseMessage));
                    //    //log.Info(string.Format("【衣架出站状态】---->服务器端发送完成--->客户端消息为:【{0}】", startmtResponseMessage));

                    //}
                    ////【衣架落入读卡器工序比较 pc---->硬件】
                    //var hDropCardMessage = SusNet2.Common.SusBusMessage.HangerDropCardResponseMessage.isEqual(rMessage.GetBytes());
                    //if (null != hDropCardMessage)
                    //{
                    //    log.Info(string.Format("【衣架落入读卡器工序比较 pc---->硬件】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));
                    //}
                    #region //【挂片终端上线制单回显示】
                    List<byte> productData = null;
                    List<byte> productNumberData = null;
                    if (SusNet2.Common.SusBusMessage.BindProcessOrderHangingPieceRequestMessage.isEqual(rMessage.GetBytes(), out productData, out productNumberData) && !BindProcessOrderHangingPieceRequestMessage.isEnd(rMessage.GetBytes()))
                    {
                        hangingPieceProductList.AddRange(productData.ToArray());
                        if (productNumberData != null)
                        {
                            g_productDataList.AddRange(productNumberData.ToArray());
                        }
                    }
                    if (BindProcessOrderHangingPieceRequestMessage.isEnd(rMessage.GetBytes()))
                    {
                        log.Info(string.Format("【pc挂片制单信息发送接收结束 pc---->硬件】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));
                        if (null != HangingPieceBindProcessMessageReceived)
                        {
                            var hexStr = HexHelper.byteToHexStr(hangingPieceProductList.ToArray());
                            var hexProductNmuber = HexHelper.byteToHexStr(g_productDataList.ToArray());
                            var productNumber = HexHelper.HexToTen(hexProductNmuber);
                            MessageEventArgs args = new MessageEventArgs(UnicodeUtils.GetChsFromHex(hexStr));//HexHelper.UnHex(hexStr, "utf-8"));
                            args.message = args.message + string.Format("  排产号:【{0}】", productNumber);
                            // args.Tag = productNumber;
                            HangingPieceBindProcessMessageReceived(this, args);
                            hangingPieceProductList.Clear();
                            productNumberData?.Clear();
                            g_productDataList.Clear();
                        }
                    }
                    #endregion

                    ////【制品界面直接上线】
                    //List<byte> productsInfo = null;
                    //if (SusNet2.Common.SusBusMessage.ProductsDirectOnlineRequestMessage.isEqual(rMessage.GetBytes(), out productsInfo) && !ProductsDirectOnlineRequestMessage.isEnd(rMessage.GetBytes()))
                    //{
                    //    g_productsInfo.AddRange(productsInfo.ToArray());
                    //}
                    //if (ProductsDirectOnlineRequestMessage.isEnd(rMessage.GetBytes()))
                    //{
                    //    log.Info(string.Format("【制品界面直接上线pc向硬件发送上线信息 pc---->硬件】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));
                    //    if (null != ProductsDirectOnlineMessageReceived)
                    //    {
                    //        var hexStr = HexHelper.byteToHexStr(g_productsInfo.ToArray());

                    //        MessageEventArgs args = new MessageEventArgs(UnicodeUtils.GetChsFromHex(hexStr));//HexHelper.UnHex(hexStr, "utf-8"));
                    //        ProductsDirectOnlineMessageReceived(this, args);
                    //        g_productsInfo?.Clear();

                    //    }
                    //}
                    #region //【挂片站上线】
                    List<byte> hangingPieceOnlineProduct = null;
                    if (SusNet2.Common.SusBusMessage.HangingPieceStatingOnlineResponseMessage.isEqual(rMessage.GetBytes(), out hangingPieceOnlineProduct) && !HangingPieceStatingOnlineResponseMessage.isEnd(rMessage.GetBytes()))
                    {
                        g_HangingPieceOnlineProductsInfo.AddRange(hangingPieceOnlineProduct.ToArray());
                    }
                    if (HangingPieceStatingOnlineResponseMessage.isEnd(rMessage.GetBytes()))
                    {
                        log.Info(string.Format("【挂片站上线pc向硬件发送上线信息 pc---->硬件】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));
                        if (null != ProductsDirectOnlineMessageReceived)
                        {
                            //var hexStr = HexHelper.byteToHexStr(g_HangingPieceOnlineProductsInfo.ToArray());
                            MessageEventArgs args = new MessageEventArgs(UnicodeUtils.GetChsFromHex(HexHelper.byteToHexStr(g_HangingPieceOnlineProductsInfo.ToArray())));//HexHelper.UnHex(hexStr, "utf-8"));
                            ProductsDirectOnlineMessageReceived(this, args);
                            g_HangingPieceOnlineProductsInfo?.Clear();

                        }
                    }
                    #endregion

                    //g_HangerDropCardCamporesInfo
                    #region //【衣架落入读卡器工序信息的比较信息推送】
                    List<byte> hangerDropCardFlowInfo = null;
                    if (SusNet2.Common.SusBusMessage.HangerDropCardResponseMessage.isEqual(rMessage.GetBytes(), out hangerDropCardFlowInfo) && !HangerDropCardResponseMessage.isEnd(rMessage.GetBytes()))
                    {
                        g_HangerDropCardCamporesInfo.AddRange(hangerDropCardFlowInfo.ToArray());
                    }
                    if (HangerDropCardResponseMessage.isEnd(rMessage.GetBytes()))
                    {
                        log.Info(string.Format("【衣架落入读卡器工序信息的比较信息推送 pc---->硬件】服务器端收到【客户端】消息:{0}", rMessage.GetHexStr()));
                        if (null != ProductsDirectOnlineMessageReceived)
                        {
                            // var hexStr = HexHelper.byteToHexStr(g_HangerDropCardCamporesInfo.ToArray());
                            MessageEventArgs args = new MessageEventArgs(UnicodeUtils.GetChsFromHex(HexHelper.byteToHexStr(g_HangerDropCardCamporesInfo.ToArray())));//HexHelper.UnHex(hexStr, "utf-8"));
                            ProductsDirectOnlineMessageReceived(this, args);
                            g_HangerDropCardCamporesInfo?.Clear();

                        }
                    }
                    #endregion


                    #region//【协议2.0】 衣架缓存清除
                    if (rMessage.CMD.Equals("03") && rMessage.ADDH.Equals("00") && rMessage.ADDL.Equals("52"))
                    {
                        var bList = new List<byte>();
                        for (var index = 7; index < 12; index++)
                        {
                            bList.Add(rMessage.GetBytes()[index]);
                        }

                        var chcResponseMessage = string.Format("{0} {1} 04 00 00 52 00 {2}", rMessage.XID, rMessage.ID, HexHelper.byteToHexStr(bList.ToArray()));//AA BB CC DD EE";
                        log.Info(string.Format("【衣架缓存清除成功回应】---->服务器端发送开始--->客户端消息为:【{0}】", chcResponseMessage));
                        SendMessageToAll(HexHelper.strToToHexByte(chcResponseMessage));
                        log.Info(string.Format("【衣架缓存清除成功回应】---->服务器端发送完成--->客户端消息为:【{0}】", chcResponseMessage));
                    }
                    #endregion

                    //if ( rMessage.body != null )
                    //{
                    //    Log.Info("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】", ClientKey(tcpClient), Encoding.UTF8.GetString(rMessage.body));
                    //    ClientManager.Instance.AddOrUpdateTcpClient("1111","1111", tcpClient);
                    //    //MessageBody messageBody = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(rMessage.body));
                    //    //if ( messageBody != null )
                    //    //{
                    //    //    ClientManager.Instance.AddOrUpdateTcpClient(messageBody.gid,messageBody.uid,tcpClient);
                    //    //    Log.Info("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】",ClientKey(tcpClient),messageBody.Describe());
                    //    //    switch ( rMessage.type )
                    //    //    {
                    //    //        case MessageType.ACK:
                    //    //            break;
                    //    //        case MessageType.Heartbeat:
                    //    //        case MessageType.Common:
                    //    //            Ack(messageBody,tcpClient);
                    //    //            break;
                    //    //        default:
                    //    //            break;
                    //    //    }
                    //    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }



        public void TestDataProcess(string strMessage, List<string> toCorpIds)
        {
            try
            {
                List<ClientUserInfo> clientInfos = ClientManager.Instance.ClientInfoWithCorpIds(toCorpIds);
                //Log.Info("将要发送消息:{0}",strMessage);
                //Log.Info("到客户端:");
                //Log.Info("[");
                log.Info(string.Format("将要发送消息:{0}", strMessage));
                log.Info(string.Format("到客户端:"));
                log.Info(string.Format("["));
                foreach (var info in clientInfos)
                {
                    //Log.Info("{0}",info.ToString());
                    log.Info(string.Format("{0}", info.ToString()));
                }
                //  Log.Info("]");
                log.Info(string.Format("]"));

                Message message = MessageFactory.Instance.CreateMessage(strMessage);
                List<string> tcpClientEndPointKeys = ClientManager.Instance.ClientKeyWithUserInfos(clientInfos);
                byte[] messageData = message.Encode();
                //Log.Info("已经发送消息:{0}",message.Describe());
                //Log.Info("到客户端:");
                //Log.Info("[");
                log.Info(string.Format("已经发送消息:{0}", message.Describe()));
                log.Info(string.Format("到客户端:"));
                log.Info(string.Format("["));
                foreach (string clientKey in tcpClientEndPointKeys)
                {
                    try
                    {
                        server.Send(clientKey, messageData);
                        //Log.Info("{0}",ClientManager.Instance.ClientUserInfoDesc(clientKey));
                        log.Info(string.Format("{0}", ClientManager.Instance.ClientUserInfoDesc(clientKey)));
                    }
                    catch (Exception ex)
                    {
                        // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                        log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
                    }
                }
                // Log.Info("]");
                log.Info("]");

                List<Message> messageList = Common.Message.MessageProcesser.Instance.ProcessRecvData("0", messageData);
                foreach (var rMessage in messageList)
                {
                    //Log.Info("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】","0",rMessage.Describe());
                    log.Info(string.Format("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】", "0", rMessage.Describe()));
                    if (rMessage.body != null)
                    {
                        MessageBody messageBody = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(rMessage.body));
                        if (messageBody != null)
                        {
                            // Log.Info("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】","0",messageBody.Describe());
                            log.Info(string.Format("收到消息完整解析消息:【客户端: {0}】-->【消息:{1}】", "0", messageBody.Describe()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                log.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        /// <summary>
        /// 【协议2.0】初始化制品推送地址与排产号的关系
        /// </summary>
        public static void InitProductsCMD()
        {
            ProductNumberAddresList.Clear();
            ProductNumberHeadAddresList.Clear();

            var begin = 24576;//60 00
            var end = 26623;//67 FF
            //var dicProductNumber = new Dictionary<int, List<string>>();
            var i = 1;
            var pNumber = 0;
            var addressList = new List<string>();
            for (var index = begin; index <= end; index++)
            {
                var hexStr = HexHelper.tenToHexString(index);
                Console.WriteLine(index + "--->" + hexStr);
                //Console.WriteLine("");
                addressList.Add(hexStr);
                if (i % 8 == 0)
                {
                    string[] aa = new string[8];
                    addressList.CopyTo(aa);
                    ProductNumberAddresList.Add(pNumber, new List<string>(aa));
                    ProductNumberHeadAddresList.Add(pNumber, new List<string>(aa)[0]);
                    addressList = new List<string>();

                    pNumber++;
                }

                i++;
            }
            //生成排产号与首地址的关系
            //foreach (var key in ) {

            //}
        }
        public int GetProductNumber(string address)
        {
            var productNumber = 0;
            foreach (var k in ProductNumberHeadAddresList.Keys)
            {
                if (ProductNumberHeadAddresList[k].Equals(address))
                {
                    productNumber = k;
                    return productNumber;
                }
            }
            return 0;
        }
        /// <summary>
        /// 【协议2.0】排产号与地址的约定关系
        /// </summary>
        public static Dictionary<int, List<string>> ProductNumberAddresList = new Dictionary<int, List<string>>();
        /// <summary>
        /// 【协议2.0】排产号约定地址的首位地址
        /// </summary>
        public static Dictionary<int, string> ProductNumberHeadAddresList = new Dictionary<int, string>();
        /// <summary>
        /// 排产号与地址关系
        /// </summary>
        // public static Dictionary<string, int> AddressProductNumberList = new Dictionary<string, int>();

        #region 业务相关
        public void SendMessageWithCANs(SusNet2.Common.Message.MessageBody message, List<ClientUserInfo> clientInfos)
        {
            try
            {
                if (null != clientInfos)
                {
                    log.InfoFormat("将要发送消息:{0}", message.GetHexStr());
                    log.InfoFormat("到客户端:");
                    log.InfoFormat("[");
                    foreach (var info in clientInfos)
                    {
                        log.InfoFormat("{0}", info.ToString());
                    }
                    log.InfoFormat("]");
                    //SendMessage(MessageFactory.Instance.CreateMessage(strMessage), ClientManager.Instance.ClientKeyWithUserInfos(clientInfos));
                    SendMessageToCAN(message, ClientManager.Instance.ClientKeyWithUserInfos(clientInfos));


                }
                else
                {
                    log.Warn("客户端列表为空，取消发送！！！！！");
                }
            }
            catch (Exception ex)
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

        private void SendMessageToCAN(SusNet2.Common.Message.MessageBody message, List<string> tcpClientEndPointKeys)
        {
            SendMessages(message, tcpClientEndPointKeys);
        }

        public void SendMessageWithCAN(SusNet2.Common.Message.MessageBody message, ClientUserInfo clientInfo)
        {
            try
            {

                if (null != clientInfo)
                {
                    log.InfoFormat("将要发送消息:{0}", message.GetHexStr());
                    log.InfoFormat("到客户端:");
                    log.InfoFormat("[");
                    log.InfoFormat("{0}", clientInfo.ToString());
                    log.InfoFormat("]");
                    SendMessage(message, ClientManager.Instance.ClientKeyWithUserInfo(clientInfo));

                }
                else
                {
                    log.Error(new ApplicationException("clientInfo为空!"));
                    log.Warn("客户端列表为空，取消发送！！！！！");
                }
            }
            catch (Exception ex)
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }
        #endregion
    }
}
