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
    public partial class FormHangingPieceStating : Form
    {
        public FormHangingPieceStating()
        {
            InitializeComponent();
        }
        List<Hanger> hangerList;
        SusTCPServer susTCPServer;
        public FormHangingPieceStating(byte[] bb, List<Hanger> _hangerList, SusTCPServer tcpServer, string statingNo) : this()
        {
            hangerList = _hangerList;
            susTCPServer = tcpServer;
            BindGridColumn();
            this.Text = string.Format("挂片站【{0}】", statingNo);
        }

        private void BindGridColumn()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Text = "放衣架读卡", UseColumnTextForButtonValue = true });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Text = "挂片站上线", UseColumnTextForButtonValue = true });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Text = "出战", UseColumnTextForButtonValue = true });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Text = "", UseColumnTextForButtonValue = true });
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
                //MessageBox.Show(e.ColumnIndex + " " + e.RowIndex);

                var list = dataGridView1.DataSource as List<Hanger>;
                var hanger = list[e.RowIndex];

                //放衣架读卡发送给软件，要求回显上线信息
                if (e.ColumnIndex == 0)
                {

                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        if (string.IsNullOrEmpty(hanger.HangerNo?.Trim()))
                        {
                            MessageBox.Show("衣架号不能为空!不能出战!，操作无效!");
                            return;
                        }
                        var hexHangerNo = HexHelper.TenToHexString10Len(hanger.HangerNo);
                        var fData = string.Format("{0} {1} 06 FF 00 54 01 {2}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo), hexHangerNo);
                        var data = HexHelper.strToToHexByte(fData);
                        susTCPServer.SendMessageToAll(data);
                        MessageBox.Show("指令已发送!");
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
                if (e.ColumnIndex == 1)
                {

                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        //if (string.IsNullOrEmpty(hanger.HangerNo?.Trim()) || hanger.HangerNo.Replace(" ", "").Length != 10)
                        //{
                        //    MessageBox.Show("衣架号不能为空或者长度不够10位!不能出战!，操作无效!");
                        //    return;
                        //}
                        var fData = string.Format("{0} {1} 06 FF 00 35 00 00 00 00 00 {2}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo),HexHelper.tenToHexString(hanger.ProductNumber));
                        var data = HexHelper.strToToHexByte(fData);
                        susTCPServer.SendMessageToAll(data);
                        MessageBox.Show("指令已发送!");
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
                //按出键操作让衣架出站
                if (e.ColumnIndex == 2)
                {
                    if (list.Count > 0)
                    {
                        // var hanger = list[e.RowIndex];
                        if (string.IsNullOrEmpty(hanger.HangerNo?.Trim()))
                        {
                            MessageBox.Show("衣架号不能为空!不能出战!，操作无效!");
                            return;
                        }
                        else
                        {
                            try
                            {
                                Cursor = Cursors.WaitCursor;
                                var hexHangerNo = HexHelper.TenToHexString10Len(hanger.HangerNo);
                                //var dataWait = string.Format("{0} {1} 06 FF 00 55 {2} AA BB CC DD EE",hanger.MainTrackNo,hanger.StatingNo,hanger.ProductNumber);
                                var dataWait = string.Format("{0} {1} 06 FF 00 55 {2} {3}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo), HexHelper.tenToHexString("01"), hexHangerNo);
                                // var dataWait = string.Format("{0} {1} 06 FF 00 59 {2} {3}", HexHelper.tenToHexString(hanger.MainTrackNo), HexHelper.tenToHexString(hanger.StatingNo), HexHelper.tenToHexString(hanger.ProductNumber), StringUtils.ToFixLenStringFormat(HexHelper.tenToHexString(hanger.HangerNo)));
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
                    //out

                }
                if (e.ColumnIndex == 7)
                {

                }
            }
        }
    }
}
