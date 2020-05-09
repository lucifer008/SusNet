using SusNet.Control;
using WinFormListviewAddContrl;

namespace Sus.Net.TestWinServer
{
    partial class FormClientNew
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMaterial = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMainTrackNumber = new System.Windows.Forms.TextBox();
            this.btnMontorUpload = new System.Windows.Forms.Button();
            this.btnStatingInfoSet = new System.Windows.Forms.Button();
            this.btnFullSite = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.btnStating = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btn2MaintrackNumberBridge11 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.btnTwoMainTrackEmployeeLogin = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.btnTwoMainTrackCommonStating05 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.btnTwoMainTrackCommonStating03 = new System.Windows.Forms.Button();
            this.btnTwoMainTrackCommonStating02 = new System.Windows.Forms.Button();
            this.btnTwoMainTrackCommonPiece01 = new System.Windows.Forms.Button();
            this.btnTwoMainTrackNumberHangpiece04 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn1MaintrackNumberBridge10 = new System.Windows.Forms.Button();
            this.btnCommonStating9 = new System.Windows.Forms.Button();
            this.btnCommonStating8 = new System.Windows.Forms.Button();
            this.btnCommonStating7 = new System.Windows.Forms.Button();
            this.btnCommonStating6 = new System.Windows.Forms.Button();
            this.btnEmployeeLogin = new System.Windows.Forms.Button();
            this.btnCommonStating5 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.btnCommonStating3 = new System.Windows.Forms.Button();
            this.btnCommonStating2 = new System.Windows.Forms.Button();
            this.btnCommonStating1 = new System.Windows.Forms.Button();
            this.btnHangingPiece04 = new System.Windows.Forms.Button();
            this.txtMainTrackNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.tcMain = new SusNet.Control.CustomTabControl();
            this.tpAllCMD = new System.Windows.Forms.TabPage();
            this.lbAllMessage = new WinFormListviewAddContrl.ListViewEx();
            this.tpClientManchineDreOnline = new System.Windows.Forms.TabPage();
            this.lvClientMancheDireOnline = new WinFormListviewAddContrl.ListViewEx();
            this.tpHangPieceOnline = new System.Windows.Forms.TabPage();
            this.lvHangPieceOnline = new WinFormListviewAddContrl.ListViewEx();
            this.tpHeartbeat = new System.Windows.Forms.TabPage();
            this.lvHeartbeat = new WinFormListviewAddContrl.ListViewEx();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpAllCMD.SuspendLayout();
            this.tpClientManchineDreOnline.SuspendLayout();
            this.tpHangPieceOnline.SuspendLayout();
            this.tpHeartbeat.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(204, 14);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "发布订单";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(182, 7);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 20);
            this.button2.TabIndex = 1;
            this.button2.Text = "连接服务器";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(48, 68);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(80, 21);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "1111";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(264, 8);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 20);
            this.button3.TabIndex = 1;
            this.button3.Text = "停止";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(550, 63);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 22);
            this.button4.TabIndex = 3;
            this.button4.Text = "中标";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "公司ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "定单号";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(188, 68);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(110, 21);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "888999998";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(372, 46);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(170, 58);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "从北京到天津的订单中标，运费1000元，10点发货，7点到达，请。。。";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "订单信息";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(14, 120);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(90, 22);
            this.button5.TabIndex = 9;
            this.button5.Text = "测试数据解析";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(939, 13);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "连续发送";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1056, 13);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(111, 23);
            this.button7.TabIndex = 10;
            this.button7.Text = "连续发送";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnMaterial);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtMainTrackNumber);
            this.panel1.Controls.Add(this.btnMontorUpload);
            this.panel1.Controls.Add(this.btnStatingInfoSet);
            this.panel1.Controls.Add(this.btnFullSite);
            this.panel1.Controls.Add(this.button19);
            this.panel1.Controls.Add(this.button18);
            this.panel1.Controls.Add(this.button17);
            this.panel1.Controls.Add(this.button16);
            this.panel1.Controls.Add(this.button15);
            this.panel1.Controls.Add(this.button14);
            this.panel1.Controls.Add(this.btnStating);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.button13);
            this.panel1.Controls.Add(this.button10);
            this.panel1.Controls.Add(this.button12);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtIP);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtPort);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button11);
            this.panel1.Controls.Add(this.button9);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1568, 306);
            this.panel1.TabIndex = 11;
            // 
            // btnMaterial
            // 
            this.btnMaterial.Location = new System.Drawing.Point(131, 224);
            this.btnMaterial.Name = "btnMaterial";
            this.btnMaterial.Size = new System.Drawing.Size(75, 23);
            this.btnMaterial.TabIndex = 27;
            this.btnMaterial.Text = "缺料请求";
            this.btnMaterial.UseVisualStyleBackColor = true;
            this.btnMaterial.Click += new System.EventHandler(this.BtnMaterial_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1240, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 12);
            this.label9.TabIndex = 26;
            this.label9.Text = "MainTrackNumber:";
            // 
            // txtMainTrackNumber
            // 
            this.txtMainTrackNumber.Location = new System.Drawing.Point(1357, 194);
            this.txtMainTrackNumber.Name = "txtMainTrackNumber";
            this.txtMainTrackNumber.Size = new System.Drawing.Size(100, 21);
            this.txtMainTrackNumber.TabIndex = 25;
            this.txtMainTrackNumber.Text = "1";
            // 
            // btnMontorUpload
            // 
            this.btnMontorUpload.Location = new System.Drawing.Point(1242, 81);
            this.btnMontorUpload.Name = "btnMontorUpload";
            this.btnMontorUpload.Size = new System.Drawing.Size(163, 23);
            this.btnMontorUpload.TabIndex = 24;
            this.btnMontorUpload.Text = "推送监测";
            this.btnMontorUpload.UseVisualStyleBackColor = true;
            this.btnMontorUpload.Click += new System.EventHandler(this.btnMontorUpload_Click);
            // 
            // btnStatingInfoSet
            // 
            this.btnStatingInfoSet.Location = new System.Drawing.Point(29, 225);
            this.btnStatingInfoSet.Name = "btnStatingInfoSet";
            this.btnStatingInfoSet.Size = new System.Drawing.Size(75, 23);
            this.btnStatingInfoSet.TabIndex = 23;
            this.btnStatingInfoSet.Text = "站点状态设置";
            this.btnStatingInfoSet.UseVisualStyleBackColor = true;
            this.btnStatingInfoSet.Click += new System.EventHandler(this.btnStatingInfoSet_Click);
            // 
            // btnFullSite
            // 
            this.btnFullSite.Location = new System.Drawing.Point(131, 194);
            this.btnFullSite.Name = "btnFullSite";
            this.btnFullSite.Size = new System.Drawing.Size(75, 23);
            this.btnFullSite.TabIndex = 23;
            this.btnFullSite.Text = "满站";
            this.btnFullSite.UseVisualStyleBackColor = true;
            this.btnFullSite.Click += new System.EventHandler(this.btnFullSite_Click);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(29, 192);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(75, 23);
            this.button19.TabIndex = 22;
            this.button19.Text = "接收衣架";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(29, 159);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(75, 23);
            this.button18.TabIndex = 22;
            this.button18.Text = "停止接收衣架";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(131, 159);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(75, 23);
            this.button17.TabIndex = 21;
            this.button17.Text = "SN上传";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(131, 130);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 23);
            this.button16.TabIndex = 21;
            this.button16.Text = "主版上传";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(29, 130);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 20;
            this.button15.Text = "断电初始化";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(1410, 14);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 23);
            this.button14.TabIndex = 19;
            this.button14.Text = "压力测试";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // btnStating
            // 
            this.btnStating.Location = new System.Drawing.Point(1288, 13);
            this.btnStating.Name = "btnStating";
            this.btnStating.Size = new System.Drawing.Size(75, 23);
            this.btnStating.TabIndex = 19;
            this.btnStating.Text = "站点相关";
            this.btnStating.UseVisualStyleBackColor = true;
            this.btnStating.Click += new System.EventHandler(this.btnStating_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btn2MaintrackNumberBridge11);
            this.panel3.Controls.Add(this.button23);
            this.panel3.Controls.Add(this.btnTwoMainTrackEmployeeLogin);
            this.panel3.Controls.Add(this.button22);
            this.panel3.Controls.Add(this.btnTwoMainTrackCommonStating05);
            this.panel3.Controls.Add(this.button21);
            this.panel3.Controls.Add(this.button20);
            this.panel3.Controls.Add(this.button25);
            this.panel3.Controls.Add(this.btnTwoMainTrackCommonStating03);
            this.panel3.Controls.Add(this.btnTwoMainTrackCommonStating02);
            this.panel3.Controls.Add(this.btnTwoMainTrackCommonPiece01);
            this.panel3.Controls.Add(this.btnTwoMainTrackNumberHangpiece04);
            this.panel3.Controls.Add(this.textBox4);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Location = new System.Drawing.Point(276, 192);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(958, 108);
            this.panel3.TabIndex = 18;
            // 
            // btn2MaintrackNumberBridge11
            // 
            this.btn2MaintrackNumberBridge11.Location = new System.Drawing.Point(804, 32);
            this.btn2MaintrackNumberBridge11.Name = "btn2MaintrackNumberBridge11";
            this.btn2MaintrackNumberBridge11.Size = new System.Drawing.Size(110, 23);
            this.btn2MaintrackNumberBridge11.TabIndex = 6;
            this.btn2MaintrackNumberBridge11.Tag = "02-11";
            this.btn2MaintrackNumberBridge11.Text = "桥接11";
            this.btn2MaintrackNumberBridge11.UseVisualStyleBackColor = true;
            this.btn2MaintrackNumberBridge11.Click += new System.EventHandler(this.btn2MaintrackNumberBridge11_Click);
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(558, 58);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(90, 23);
            this.button23.TabIndex = 5;
            this.button23.Tag = "02-09";
            this.button23.Text = "普通站【09】";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.btnCommonStating9_Click);
            // 
            // btnTwoMainTrackEmployeeLogin
            // 
            this.btnTwoMainTrackEmployeeLogin.Location = new System.Drawing.Point(709, 32);
            this.btnTwoMainTrackEmployeeLogin.Name = "btnTwoMainTrackEmployeeLogin";
            this.btnTwoMainTrackEmployeeLogin.Size = new System.Drawing.Size(89, 23);
            this.btnTwoMainTrackEmployeeLogin.TabIndex = 4;
            this.btnTwoMainTrackEmployeeLogin.Text = "员工相关";
            this.btnTwoMainTrackEmployeeLogin.UseVisualStyleBackColor = true;
            this.btnTwoMainTrackEmployeeLogin.Click += new System.EventHandler(this.btnEmployeeLogin_Click);
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(456, 58);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(90, 23);
            this.button22.TabIndex = 5;
            this.button22.Tag = "02-08";
            this.button22.Text = "普通站【08】";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.btnCommonStating8_Click);
            // 
            // btnTwoMainTrackCommonStating05
            // 
            this.btnTwoMainTrackCommonStating05.Location = new System.Drawing.Point(162, 59);
            this.btnTwoMainTrackCommonStating05.Name = "btnTwoMainTrackCommonStating05";
            this.btnTwoMainTrackCommonStating05.Size = new System.Drawing.Size(93, 23);
            this.btnTwoMainTrackCommonStating05.TabIndex = 3;
            this.btnTwoMainTrackCommonStating05.Tag = "02-05";
            this.btnTwoMainTrackCommonStating05.Text = "普通站【05】";
            this.btnTwoMainTrackCommonStating05.UseVisualStyleBackColor = true;
            this.btnTwoMainTrackCommonStating05.Click += new System.EventHandler(this.btnCommonStating5_Click);
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(360, 59);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(90, 23);
            this.button21.TabIndex = 5;
            this.button21.Tag = "02-07";
            this.button21.Text = "普通站【07】";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.btnCommonStating7_Click);
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(258, 59);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(90, 23);
            this.button20.TabIndex = 5;
            this.button20.Tag = "02-06";
            this.button20.Text = "普通站【06】";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.btnCommonStating6_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(558, 29);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(93, 23);
            this.button25.TabIndex = 3;
            this.button25.Tag = "02-04";
            this.button25.Text = "普通站【04】";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.btnCommonStating3_Click);
            // 
            // btnTwoMainTrackCommonStating03
            // 
            this.btnTwoMainTrackCommonStating03.Location = new System.Drawing.Point(456, 30);
            this.btnTwoMainTrackCommonStating03.Name = "btnTwoMainTrackCommonStating03";
            this.btnTwoMainTrackCommonStating03.Size = new System.Drawing.Size(93, 23);
            this.btnTwoMainTrackCommonStating03.TabIndex = 3;
            this.btnTwoMainTrackCommonStating03.Tag = "02-03";
            this.btnTwoMainTrackCommonStating03.Text = "普通站【03】";
            this.btnTwoMainTrackCommonStating03.UseVisualStyleBackColor = true;
            this.btnTwoMainTrackCommonStating03.Click += new System.EventHandler(this.btnCommonStating3_Click);
            // 
            // btnTwoMainTrackCommonStating02
            // 
            this.btnTwoMainTrackCommonStating02.Location = new System.Drawing.Point(357, 30);
            this.btnTwoMainTrackCommonStating02.Name = "btnTwoMainTrackCommonStating02";
            this.btnTwoMainTrackCommonStating02.Size = new System.Drawing.Size(93, 23);
            this.btnTwoMainTrackCommonStating02.TabIndex = 3;
            this.btnTwoMainTrackCommonStating02.Tag = "02-02";
            this.btnTwoMainTrackCommonStating02.Text = "普通站【02】";
            this.btnTwoMainTrackCommonStating02.UseVisualStyleBackColor = true;
            this.btnTwoMainTrackCommonStating02.Click += new System.EventHandler(this.btnCommonStating2_Click);
            // 
            // btnTwoMainTrackCommonPiece01
            // 
            this.btnTwoMainTrackCommonPiece01.Location = new System.Drawing.Point(258, 30);
            this.btnTwoMainTrackCommonPiece01.Name = "btnTwoMainTrackCommonPiece01";
            this.btnTwoMainTrackCommonPiece01.Size = new System.Drawing.Size(93, 23);
            this.btnTwoMainTrackCommonPiece01.TabIndex = 3;
            this.btnTwoMainTrackCommonPiece01.Tag = "02-01";
            this.btnTwoMainTrackCommonPiece01.Text = "普通站【01】";
            this.btnTwoMainTrackCommonPiece01.UseVisualStyleBackColor = true;
            this.btnTwoMainTrackCommonPiece01.Click += new System.EventHandler(this.btnCommonStating1_Click);
            // 
            // btnTwoMainTrackNumberHangpiece04
            // 
            this.btnTwoMainTrackNumberHangpiece04.Location = new System.Drawing.Point(162, 30);
            this.btnTwoMainTrackNumberHangpiece04.Name = "btnTwoMainTrackNumberHangpiece04";
            this.btnTwoMainTrackNumberHangpiece04.Size = new System.Drawing.Size(90, 23);
            this.btnTwoMainTrackNumberHangpiece04.TabIndex = 2;
            this.btnTwoMainTrackNumberHangpiece04.Tag = "02-01";
            this.btnTwoMainTrackNumberHangpiece04.Text = "挂片站【01】";
            this.btnTwoMainTrackNumberHangpiece04.UseVisualStyleBackColor = true;
            this.btnTwoMainTrackNumberHangpiece04.Click += new System.EventHandler(this.btnHangingPiece04_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(72, 32);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(64, 21);
            this.textBox4.TabIndex = 1;
            this.textBox4.Text = "2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "主轨号:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btn1MaintrackNumberBridge10);
            this.panel2.Controls.Add(this.btnCommonStating9);
            this.panel2.Controls.Add(this.btnCommonStating8);
            this.panel2.Controls.Add(this.btnCommonStating7);
            this.panel2.Controls.Add(this.btnCommonStating6);
            this.panel2.Controls.Add(this.btnEmployeeLogin);
            this.panel2.Controls.Add(this.btnCommonStating5);
            this.panel2.Controls.Add(this.button24);
            this.panel2.Controls.Add(this.btnCommonStating3);
            this.panel2.Controls.Add(this.btnCommonStating2);
            this.panel2.Controls.Add(this.btnCommonStating1);
            this.panel2.Controls.Add(this.btnHangingPiece04);
            this.panel2.Controls.Add(this.txtMainTrackNo);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(276, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(958, 119);
            this.panel2.TabIndex = 18;
            // 
            // btn1MaintrackNumberBridge10
            // 
            this.btn1MaintrackNumberBridge10.Location = new System.Drawing.Point(804, 36);
            this.btn1MaintrackNumberBridge10.Name = "btn1MaintrackNumberBridge10";
            this.btn1MaintrackNumberBridge10.Size = new System.Drawing.Size(110, 23);
            this.btn1MaintrackNumberBridge10.TabIndex = 6;
            this.btn1MaintrackNumberBridge10.Tag = "01-10";
            this.btn1MaintrackNumberBridge10.Text = "桥接10";
            this.btn1MaintrackNumberBridge10.UseVisualStyleBackColor = true;
            this.btn1MaintrackNumberBridge10.Click += new System.EventHandler(this.btn1MaintrackNumberBridge10_Click);
            // 
            // btnCommonStating9
            // 
            this.btnCommonStating9.Location = new System.Drawing.Point(558, 68);
            this.btnCommonStating9.Name = "btnCommonStating9";
            this.btnCommonStating9.Size = new System.Drawing.Size(90, 23);
            this.btnCommonStating9.TabIndex = 5;
            this.btnCommonStating9.Tag = "01-09";
            this.btnCommonStating9.Text = "普通站【09】";
            this.btnCommonStating9.UseVisualStyleBackColor = true;
            this.btnCommonStating9.Click += new System.EventHandler(this.btnCommonStating9_Click);
            // 
            // btnCommonStating8
            // 
            this.btnCommonStating8.Location = new System.Drawing.Point(456, 68);
            this.btnCommonStating8.Name = "btnCommonStating8";
            this.btnCommonStating8.Size = new System.Drawing.Size(90, 23);
            this.btnCommonStating8.TabIndex = 5;
            this.btnCommonStating8.Tag = "01-08";
            this.btnCommonStating8.Text = "普通站【08】";
            this.btnCommonStating8.UseVisualStyleBackColor = true;
            this.btnCommonStating8.Click += new System.EventHandler(this.btnCommonStating8_Click);
            // 
            // btnCommonStating7
            // 
            this.btnCommonStating7.Location = new System.Drawing.Point(360, 69);
            this.btnCommonStating7.Name = "btnCommonStating7";
            this.btnCommonStating7.Size = new System.Drawing.Size(90, 23);
            this.btnCommonStating7.TabIndex = 5;
            this.btnCommonStating7.Tag = "01-07";
            this.btnCommonStating7.Text = "普通站【07】";
            this.btnCommonStating7.UseVisualStyleBackColor = true;
            this.btnCommonStating7.Click += new System.EventHandler(this.btnCommonStating7_Click);
            // 
            // btnCommonStating6
            // 
            this.btnCommonStating6.Location = new System.Drawing.Point(258, 69);
            this.btnCommonStating6.Name = "btnCommonStating6";
            this.btnCommonStating6.Size = new System.Drawing.Size(90, 23);
            this.btnCommonStating6.TabIndex = 5;
            this.btnCommonStating6.Tag = "01-06";
            this.btnCommonStating6.Text = "普通站【06】";
            this.btnCommonStating6.UseVisualStyleBackColor = true;
            this.btnCommonStating6.Click += new System.EventHandler(this.btnCommonStating6_Click);
            // 
            // btnEmployeeLogin
            // 
            this.btnEmployeeLogin.Location = new System.Drawing.Point(699, 36);
            this.btnEmployeeLogin.Name = "btnEmployeeLogin";
            this.btnEmployeeLogin.Size = new System.Drawing.Size(89, 23);
            this.btnEmployeeLogin.TabIndex = 4;
            this.btnEmployeeLogin.Text = "员工相关";
            this.btnEmployeeLogin.UseVisualStyleBackColor = true;
            this.btnEmployeeLogin.Click += new System.EventHandler(this.btnEmployeeLogin_Click);
            // 
            // btnCommonStating5
            // 
            this.btnCommonStating5.Location = new System.Drawing.Point(159, 68);
            this.btnCommonStating5.Name = "btnCommonStating5";
            this.btnCommonStating5.Size = new System.Drawing.Size(93, 23);
            this.btnCommonStating5.TabIndex = 3;
            this.btnCommonStating5.Tag = "01-05";
            this.btnCommonStating5.Text = "普通站【05】";
            this.btnCommonStating5.UseVisualStyleBackColor = true;
            this.btnCommonStating5.Click += new System.EventHandler(this.btnCommonStating5_Click);
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(555, 30);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(93, 23);
            this.button24.TabIndex = 3;
            this.button24.Tag = "01-04";
            this.button24.Text = "普通站【04】";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.btnCommonStating3_Click);
            // 
            // btnCommonStating3
            // 
            this.btnCommonStating3.Location = new System.Drawing.Point(456, 30);
            this.btnCommonStating3.Name = "btnCommonStating3";
            this.btnCommonStating3.Size = new System.Drawing.Size(93, 23);
            this.btnCommonStating3.TabIndex = 3;
            this.btnCommonStating3.Tag = "01-03";
            this.btnCommonStating3.Text = "普通站【03】";
            this.btnCommonStating3.UseVisualStyleBackColor = true;
            this.btnCommonStating3.Click += new System.EventHandler(this.btnCommonStating3_Click);
            // 
            // btnCommonStating2
            // 
            this.btnCommonStating2.Location = new System.Drawing.Point(357, 30);
            this.btnCommonStating2.Name = "btnCommonStating2";
            this.btnCommonStating2.Size = new System.Drawing.Size(93, 23);
            this.btnCommonStating2.TabIndex = 3;
            this.btnCommonStating2.Tag = "01-02";
            this.btnCommonStating2.Text = "普通站【02】";
            this.btnCommonStating2.UseVisualStyleBackColor = true;
            this.btnCommonStating2.Click += new System.EventHandler(this.btnCommonStating2_Click);
            // 
            // btnCommonStating1
            // 
            this.btnCommonStating1.Location = new System.Drawing.Point(258, 30);
            this.btnCommonStating1.Name = "btnCommonStating1";
            this.btnCommonStating1.Size = new System.Drawing.Size(93, 23);
            this.btnCommonStating1.TabIndex = 3;
            this.btnCommonStating1.Tag = "01-01";
            this.btnCommonStating1.Text = "普通站【01】";
            this.btnCommonStating1.UseVisualStyleBackColor = true;
            this.btnCommonStating1.Click += new System.EventHandler(this.btnCommonStating1_Click);
            // 
            // btnHangingPiece04
            // 
            this.btnHangingPiece04.Location = new System.Drawing.Point(162, 30);
            this.btnHangingPiece04.Name = "btnHangingPiece04";
            this.btnHangingPiece04.Size = new System.Drawing.Size(90, 23);
            this.btnHangingPiece04.TabIndex = 2;
            this.btnHangingPiece04.Tag = "01-01";
            this.btnHangingPiece04.Text = "挂片站【01】";
            this.btnHangingPiece04.UseVisualStyleBackColor = true;
            this.btnHangingPiece04.Click += new System.EventHandler(this.btnHangingPiece04_Click);
            // 
            // txtMainTrackNo
            // 
            this.txtMainTrackNo.Location = new System.Drawing.Point(72, 41);
            this.txtMainTrackNo.Name = "txtMainTrackNo";
            this.txtMainTrackNo.Size = new System.Drawing.Size(64, 21);
            this.txtMainTrackNo.TabIndex = 1;
            this.txtMainTrackNo.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "主轨号:";
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(508, 13);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(123, 23);
            this.button13.TabIndex = 17;
            this.button13.Text = "挂片站衣架出站";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(131, 101);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(102, 23);
            this.button10.TabIndex = 16;
            this.button10.Text = "清除消息";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(781, 13);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(152, 23);
            this.button12.TabIndex = 15;
            this.button12.Text = "衣架落入读卡器对比工序";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "服务器端收到的消息";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(39, 42);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(135, 21);
            this.txtIP.TabIndex = 14;
            this.txtIP.Text = "127.0.0.1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "IP";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(39, 7);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(135, 21);
            this.txtPort.TabIndex = 14;
            this.txtPort.Text = "9998";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "端口";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(650, 13);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(109, 23);
            this.button11.TabIndex = 12;
            this.button11.Text = "普通站衣架出站";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(1184, 13);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(84, 23);
            this.button9.TabIndex = 11;
            this.button9.Text = "衣架进站";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(369, 13);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(119, 23);
            this.button8.TabIndex = 2;
            this.button8.Text = "挂片站上线";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpAllCMD);
            this.tcMain.Controls.Add(this.tpClientManchineDreOnline);
            this.tcMain.Controls.Add(this.tpHangPieceOnline);
            this.tcMain.Controls.Add(this.tpHeartbeat);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.ItemSize = new System.Drawing.Size(0, 16);
            this.tcMain.Location = new System.Drawing.Point(0, 306);
            this.tcMain.Name = "tcMain";
            this.tcMain.Padding = new System.Drawing.Point(9, 0);
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1568, 403);
            this.tcMain.TabIndex = 13;
            // 
            // tpAllCMD
            // 
            this.tpAllCMD.Controls.Add(this.lbAllMessage);
            this.tpAllCMD.Location = new System.Drawing.Point(4, 20);
            this.tpAllCMD.Name = "tpAllCMD";
            this.tpAllCMD.Padding = new System.Windows.Forms.Padding(3);
            this.tpAllCMD.Size = new System.Drawing.Size(1560, 379);
            this.tpAllCMD.TabIndex = 0;
            this.tpAllCMD.Text = "所有来自pc的命令";
            this.tpAllCMD.UseVisualStyleBackColor = true;
            // 
            // lbAllMessage
            // 
            this.lbAllMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAllMessage.FullRowSelect = true;
            this.lbAllMessage.GridLines = true;
            this.lbAllMessage.HideSelection = false;
            this.lbAllMessage.Location = new System.Drawing.Point(3, 3);
            this.lbAllMessage.Name = "lbAllMessage";
            this.lbAllMessage.Size = new System.Drawing.Size(1554, 373);
            this.lbAllMessage.TabIndex = 0;
            this.lbAllMessage.UseCompatibleStateImageBehavior = false;
            this.lbAllMessage.View = System.Windows.Forms.View.List;
            // 
            // tpClientManchineDreOnline
            // 
            this.tpClientManchineDreOnline.Controls.Add(this.lvClientMancheDireOnline);
            this.tpClientManchineDreOnline.Location = new System.Drawing.Point(4, 20);
            this.tpClientManchineDreOnline.Name = "tpClientManchineDreOnline";
            this.tpClientManchineDreOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tpClientManchineDreOnline.Size = new System.Drawing.Size(1560, 379);
            this.tpClientManchineDreOnline.TabIndex = 1;
            this.tpClientManchineDreOnline.Text = "客户机直接上线";
            this.tpClientManchineDreOnline.UseVisualStyleBackColor = true;
            // 
            // lvClientMancheDireOnline
            // 
            this.lvClientMancheDireOnline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvClientMancheDireOnline.FullRowSelect = true;
            this.lvClientMancheDireOnline.GridLines = true;
            this.lvClientMancheDireOnline.HideSelection = false;
            this.lvClientMancheDireOnline.Location = new System.Drawing.Point(3, 3);
            this.lvClientMancheDireOnline.Name = "lvClientMancheDireOnline";
            this.lvClientMancheDireOnline.Size = new System.Drawing.Size(1554, 373);
            this.lvClientMancheDireOnline.TabIndex = 1;
            this.lvClientMancheDireOnline.UseCompatibleStateImageBehavior = false;
            this.lvClientMancheDireOnline.View = System.Windows.Forms.View.List;
            // 
            // tpHangPieceOnline
            // 
            this.tpHangPieceOnline.BackColor = System.Drawing.Color.Transparent;
            this.tpHangPieceOnline.Controls.Add(this.lvHangPieceOnline);
            this.tpHangPieceOnline.Location = new System.Drawing.Point(4, 20);
            this.tpHangPieceOnline.Name = "tpHangPieceOnline";
            this.tpHangPieceOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tpHangPieceOnline.Size = new System.Drawing.Size(1560, 379);
            this.tpHangPieceOnline.TabIndex = 2;
            this.tpHangPieceOnline.Text = "挂片站上线";
            this.tpHangPieceOnline.UseVisualStyleBackColor = true;
            // 
            // lvHangPieceOnline
            // 
            this.lvHangPieceOnline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHangPieceOnline.FullRowSelect = true;
            this.lvHangPieceOnline.GridLines = true;
            this.lvHangPieceOnline.HideSelection = false;
            this.lvHangPieceOnline.Location = new System.Drawing.Point(3, 3);
            this.lvHangPieceOnline.Name = "lvHangPieceOnline";
            this.lvHangPieceOnline.Size = new System.Drawing.Size(1554, 373);
            this.lvHangPieceOnline.TabIndex = 3;
            this.lvHangPieceOnline.UseCompatibleStateImageBehavior = false;
            this.lvHangPieceOnline.View = System.Windows.Forms.View.List;
            // 
            // tpHeartbeat
            // 
            this.tpHeartbeat.Controls.Add(this.lvHeartbeat);
            this.tpHeartbeat.Location = new System.Drawing.Point(4, 20);
            this.tpHeartbeat.Name = "tpHeartbeat";
            this.tpHeartbeat.Size = new System.Drawing.Size(1560, 379);
            this.tpHeartbeat.TabIndex = 3;
            this.tpHeartbeat.Text = "心跳包";
            this.tpHeartbeat.UseVisualStyleBackColor = true;
            // 
            // lvHeartbeat
            // 
            this.lvHeartbeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHeartbeat.FullRowSelect = true;
            this.lvHeartbeat.GridLines = true;
            this.lvHeartbeat.HideSelection = false;
            this.lvHeartbeat.Location = new System.Drawing.Point(0, 0);
            this.lvHeartbeat.Name = "lvHeartbeat";
            this.lvHeartbeat.Size = new System.Drawing.Size(1560, 379);
            this.lvHeartbeat.TabIndex = 2;
            this.lvHeartbeat.UseCompatibleStateImageBehavior = false;
            this.lvHeartbeat.View = System.Windows.Forms.View.List;
            // 
            // FormClientNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1568, 709);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormClientNew";
            this.Text = "客户端";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpAllCMD.ResumeLayout(false);
            this.tpClientManchineDreOnline.ResumeLayout(false);
            this.tpHangPieceOnline.ResumeLayout(false);
            this.tpHeartbeat.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button8;
        //private System.Windows.Forms.ListView listBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMainTrackNo;
        private System.Windows.Forms.Button btnHangingPiece04;
        private System.Windows.Forms.Button btnCommonStating1;
        private System.Windows.Forms.Button btnCommonStating5;
        private System.Windows.Forms.Button btnCommonStating3;
        private System.Windows.Forms.Button btnCommonStating2;
        private System.Windows.Forms.Button btnEmployeeLogin;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnTwoMainTrackEmployeeLogin;
        private System.Windows.Forms.Button btnTwoMainTrackCommonStating05;
        private System.Windows.Forms.Button btnTwoMainTrackCommonStating03;
        private System.Windows.Forms.Button btnTwoMainTrackCommonStating02;
        private System.Windows.Forms.Button btnTwoMainTrackCommonPiece01;
        private System.Windows.Forms.Button btnTwoMainTrackNumberHangpiece04;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label7;
        private CustomTabControl tcMain;
        private System.Windows.Forms.TabPage tpAllCMD;
        private System.Windows.Forms.TabPage tpClientManchineDreOnline;
        private System.Windows.Forms.TabPage tpHangPieceOnline;
        private ListViewEx lbAllMessage;
        private ListViewEx lvClientMancheDireOnline;
        private System.Windows.Forms.TabPage tpHeartbeat;
        private ListViewEx lvHeartbeat;
        private ListViewEx lvHangPieceOnline;
        private System.Windows.Forms.Button btnStating;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button btnCommonStating9;
        private System.Windows.Forms.Button btnCommonStating8;
        private System.Windows.Forms.Button btnCommonStating7;
        private System.Windows.Forms.Button btnCommonStating6;
        private System.Windows.Forms.Button btnFullSite;
        private System.Windows.Forms.Button btnMontorUpload;
        private System.Windows.Forms.Button btnStatingInfoSet;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMainTrackNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button btn2MaintrackNumberBridge11;
        private System.Windows.Forms.Button btn1MaintrackNumberBridge10;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button btnMaterial;
    }
}

