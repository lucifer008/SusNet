using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SusNet.Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            WaitDialogForm waitDialogForm = new WaitDialogForm("请稍后...", "正在加载系统应用", new Size(300, 40), this);
            waitDialogForm.Visible = false;
            new Thread(test).Start(waitDialogForm);
            waitDialogForm.ShowDialog();
        }
        void test(object data) {
           // Thread.Sleep(5000);
            var waitDialogForm = data as WaitDialogForm;
            waitDialogForm.Invoke((EventHandler)delegate { waitDialogForm.Close(); });
        }
    }
}
