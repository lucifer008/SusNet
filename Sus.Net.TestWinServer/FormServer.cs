using log4net;
using Newtonsoft.Json;
using Sus.Net.Common.Message;
using Sus.Net.Server;

using SusNet2.Common.Model;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WinFormListviewAddContrl;
using Zhuantou.Interface.Service.Remoting.Enum;
using Zhuantou.Interface.Service.Remoting.Type;

namespace Sus.Net.TestWinServer
{
    public partial class frmServer : Form
    {
        const string ADDRESS_Client_Manche_Online = "0035";//客户机直接上线
        const string ADDRESS_Client_Manche_Online_Return_Display = "005A";//客户机直接上线回显给挂片站
        const string CMD_Client_Manche_Online_Return_Display = "05";//客户机直接上线回显给挂片站CMD
        const string ADDRESS_Hanger_OutSite = "0055";//衣架出站
        const string ADDRESS_Site_Allocation_Hanger = "0051";//给站点分配衣架
        const string ADDRESS_Hanger_InSite = "0050";//衣架进站
        const string ADDRESS_Flow_Compare = "0054";

        const string ADDRESS_PRODUCT_SEND = "005b";//产量推送
        const string cmd_PRODUCT_SEND = "05";//产量推送

        const int DEC_Compare_Begin = 160;//0160
        const int DEC_Compare_End = 383;//017F
        private readonly static ILog logInfo = LogManager.GetLogger("TcpLogInfo");
        private readonly static ILog logError = LogManager.GetLogger("TcpErrorInfo");
        private SusTCPServer server;

