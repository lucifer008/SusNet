using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sus.Net.Server;
using SusNet2.Common.Utils;

namespace Sus.Net.TestWinServer
{
    public partial class frmInComeStating : Form
    {
        private SusTCPServer server;

        public frmInComeStating()
        {
            InitializeComponent();
        }

        public frmInComeStating(SusTCPServer server):this()
        {
            this.server = server;
        }

        private void btnIncomeStating_Click(object sender, EventArgs e)
        {
            try
            {
                var mainTrackNumber = HexHelper.TenToHexString2Len(txtMainTrackNumber.Text);
                var statingNo = HexHelper.TenToHexString2Len(txtStatingNo.Text);
                var hangerNo = HexHelper.TenToHexString10Len(txtHangerNo.Text);

                btnIncomeStating.Cursor = Cursors.WaitCursor;
                var data = HexHelper.strToToHexByte(string.Format("{0} {1} 02 FF 00 50 00 {2}", mainTrackNumber,statingNo,hangerNo));
                server.SendMessageToAll(data);


                MessageBox.Show("sucess!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnIncomeStating.Cursor = Cursors.Default;
            }
        }
    }
}
