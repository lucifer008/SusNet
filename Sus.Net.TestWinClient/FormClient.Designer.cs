using WinFormListviewAddContrl;

namespace Sus.Net.TestWinClient
{
    partial class FormClient
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Stop = new System.Windows.Forms.Button();
            this.Connet = new System.Windows.Forms.Button();
            this.Send = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtMainTrackNumber = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtUDPPort = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUDPServerIP = new System.Windows.Forms.TextBox();
            this.btnUDPStart = new System.Windows.Forms.Button();
            this.btnSerialPortTest = new System.Windows.Forms.Button();
            this.btnSerialPort = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.listBox1 = new WinFormListviewAddContrl.ListViewEx();
            this.btnNew = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(649, 11);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(133, 21);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "1111";
            this.textBox1.Visible = false;
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(497, 10);
            this.Stop.Margin = new System.Windows.Forms.Padding(2);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(68, 22);
            this.Stop.TabIndex = 4;
            this.Stop.Text = "断开";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Connet
            // 
            this.Connet.Location = new System.Drawing.Point(423, 10);
            this.Connet.Margin = new System.Windows.Forms.Padding(2);
            this.Connet.Name = "Connet";
            this.Connet.Size = new System.Drawing.Size(64, 22);
            this.Connet.TabIndex = 5;
            this.Connet.Text = "连接";
            this.Connet.UseVisualStyleBackColor = true;
            this.Connet.Click += new System.EventHandler(this.Connet_Click);
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(10, 56);
            this.Send.Margin = new System.Windows.Forms.Padding(2);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(78, 22);
            this.Send.TabIndex = 3;
            this.Send.Text = "发送";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(791, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "UserID";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(604, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "CorpID";
            this.label2.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(935, 11);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(36, 21);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "u1234";
            this.textBox2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(146, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "启动主轨";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(227, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "停止主轨";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(308, 54);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "急停主轨";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(391, 56);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "制品界面上线";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(497, 56);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(96, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "制品界面挂片";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "客户端收到的消息";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(115, 93);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 23);
            this.button6.TabIndex = 15;
            this.button6.Text = "清除消息";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(270, 9);
            this.txtPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(133, 21);
            this.txtPort.TabIndex = 6;
            this.txtPort.Text = "9998";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(225, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "端口";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "IP";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(65, 9);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(133, 21);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "127.0.0.1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-183, 15);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "端口";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.txtMainTrackNumber);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnSerialPortTest);
            this.panel1.Controls.Add(this.btnSerialPort);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.Send);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.Connet);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.Stop);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.txtPort);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1448, 124);
            this.panel1.TabIndex = 16;
            // 
            // txtMainTrackNumber
            // 
            this.txtMainTrackNumber.Location = new System.Drawing.Point(838, 12);
            this.txtMainTrackNumber.Margin = new System.Windows.Forms.Padding(2);
            this.txtMainTrackNumber.Name = "txtMainTrackNumber";
            this.txtMainTrackNumber.Size = new System.Drawing.Size(133, 21);
            this.txtMainTrackNumber.TabIndex = 6;
            this.txtMainTrackNumber.Text = "1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtUDPPort);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtUDPServerIP);
            this.groupBox1.Controls.Add(this.btnUDPStart);
            this.groupBox1.Location = new System.Drawing.Point(606, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 75);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "udp";
            // 
            // txtUDPPort
            // 
            this.txtUDPPort.Location = new System.Drawing.Point(265, 30);
            this.txtUDPPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtUDPPort.Name = "txtUDPPort";
            this.txtUDPPort.Size = new System.Drawing.Size(133, 21);
            this.txtUDPPort.TabIndex = 6;
            this.txtUDPPort.Text = "1901";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 31);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "ip";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(220, 31);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "端口";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtUDPServerIP
            // 
            this.txtUDPServerIP.Location = new System.Drawing.Point(60, 30);
            this.txtUDPServerIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtUDPServerIP.Name = "txtUDPServerIP";
            this.txtUDPServerIP.Size = new System.Drawing.Size(133, 21);
            this.txtUDPServerIP.TabIndex = 6;
            this.txtUDPServerIP.Text = "192.168.10.139";
            // 
            // btnUDPStart
            // 
            this.btnUDPStart.Location = new System.Drawing.Point(491, 32);
            this.btnUDPStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnUDPStart.Name = "btnUDPStart";
            this.btnUDPStart.Size = new System.Drawing.Size(64, 22);
            this.btnUDPStart.TabIndex = 5;
            this.btnUDPStart.Text = "开启";
            this.btnUDPStart.UseVisualStyleBackColor = true;
            this.btnUDPStart.Click += new System.EventHandler(this.btnUDPStart_Click);
            // 
            // btnSerialPortTest
            // 
            this.btnSerialPortTest.Location = new System.Drawing.Point(1312, 72);
            this.btnSerialPortTest.Name = "btnSerialPortTest";
            this.btnSerialPortTest.Size = new System.Drawing.Size(124, 23);
            this.btnSerialPortTest.TabIndex = 18;
            this.btnSerialPortTest.Text = "串口设备调试";
            this.btnSerialPortTest.UseVisualStyleBackColor = true;
            this.btnSerialPortTest.Click += new System.EventHandler(this.btnSerialPortTest_Click);
            // 
            // btnSerialPort
            // 
            this.btnSerialPort.Location = new System.Drawing.Point(619, 12);
            this.btnSerialPort.Name = "btnSerialPort";
            this.btnSerialPort.Size = new System.Drawing.Size(100, 23);
            this.btnSerialPort.TabIndex = 17;
            this.btnSerialPort.Text = "搜索串口设备";
            this.btnSerialPort.UseVisualStyleBackColor = true;
            this.btnSerialPort.Click += new System.EventHandler(this.btnSerialPort_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(740, 12);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 16;
            this.button7.Text = "连续发送";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FullRowSelect = true;
            this.listBox1.GridLines = true;
            this.listBox1.Location = new System.Drawing.Point(0, 124);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1448, 449);
            this.listBox1.TabIndex = 10;
            this.listBox1.UseCompatibleStateImageBehavior = false;
            this.listBox1.View = System.Windows.Forms.View.List;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(1085, 8);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 20;
            this.btnNew.Text = "new";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1448, 573);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormClient";
            this.Text = "客户端";
            this.Load += new System.EventHandler(this.FormClient_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button Connet;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private ListViewEx listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnSerialPort;
        private System.Windows.Forms.Button btnSerialPortTest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtUDPPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUDPServerIP;
        private System.Windows.Forms.Button btnUDPStart;
        private System.Windows.Forms.TextBox txtMainTrackNumber;
        private System.Windows.Forms.Button btnNew;
    }
}

