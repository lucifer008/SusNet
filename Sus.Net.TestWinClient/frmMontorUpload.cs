using Sus.Net.Client;
using Sus.Net.TestWinClient;
using SusNet2.Common.Utils;
using System;
using System.Windows.Forms;

namespace Sus.Net.TestWinServer
{
    public partial class frmMontorUpload : Form
    {
        public frmMontorUpload()
        {
            InitializeComponent();
        }
        //private SusTCPServer _server;
        SusTCPClient client;
        public frmMontorUpload(SusTCPClient client) : this() {
            this.client = client;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            var mainTrackNumber = txtMainTrackNumber.Text;
            var hexHangerNo = HexHelper.TenToHexString10Len(txtHangerNo.Text);
            var statingNo = txtStatingNo.Text;

            var dataWait = string.Format("{0} {1} 02 FF 00 06 {2} {3}", HexHelper.tenToHexString(mainTrackNumber), HexHelper.tenToHexString(statingNo), "00", hexHangerNo);
            var data = HexHelper.strToToHexByte(dataWait);
            client.SendData(data);
            MessageBox.Show("出战指令已发送!");
        }
    }
}
