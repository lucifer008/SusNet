namespace Sus.Net.TestWinServer
{
    partial class frmMontorUpload
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
            this.txtMainTrackNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStatingNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHangerNo = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "主轨";
            // 
            // txtMainTrackNumber
            // 
            this.txtMainTrackNumber.Location = new System.Drawing.Point(121, 34);
            this.txtMainTrackNumber.Name = "txtMainTrackNumber";
            this.txtMainTrackNumber.Size = new System.Drawing.Size(117, 21);
            this.txtMainTrackNumber.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "站点";
            // 
            // txtStatingNo
            // 
            this.txtStatingNo.Location = new System.Drawing.Point(121, 75);
            this.txtStatingNo.Name = "txtStatingNo";
            this.txtStatingNo.Size = new System.Drawing.Size(117, 21);
            this.txtStatingNo.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "衣架";
            // 
            // txtHangerNo
            // 
            this.txtHangerNo.Location = new System.Drawing.Point(121, 118);
            this.txtHangerNo.Name = "txtHangerNo";
            this.txtHangerNo.Size = new System.Drawing.Size(117, 21);
            this.txtHangerNo.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(95, 198);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "推送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // frmMontorUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 476);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtHangerNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStatingNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMainTrackNumber);
            this.Controls.Add(this.label1);
            this.Name = "frmMontorUpload";
            this.Text = "监测上传";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMainTrackNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStatingNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHangerNo;
        private System.Windows.Forms.Button btnSend;
    }
}