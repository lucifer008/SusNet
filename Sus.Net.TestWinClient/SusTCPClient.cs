using System;
using System.Net;
using System.Reflection;
using System.Text;
using Sus.Net.Client.Sockets;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Message;
using SuspeSys.Utils;
using SusNet2.Common.Utils;
using System.Collections.Generic;
using SusNet2.Common.Model;
using log4net;
using Sus.Net.Client;
using System.Net.Sockets;
using Sus.Net.Common.Utils;
using Sus.Net.Common.Event;
using SusNet2.Common.SusBusMessage;
using Sus.Net.TestWinServer;
using WinFormListviewAddContrl;
using System.Drawing;
using SusNet.Services.Action;

namespace Sus.Net.TestWinClient
{
    public class SusTCPClient : SusLog
    {
        private ILog log = LogManager.GetLogger(typeof(SusTCPClient));
        // private static readonly ILog tcpLogInfo = LogManager.GetLogger("TcpLogInfo");

        private readonly AsyncTcpClient client;

        private readonly ClientUserInfo clientUserInfo;
        /// <summary>
        /// 接收到消息
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        public event EventHandler<MessageEventArgs> HangingPieceBindProcessMessageReceived;

        //internal void SendMessageToAll(byte[] data)
        //{
        //    try
        //    {
        //        //Message message = MessageFactory.Instance.CreateMessage(strMessage);
        //        // Log.Info("已经给所有终端发送消息:{0}",message.Describe());
        //        log.Info(string.Format("已经给所有终端发送消息:{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(data)));
        //        SendData(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
        //    }
        //}

        public event EventHandler<MessageEventArgs> ProductsDirectOnlineMessageReceived;

        //   public ListViewEx LvAllMessage { get { return lbAllMessage; } }

        /// <summary>
        /// 服务器地址
        /// </summary>
        private readonly string serverIP;
        /// <summary>
        /// 服务器端口
        /// </summary>
        private readonly int serverPort;
        //static int index = 1;
        //internal void AddMessage(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var m = sender as MessageModel;

