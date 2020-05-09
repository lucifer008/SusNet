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

namespace Sus.Net.TestWinClient
{
    public partial class frmInComeStataing : Form
    {
        private SusTCPClient client;

        public frmInComeStataing()
        {
            InitializeComponent();
        }

        public frmInComeStataing(SusTCPClient client):this()
        {
            this.client = client;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                btnSend.Cursor = Cursors.WaitCursor;
                var hexHangerNo = HexHelper.TenToHexString10Len(txtHangerNo.Text.Trim());
                var statingNo = HexHelper.TenToHexString2Len(txtStatingNo.Text);
                var mainTrackNumber = HexHelper.TenToHexString2Len(txtMainTrackNumber.Text);
                var message = string.Format("{0} {1} 02 FF 00 50 00 {2}",mainTrackNumber,statingNo,hexHangerNo);
                var dBytes1 = HexHelper.strToToHexByte(message);
                client.SendData(dBytes1);
                MessageBox.Show("成功!");
            }
            finally
            {
                btnSend.Cursor = Cursors.Default;
            }
        }
    }
}