        public frmServer()
        {
            InitializeComponent();

        }
        private static List<Hanger> hangerList = new List<Hanger>();
        private void Server_MessageReceived(object sender, Common.Event.MessageEventArgs e)
        {
            var message = e.Tag as SusNet2.Common.Message.MessageBody;
            var address = string.Format("{0}{1}", message?.ADDH, message?.ADDL);
            var cmd = message?.CMD;
            var dataFirst = string.Format(message.DATA1);
            var decAddress = HexHelper.HexToTen(address);
            try
            {
                switch (address)
                {
                    case ADDRESS_Client_Manche_Online://0035 客户机直接上线回应/挂片站设置上线pc回应
                        if (cmd.Equals("03"))
                        {
                            var model = new MessageModel()
                            {
                                lvMessage = lvClientMancheDireOnline,
                                Message = string.Format("收到客户机上线指令:【{0}】 时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                            };
                            this.Invoke(new EventHandler(this.AddMessage), model, null);
                            var cmRsMessage = SusNet2.Common.SusBusMessage.ClientMachineRequestMessage.isEqual(message.GetBytes());
                            if (null != cmRsMessage)
                            {
                                logInfo.Info(string.Format("【制品界面上线来自硬件的响应】服务器端收到【客户端】消息:{0} 时间:{1}", message.GetHexStr(), DateTime.Now.ToString(dateFormat)));
                                cmRsMessage.CMD = "04";
                                var startmtResponseMessage = cmRsMessage.ToString(); //"01 01 04 FF 00 35 00 00 00 00 00 05";
                                logInfo.Info(string.Format("【制品界面上线来自硬件的响应】---->服务器端发送开始--->客户端消息为:【{0}】 时间:{1}", startmtResponseMessage, DateTime.Now.ToString(dateFormat)));
                                server.SendMessageToAll(cmRsMessage.GetBytes());
                                logInfo.Info(string.Format("【制品界面上线来自硬件的响应】---->服务器端发送完成--->客户端消息为:【{0}】 时间:{1}", startmtResponseMessage, DateTime.Now.ToString(dateFormat)));

                            }
                            model.Message = string.Format("硬件回应完毕! 回应消息为:{0} 时间:{1}", cmRsMessage.GetHexStr(), DateTime.Now.ToString(dateFormat));
                            this.Invoke(new EventHandler(this.AddMessage), model, null);
                        }
                        //消息发起来自pc
                        if (cmd.Equals("05"))
                        {
                            var model = new MessageModel()
                            {
                                lvMessage = lvHangPieceOnline,
                                Message = string.Format("收到客户机对挂片站上线的回应指令:【{0}】 时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                            };
                            this.Invoke(new EventHandler(this.AddMessage), model, null);

                        }
                        break;

                    case ADDRESS_Client_Manche_Online_Return_Display://客户机直接上线回显给挂片站
                        var modelDis = new MessageModel()
                        {
                            lvMessage = lvClientMancheDireOnline,
                            Message = string.Format("上线制品信息:(十六进制):{0}  时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                        };
                        this.Invoke(new EventHandler(this.AddMessage), modelDis, null);
                        if (cmd.Equals(CMD_Client_Manche_Online_Return_Display))
                        {
                            var mBytes = message.GetBytes();
                            var taskList = new List<byte>();
                            var totalList = new List<byte>();
                            var totalNumList = new List<byte>();
                            if (mBytes[6] != 0)
                            {
                                taskList.Add(mBytes[6]);
                            }
                            if (mBytes[7] != 0)
                            {
                                taskList.Add(mBytes[7]);
                            }

                            if (mBytes[8] != 0)
                            {
                                totalList.Add(mBytes[8]);
                            }
                            if (mBytes[9] != 0)
                            {
                                totalList.Add(mBytes[9]);
                            }
                            if (mBytes[10] != 0)
                            {
                                totalNumList.Add(mBytes[10]);
                            }
                            if (mBytes[11] != 0)
                            {
                                totalNumList.Add(mBytes[11]);
                            }

                            var taskNum = taskList.Count == 0 ? 0 : HexHelper.HexToTen(HexHelper.byteToHexStr(taskList.ToArray()));
                            var totalNum = totalList.Count == 0 ? 0 : HexHelper.HexToTen(HexHelper.byteToHexStr(totalList.ToArray()));
                            var todayNum = totalNumList.Count == 0 ? 0 : HexHelper.HexToTen(HexHelper.byteToHexStr(totalNumList.ToArray()));
                            var direOnlineTaskInfo = string.Format("任务数:{0} 累计数:{1} 本日数:{2} 时间:{3}", taskNum, totalNum, todayNum, DateTime.Now.ToString(dateFormat));
                            modelDis.Message = direOnlineTaskInfo;
                            this.Invoke(new EventHandler(this.AddMessage), modelDis, null);
                        }
                        break;
                    case ADDRESS_Site_Allocation_Hanger://站点衣架分配
                        var modelAllo = new MessageModel()
                        {
                            lvMessage = lvClientMancheDireOnline,
                            Message = string.Format("收到衣架分配指令:【{0}】 时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                        };
                        this.Invoke(new EventHandler(this.AddMessage), modelAllo, null);

                        //给pc推送写入成功消息
                        var allHangerMessage = SusNet2.Common.SusBusMessage.AllocationHangerRequestMessage.isEqual(message.GetBytes());
                        if (null != allHangerMessage)
                        {
                            logInfo.Info(string.Format("【分配工序到衣架成功回应】服务器端收到【客户端】消息:{0} 时间:{1}", message.GetHexStr(), DateTime.Now.ToString(dateFormat)));

                            var startmtResponseMessage = string.Format("{0} {1} 04 FF 00 51 00 {2}", allHangerMessage.XID, allHangerMessage.ID, allHangerMessage.HangerNo);//AA BB CC DD EE";
                            logInfo.Info(string.Format("【分配工序到衣架成功回应】---->服务器端发送开始--->客户端消息为:【{0}】 时间:{1}", startmtResponseMessage, DateTime.Now.ToString(dateFormat)));
                            server.SendMessageToAll(HexHelper.strToToHexByte(startmtResponseMessage));
                            logInfo.Info(string.Format("【分配工序到衣架成功回应】---->服务器端发送完成--->客户端消息为:【{0}】 时间:{1}", startmtResponseMessage, DateTime.Now.ToString(dateFormat)));
                        }
                        modelAllo.Message = string.Format("【给下一站点:{0} 分配成功!】硬件回应完毕! 回应消息为:{1} 时间:{2}", allHangerMessage.ID, allHangerMessage.GetHexStr(), DateTime.Now.ToString(dateFormat));
                        this.Invoke(new EventHandler(this.AddMessage), modelAllo, null);
                        break;
                    case ADDRESS_Hanger_OutSite://出站
                        var modelOutSite = new MessageModel()
                        {
                            lvMessage = lvClientMancheDireOnline,
                            Message = string.Format("收到衣架出站指令：【{0}】 时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                        };
                        this.Invoke(new EventHandler(this.AddMessage), modelOutSite, null);
                        var hSOutangerMessage = SusNet2.Common.SusBusMessage.HangerOutStatingResponseMessage.isEqual(message.GetBytes());
                        if (null != hSOutangerMessage)
                        {
                            logInfo.Info(string.Format("【衣架出站状态】服务器端收到【客户端】消息:{0} 时间:{1}", hSOutangerMessage.GetHexStr(), DateTime.Now.ToString(dateFormat)));
                        }
                        break;
                    case ADDRESS_Hanger_InSite://衣架进站
                        break;
                    case ADDRESS_Flow_Compare://工序比较
                        var flowMessgae = "工序相同";
                        if (dataFirst.Equals("01"))
                        {
                            flowMessgae = "工序不同";
                        }
                        var modelFlowCompare = new MessageModel()
                        {
                            lvMessage = lvClientMancheDireOnline,
                            Message = string.Format("收到工序比较指令，【{0}】,指令内容：【{1}】 时间:{2}", flowMessgae, e.message, DateTime.Now.ToString(dateFormat))
                        };
                        this.Invoke(new EventHandler(this.AddMessage), modelFlowCompare, null);
                        break;
                    case ADDRESS_PRODUCT_SEND:
                        var modelOutDis = new MessageModel()
                        {
                            lvMessage = lvClientMancheDireOnline,
                            Message = string.Format("出站制品信息:(十六进制):{0}  时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                        };
                        this.Invoke(new EventHandler(this.AddMessage), modelOutDis, null);
                        if (cmd.Equals(cmd_PRODUCT_SEND))
                        {
                            var mBytes = message.GetBytes();
                            var todayNumList = new List<byte>();
                            var currentRateList = new List<byte>();
                            var currentFlowUseTimeList = new List<byte>();
                            if (mBytes[6] != 0)
                            {
                                todayNumList.Add(mBytes[6]);
                            }
                            if (mBytes[7] != 0)
                            {
                                todayNumList.Add(mBytes[7]);
                            }

                            if (mBytes[8] != 0)
                            {
                                currentRateList.Add(mBytes[8]);
                            }
                            if (mBytes[9] != 0)
                            {
                                currentRateList.Add(mBytes[9]);
                            }
                            if (mBytes[10] != 0)
                            {
                                currentFlowUseTimeList.Add(mBytes[10]);
                            }
                            if (mBytes[11] != 0)
                            {
                                currentFlowUseTimeList.Add(mBytes[11]);
                            }

                            var taskNum = todayNumList.Count == 0 ? 0 : HexHelper.HexToTen(HexHelper.byteToHexStr(todayNumList.ToArray()));
                            var rate = currentRateList.Count == 0 ? 0 : decimal.Parse(HexHelper.HexToTen(HexHelper.byteToHexStr(currentRateList.ToArray())).ToString()) / 10000;
                            var currentTimes = currentFlowUseTimeList.Count == 0 ? 0 : HexHelper.HexToTen(HexHelper.byteToHexStr(currentFlowUseTimeList.ToArray()));
                            var direOnlineTaskInfo = string.Format("今日数:{0} 效率:{1} 本次工序时间:{2} 时间:{3}", taskNum, rate, currentTimes, DateTime.Now.ToString(dateFormat));
                            modelOutDis.Message = direOnlineTaskInfo;
                            this.Invoke(new EventHandler(this.AddMessage), modelOutDis, null);
                        }
                        break;
                    default:
                        //工序比较回显
                        if (decAddress >= DEC_Compare_Begin && decAddress <= DEC_Compare_End)
                        {
                            var modelFlowCompareContent = new MessageModel()
                            {
                                lvMessage = lvClientMancheDireOnline,
                                Message = string.Format("收到工序比较内容指令，指令内容：【{0}】 时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                            };
                            this.Invoke(new EventHandler(this.AddMessage), modelFlowCompareContent, null);
                        }
                        break;
                }
                Console.WriteLine(string.Format("Server message: {0} --> ", e.message));
            }
            finally
            {
                //过滤心跳包
                var heartbeatMessage = "00 00 00 00 00 00 00 00 00 00 00 00";
                var lv = e.message.Equals(heartbeatMessage) ? lvHeartbeat : LvAllMessage;
                var model = new MessageModel()
                {
                    lvMessage = lv,
                    Message = string.Format("{0} 时间:{1}", e.message, DateTime.Now.ToString(dateFormat))
                };
                this.Invoke(new EventHandler(this.AddMessage), model, null);
            }
        }
        const string dateFormat = "dd HH:mm:ss:ffff";
        static int index = 1;
        public void AddMessage(object sender, EventArgs e)
        {
            try
            {
                var m = sender as MessageModel;

                var data = string.Format("{0}--->{1}", index, m.Message);
                m.lvMessage = m.lvMessage == null ? lbAllMessage : m.lvMessage;
                m.lvMessage.Items.Add(data);
                SetStyle(m.lvMessage);
                index++;
            }
            catch (Exception ex)
            {
                logInfo.ErrorFormat("AddMessage--->", ex);
            }
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
        bool isConnected = false;
        //启动
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPort.Text.Trim()))
                {
                    MessageBox.Show("端口不能为空!");
                    return;
                }
                server = new SusTCPServer(int.Parse(txtPort.Text.Trim()));
                server.MessageReceived += Server_MessageReceived;
                server.HangingPieceBindProcessMessageReceived += Server_HangingPieceBindProcessMessageReceived;
                server.ProductsDirectOnlineMessageReceived += Server_ProductsDirectOnlineMessageReceived;
                server.ClientConnected += Server_ClientConnected;
                server.ClientDisconnected += Server_ClientDisconnected;
                server.Start();
                AddMessage(new MessageModel()
                {
                    Message = string.Format("端口:{0}已开启!", txtPort.Text.Trim())
                }, null);
                button2.Enabled = false;
                button3.Enabled = true;
                isConnected = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Server_ClientDisconnected(object sender, Server.Sockets.TcpClientDisconnectedEventArgs e)
        {
            var message = string.Format("ClientDisconnected message: {0} 已断开! --> ", e.TcpClient.Client.RemoteEndPoint.ToString());
            Console.WriteLine(message);
            this.Invoke(new EventHandler(this.AddMessage), new MessageModel() { Message = message });
        }

        private void Server_ClientConnected(object sender, Server.Sockets.TcpClientConnectedEventArgs e)
        {
            var message = string.Format("ClientConnected message: {0} 已连接! --> ", e.TcpClient.Client.RemoteEndPoint.ToString());
            Console.WriteLine(message);
            this.Invoke(new EventHandler(this.AddMessage), new MessageModel() { Message = message }, null);
        }

        private void Server_ProductsDirectOnlineMessageReceived(object sender, Common.Event.MessageEventArgs e)
        {
            Console.WriteLine(string.Format("Server message: {0} --> ", e.message));
            this.Invoke(new EventHandler(this.AddMessage), new MessageModel() { Message = e.message }, null);
        }

        private void Server_HangingPieceBindProcessMessageReceived(object sender, Common.Event.MessageEventArgs e)
        {
            Console.WriteLine(string.Format("Server message: {0} --> ", e.message));
            this.Invoke(new EventHandler(this.AddMessage), new MessageModel() { Message = e.message }, null);

        }

        //发送
        private void button1_Click(object sender, EventArgs e)
        {
            //OrderMessage orderMessage = new OrderMessage();
            //orderMessage.type = (byte)OrderMessageType.OrderStatusChange;
            //server?.SendMessageToAll(JsonConvert.SerializeObject(orderMessage));
        }

        //停止
        private void button3_Click(object sender, EventArgs e)
        {
            server?.Stop();
            isConnected = false;
            button2.Enabled = true;
            button3.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }

        //中标
        private void button4_Click(object sender, EventArgs e)
        {
            //for (var i=0;i<10000;i++) {
            //    OrderMessage message = new OrderMessage()
            //    {
            //        type = (byte)OrderMessageType.OrderWinTheBid,
            //        orderId = textBox2.Text+"-->"+i,
            //        data = textBox3.Text
            //    };
            //    List<string> corpIds = new List<string>() { textBox1.Text };
            //    server.SendMessageWithCorps(JsonConvert.SerializeObject(message), corpIds);
            //    Thread.Sleep(1000);
            //}
            //OrderMessage message = new OrderMessage()
            //{
            //    type = (byte)OrderMessageType.OrderWinTheBid,
            //    orderId = textBox2.Text,
            //    data = new SusContent().ToString()
            //};
            //List<string> corpIds = new List<string>() { textBox1.Text };
            //server.SendMessageWithCorps(JsonConvert.SerializeObject(message), corpIds);


            //OrderMessage message = new OrderMessage()
            //{
            //    type = (byte)OrderMessageType.OrderWinTheBid,
            //    orderId = textBox2.Text,
            //    data = textBox3.Text
            //};
            //List<string> corpIds = new List<string>() { textBox1.Text };
            //server.SendMessageWithCorps(JsonConvert.SerializeObject(message), corpIds);
        }

        //测试数据解析
        private void button5_Click(object sender, EventArgs e)
        {
            OrderMessage message = new OrderMessage()
            {
                type = (byte)OrderMessageType.OrderWinTheBid,
                orderId = textBox2.Text,
                data = textBox3.Text
            };
            List<string> corpIds = new List<string>() { textBox1.Text };
            server.TestDataProcess(JsonConvert.SerializeObject(message), corpIds);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            new Thread(new ThreadStart(SendData)).Start();
        }
        void SendData()
        {
            var data = HexHelper.strToToHexByte("01 03 02 10 00 11 00 00 00 00 00 01");
            for (int i = 0; i < 10000; i++)
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                int randNumber = rand.Next(1, 100);
                server.SendMessageToAll(data);
            }
        }
        void SendData2()
        {
            var data = HexHelper.strToToHexByte("01 03 02 10 00 11 00 00 00 00 00 01");
            for (int i = 0; i < 10000; i++)
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                int randNumber = rand.Next(1, 100);
                server.SendMessageToAll(data);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            new Thread(new ThreadStart(SendData2)).Start();
        }
        //挂片站上线
        private void button8_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("服务器未启动!请先启动服务器！");
                return;
            }
            try
            {
                button8.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 01 06 FF 00 35 00 00 00 00 00 05");
                server.SendMessageToAll(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button8.Cursor = Cursors.Default;
            }
        }
        //衣架进站
        private void button9_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    button9.Cursor = Cursors.WaitCursor;
            //    var data = HexHelper.strToToHexByte("01 03 02 50 00 50 00 00 00 00 03 03");
            //    server.SendMessageToAll(data);
            //    MessageBox.Show("sucess!");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    button9.Cursor = Cursors.Default;
            //}
            var frmHangerInComeStating = new frmInComeStating(server);
            frmHangerInComeStating.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            lbAllMessage.Items.Clear();
            LvClientMancheDireOnline.Items.Clear();
            index = 1;
        }
        public ListViewEx lv;

        //衣架出站
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                button11.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 02 06 FF 00 55 05 AA BB CC DD EE");
                server.SendMessageToAll(data);
                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button11.Cursor = Cursors.Default;
            }
        }
        //衣架落入读卡器对比工序
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                button12.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 04 06 FF 00 54 00 AA BB CC DD EE");
                server.SendMessageToAll(data);
                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button12.Cursor = Cursors.Default;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void btnHangingPiece04_Click(object sender, EventArgs e)
        {
            var hangingNo = HexHelper.tenToHexString(btnHangingPiece04.Tag.ToString());
            if (!SusTCPServer.dicHangingPiece.ContainsKey(hangingNo))
            {
                MessageBox.Show("挂片站无信息!");
                return;
            }
            var fcs = new FormHangingPieceStating(null, SusTCPServer.dicHangingPiece[HexHelper.tenToHexString(btnHangingPiece04.Tag.ToString())], server, hangingNo);
            fcs.Show();
        }

        private void btnCommonStating1_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating1.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }



            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }

        private void btnCommonStating2_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating2.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }
            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }

        private void btnCommonStating3_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating3.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }
            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }



        private void btnCommonStating5_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating5.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }
            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }
        public ListViewEx LvClientMancheDireOnline { get { return lvClientMancheDireOnline; } }
        public ListViewEx LvAllMessage { get { return lbAllMessage; } }

