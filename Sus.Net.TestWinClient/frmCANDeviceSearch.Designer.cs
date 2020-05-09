namespace Sus.Net.TestWinClient
{
    partial class frmCANDeviceSearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearchIP = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnPingTest = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPingTest);
            this.panel1.Controls.Add(this.btnSearchIP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1398, 150);
            this.panel1.TabIndex = 0;
            // 
            // btnSearchIP
            // 
            this.btnSearchIP.Location = new System.Drawing.Point(158, 25);
            this.btnSearchIP.Name = "btnSearchIP";
            this.btnSearchIP.Size = new System.Drawing.Size(75, 23);
            this.btnSearchIP.TabIndex = 0;
            this.btnSearchIP.Text = "搜索";
            this.btnSearchIP.UseVisualStyleBackColor = true;
            this.btnSearchIP.Click += new System.EventHandler(this.btnSearchIP_Click_1);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 150);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1398, 409);
            this.panel2.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1398, 409);
            this.listBox1.TabIndex = 11;
            // 
            // btnPingTest
            // 
            this.btnPingTest.Location = new System.Drawing.Point(45, 25);
            this.btnPingTest.Name = "btnPingTest";
            this.btnPingTest.Size = new System.Drawing.Size(75, 23);
            this.btnPingTest.TabIndex = 0;
            this.btnPingTest.Text = "测试通信";
            this.btnPingTest.UseVisualStyleBackColor = true;
            this.btnPingTest.Click += new System.EventHandler(this.btnPingTest_Click);
            // 
            // frmCANDeviceSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1398, 559);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmCANDeviceSearch";
            this.Text = "frmCANDeviceSearch";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnSearchIP;
        private string ip;
        private int port;

        public frmCANDeviceSearch(string text, int v):this()
        {
            ip = text;
            this.port = v;
        }

        private System.Windows.Forms.Button btnPingTest;
    }
}