namespace Sus.Net.TestWinServer
{
    partial class FormEmployeeLogin
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtMainTrackNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cobStatingNo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cobEmployeeList = new System.Windows.Forms.ComboBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "主轨";
            // 
            // txtMainTrackNo
            // 
            this.txtMainTrackNo.Location = new System.Drawing.Point(107, 35);
            this.txtMainTrackNo.Name = "txtMainTrackNo";
            this.txtMainTrackNo.Size = new System.Drawing.Size(133, 21);
            this.txtMainTrackNo.TabIndex = 1;
            this.txtMainTrackNo.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "站点";
            // 
            // cobStatingNo
            // 
            this.cobStatingNo.FormattingEnabled = true;
            this.cobStatingNo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cobStatingNo.Location = new System.Drawing.Point(107, 84);
            this.cobStatingNo.Name = "cobStatingNo";
            this.cobStatingNo.Size = new System.Drawing.Size(133, 20);
            this.cobStatingNo.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "员工";
            // 
            // cobEmployeeList
            // 
            this.cobEmployeeList.FormattingEnabled = true;
            this.cobEmployeeList.Location = new System.Drawing.Point(107, 120);
            this.cobEmployeeList.Name = "cobEmployeeList";
            this.cobEmployeeList.Size = new System.Drawing.Size(133, 20);
            this.cobEmployeeList.TabIndex = 2;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(61, 212);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "上班登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(165, 212);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(75, 23);
            this.btnOut.TabIndex = 3;
            this.btnOut.Text = "下班";
            this.btnOut.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(375, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(217, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "全部登录或者登出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormEmployeeLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 419);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.cobEmployeeList);
            this.Controls.Add(this.cobStatingNo);
            this.Controls.Add(this.txtMainTrackNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormEmployeeLogin";
            this.Text = "员工管理";
            this.Load += new System.EventHandler(this.FormEmployeeLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMainTrackNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cobStatingNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cobEmployeeList;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.Button button1;
    }
}