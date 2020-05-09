using log4net;
using Newtonsoft.Json;
using Sus.Net.Client;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Event;
using Sus.Net.Common.Utils;
using SusNet2.Common.Utils;
using System;
using System.IO.Ports;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinFormListviewAddContrl;
using Zhuantou.Interface.Service.Remoting.Enum;
using Zhuantou.Interface.Service.Remoting.Type;
using System.Drawing;
using Sus.Net.TestWinServer;

namespace Sus.Net.TestWinClient
{
    public partial class FormClient : Form
    {
        #region 其他
        private ILog log = LogManager.GetLogger(typeof(FormClient));
        static SusTCPClient client;
        public FormClient()
        {
            InitializeComponent();
        }
        static int index = 1;
        void AddMessage(object sender, EventArgs e)
        {

            var data = string.Format("{0}--->{1}", index, sender.ToString());
            listBox1.Items.Add(data);
            SetStyle(listBox1);
            index++;
        }
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
        public void client_PlaintextReceived(object sender, MessageEventArgs e)
        {
            Console.WriteLine(string.Format("Server message: {0} --> ", e.message));
            this.Invoke(new EventHandler(this.AddMessage), e.message, null);
            //this.Invoke(new System.EventHandler(this.showForm),new object[] { null,null });
            //用模态弹窗必须开启新线程才不会阻塞消息，否则就用show弹窗

            //OrderMessage message = JsonConvert.DeserializeObject<OrderMessage>(e.message);

            //if ( message.type == (byte)OrderMessageType.OrderStatusChange )
            //{
            //    //订单状态变更
            //    NewThreadForm();
            //}
            //else if ( message.type == (byte)OrderMessageType.OrderWinTheBid )
            //{
            //    //中标
            //    Log.Info("订单:{0} 已中标，详情: {1}",message.orderId,message.data);
            //}

        }
        DialogForm f1 = null;
        public void showForm(object sender, EventArgs e)
        {
            if (null == f1)
            {
                f1 = new DialogForm();
                f1.TopMost = true;
                f1.ShowDialog();
            }
            else
            {
                if (f1.IsDisposed)
                {
                    f1 = new DialogForm();
                    f1.TopMost = true;
                    f1.ShowDialog();
                }
                else
                {
                    f1.Visible = true;
                    f1.TopMost = true;
                    f1.WindowState = FormWindowState.Maximized;
                }
            }
        }
        public void NewThreadForm()
        {
            Thread thread = new Thread(new ThreadStart(startForm));
            thread.Start();
        }

        private void startForm()
        {
            this.Invoke(new System.EventHandler(this.showForm), new object[] { null, null });
        }

