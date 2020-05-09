using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Sus.Net.TestWinClient
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class DialogForm : Form
    {
        public DialogForm()
        {
            InitializeComponent();
            webBrowser1.AllowWebBrowserDrop = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;
            webBrowser1.ObjectForScripting = this;
            webBrowser1.Url = new Uri("https://www.baidu.com");
        }

        private void button1_Click(object sender,EventArgs e)
        {
            Close();
        }
    }
}
