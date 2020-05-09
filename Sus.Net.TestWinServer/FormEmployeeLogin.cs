using Sus.Net.Common.Utils;
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
    public partial class FormEmployeeLogin : Form
    {
        SusTCPServer susTCPServer;
        public FormEmployeeLogin(SusTCPServer tcpServer)
        {
            InitializeComponent();
            susTCPServer = tcpServer;
        }

        private void FormEmployeeLogin_Load(object sender, EventArgs e)
        {
            var data = new DataTable();
            data.Columns.Add("carNo");
            data.Columns.Add("EmployeeName");

            var cardList = new List<string>() { "1003479933", "1003479934", "1003479935", "1003479936" };
            var employeeList = new List<string>() { "张三0", "张三1", "张三2", "张三3" };

            for (var index = 0; index < 4; index++)
            {
                var dr = data.NewRow();
                dr["carNo"] = cardList[index];
                dr["EmployeeName"] = employeeList[index] + "__" + cardList[index];
                data.Rows.Add(dr);
            }

            cobEmployeeList.ValueMember = "carNo";
            cobEmployeeList.DisplayMember = "EmployeeName";
            cobEmployeeList.DataSource = data;
            cobEmployeeList.SelectedValue = "1003479933";
            cobStatingNo.SelectedIndex = 0;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var mainTrackNo = txtMainTrackNo.Text.Trim();
            var statingNo = cobStatingNo.Text?.Trim();
            var cardNo = cobEmployeeList.SelectedValue?.ToString()?.Trim();
            //01 04 06 XX 00 60 00 AA BB CC DD EE
            var fData = string.Format("{0} {1} 06 00 00 60 00 {2}", HexHelper.tenToHexString(mainTrackNo), HexHelper.tenToHexString(statingNo), StringUtils.ToFixLenStringFormat(HexHelper.tenToHexString(cardNo)));
            var data = HexHelper.strToToHexByte(fData);
            susTCPServer.SendMessageToAll(data);
            MessageBox.Show("指令已发送!");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mainTrackNo = txtMainTrackNo.Text.Trim();
            var statingNo = cobStatingNo.Text?.Trim();
            for (var index = 1; index < 10; index++)
            {
                var cardNo = cobEmployeeList.SelectedValue?.ToString()?.Trim();
                //01 04 06 XX 00 60 00 AA BB CC DD EE
                var fData = string.Format("{0} {1} 06 00 00 60 00 {2}", HexHelper.tenToHexString(mainTrackNo), HexHelper.tenToHexString(index), StringUtils.ToFixLenStringFormat(HexHelper.tenToHexString(cardNo)));
                var data = HexHelper.strToToHexByte(fData);
                susTCPServer.SendMessageToAll(data);
            }
            MessageBox.Show("指令已发送!");
        }
    }
}