        private void Send_Click(object sender, EventArgs e)
        {
        }
        bool isConnected = false;
        private void Connet_Click(object sender, EventArgs e)
        {
            try
            {
                ClientUserInfo info = new ClientUserInfo(textBox1.Text, txtMainTrackNumber.Text);
                client = new SusTCPClient(info, textBox3.Text.Trim(), int.Parse(txtPort.Text.Trim()));
                client.ServerConnected += Client_ServerConnected;
                client.ServerDisconnected += Client_ServerDisconnected;
                client.ServerExceptionOccurred += Client_ServerExceptionOccurred;
                client.MessageReceived += Client_MessageReceived; ;
                client.Connect();
                //client.EmergencyStopMainTrackResponseMessageReceived += Client_EmergencyStopMainTrackResponseMessageReceived;
                //client.StartMainTrackResponseMessageReceived += Client_StartMainTrackResponseMessageReceived;
                //client.StopMainTrackResponseMessageReceived += Client_StopMainTrackResponseMessageReceived;
                //client.ClientMachineResponseMessageReceived += Client_ClientMachineResponseMessageReceived;
                //client.HangingPieceStatingOnlineMessageReceived += Client_HangingPieceStatingOnlineMessageReceived;
                //client.HangerArrivalStatingMessageReceived += Client_HangerArrivalStatingMessageReceived;
                //client.HangerOutStatingRequestMessageReceived += Client_HangerOutStatingRequestMessageReceived;
                //client.AllocationHangerResponseMessageReceived += Client_AllocationHangerResponseMessageReceived;
                //client.HangerDropCardRequestMessageReceived += Client_HangerDropCardRequestMessageReceived;
                this.Connet.Enabled = false;
                this.Stop.Enabled = true;
                isConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            var susRemotingMessage = string.Format("【消息】{0}", e.message);
            this.Invoke(new EventHandler(this.AddMessage), susRemotingMessage, null);
            log.Info(susRemotingMessage);
        }

        private void Client_ServerExceptionOccurred(object sender, Client.Sockets.TcpServerExceptionOccurredEventArgs e)
        {
            var message = string.Format("服务端连接异常->异常:{0}", e.Exception?.Message);
            this.Invoke(new EventHandler(this.AddMessage), message, null);
            log.Info(message);
        }

        private void Client_ServerDisconnected(object sender, Client.Sockets.TcpServerDisconnectedEventArgs e)
        {
            var message = string.Format("服务端连接断开->地址:{0}", e.Addresses);
            this.Invoke(new EventHandler(this.AddMessage), message, null);
            log.Info(message);
        }

        private void Client_ServerConnected(object sender, Client.Sockets.TcpServerConnectedEventArgs e)
        {
                var message = string.Format("已连接到服务端->地址:{0}", e.Addresses);
                this.Invoke(new EventHandler(this.AddMessage), message, null);
            log.Info(message);
            InitBasic(null);
        }

        void InitBasic(string mainTrackNumber) {

            var _mainTrackNumber = int.Parse(txtMainTrackNumber.Text.Trim());
            if (_mainTrackNumber == 1) {
                var hexMessage1 = string.Format("{0} 00 06 FF 02 10 00 00 00 00 00 00", HexHelper.tenToHexString(int.Parse(txtMainTrackNumber.Text.Trim())));
                log.InfoFormat("初始化指令发送开始---->{0}", hexMessage1);
                var data1 = HexHelper.strToToHexByte(hexMessage1);
                SusNet2.Common.Message.MessageBody message1 = new SusNet2.Common.Message.MessageBody(data1);
                client.SendData(message1.GetBytes());
                log.InfoFormat("初始化指令发送完成---->{0}", hexMessage1);
                this.Invoke(new EventHandler(this.AddMessage), string.Format("初始化指令发送成功,发送内容：{0}", hexMessage1), null);


                var hexMessage3 = string.Format("{0} 00 06 FF 02 10 00 00 00 00 00 00", HexHelper.tenToHexString("3"));
                log.InfoFormat("初始化指令发送开始---->{0}", hexMessage3);
                var data3 = HexHelper.strToToHexByte(hexMessage3);
                SusNet2.Common.Message.MessageBody message3 = new SusNet2.Common.Message.MessageBody(data3);
                client.SendData(message3.GetBytes());
                log.InfoFormat("初始化指令发送完成---->{0}", hexMessage3);
                this.Invoke(new EventHandler(this.AddMessage), string.Format("初始化指令发送成功,发送内容：{0}", hexMessage3), null);

                return;
            }
            var hexMessage = string.Format("{0} 00 06 FF 02 10 00 00 00 00 00 00", HexHelper.tenToHexString(int.Parse(txtMainTrackNumber.Text.Trim())));
            log.InfoFormat("初始化指令发送开始---->{0}", hexMessage);
            var data = HexHelper.strToToHexByte(hexMessage);
            SusNet2.Common.Message.MessageBody message = new SusNet2.Common.Message.MessageBody(data);
            client.SendData(message.GetBytes());
            log.InfoFormat("初始化指令发送完成---->{0}", hexMessage);
            this.Invoke(new EventHandler(this.AddMessage), string.Format("初始化指令发送成功,发送内容：{0}", hexMessage), null);
        }

        ////衣架落入读卡器硬件--->pc 工序比较
        //private void Client_HangerDropCardRequestMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 /衣架落入读卡器硬件--->pc 工序比较【{1}】", "Client_AllocationHangerResponseMessageReceived", e.message));
        //    //相同工序
        //    client.HangerDropCardProcessFlowCompare("01", "02", "AA BB CC DD EE", 0);

        //    try
        //    {
        //        var data = string.Format("933304-9BUY1,012,2XL,1件,本站工序29,西服XXXXX");
        //        // var hexData = HexHelper.ToHex(data, "utf-8", false);
        //        var hexBytes = UnicodeUtils.GetBytesByUnicode(data);// HexHelper.strToToHexByte(hexData);

        //        string mainTrackNo = "88";
        //        string statingNo = "79";
        //        log.Info(string.Format("【衣架落入读卡器硬件--->pc 工序比较】 将要发送的内容--->【{0}】", data));
        //        Console.WriteLine(string.Format("【衣架落入读卡器硬件--->pc 工序比较】 将要发送的内容 pc--->硬件【{0}】", data));
        //        client.SendDataByHangerDropCardCompare(new System.Collections.Generic.List<byte>(hexBytes), mainTrackNo, statingNo, null);

        //        log.Info(string.Format("【衣架落入读卡器硬件--->pc 工序比较】 发送完成 pc--->硬件【{0}】", data));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        ////分配下一工序站点硬件回应
        //private void Client_AllocationHangerResponseMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 分配下一工序站点硬件回应-->【{1}】", "Client_AllocationHangerResponseMessageReceived", e.message));
        //    client.AutoHangerOutStating("01", "02", true, "AA BB CC DD EE");
        //}

        //private void Client_HangerOutStatingRequestMessageReceived(object sender, MessageEventArgs e)
        //{
        //    /*
        //     * "01 02 05 XX 00 55 00 AA BB CC DD EE
        //        01 02 05 XX 00 55 01 AA BB CC DD EE"
        //     */
        //    Console.WriteLine(string.Format("【{0}】 收到消息-->【{1}】", "Client_HangerOutStatingRequestMessageReceived", e.message));
        //    client.AllocationHangerToNextStating("01", "02", "AA BB CC DD EE");

        //}

        //private void Client_HangerArrivalStatingMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 收到消息-->【{1}】", "Client_HangerArrivalStatingMessageReceived", e.message));
        //    client.HangerArrivalStating("01", "04", "AA BB CC DD EE");
        //}

        //private void Client_HangingPieceStatingOnlineMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 收到消息-->【{1}】", "Client_HangingPieceStatingOnlineMessageReceived", e.message));
        //    client.HangingPieceOnLine("01", "01", "05");

        //    //挂片站选择制品上线后，上线制品信息发送到挂片站推送
        //    try
        //    {
        //        var data = string.Format("933304-9BUY1,012,27,任务1865件,单位1件,累计出11173件,今日出2113件");
        //        //var hexData = HexHelper.ToHex(data, "utf-8", false);
        //        var hexBytes = UnicodeUtils.GetBytesByUnicode(data);//HexHelper.strToToHexByte(hexData);

        //        string mainTrackNo = "88";
        //        string statingNo = "78";
        //        log.Info(string.Format("【挂片站上线】 将要发送的内容--->【{0}】", data));
        //        Console.WriteLine(string.Format("【挂片站上线】 将要发送的内容 pc--->硬件【{0}】", data));
        //        client.SendDataByHangingPieceOnline(new System.Collections.Generic.List<byte>(hexBytes), mainTrackNo, statingNo, null);

        //        log.Info(string.Format("【挂片站上线】 发送完成 pc--->硬件【{0}】", data));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        //private void Client_ClientMachineResponseMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 收到消息-->【{1}】", "Client_ClientMachineResponseMessageReceived", e.message));

        //    //制品界面上线后，上线制品信息给挂片站推送
        //    try
        //    {
        //        var data = string.Format("933304-9BUY,010,28,任务1863件,单位1件,累计出1117件,今日出213件");
        //        //   var hexData = HexHelper.ToHex(data, "utf-8", false);
        //        var hexBytes = UnicodeUtils.GetBytesByUnicode(data);//HexHelper.strToToHexByte(hexData);

        //        string mainTrackNo = "88";
        //        string statingNo = "77";
        //        log.Info(string.Format("【制品界面直接上线】 将要发送的内容--->【{0}】", data));
        //        Console.WriteLine(string.Format("【制品界面直接上线】 将要发送的内容 pc--->硬件【{0}】", data));
        //        client.SendDataByProductsDirectOnline(new System.Collections.Generic.List<byte>(hexBytes), mainTrackNo, statingNo, null);

        //        log.Info(string.Format("【制品界面直接上线】 发送完成 pc--->硬件【{0}】", data));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        //private void Client_StopMainTrackResponseMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 收到消息-->【{1}】", "Client_StopMainTrackResponseMessageReceived", e.message));
        //}

        //private void Client_StartMainTrackResponseMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 收到消息-->【{1}】", "Client_StartMainTrackResponseMessageReceived", e.message));
        //}

        //private void Client_EmergencyStopMainTrackResponseMessageReceived(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine(string.Format("【{0}】 收到消息-->【{1}】", "Client_EmergencyStopMainTrackResponseMessageReceived", e.message));
        //}

        private void Stop_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.Disconnect();
                client = null;
                this.Connet.Enabled = true;
                this.Stop.Enabled = false;
                isConnected = false;
            }
        }
        //启动主轨
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("未连接服务器，请先连接服务器!");
                return;
            }
            try
            {
              client.StartMainTrack("01");
                MessageBox.Show("操作完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //停止主轨
        private void button2_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("未连接服务器，请先连接服务器!");
                return;
            }
            try
            {
             client.StopMainTrack("01");
                MessageBox.Show("操作完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //急停主轨
        private void button3_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("未连接服务器，请先连接服务器!");
                return;
            }
            try
            {
              client.EmergencyStopMainTrack("01");
                MessageBox.Show("操作完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //制品界面挂片
        private void button5_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("未连接服务器，请先连接服务器!");
                return;
            }
            //var data = string.Format("1.92085,100,S");
            //var hexData = SusNet2.Common.Utils.HexHelper.ToHex(data, "utf-8", false);
            //Console.WriteLine("16进制字符串----->" + hexData);
            //Console.WriteLine("中文----->" + HexHelper.UnHex(hexData, "utf-8"));

            //var hexBytes = HexHelper.strToToHexByte(hexData);
            //var length = hexBytes.Length;
            //Console.WriteLine("字节长度--->" + length);
            //Console.WriteLine("中文----->" + HexHelper.UnHex(HexHelper.byteToHexStr(new byte[] { hexBytes[length - 2], hexBytes[length - 1] }), "utf-8"));
            try
            {
                var data = string.Format("1.92085,100,S");
                // var hexData = HexHelper.ToHex(data, "utf-8", false);
                var hexBytes = UnicodeUtils.GetBytesByUnicode(data);//HexHelper.strToToHexByte(hexData);
                //client.BindProudctsToHangingPiece(new System.Collections.Generic.List<byte>(hexBytes), "88", "77", 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //制品界面上线
        private void button4_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("未连接服务器，请先连接服务器!");
                return;
            }
            try
            {
                client.ClientMancheOnLine("01", "02", "05");
                MessageBox.Show("操作完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            index = 1;

        }
        private void button7_Click(object sender, EventArgs e)
        {
            //button6.Enabled = false;
            //new Thread(new ThreadStart(SendData)).Start();
            var t = new FormClientTest();
            t.ShowDialog();
        }
        void SendData()
        {
            while (true)
            {
                var data = HexHelper.strToToHexByte("01 00 03 FF 00 06 00 00 00 00 00 10");
                client?.SendData(data);
                listBox1?.Invoke(new EventHandler(this.AddMessage), data, null);
            }
        }

        private void btnSerialPort_Click(object sender, EventArgs e)
        {
            try
            {
                var searcher =
                    new ManagementObjectSearcher("root\\WMI",
                    "SELECT * FROM MSSerial_PortName");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["InstanceName"].ToString().Contains("FTDIBUS"))
                    {
                        //Console.WriteLine(queryObj["PortName"] + " is a FDTIBUS device."
                        listBox1?.Invoke(new EventHandler(this.AddMessage), queryObj["PortName"], null);

                    }
                }
                if (searcher.Get().Count == 0)
                {
                    MessageBox.Show("无设备！");
                }
                //Console.Read();
                MessageBox.Show("搜索完毕！");
            }
            catch (ManagementException ex)
            {
                //System.Diagnostics.Debug.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                listBox1?.Invoke(new EventHandler(this.AddMessage), ex.Message, null);
            }

        }
        // -------------------- 定义串口对象及委托事件 --------------------
        private SerialPort Com_SerialPort = new SerialPort();
        private delegate void UpdateTextEventHandler(string data);
       // private UpdateTextEventHandler updateText;
        private void btnSerialPortTest_Click(object sender, EventArgs e)
        {

            //Com_SerialPort.PortName = "COM1";   // 使用哪个串口
            //Com_SerialPort.BaudRate = 9600; // 波特率
            //Com_SerialPort.DataBits = 7;        // 数据位
            //Com_SerialPort.Parity = Parity.None;    // 校验位
            //Com_SerialPort.StopBits = StopBits.None;// 停止位
            //Com_SerialPort.Open();
            //Com_SerialPort.ReceivedBytesThreshold = 1;
            //Com_SerialPort.DataReceived += new SerialDataReceivedEventHandler(Com_SerialPort_DataReceive);
            var canDeviceSearch = new frmCANDeviceSearch(txtUDPServerIP.Text, int.Parse(txtUDPPort.Text));
            canDeviceSearch.Show();
        }

        private void Com_SerialPort_DataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(20); // 时间短可能导致数据读取不完整
            string data = Com_SerialPort.ReadExisting();
            //this.Invoke(updateText, new string[] { data });
            listBox1?.Invoke(new EventHandler(this.AddMessage), data, null);
        }

        #endregion

        #region udp相关
        private void FormClient_Load(object sender, EventArgs e)
        {
            //try
            //{

            //    // 注册委托事件，核心
            //    //updateText = new UpdateTextEventHandler(UpdateTextBox);
            //    UdpInit();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void UdpInit()
        {
            var thread = new Thread(UDPInit);
            thread.Start();
        }
        static Socket server = null;
        private void UDPInit()
        {
            if (server == null)
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                server.Bind(new IPEndPoint(IPAddress.Parse(txtUDPServerIP.Text), int.Parse(txtUDPPort.Text)));//"255.255.255.255"), 1901));//绑定端口号和IP
            }
            var data = string.Format(" udp server 已开启");
            log.Info(data);
            listBox1?.Invoke(new EventHandler(this.AddMessage), data, null);

            var threadSocket = new Thread(UDPServerReciveMessage);
            threadSocket.Start();

            log.Info("消息接收线程已开启");

            var threadSendSocket = new Thread(UDPServerSendMessage);
            threadSendSocket.Start();

            log.Info("消息发送线程已开启");
        }

        private void UDPServerSendMessage()
        {
            //EndPoint point = new IPEndPoint(IPAddress.Parse("1255.255.255.255"), 1901);
            //while (true)
            //{
            //    string msg = Console.ReadLine();
            //    server.SendTo(Encoding.UTF8.GetBytes(msg), point);
            //}
        }

        private void UDPServerReciveMessage()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine(point.ToString() + message);

                listBox1?.Invoke(new EventHandler(this.AddMessage), "收到-->"+point.ToString() + message, null);
            }
        }

        private void UpdateTextBox(string data)
        {

        }

        #endregion

        private void btnUDPStart_Click(object sender, EventArgs e)
        {
            try
            {

                // 注册委托事件，核心
                //updateText = new UpdateTextEventHandler(UpdateTextBox);
                UdpInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSendToMainTrackNumber1_Click(object sender, EventArgs e)
        {

        }

        private void btnSendToMainTrackNumber2_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var frmClientNew = new FormClientNew();
            frmClientNew.Show();
        }
    }
}