        private void btnEmployeeLogin_Click(object sender, EventArgs e)
        {
            var formEmployee = new FormEmployeeLogin(server);
            formEmployee.ShowDialog();
        }

        private void btnStating_Click(object sender, EventArgs e)
        {
            FormStating stating = new FormStating(server);
            stating.ShowDialog();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var t = new Form2(server);
            t.Show();
        }
        //断电初始化
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                button12.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 01 02 FF 02 08 00 00 00 00 00 00");
                server.SendMessageToAll(data);

                data = HexHelper.strToToHexByte("01 04 02 FF 02 08 00 00 00 00 00 00");
                server.SendMessageToAll(data);

                data = HexHelper.strToToHexByte("01 11 06 FF 02 08 00 00 00 00 00 00");
                server.SendMessageToAll(data);

                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button12.Cursor = Cursors.Default;
            }
        }
        //主版上传
        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                button12.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 01 02 FF 00 08 00 00 00 00 01 33");
                server.SendMessageToAll(data);


                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button12.Cursor = Cursors.Default;
            }
        }
        //SN上传
        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                button12.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 01 02 FF 00 07 00 00 00 00 01 30");
                server.SendMessageToAll(data);


                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button12.Cursor = Cursors.Default;
            }
        }
        //停止接收衣架
        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                button12.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 01 06 FF 00 19 00 00 00 00 00 01");
                server.SendMessageToAll(data);


                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button12.Cursor = Cursors.Default;
            }
        }
        //接衣架
        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                button12.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte("01 01 06 FF 00 19 00 00 00 00 00 00");
                server.SendMessageToAll(data);


                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button12.Cursor = Cursors.Default;
            }
        }

        private void btnCommonStating6_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating6.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }
            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }

        private void btnCommonStating7_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating7.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }
            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }

        private void btnCommonStating8_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating8.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }
            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }

        private void btnCommonStating9_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(btnCommonStating9.Tag.ToString());
            if (!SusTCPServer.dicCommonPiece.ContainsKey(statingNo))
            {
                MessageBox.Show("无信息!");
                return;
            }
            var fcs = new FormCommonStating(null, SusTCPServer.dicCommonPiece[statingNo], server, statingNo, this);
            fcs.Show();
        }

        private void btnFullSite_Click(object sender, EventArgs e)
        {
            var fullSiteFrm = new frmFullSite(server);
            fullSiteFrm.Show();
        }

        private void btnMontorUpload_Click(object sender, EventArgs e)
        {
            var montor = new frmMontorUpload(server);
            montor.Show();
        }

        private void btnStatingInfoSet_Click(object sender, EventArgs e)
        {
            try
            {
                var statingInfo = new frmStatingInfoSet(server);
                statingInfo.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        #region 交互测试
        private void btnSendToMainTrackNumber2_Click(object sender, EventArgs e)
        {
            btnSendToMainTrackNumber1.Cursor = Cursors.WaitCursor;
            Thread thread = new Thread(new ThreadStart(NewThreadForm2));
            thread.Start();
            MessageBox.Show("success");
            btnSendToMainTrackNumber1.Cursor = Cursors.Default;
        }

        private void btnSendToMainTrackNumber1_Click(object sender, EventArgs e)
        {
            btnSendToMainTrackNumber1.Cursor = Cursors.WaitCursor;
            Thread thread = new Thread(new ThreadStart(NewThreadForm1));
            thread.Start();
            MessageBox.Show("success");
            btnSendToMainTrackNumber1.Cursor = Cursors.Default;
        }
        #endregion

        public void NewThreadForm1()
        {
            for (var index = 1; index < 100; index++)
            {
                Thread thread = new Thread(new ThreadStart(startForm1));
                thread.Start();
            }
        }

        private void startForm1()
        {
            var data = HexHelper.strToToHexByte("01 01 06 FF 00 11 00 00 00 00 00 01");
            SusNet2.Common.Message.MessageBody message = new SusNet2.Common.Message.MessageBody(data);
            Common.Entity.ClientUserInfo clientInfo = new Common.Entity.ClientUserInfo("1", "01");
            server.SendMessageWithCAN(message, clientInfo);
        }

        public void NewThreadForm2()
        {
            for (var index = 1; index < 100; index++)
            {
                Thread thread = new Thread(new ThreadStart(startForm2));
                thread.Start();
            }
        }

        private void startForm2()
        {
            var data = HexHelper.strToToHexByte("02 01 06 FF 00 12 00 00 00 00 00 01");
            SusNet2.Common.Message.MessageBody message = new SusNet2.Common.Message.MessageBody(data);
            Common.Entity.ClientUserInfo clientInfo = new Common.Entity.ClientUserInfo("1", "02");
            server.SendMessageWithCAN(message, clientInfo);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            btnSendToMainTrackNumber1.Cursor = Cursors.WaitCursor;
            Thread thread = new Thread(new ThreadStart(NewThreadForm3));
            thread.Start();
            MessageBox.Show("success");
            btnSendToMainTrackNumber1.Cursor = Cursors.Default;
        }
        public void NewThreadForm3()
        {
            for (var index = 1; index < 100; index++)
            {
                Thread thread = new Thread(new ThreadStart(startForm3));
                thread.Start();
            }
        }

        private void startForm3()
        {
            var data = HexHelper.strToToHexByte("03 01 06 FF 00 12 00 00 00 00 00 01");
            SusNet2.Common.Message.MessageBody message = new SusNet2.Common.Message.MessageBody(data);
            Common.Entity.ClientUserInfo clientInfo = new Common.Entity.ClientUserInfo("1", "03");
            server.SendMessageWithCAN(message, clientInfo);
        }
    }
    class MessageModel
    {
        public ListViewEx lvMessage { get; set; }
        public string Message { set; get; }
    }
}
