using Sus.Net.Common.Utils;
using Sus.Net.Server;
using SusNet2.Common.Model;
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
    public partial class FormDefect : Form
    {
        public FormDefect()
        {
            InitializeComponent();
        }
        private Hanger hanger;
        private SusTCPServer susTCPServer;
        public FormDefect(Hanger _hanger, SusTCPServer _tcpServer) :this() {
            this.hanger = _hanger;
            this.susTCPServer = _tcpServer;
        }

        private void FormDefect_Load(object sender, EventArgs e)
        {
            txtHangerNo.Text = hanger.HangerNo;
            txtMainTrackNo.Text = hanger.MainTrackNo;
            txtStatingNo.Text = hanger.StatingNo;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var defectCode = txtDefectCode.Text.Trim();
                if (string.IsNullOrEmpty(defectCode))
                {
                    MessageBox.Show("返工疵点代码不能为空!");
                    return;
                }
                //var dataWait = string.Format("{0} {1} 06 FF 00 55 {2} AA BB CC DD EE",hanger.MainTrackNo,hanger.StatingNo,hanger.ProductNumber);
                var dataWait = string.Format("{0} {1} 06 FF 00 57 {2} {3}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo),
                    HexHelper.tenToHexString(defectCode), StringUtils.ToFixLenStringFormat(HexHelper.tenToHexString(hanger.HangerNo)));
                var data = HexHelper.strToToHexByte(dataWait);
                susTCPServer.SendMessageToAll(data);
                MessageBox.Show("返工疵点指令已发送!");
                var defect = new FormDefect(hanger, susTCPServer);
                defect.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
