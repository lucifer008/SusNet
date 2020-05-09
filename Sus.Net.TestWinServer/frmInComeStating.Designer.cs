namespace Sus.Net.TestWinServer
{
    partial class frmInComeStating
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMainTrackNumber = new System.Windows.Forms.TextBox();
            this.txtStatingNo = new System.Windows.Forms.TextBox();
            this.txtHangerNo = new System.Windows.Forms.TextBox();
            this.btnIncomeStating = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "主轨:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "站点：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "衣架：";
            // 
            // txtMainTrackNumber
            // 
            this.txtMainTrackNumber.Location = new System.Drawing.Point(141, 36);
            this.txtMainTrackNumber.Name = "txtMainTrackNumber";
            this.txtMainTrackNumber.Size = new System.Drawing.Size(140, 21);
            this.txtMainTrackNumber.TabIndex = 1;
            this.txtMainTrackNumber.Text = "1";
            // 
            // txtStatingNo
            // 
            this.txtStatingNo.Location = new System.Drawing.Point(141, 78);
            this.txtStatingNo.Name = "txtStatingNo";
            this.txtStatingNo.Size = new System.Drawing.Size(140, 21);
            this.txtStatingNo.TabIndex = 1;
            this.txtStatingNo.Text = "4";
            // 
            // txtHangerNo
            // 
            this.txtHangerNo.Location = new System.Drawing.Point(141, 112);
            this.txtHangerNo.Name = "txtHangerNo";
            this.txtHangerNo.Size = new System.Drawing.Size(140, 21);
            this.txtHangerNo.TabIndex = 1;
            // 
            // btnIncomeStating
            // 
            this.btnIncomeStating.Location = new System.Drawing.Point(62, 207);
            this.btnIncomeStating.Name = "btnIncomeStating";
            this.btnIncomeStating.Size = new System.Drawing.Size(75, 23);
            this.btnIncomeStating.TabIndex = 2;
            this.btnIncomeStating.Text = "进站";
            this.btnIncomeStating.UseVisualStyleBackColor = true;
            this.btnIncomeStating.Click += new System.EventHandler(this.btnIncomeStating_Click);
            // 
            // frmInComeStating
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 383);
            this.Controls.Add(this.btnIncomeStating);
            this.Controls.Add(this.txtHangerNo);
            this.Controls.Add(this.txtStatingNo);
            this.Controls.Add(this.txtMainTrackNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmInComeStating";
            this.Text = "frmInComeStating";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMainTrackNumber;
        private System.Windows.Forms.TextBox txtStatingNo;
        private System.Windows.Forms.TextBox txtHangerNo;
        private System.Windows.Forms.Button btnIncomeStating;
    }
}