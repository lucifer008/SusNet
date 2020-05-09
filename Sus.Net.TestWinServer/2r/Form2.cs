using Sus.Net.Server;
using SusNet2.Common.Utils;
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

namespace Sus.Net.TestWinServer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SusTCPServer susTCPServer;
        public Form2(SusTCPServer _susTCPServer) : this() {
            this.susTCPServer = _susTCPServer;
        }
        Thread td;
        private void btnStart_Click(object sender, EventArgs e)
        {
            //susTCPServer.SendMessageToAll();
            btnStart.Enabled = false;
            var dd = new data() { beginNum=int.Parse(txtBeginNum.Text),endNum=int.Parse(txtEndNum.Text),times=0,server=susTCPServer };
            td = new Thread(new ThreadStart(dd.SendData2));
            td.Start();
        }
     
        private void btnStop_Click(object sender, EventArgs e)
        {
            td.Abort();
            btnStart.Enabled = true;
        }
    }
    class data {
        public SusTCPServer server { set; get; }
        public int beginNum { set; get; }
        public int endNum { set; get; }
        public int times { set; get; }
        public void SendData2()
        {
            // var data = HexHelper.strToToHexByte("01 00 03 FF 00 06 00 00 00 00 00 10");
            for (int i = beginNum; i < endNum; i++)
            {
                //var data = HexHelper.strToToHexByte("01 00 03 FF 00 55 01 {0}");
                var hexHangerNo = HexHelper.TenToHexString10Len(i);
                var dataWait = string.Format("{0} {1} 06 FF 00 55 {2} {3}", HexHelper.tenToHexString(1), HexHelper.tenToHexString(4), "01", hexHangerNo);
                server.SendMessageToAll(HexHelper.strToToHexByte(dataWait));
                Thread.Sleep(200);
            }

        }
    }
}
