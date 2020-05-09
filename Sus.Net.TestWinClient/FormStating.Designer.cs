namespace Sus.Net.TestWinServer
{
    partial class FormStating
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMainTrackNumber = new System.Windows.Forms.TextBox();
            this.txtStatingNo = new System.Windows.Forms.TextBox();
            this.txtCapacity = new System.Windows.Forms.TextBox();
            this.btnModifyCapacity = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnModifyCapacity);
            this.groupBox1.Controls.Add(this.txtCapacity);
            this.groupBox1.Controls.Add(this.txtCmd);
            this.groupBox1.Controls.Add(this.txtStatingNo);
            this.groupBox1.Controls.Add(this.txtMainTrackNumber);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(35, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(654, 119);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "容量";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "主轨";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "站号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(491, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "容量";
            // 
            // txtMainTrackNumber
            // 
            this.txtMainTrackNumber.Location = new System.Drawing.Point(55, 18);
            this.txtMainTrackNumber.Name = "txtMainTrackNumber";
            this.txtMainTrackNumber.Size = new System.Drawing.Size(100, 21);
            this.txtMainTrackNumber.TabIndex = 1;
            this.txtMainTrackNumber.Text = "1";
            // 
            // txtStatingNo
            // 
            this.txtStatingNo.Location = new System.Drawing.Point(208, 18);
            this.txtStatingNo.Name = "txtStatingNo";
            this.txtStatingNo.Size = new System.Drawing.Size(100, 21);
            this.txtStatingNo.TabIndex = 1;
            this.txtStatingNo.Text = "1";
            // 
            // txtCapacity
            // 
            this.txtCapacity.Location = new System.Drawing.Point(536, 18);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.Size = new System.Drawing.Size(100, 21);
            this.txtCapacity.TabIndex = 1;
            this.txtCapacity.Text = "1";
            // 
            // btnModifyCapacity
            // 
            this.btnModifyCapacity.Location = new System.Drawing.Point(561, 68);
            this.btnModifyCapacity.Name = "btnModifyCapacity";
            this.btnModifyCapacity.Size = new System.Drawing.Size(75, 23);
            this.btnModifyCapacity.TabIndex = 2;
            this.btnModifyCapacity.Text = "发送";
            this.btnModifyCapacity.UseVisualStyleBackColor = true;
            this.btnModifyCapacity.Click += new System.EventHandler(this.btnModifyCapacity_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "命令";
            // 
            // txtCmd
            // 
            this.txtCmd.Location = new System.Drawing.Point(369, 18);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(100, 21);
            this.txtCmd.TabIndex = 1;
            this.txtCmd.Text = "04";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(257, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "04 ： 设置站容量（液晶设置时，主动上传PC）";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "06 ： 设置站容量（回复）";
            // 
            // FormStating
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 390);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormStating";
            this.Text = "站点相关";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnModifyCapacity;
        private System.Windows.Forms.TextBox txtCapacity;
        private System.Windows.Forms.TextBox txtStatingNo;
        private System.Windows.Forms.TextBox txtMainTrackNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.Label label4;
    }
}