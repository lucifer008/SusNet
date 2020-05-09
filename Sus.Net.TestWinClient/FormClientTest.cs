using log4net;
using Newtonsoft.Json;
using Sus.Net.Client;
using Sus.Net.Common.Entity;
using Sus.Net.Common.Event;
using Sus.Net.Common.Utils;
using SusNet2.Common.Utils;
using System;
using System.Threading;
using System.Windows.Forms;
using Zhuantou.Interface.Service.Remoting.Enum;
using Zhuantou.Interface.Service.Remoting.Type;

namespace Sus.Net.TestWinClient
{
    public partial class FormClientTest : Form
    {
        private ILog log = LogManager.GetLogger(typeof(FormClient));
        static SusTCPClient client;
        public FormClientTest()
        {
            InitializeComponent();
        }
        static int index = 1;
        void AddMessage(object sender, EventArgs e)
        {

            var data = string.Format("{0}--->{1}", index, sender.ToString());
            listBox1.Items.Add(data);
            index++;
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
                //ClientUserInfo info = new ClientUserInfo(textBox1.Text, textBox2.Text);
                //client = new SusTCPClient(info, textBox3.Text.Trim(), int.Parse(txtPort.Text.Trim()));
                //client.MessageReceived += client_PlaintextReceived;
                //client.Connect();
                //client.EmergencyStopMainTrackResponseMessageReceived += Client_EmergencyStopMainTrackResponseMessageReceived;
                //client.StartMainTrackResponseMessageReceived += Client_StartMainTrackResponseMessageReceived;
                //client.StopMainTrackResponseMessageReceived += Client_StopMainTrackResponseMessageReceived;
                //client.ClientMachineResponseMessageReceived += Client_ClientMachineResponseMessageReceived;
                //client.HangingPieceStatingOnlineMessageReceived += Client_HangingPieceStatingOnlineMessageReceived;
                //client.HangerArrivalStatingMessageReceived += Client_HangerArrivalStatingMessageReceived;
                //client.HangerOutStatingRequestMessageReceived += Client_HangerOutStatingRequestMessageReceived;
                //client.AllocationHangerResponseMessageReceived += Client_AllocationHangerResponseMessageReceived;
                //client.HangerDropCardRequestMessageReceived += Client_HangerDropCardRequestMessageReceived;
                //client.ServerConnected += Client_ServerConnected;
                //client.ServerDisconnected += Client_ServerDisconnected;

                //this.Connet.Enabled = false;
                //this.Stop.Enabled = true;
                //isConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Client_ServerDisconnected(object sender, Client.Sockets.TcpServerDisconnectedEventArgs e)
        {
            listBox1?.Invoke(new EventHandler(this.AddMessage), string.Format("与Server端连接已断开！---时间:【{0}】", DateTime.Now.ToString("yy-MM-dd HH:mm:ss ffff")), null);
        }

        private void Client_ServerConnected(object sender, Client.Sockets.TcpServerConnectedEventArgs e)
        {
            listBox1?.Invoke(new EventHandler(this.AddMessage), string.Format("已连接到Server端！---时间:【{0}】", DateTime.Now.ToString("yy-MM-dd HH:mm:ss ffff")), null);
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
        //       // var hexData = HexHelper.ToHex(data, "utf-8", false);
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
        //     //   var hexData = HexHelper.ToHex(data, "utf-8", false);
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
        ////启动主轨
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (!isConnected)
        //    {
        //        MessageBox.Show("未连接服务器，请先连接服务器!");
        //        return;
        //    }
        //    try
        //    {
        //        client.StartMainTrack("01");
        //        MessageBox.Show("操作完成!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        ////停止主轨
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (!isConnected)
        //    {
        //        MessageBox.Show("未连接服务器，请先连接服务器!");
        //        return;
        //    }
        //    try
        //    {
        //        client.StopMainTrack("01");
        //        MessageBox.Show("操作完成!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        ////急停主轨
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    if (!isConnected)
        //    {
        //        MessageBox.Show("未连接服务器，请先连接服务器!");
        //        return;
        //    }
        //    try
        //    {
        //        client.EmergencyStopMainTrack("01");
        //        MessageBox.Show("操作完成!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //制品界面挂片
        //private void button5_Click(object sender, EventArgs e)
        //{
        //    if (!isConnected)
        //    {
        //        MessageBox.Show("未连接服务器，请先连接服务器!");
        //        return;
        //    }
        //    //var data = string.Format("1.92085,100,S");
        //    //var hexData = SusNet2.Common.Utils.HexHelper.ToHex(data, "utf-8", false);
        //    //Console.WriteLine("16进制字符串----->" + hexData);
        //    //Console.WriteLine("中文----->" + HexHelper.UnHex(hexData, "utf-8"));

        //    //var hexBytes = HexHelper.strToToHexByte(hexData);
        //    //var length = hexBytes.Length;
        //    //Console.WriteLine("字节长度--->" + length);
        //    //Console.WriteLine("中文----->" + HexHelper.UnHex(HexHelper.byteToHexStr(new byte[] { hexBytes[length - 2], hexBytes[length - 1] }), "utf-8"));
        //    try
        //    {
        //        var data = string.Format("1.92085,100,S");
        //       // var hexData = HexHelper.ToHex(data, "utf-8", false);
        //        var hexBytes = UnicodeUtils.GetBytesByUnicode(data);//HexHelper.strToToHexByte(hexData);
        //        client.BindProudctsToHangingPiece(new System.Collections.Generic.List<byte>(hexBytes), "88", "77", 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        ////制品界面上线
        //private void button4_Click(object sender, EventArgs e)
        //{
        //    if (!isConnected)
        //    {
        //        MessageBox.Show("未连接服务器，请先连接服务器!");
        //        return;
        //    }
        //    try
        //    {
        //        client.ClientMancheOnLine("01", "02", "05");
        //        MessageBox.Show("操作完成!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        //private void button6_Click(object sender, EventArgs e)
        //{

        //}

        private void button6_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            index = 1;

        }
        private void button7_Click(object sender, EventArgs e)
        {
            //button6.Enabled = false;
            if (null == client) {
                MessageBox.Show("未连接服务器，请先连接服务器!");
                return;
            }
            StartSendData();
        }
        void StartSendData() {
            var sp = new SendOption() {Frequency=int.Parse(txtFrequency.Text),SendContext=txtSendContent.Text,IsAutoFrequency= rbHave.Checked };
            new Thread(SendData).Start(sp);
        }
        void SendData(object data2)
        {
            var isFrequency = (data2 as SendOption).IsAutoFrequency;
            var frequencyNum = (data2 as SendOption).Frequency;
            var sContent = (data2 as SendOption).SendContext;
            while (true) {
                if (isFrequency) {
                    Thread.Sleep(frequencyNum*1000);
                }
                var data = HexHelper.strToToHexByte(sContent);
                
                //listBox1?.Invoke(new EventHandler(this.AddMessage), string.Format("发送开始---时间:【{0}】 内容:【{1}】",DateTime.Now.ToString("yy-MM-dd HH:mm:ss ffff"), sContent), null);
                if (null== client) {
                    listBox1?.Invoke(new EventHandler(this.AddMessage), string.Format("客户端已经端口连接---时间:【{0}】", DateTime.Now.ToString("yy-MM-dd HH:mm:ss")), null);
                    Thread.CurrentThread.Abort();
                    return;
                }
                var beginDate = DateTime.Now;
                client.SendData(data);
                var endDateTime = DateTime.Now;
                listBox1?.Invoke(new EventHandler(this.AddMessage), string.Format("发送完成---内容:【{0}】 --->开始发送时间:【{1}】 结束发送时间:{2} ", sContent, beginDate.ToString("yy-MM-dd HH:mm:ss ffff"), endDateTime.ToString("yy-MM-dd HH:mm:ss ffff")), null);
            }
        }

        private void FormClientTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
    class SendOption {
        public int Frequency { set; get; }
        public String SendContext { set; get; }
        public bool IsAutoFrequency { set; get; }
    }
}
