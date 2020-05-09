using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WinFormListviewAddContrl
{
    //msg=0x115   (WM_VSCROLL)     
    //msg=0x114   (WM_HSCROLL)   
    ///   <summary>   
    ///   CListView   的摘要说明。   
    ///   </summary>   
    public class ListViewEx : ListView
    {
        private TextBox m_tb;
        private ComboBox m_cb;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ShowScrollBar(IntPtr hWnd, int iBar, int bShow);
        const int SB_HORZ = 0;
        const int SB_VERT = 1;
        public ListViewEx()
        {
            m_tb = new TextBox();
            m_cb = new ComboBox();
            m_tb.Multiline = false;
            m_tb.Visible = false;
            m_cb.Visible = false;
            m_cb.FlatStyle = FlatStyle.Popup;

            this.GridLines = true;
            this.FullRowSelect = true;
            this.Controls.Add(m_tb);
            this.Controls.Add(m_cb);
        }
        private void EditItem(ListViewItem  subItem)
        {
            if (this.SelectedItems.Count <= 0)
            {
                return;
            }

            Rectangle _rect = subItem.Bounds;
            //Graphics g = this.CreateGraphics();
            //g.DrawRectangle(Pens.Black,_rect);   

            m_tb.Bounds = new Rectangle(_rect.Left + 2, _rect.Top + 1, _rect.Width - 3, _rect.Height - 3);//避免网格被遮掉
            m_tb.BorderStyle = BorderStyle.None;
            m_tb.BringToFront();
            m_tb.Text = subItem.Text;
            m_tb.Leave += new EventHandler(tb_Leave);
            m_tb.TextChanged += new EventHandler(m_tb_TextChanged);
            m_tb.Visible = true;
            m_tb.Tag = subItem;
            m_tb.Select();
        }
        private void EditItem(ListViewItem.ListViewSubItem subItem, Rectangle rt)
        {
            if (this.SelectedItems.Count <= 0)
            {
                return;
            }

            Rectangle _rect = rt;
            m_cb.Bounds = _rect;
            m_cb.BringToFront();
            m_cb.Items.Add(subItem.Text);
            m_cb.Text = subItem.Text;
            m_cb.Leave += new EventHandler(lstb_Leave);
            m_cb.TextChanged += new EventHandler(m_lstb_TextChanged);
            m_cb.Visible = true;
            m_cb.Tag = subItem;
            m_cb.Select();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {

                if (this.SelectedItems.Count > 0)
                {
                    //this.SelectedItems[0].BeginEdit();
                    ListViewItem lvi = this.SelectedItems[0];
                    EditItem(lvi.SubItems[0], new Rectangle(lvi.Bounds.Left, lvi.Bounds.Top, this.Columns[0].Width, lvi.Bounds.Height - 5));
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            this.m_tb.Visible = false;
            this.m_cb.Visible = false;
            base.OnSelectedIndexChanged(e);
        }


        protected override void OnDoubleClick(EventArgs e)
        {
            Point tmpPoint = this.PointToClient(Cursor.Position);
           var item = this.HitTest(tmpPoint).Item;
            ListViewItem item1 = this.HitTest(tmpPoint).Item;
            //if (item1 != null)
            //{
            //    if (item.SubItems[0].Equals(subitem))
            //    {
            //        EditItem(subitem, new Rectangle(item.Bounds.Left, item.Bounds.Top, this.Columns[0].Width, item.Bounds.Height));
            //    }
            //    else
            //    {
            //        EditItem(subitem);
            //    }
            //}
            if(null!= item1)
                EditItem(item1);
            base.OnDoubleClick(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x115 || m.Msg == 0x114)
            {
                this.m_tb.Visible = false;
            }
            if (this.View == View.List)
            {
                ShowScrollBar(this.Handle, SB_VERT, 1);
                ShowScrollBar(this.Handle, SB_HORZ, 0);
            }
            base.WndProc(ref m);
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            m_tb.TextChanged -= new EventHandler(m_tb_TextChanged);
            (sender as TextBox).Visible = false;
        }

        private void m_tb_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Tag is ListViewItem.ListViewSubItem)
            {
                (this.m_tb.Tag as ListViewItem.ListViewSubItem).Text = this.m_tb.Text;
            }

        }
        private void lstb_Leave(object sender, EventArgs e)
        {
            m_cb.TextChanged -= new EventHandler(m_lstb_TextChanged);
        }
        private void m_lstb_TextChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox) != null)
            {
                if ((sender as ComboBox).Tag is ListViewItem.ListViewSubItem)
                {
                    (this.m_cb.Tag as ListViewItem.ListViewSubItem).Text = this.m_cb.Text;
                }
            }
        }
    }
}