        //        var data = string.Format("{0}--->{1}", index, m.Message);
        //        m.lvMessage = m.lvMessage == null ? lbAllMessage : m.lvMessage;
        //        m.lvMessage.Items.Add(data);
        //        SetStyle(m.lvMessage);
        //        index++;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.ErrorFormat("AddMessage--->", ex);
        //    }
        //}
        void SetStyle(ListViewEx lv)
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
                if (i % 2 == 0)
                {
                    //for (int j = 0; j < listView1.Items[i].SubItems.Count; j++)
                    //{
                    //    listView1.Items[i].SubItems[j].BackColor = Color.LightGreen;
                    //}
                    lv.Items[i].BackColor = Color.LightGreen;
                }
                else
                {
                    //for (int j = 0; j < listView1.Items[i].SubItems.Count; j++)
                    //{
                    //    listView1.Items[i].SubItems[j].BackColor = Color.Turquoise;
                    //}
                    lv.Items[i].BackColor = Color.Turquoise;
                }
                lv.Items[i].UseItemStyleForSubItems = true;
            }
        }
        /// <summary>
        /// 心跳定时器
        /// </summary>
        private System.Timers.Timer heartbeatTimer = new System.Timers.Timer();
        static List<byte> hangingPieceProductList = new List<byte>();
        static List<byte> g_productDataList = new List<byte>();
        static List<byte> g_productsInfo = new List<byte>();

        static List<byte> g_HangingPieceOnlineProductsInfo = new List<byte>();
        static List<byte> g_HangerDropCardCamporesInfo = new List<byte>();
        public static Dictionary<string, List<Hanger>> dicHangingPiece = new Dictionary<string, List<Hanger>>();
        public static Dictionary<string, List<Hanger>> dicCommonPiece = new Dictionary<string, List<Hanger>>();
        public static Dictionary<string, StatingInfo> DictStatingInfo = new Dictionary<string, StatingInfo>();


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
        /// 【协议2.0】排产号与地址的约定关系
        /// </summary>
        public static Dictionary<int, List<string>> ProductNumberAddresList = new Dictionary<int, List<string>>();
        static SusTCPClient()
        {


        }
        public SusTCPClient(ClientUserInfo info, string serverIP, int serverPort)
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
            // client.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(client_PlaintextReceived);
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
                tcpLogInfo.Info(string.Format("开始连接服务器:({0}:{1})", serverIP, serverPort));
            }
            else
            {
            }
        }

        public void Disconnect()
        {
            client.Close(true);
        }
        public void SendData(byte[] data)
        {
            try
            {
                if (client.Connected)
                {
                    client.Send(data);
                    //Log.Info("已经发送消息:{0}",message.Describe());
                    tcpLogInfo.Info(string.Format("已经发送消息:{0}", HexHelper.byteToHexStr(data)));
                }

                else
                {
                    // Log.Error("未连接，无法发送消息:{0}",message);
                    tcpLogError.Error(string.Format("未连接，无法发送消息:{0}", HexHelper.byteToHexStr(data)));
                }
            }
            catch (InvalidProgramException ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                tcpLogError.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                tcpLogError.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
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
                    tcpLogInfo.Info(string.Format("已经发送消息:{0}", message.Describe()));
                }

                else
                {
                    // Log.Error("未连接，无法发送消息:{0}",message);
                    tcpLogError.Error(string.Format("未连接，无法发送消息:{0}", message));
                }
            }
            catch (InvalidProgramException ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                tcpLogError.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                tcpLogError.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
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
            //SendMessage(message);
            client.Send(message.GetBytes());
            log.Info(string.Format("【衣架落入读卡器工序比较 pc---->硬件】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
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
            //  var message = new SusMessage(HexHelper.StringToHexByte("00 00 03 00 00 00 00 00 00 00 00 00"));
            //SendMessage(MessageFactory.Instance.CreateMessage(string.Empty, Common.Message.MessageType.Heartbeat));
            var data = HexHelper.strToToHexByte("00 00 00 00 00 00 00 00 00 00 00 00");
            SendData(data);
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
                tcpLogInfo.Info(string.Format("已连接->服务器:{0} 端口:{1}", e.ToString(), e.Port));
                //  Login();
                ServerConnected?.Invoke(this, e);
                startHeartbeatTimer();
            }
            catch (Exception ex)
            {
                // Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                tcpLogError.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        private void client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            try
            {
                //Log.Warn("已经断开->服务器 {0} .",e.ToString());
                tcpLogInfo.Warn(string.Format("已经断开->服务器 {0} 端口:{1}", e.ToString(), e.Port));
                ServerDisconnected?.Invoke(this, e);
                stopHeartbeatTimer();
            }
            catch (Exception ex)
            {
                //  Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                tcpLogError.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        private void client_ServerExceptionOccurred(object sender, TcpServerExceptionOccurredEventArgs e)
        {
            try
            {
                //Log.Error("服务器:{0} 发生异常:{1}.",e.ToString(),e.Exception.Message);
                tcpLogError.Error(string.Format("服务器:{0} 端口:{1} 发生异常:{2}.", e.ToString(), e.Port, e.Exception.Message));
                ServerExceptionOccurred?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                //Log.Fatal("Class:{0}\nMethod:{1}\nException:{2}",this.GetType().Name,MethodBase.GetCurrentMethod().Name,ex.ToString());
                tcpLogError.Error(string.Format("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
        }

        /// <summary>
        /// 停止主轨
        /// </summary>
        public void StopMainTrack(string mainTrackNo, string xor = null)
        {
            //01 00 03 XX 00 06 00 00 00 00 00 11
            var message = new SusNet2.Common.SusBusMessage.StopMainTrackRequestMessage(mainTrackNo, xor);
            log.Info(string.Format("【停止主轨】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            SendData(message.GetBytes());
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
            SendData(message.GetBytes());
            log.Info(string.Format("【急停主轨】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
        /// <summary>
        /// 启动主轨
        /// </summary>
        public void StartMainTrack(string mainTrackNo, string xor = null)
        {
            //01 00 03 XX 00 06 00 00 00 00 00 10
            var message = new SusNet2.Common.SusBusMessage.StartMainTrackRequestMessage(mainTrackNo, xor);
            log.Info(string.Format("【启动主轨】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            client.Send(message.GetBytes());
            log.Info(string.Format("【启动主轨】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }

        private string ClientKey(TcpClient tcpClient)
        {
            if (tcpClient != null)
            {
                return tcpClient.Client.RemoteEndPoint.ToString();
            }
            return string.Empty;
        }
        private void client_DatagramReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
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

                    #region 上位机发起下位机初始化回应
                    //if (rMessage.CMD.Equals("01") && rMessage.ADDH.Equals("02") && rMessage.ADDL.Equals("09"))
                    //{
                    //    //var cmd = "02";
                    //    //var ddd = "FF FF FF FF 00 7F";

                    //    for (var m = 1; m < 2; m++)
                    //    {
                    //        for (var stat = 1; stat < 13; stat++)
                    //        {
                    //            var mess = string.Format("{0} {1} 02 00 02 09 FF FF FF FF 00 7F",HexHelper.tenToHexString(m), HexHelper.tenToHexString(stat));
                    //            var dBytes1 = HexHelper.strToToHexByte(mess);
                    //            SendMessageToAll(dBytes1);
                    //        }
                    //    }
                    //}
                    #endregion

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
                    var hPieceNo = "01";
                    var hexMainTrackNum = rMessage.XID;
                    var hPieceKey = string.Format("{0}-{1}", hexMainTrackNum, hPieceNo);

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
                        if (!dicHangingPiece.ContainsKey(hPieceKey))//!dicHangingPiece.ContainsKey(hPieceNo))
                        {
                            // dicHangingPiece.Add(rMessage.ID, new List<Hanger>() { hg });
                            dicHangingPiece.Add(hPieceKey, new List<Hanger>() { hg });
                        }
                        else
                        {
                            //dicHangingPiece[hPieceNo].Add(hg);
                            dicHangingPiece[hPieceKey].Add(hg);
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
                    // var hPieceKey = string.Format("{0}:{1}", hexMainTrackNum, hPieceNo);
                    if (!dicHangingPiece.ContainsKey(hexMainTrackNum + $"-{hPieceNo}") && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("35"))
                    {//首次制品上线-->挂片
                        if (!dicHangingPiece.ContainsKey(hexMainTrackNum + hPieceNo))
                        {
                            dicHangingPiece.Add(hexMainTrackNum + "-" + rMessage.ID, new List<Hanger>() {
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
                            dicHangingPiece[hexMainTrackNum + "-" + hPieceNo].Add(new Hanger()
                            {
                                ProductNumber = HexHelper.HexToTen(rMessage.DATA6).ToString(),
                                Index = dicHangingPiece.Count,
                                MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                Content = "制品发送挂片站确认"
                            });
                        }
                    }
                    else if (dicHangingPiece.ContainsKey(hexMainTrackNum + "-" + hPieceNo) && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("35"))
                    {//多次  制品上线-->挂片
                        if (!dicHangingPiece.ContainsKey(hexMainTrackNum + "-" + hPieceNo))
                        {
                            dicHangingPiece[hexMainTrackNum + "-" + rMessage.ID].Add(new Hanger()
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
                            dicHangingPiece[hexMainTrackNum + "-" + hPieceNo].Add(new Hanger()
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
                    else if (dicHangingPiece.ContainsKey(hexMainTrackNum + "-" + hPieceNo) && rMessage.CMD.Equals("05") && rMessage.ADDH.Equals("01"))
                    {

                    }
                    #endregion

                    #region 普通站
                    if (!statingNo.Equals(hPieceNo))
                    {
                        int tenMainTrackNumber = HexHelper.HexToTen(rMessage.XID);
                        int tenStatingNo = HexHelper.HexToTen(rMessage.ID);
                        SusNet.Services.Da.Domain.BridgeSet bridgeSet = null;
                        var isBridge = BusAction.Instance.IsBridgeStating(tenMainTrackNumber, tenStatingNo, ref bridgeSet);
                        var keys = string.Format("{0:D2}", tenMainTrackNumber) + "-" + string.Format("{0:D2}", tenStatingNo);
                        //站1
                        //给站点分配衣架
                        if (!dicCommonPiece.ContainsKey(keys) && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("51"))
                        {//首次制品上线-->挂片
                            if (!dicCommonPiece.ContainsKey(keys))
                            {
                                dicCommonPiece.Add(keys, new List<Hanger>() {
                                    new Hanger() {
                                        //ProductNumber=rMessage.DATA6,
                                        Index=dicCommonPiece.Count,
                                        MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                        StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                        HangerNo=StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2+rMessage.DATA3+rMessage.DATA4+rMessage.DATA5+rMessage.DATA6)).ToString()),
                                    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                                    }
                                });
                                //衣架进站回应
                                var data1 = string.Format("{0} {1} 02 FF 00 50 00 {2}", rMessage.XID, rMessage.ID, string.Format("{0}{1}{2}{3}{4}", rMessage.DATA2, rMessage.DATA3, rMessage.DATA4, rMessage.DATA5, rMessage.DATA6));
                                var dBytes1 = HexHelper.strToToHexByte(data1);
                                SendMessageToAll(dBytes1);


                                if (isBridge)
                                {
                                    #region bridge
                                    var bKey = string.Format("{0:D2}-", bridgeSet.BMainTrackNumber.Value) + string.Format("{0:D2}", bridgeSet.BSiteNo.Value);//"02" + "-" + "11";
                                    if (dicCommonPiece.ContainsKey(bKey))
                                    {
                                        dicCommonPiece[bKey].Add(new Hanger()
                                        {
                                            //  ProductNumber = rMessage.DATA6,
                                            Index = dicCommonPiece.Count,
                                            MainTrackNo = string.Format("{0:D2}", bridgeSet.BMainTrackNumber.Value),
                                            StatingNo = string.Format("{0:D2}", bridgeSet.BSiteNo.Value),
                                            Content = "普通站消息",
                                            HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                                        });
                                    }
                                    else
                                    {
                                        dicCommonPiece.Add(bKey, new List<Hanger>() {
                            new Hanger() {
                                //ProductNumber=rMessage.DATA6,
                                 Index = dicCommonPiece.Count,
                                            MainTrackNo = string.Format("{0:D2}", bridgeSet.BMainTrackNumber.Value),
                                            StatingNo = string.Format("{0:D2}", bridgeSet.BSiteNo.Value),
                                HangerNo=StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2+rMessage.DATA3+rMessage.DATA4+rMessage.DATA5+rMessage.DATA6)).ToString()),
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            }
                        });
                                    }
                                    #endregion
                                }
                                return;
                            }

                            dicCommonPiece[keys].Add(new Hanger()
                            {
                                //ProductNumber=rMessage.DATA6,
                                Index = dicHangingPiece.Count,
                                MainTrackNo = HexHelper.HexToTen(rMessage.XID).ToString(),
                                StatingNo = HexHelper.HexToTen(rMessage.ID).ToString(),
                                Content = "普通站消息",
                                HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                                DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            });
                            //衣架进站回应
                            var data = string.Format("{0} {1} 02 FF 00 50 00 {2}", rMessage.XID, rMessage.ID, string.Format("{0}{1}{2}{3}{4}", rMessage.DATA2, rMessage.DATA3, rMessage.DATA4, rMessage.DATA5, rMessage.DATA6));
                            var dBytes = HexHelper.strToToHexByte(data);
                            SendMessageToAll(dBytes);


                            if (isBridge)
                            {
                                #region bridge
                                var bKey = string.Format("{0:D2}-", bridgeSet.BMainTrackNumber.Value) + string.Format("{0:D2}", bridgeSet.BSiteNo.Value);//"02" + "-" + "11";
                                if (dicCommonPiece.ContainsKey(bKey))
                                {
                                    dicCommonPiece[bKey].Add(new Hanger()
                                    {
                                        //  ProductNumber = rMessage.DATA6,
                                        Index = dicCommonPiece.Count,
                                        MainTrackNo = string.Format("{0:D2}", bridgeSet.BMainTrackNumber.Value),
                                        StatingNo = string.Format("{0:D2}", bridgeSet.BSiteNo.Value),
                                        Content = "普通站消息",
                                        HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                                    });
                                }
                                else
                                {
                                    dicCommonPiece.Add(bKey, new List<Hanger>() {
                            new Hanger() {
                                //ProductNumber=rMessage.DATA6,
                                 Index = dicCommonPiece.Count,
                                            MainTrackNo = string.Format("{0:D2}", bridgeSet.BMainTrackNumber.Value),
                                            StatingNo = string.Format("{0:D2}", bridgeSet.BSiteNo.Value),
                                HangerNo=StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2+rMessage.DATA3+rMessage.DATA4+rMessage.DATA5+rMessage.DATA6)).ToString()),
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            }
                        });
                                }
                                #endregion
                            }

                            ////衣架工序对比
                            //var dataCompare = string.Format("{0} {1} 06 FF 00 54 00 {2}", rMessage.XID, rMessage.ID, string.Format("{0}{1}{2}{3}{4}", rMessage.DATA2, rMessage.DATA3, rMessage.DATA4, rMessage.DATA5, rMessage.DATA6));
                            //var dataCompareBytes = HexHelper.strToToHexByte(dataCompare);
                            //SendMessageToAll(dataCompareBytes);

                        }
                        //给站点分配衣架
                        else if (dicCommonPiece.ContainsKey(keys) && rMessage.CMD.Equals("03") && rMessage.ADDL.Equals("51"))
                        {//多次  制品上线-->挂片
                            if (!dicHangingPiece.ContainsKey(keys))
                            {
                                dicCommonPiece[keys].Add(new Hanger()
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
                                dicCommonPiece[keys].Add(new Hanger()
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

                            if (isBridge)
                            {
                                #region bridge
                                var bKey = string.Format("{0:D2}-", bridgeSet.BMainTrackNumber.Value) + string.Format("{0:D2}", bridgeSet.BSiteNo.Value);//"02" + "-" + "11";
                                if (dicCommonPiece.ContainsKey(bKey))
                                {
                                    dicCommonPiece[bKey].Add(new Hanger()
                                    {
                                        //  ProductNumber = rMessage.DATA6,
                                        Index = dicCommonPiece.Count,
                                        MainTrackNo = string.Format("{0:D2}", bridgeSet.BMainTrackNumber.Value),
                                        StatingNo = string.Format("{0:D2}", bridgeSet.BSiteNo.Value),
                                        Content = "普通站消息",
                                        HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                                    });
                                }
                                else
                                {
                                    dicCommonPiece.Add(bKey, new List<Hanger>() {
                            new Hanger() {
                                //ProductNumber=rMessage.DATA6,
                                 Index = dicCommonPiece.Count,
                                            MainTrackNo = string.Format("{0:D2}", bridgeSet.BMainTrackNumber.Value),
                                            StatingNo = string.Format("{0:D2}", bridgeSet.BSiteNo.Value),
                                HangerNo=StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2+rMessage.DATA3+rMessage.DATA4+rMessage.DATA5+rMessage.DATA6)).ToString()),
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                            }
                        });
                                }
                                #endregion
                            }
                        }

                        ////【桥接】 1--->2 分配到10时
                        //if (hexMainTrackNum.Equals("01") && statingNo.Equals(HexHelper.TenToHexString2Len(10)))
                        //{
                        //    if (dicCommonPiece.ContainsKey("02" + "-" + "11"))
                        //    {
                        //        dicCommonPiece["02" + "-" + "11"].Add(new Hanger()
                        //        {
                        //            //  ProductNumber = rMessage.DATA6,
                        //            Index = dicCommonPiece.Count,
                        //            MainTrackNo = "02",
                        //            StatingNo = "11",
                        //            Content = "普通站消息",
                        //            HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                        //            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                        //        });
                        //    }
                        //    else
                        //    {
                        //        dicCommonPiece.Add("02" + "-" + "11", new List<Hanger>() {
                        //    new Hanger() {
                        //        //ProductNumber=rMessage.DATA6,
                        //        Index=dicCommonPiece.Count,
                        //        MainTrackNo = "02",
                        //        StatingNo = "11",
                        //        HangerNo=StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2+rMessage.DATA3+rMessage.DATA4+rMessage.DATA5+rMessage.DATA6)).ToString()),
                        //    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                        //    }
                        //});
                        //    }
                        //}
                        ////【桥接】 2--->1 分配到11时
                        //if (hexMainTrackNum.Equals("02") && statingNo.Equals((HexHelper.TenToHexString2Len(11))))
                        //{
                        //    if (dicCommonPiece.ContainsKey("01" + "-" + "10"))
                        //    {
                        //        dicCommonPiece["01" + "-" + "10"].Add(new Hanger()
                        //        {
                        //            //  ProductNumber = rMessage.DATA6,
                        //            Index = dicCommonPiece.Count,
                        //            MainTrackNo = "01",
                        //            StatingNo = "10",
                        //            Content = "普通站消息",
                        //            HangerNo = StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2 + rMessage.DATA3 + rMessage.DATA4 + rMessage.DATA5 + rMessage.DATA6)).ToString()),
                        //            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                        //        });
                        //    }
                        //    else
                        //    {
                        //        dicCommonPiece.Add("01" + "-" + "10", new List<Hanger>() {
                        //    new Hanger() {
                        //        //ProductNumber=rMessage.DATA6,
                        //        Index=dicCommonPiece.Count,
                        //        MainTrackNo = "01",
                        //        StatingNo = "10",
                        //        HangerNo=StringUtils.ToFixLenStringFormat(HexHelper.HexToTen(string.Format(rMessage.DATA2+rMessage.DATA3+rMessage.DATA4+rMessage.DATA5+rMessage.DATA6)).ToString()),
                        //    DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm.sss")
                        //    }
                        //});
                        //    }
                        //}
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
                    #region//【挂片终端上线制单回显示】
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
                    #region//【挂片站上线】
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
                    #region//【衣架落入读卡器工序信息的比较信息推送】
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

                    #region //【协议2.0】 衣架缓存清除
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
                throw ex;
            }
        }

        public void InitProductsCMD()
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
        }

        /// <summary>
        /// 【协议2.0】排产号约定地址的首位地址
        /// </summary>
        public static Dictionary<int, string> ProductNumberHeadAddresList = new Dictionary<int, string>();

        public ListViewEx LvAllMessage { get; internal set; }

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
        public void SendMessageToAll(byte[] v)
        {
            //client.Send(v);
            try
            {
                //Message message = MessageFactory.Instance.CreateMessage(strMessage);
                // Log.Info("已经给所有终端发送消息:{0}",message.Describe());
                log.Info(string.Format("已经给所有终端发送消息:{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(v)));
                SendData(v);
            }
            catch (Exception ex)
            {
                log.FatalFormat("Class:{0}\nMethod:{1}\nException:{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

        public void ClientMancheOnLine(string mainTrackNo, string statingNo, string productNumber, string xor = null)
        {
            var message = new SusNet2.Common.SusBusMessage.ClientMachineRequestMessage(mainTrackNo, statingNo, "00", "35", productNumber, xor);
            log.Info(string.Format("【制品界面上线请求】发送开始,消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
            client.Send(message.GetBytes());
            log.Info(string.Format("【制品界面上线请求】发送完成，消息:--->{0}", SusNet2.Common.Utils.HexHelper.byteToHexStr(message.GetBytes())));
        }
    }
}
