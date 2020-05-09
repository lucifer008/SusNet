using Sus.Net.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sus.Net.TestWinClient
{
    public partial class frmCANDeviceSearch : Form
    {
        public frmCANDeviceSearch()
        {
            InitializeComponent();
        }
        static int index = 1;
        void AddMessage(object sender, EventArgs e)
        {

            var data = string.Format("{0}--->{1}", index, sender.ToString());
            listBox1.Items.Add(data);
            index++;
        }

        private void btnPingTest_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string ip = this.ip;// "192.168.10.139";
                int port = this.port;
                UDPClient.Instance.Init(ip);
                var socket = UDPClient.Instance.Socket;
                var point = new IPEndPoint(IPAddress.Parse(ip), port);
                var content = "hello---->" + DateTime.Now.ToString("yyMMdd HH:mm.sss");
                socket.SendTo(Encoding.UTF8.GetBytes(content), point);
                //MessageBox.Show("发送完成!");
                AddMessage("发送内容:" + content, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSearchIP_Click_1(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


    }
}
