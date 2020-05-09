using Sus.Net.Client;
using Sus.Net.Common.Utils;
using Sus.Net.TestWinClient;
using SusNet2.Common.Model;
using SusNet2.Common.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sus.Net.TestWinServer
{
    public partial class FormRework : Form
    {
        public FormRework()
        {
            InitializeComponent();
        }
        private Hanger hanger;
        // private SusTCPServer susTCPServer;
        private SusTCPClient susTCPClient;
        public FormRework(Hanger _hanger, SusTCPClient _tcpClient) : this()
        {
            this.hanger = _hanger;
            this.susTCPClient = _tcpClient;
        }
        const int beginAddress = 0x0090;
        const int reciprocalAddress = 0x0098;
        const int endAddress = 0x0099;
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var flowOrDefectCodes = txtReworkFlowCode.Text.Trim();
                if (string.IsNullOrEmpty(flowOrDefectCodes))
                {
                    MessageBox.Show("返工工序代码及疵点代码不能为空!");
                    return;
                }
                //var dataWait = string.Format("{0} {1} 06 FF 00 55 {2} AA BB CC DD EE",hanger.MainTrackNo,hanger.StatingNo,hanger.ProductNumber);
                var dataWait = string.Format("{0} {1} 06 FF 00 56 00 {2}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo),
                     StringUtils.ToFixLenStringFormat(HexHelper.tenToHexString(hanger.HangerNo)));
                var data = HexHelper.strToToHexByte(dataWait);
                //susTCPServer.SendMessageToAll(data);
                susTCPClient.SendData(data);

                var j = 0;
                var bytesFlowDefect = AssicUtils.EncodeByStr(flowOrDefectCodes);
                var times = bytesFlowDefect.Length % 6 == 0 ? bytesFlowDefect.Length / 6 : (bytesFlowDefect.Length / 6 + 1);
                var address = beginAddress;
                for (var t=0;t<times;t++)
                {
                    if (address > reciprocalAddress) {
                        break;
                    }
                    var dataList = new List<byte>();
                    var header = string.Format("{0} {1} 06 FF {2}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo), HexHelper.TenToHexString4Len(address));
                    dataList.AddRange(HexHelper.strToToHexByte(header));
                    if (j < bytesFlowDefect.Length)
                    {
                        for (int b = j; j < bytesFlowDefect.Length; j++)
                        {
                            if (dataList.Count == 12)
                            {
                                break;
                            }
                            dataList.Add(bytesFlowDefect[j]);
                        }
                    }
                    var teLen = dataList.Count;
                    for (var ii = 0; ii < 12 - teLen; ii++)
                    {
                        if (dataList.Count == 12)
                        {
                            break;
                        }
                        //数据位不足补FF
                        dataList.AddRange(AssicUtils.EncodeByStr("@"));
                    }
                    // susTCPServer.SendMessageToAll(dataList.ToArray());
                    susTCPClient.SendData(dataList.ToArray());
                    address++;
                }
                var endHeaderMessage = string.Format("{0} {1} 06 FF 00 99", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo),
                     StringUtils.ToFixLenStringFormat(HexHelper.tenToHexString(hanger.HangerNo)));
                var endList = new List<byte>();
                endList.AddRange(HexHelper.strToToHexByte(endHeaderMessage));
                endList.AddRange(AssicUtils.EncodeByStr("000000"));
                // susTCPServer.SendMessageToAll(endList.ToArray());
                susTCPClient.SendData(endList.ToArray());
                MessageBox.Show("返工指令已发送!");
                //var defect = new FormDefect(hanger, susTCPServer);
                //defect.ShowDialog();
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

        private void FormRework_Load(object sender, EventArgs e)
        {
            txtHangerNo.Text = hanger.HangerNo;
            txtMainTrackNo.Text = hanger.MainTrackNo;
            txtStatingNo.Text = hanger.StatingNo;

        }
    }
}
