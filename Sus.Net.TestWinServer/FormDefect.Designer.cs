namespace Sus.Net.TestWinServer
{
    partial class FormDefect
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
            this.txtDefectCode = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtHangerNo = new System.Windows.Forms.TextBox();
            this.txtStatingNo = new System.Windows.Forms.TextBox();
            this.txtMainTrackNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "疵点代码";
            // 
            // txtDefectCode
            // 
            this.txtDefectCode.Location = new System.Drawing.Point(175, 144);
            this.txtDefectCode.Name = "txtDefectCode";
            this.txtDefectCode.Size = new System.Drawing.Size(131, 21);
            this.txtDefectCode.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(135, 189);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtHangerNo
            // 
            this.txtHangerNo.Location = new System.Drawing.Point(175, 106);
            this.txtHangerNo.Name = "txtHangerNo";
            this.txtHangerNo.Size = new System.Drawing.Size(131, 21);
            this.txtHangerNo.TabIndex = 6;
            // 
            // txtStatingNo
            // 
            this.txtStatingNo.Location = new System.Drawing.Point(175, 71);
            this.txtStatingNo.Name = "txtStatingNo";
            this.txtStatingNo.Size = new System.Drawing.Size(131, 21);
            this.txtStatingNo.TabIndex = 7;
            // 
            // txtMainTrackNo
            // 
            this.txtMainTrackNo.Location = new System.Drawing.Point(175, 19);
            this.txtMainTrackNo.Name = "txtMainTrackNo";
            this.txtMainTrackNo.Size = new System.Drawing.Size(131, 21);
            this.txtMainTrackNo.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "衣架号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "站点";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "主轨";
            // 
            // FormDefect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 244);
            this.Controls.Add(this.txtHangerNo);
            this.Controls.Add(this.txtStatingNo);
            this.Controls.Add(this.txtMainTrackNo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtDefectCode);
            this.Controls.Add(this.label1);
            this.Name = "FormDefect";
            this.Text = "返工疵点";
            this.Load += new System.EventHandler(this.FormDefect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDefectCode;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtHangerNo;
        private System.Windows.Forms.TextBox txtStatingNo;
        private System.Windows.Forms.TextBox txtMainTrackNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}