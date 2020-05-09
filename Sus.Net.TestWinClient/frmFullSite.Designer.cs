namespace Sus.Net.TestWinServer
{
    partial class frmFullSite
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
            this.btnFullSiteOrNot = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStatingNo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnFullSiteOrNot
            // 
            this.btnFullSiteOrNot.Location = new System.Drawing.Point(111, 180);
            this.btnFullSiteOrNot.Name = "btnFullSiteOrNot";
            this.btnFullSiteOrNot.Size = new System.Drawing.Size(167, 23);
            this.btnFullSiteOrNot.TabIndex = 0;
            this.btnFullSiteOrNot.Text = "满站/否";
            this.btnFullSiteOrNot.UseVisualStyleBackColor = true;
            this.btnFullSiteOrNot.Click += new System.EventHandler(this.btnFullSiteOrNot_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "站号";
            // 
            // txtStatingNo
            // 
            this.txtStatingNo.Location = new System.Drawing.Point(111, 45);
            this.txtStatingNo.Name = "txtStatingNo";
            this.txtStatingNo.Size = new System.Drawing.Size(100, 21);
            this.txtStatingNo.TabIndex = 3;
            // 
            // frmFullSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 280);
            this.Controls.Add(this.txtStatingNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFullSiteOrNot);
            this.Name = "frmFullSite";
            this.Text = "frmFullSite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFullSiteOrNot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStatingNo;
    }
}