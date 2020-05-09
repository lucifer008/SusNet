using Sus.Net.Client;
using Sus.Net.TestWinClient;
using SusNet2.Common.Utils;
using System;
using System.Windows.Forms;

namespace Sus.Net.TestWinServer
{
    public partial class FormStating : Form
    {
        // private SusTCPServer _server;
        private SusTCPClient susTCPClient;
        public FormStating(SusTCPClient _susTCPClient)
        {
            this.susTCPClient = _susTCPClient;
            InitializeComponent();
        }

        private void btnModifyCapacity_Click(object sender, EventArgs e)
        {
            string xid = txtMainTrackNumber.Text.Trim().PadLeft(2, '0');
            string no = txtStatingNo.Text.Trim().PadLeft(2, '0');
            string cmd = txtCmd.Text.Trim().PadLeft(2, '0');
            string data = HexHelper.TenToHexString4Len(int.Parse(txtCapacity.Text.Trim()));

            string msg = string.Format("{0} {1} {2} FF 00 33 00 00 00 00 {3} {4}", xid, no, cmd, data.Substring(0,2), data.Substring(2,2));
            susTCPClient.SendData(HexHelper.strToToHexByte(msg));

        }
    }
}
