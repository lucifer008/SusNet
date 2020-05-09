using Sus.Net.Server;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sus.Net.TestWinServer
{
    public partial class FormStating : Form
    {
        private SusTCPServer _server;
        public FormStating(SusTCPServer server)
        {
            this._server = server;
            InitializeComponent();
        }

        private void btnModifyCapacity_Click(object sender, EventArgs e)
        {
            string xid = txtMainTrackNumber.Text.Trim().PadLeft(2, '0');
            string no = txtStatingNo.Text.Trim().PadLeft(2, '0');
            string cmd = txtCmd.Text.Trim().PadLeft(2, '0');
            string data = HexHelper.TenToHexString4Len(int.Parse(txtCapacity.Text.Trim()));

            string msg = string.Format("{0} {1} {2} FF 00 33 00 00 00 00 {3} {4}", xid, no, cmd, data.Substring(0,2), data.Substring(2,2));
            _server.SendMessageToAll(HexHelper.strToToHexByte(msg));

        }
    }
}
