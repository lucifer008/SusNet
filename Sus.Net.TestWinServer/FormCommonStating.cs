
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
    public partial class FormCommonStating : Form
    {
        public FormCommonStating()
        {
            InitializeComponent();
        }
        List<Hanger> hangerList;
        SusTCPServer susTCPServer;
        public FormCommonStating(byte[] bb,List<Hanger> _hangerList, SusTCPServer tcpServer,String statingNo, frmServer fServer ) : this()
        {
            MessageBox.Show("比较衣架工序在此对话框确定后进行！");
            var current = _hangerList[_hangerList.Count - 1];
            var fData = string.Format("{0} {1} 06 FF 00 54 00 {2}", HexHelper.tenToHexString(current.MainTrackNo), HexHelper.tenToHexString(current.StatingNo), StringUtils.ToFixLenStringFormat(HexHelper.tenToHexString(current.HangerNo)));
            var data = HexHelper.strToToHexByte(fData);
            tcpServer.SendMessageToAll(data);

            var modelCompareFlow = new MessageModel() {
                lvMessage=fServer.LvAllMessage,
                Message=string.Format("【比较工序指令发送成功！发送内容:{0}】", fData)
            };
            fServer.AddMessage(modelCompareFlow, null);

            hangerList = _hangerList;
            susTCPServer = tcpServer;
            BindGridColumn();
            this.Text =string.Format("普通站【{0}】", statingNo);
        }

        private void BindGridColumn()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Text = "出战", UseColumnTextForButtonValue = true });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Text = "返工", UseColumnTextForButtonValue = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Index", HeaderText = "序号" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "MainTrackNo", HeaderText = "主轨" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "StatingNo", HeaderText = "站号" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "HangerNo", HeaderText = "衣架号" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "ProductNumber", HeaderText = "排产号" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Content", HeaderText = "消息内容", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "DateTime", HeaderText = "时间", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView1.DataSource = hangerList;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
               
                if (e.ColumnIndex == 0)
                {
                    var list = dataGridView1.DataSource as List<Hanger>;
                    if (list.Count > 0)
                    {
                        var hanger = list[e.RowIndex];
                        if (string.IsNullOrEmpty(hanger.HangerNo?.Trim()))
                        {
                            MessageBox.Show("衣架号不能为空，不能出战!，操作无效!");
                            return;
                        }
                        else
                        {
                            try
                            {
                                Cursor = Cursors.WaitCursor;

                                //var dataWait = string.Format("{0} {1} 06 FF 00 55 {2} AA BB CC DD EE",hanger.MainTrackNo,hanger.StatingNo,hanger.ProductNumber);
                                var hexHangerNo = HexHelper.TenToHexString10Len(hanger.HangerNo);
                                var dataWait = string.Format("{0} {1} 06 FF 00 55 {2} {3}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo), "00", hexHangerNo);
                                var data = HexHelper.strToToHexByte(dataWait);
                                susTCPServer.SendMessageToAll(data);
                                MessageBox.Show("出战指令已发送!");
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
                if (e.ColumnIndex == 1)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        var list = dataGridView1.DataSource as List<Hanger>;
                        var hanger = list[e.RowIndex];
                        var rework = new FormRework(hanger, susTCPServer);
                        rework.Show();
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
    }
}
