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
    public partial class frmFullSite : Form
    {
        public frmFullSite()
        {
            InitializeComponent();
        }
        private SusTCPServer _server;
        public frmFullSite(SusTCPServer server) : this() {
            this._server = server;
        }
        bool isFullSite = false;
        private void btnFullSiteOrNot_Click(object sender, EventArgs e)
        {
            var statingNo = HexHelper.tenToHexString(txtStatingNo.Text);
            if (!isFullSite)
            {
                isFullSite = true;
                try
                {
                   
                    btnFullSiteOrNot.Cursor = Cursors.WaitCursor;
                    var data = HexHelper.strToToHexByte(string.Format("01 {0} 06 FF 00 1B 00 00 00 00 00 01",statingNo));
                    _server.SendMessageToAll(data);


                    MessageBox.Show("sucess!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    btnFullSiteOrNot.Cursor = Cursors.Default;
                }
            }
            else
            {
                isFullSite = false;
                try
                {
                    btnFullSiteOrNot.Cursor = Cursors.WaitCursor;
                    var data = HexHelper.strToToHexByte(string.Format("01 {0} 06 FF 00 1B 00 00 00 00 00 00",statingNo));
                    _server.SendMessageToAll(data);


                    MessageBox.Show("sucess!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    btnFullSiteOrNot.Cursor = Cursors.Default;
                }
            }
        }
    }
}
