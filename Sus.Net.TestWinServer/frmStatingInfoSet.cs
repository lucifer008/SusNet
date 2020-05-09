using Sus.Net.Server;
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
    public partial class frmStatingInfoSet : Form
    {
        public frmStatingInfoSet()
        {
            InitializeComponent();
        }
        SusTCPServer susServer;
        public frmStatingInfoSet(SusTCPServer server) : this()
        {
            this.susServer = server;
            BindGridColumn();
        }
        List<StatingInfo> statingInfoList;
        private void BindGridColumn()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Selected", TrueValue = true, FalseValue = false });
            // dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Text = "返工", UseColumnTextForButtonValue = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Index", HeaderText = "序号" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "MainTrackNo", HeaderText = "主轨" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "StatingNo", HeaderText = "站号" });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "HangerNo", HeaderText = "衣架号" });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "ProductNumber", HeaderText = "排产号" });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Content", HeaderText = "消息内容", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "DateTime", HeaderText = "时间", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            statingInfoList = SusTCPServer.DictStatingInfo.Values.ToList();
            dataGridView1.DataSource = new BindingList<StatingInfo>(statingInfoList);
            dataGridView1.Rows.Clear();
            dataGridView1.AllowUserToAddRows = false;
        }
        int index = 1;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //var list=SusTCPServer.DictStatingInfo.Values.ToList();
            statingInfoList.Add(new StatingInfo() { Index = index++ });
            dataGridView1.DataSource = new BindingList<StatingInfo>(statingInfoList);
            dataGridView1.Refresh();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // var count=dataGridView1.SelectedRows.Count;


            List<StatingInfo> selectList = new List<StatingInfo>();
            Console.Write("选中行：");
            for (int iRow = 0; iRow < dataGridView1.RowCount; ++iRow)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[iRow].Cells["Selected"].Value))
                {
                    //selectList.Add(dataGridView1.Rows[iRow].);
                    var stInfo = (StatingInfo)(dataGridView1.Rows[iRow]).DataBoundItem;
                    selectList.Add(stInfo);
                    // Console.Write(iRow.ToString() + ",");
                }
            }
            // Console.WriteLine();

            //if (!CheckDocumentStatus(selectList))
            //{
            //    return;
            //}
            //MessageBox.Show(selectList.Count.ToString());
            foreach (var sInfo in selectList)
            {
                var keys = string.Format("{0}:{1}", sInfo.MainTrackNo, sInfo.StatingNo);
                if (SusTCPServer.DictStatingInfo.ContainsKey(keys)) {
                    SusTCPServer.DictStatingInfo.Remove(keys);
                }
            }
            statingInfoList = SusTCPServer.DictStatingInfo.Values.ToList();
            dataGridView1.DataSource = new BindingList<StatingInfo>(statingInfoList);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<StatingInfo> selectList = new List<StatingInfo>();
            Console.Write("选中行：");
            for (int iRow = 0; iRow < dataGridView1.RowCount; ++iRow)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[iRow].Cells["Selected"].Value))
                {
                    //selectList.Add(dataGridView1.Rows[iRow].);
                    var stInfo = (StatingInfo)(dataGridView1.Rows[iRow]).DataBoundItem;
                    selectList.Add(stInfo);
                    // Console.Write(iRow.ToString() + ",");
                }
            }
            // Console.WriteLine();

            //if (!CheckDocumentStatus(selectList))
            //{
            //    return;
            //}
            //MessageBox.Show(selectList.Count.ToString());
            foreach (var sInfo in selectList)
            {
                if (string.IsNullOrEmpty(sInfo.MainTrackNo) || string.IsNullOrEmpty(sInfo.StatingNo)) {
                    MessageBox.Show("主轨或者站点为空!");
                    return;
                }
                var keys = string.Format("{0}:{1}", sInfo.MainTrackNo, sInfo.StatingNo);
                if (!SusTCPServer.DictStatingInfo.ContainsKey(keys))
                {
                    SusTCPServer.DictStatingInfo.Add(keys,sInfo);
                }
            }
            statingInfoList = SusTCPServer.DictStatingInfo.Values.ToList();
            dataGridView1.DataSource = new BindingList<StatingInfo>(statingInfoList);
            MessageBox.Show("sucess!");
        }
    }
}
