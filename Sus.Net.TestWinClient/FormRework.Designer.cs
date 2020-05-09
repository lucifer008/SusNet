namespace Sus.Net.TestWinServer
{
    partial class FormRework
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
            this.txtReworkFlowCode = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMainTrackNo = new System.Windows.Forms.TextBox();
            this.txtStatingNo = new System.Windows.Forms.TextBox();
            this.txtHangerNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "返工工序代码及疵点代码";
            // 
            // txtReworkFlowCode
            // 
            this.txtReworkFlowCode.Location = new System.Drawing.Point(208, 160);
            this.txtReworkFlowCode.Multiline = true;
            this.txtReworkFlowCode.Name = "txtReworkFlowCode";
            this.txtReworkFlowCode.Size = new System.Drawing.Size(173, 52);
            this.txtReworkFlowCode.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(160, 218);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "主轨";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(92, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "站点";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(92, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "衣架号";
            // 
            // txtMainTrackNo
            // 
            this.txtMainTrackNo.Location = new System.Drawing.Point(208, 34);
            this.txtMainTrackNo.Name = "txtMainTrackNo";
            this.txtMainTrackNo.Size = new System.Drawing.Size(131, 21);
            this.txtMainTrackNo.TabIndex = 1;
            // 
            // txtStatingNo
            // 
            this.txtStatingNo.Location = new System.Drawing.Point(208, 86);
            this.txtStatingNo.Name = "txtStatingNo";
            this.txtStatingNo.Size = new System.Drawing.Size(131, 21);
            this.txtStatingNo.TabIndex = 1;
            // 
            // txtHangerNo
            // 
            this.txtHangerNo.Location = new System.Drawing.Point(208, 121);
            this.txtHangerNo.Name = "txtHangerNo";
            this.txtHangerNo.Size = new System.Drawing.Size(131, 21);
            this.txtHangerNo.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(452, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "格式【工序1，疵点1；工序2，疵点2】";
            // 
            // FormRework
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 301);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtHangerNo);
            this.Controls.Add(this.txtStatingNo);
            this.Controls.Add(this.txtMainTrackNo);
            this.Controls.Add(this.txtReworkFlowCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Name = "FormRework";
            this.Text = "返工";
            this.Load += new System.EventHandler(this.FormRework_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReworkFlowCode;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMainTrackNo;
        private System.Windows.Forms.TextBox txtStatingNo;
        private System.Windows.Forms.TextBox txtHangerNo;
        private System.Windows.Forms.Label label5;
    }
}