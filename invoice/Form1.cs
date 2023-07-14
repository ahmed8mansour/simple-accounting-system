using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invoice
{
    public partial class Form1 : Form
    {
        public  Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtdate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            Dictionary<int, string> itemsdata = new Dictionary<int, string>();
            itemsdata.Add(100, "كيبورد عادي");
            itemsdata.Add(200,  "ماوس ");
            itemsdata.Add(330,"لابتوب hp");
            comid.DataSource = new BindingSource(itemsdata, null);
            comid.DisplayMember = "value";
            comid.ValueMember = "key";
            txtprice.Text = comid.SelectedValue.ToString();
            foreach (DataGridViewColumn col in dgv .Columns )
            {
                col.DefaultCellStyle.ForeColor = Color.Navy;
            }
            dgv.Columns[1].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns[3].DefaultCellStyle.ForeColor = Color.DarkBlue;

            txtname.Focus();
            txtname.SelectAll();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

  

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtdate_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button ==MouseButtons.Right )
            {
                txtdate.ContextMenu = new ContextMenu();
            }
        }

        private void txtall_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }


        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData ==Keys.Enter )
            {
                comid.Focus();
            }
        }

        private void comid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData ==Keys.Enter )
            {
                txtqty.Focus();
                txtqty.SelectAll();
            }
        }

        private void txtqty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData ==Keys .Enter )
                {
                btnadd.PerformClick();
                comid.Focus();
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (comid.SelectedIndex <= -1) return;
            string item = comid.Text;
            int qty = Convert.ToInt32(txtqty.Text);
            int price = Convert.ToInt32(txtprice.Text);
            int all = qty * price;
            object[] row = { item, qty, price, all };
            dgv.Rows.Add(row);
            txtall.Text = (Convert.ToInt32(txtall.Text) + all) + "";


        }

        private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char .IsDigit (e.KeyChar )&&!char .IsControl (e.KeyChar ))
                {
                e.Handled = true;
            }
        }
        string oldqty = "1";
        private void dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgv .CurrentRow !=null )
            {
                oldqty = dgv.CurrentRow.Cells["colqty"].Value + "";
            }

        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string newqty = dgv.CurrentRow.Cells["colqty"].Value + "";
            if (oldqty == newqty) return;
            if (!Regex.IsMatch(newqty, "^\\d+$"))
                { 
                dgv.CurrentRow.Cells["colqty"].Value = oldqty;
            }else
            {
                int p = (int)dgv.CurrentRow.Cells["colprice"].Value;
                int q = Convert.ToInt32(newqty);
                dgv.CurrentRow.Cells["colall"].Value = (q * p);
            }
            int newall = 0;
            foreach (DataGridViewRow r in dgv .Rows )
            {
                newall += Convert.ToInt32(r.Cells["colall"].Value);
            }
            txtall.Text = newall + "";

        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            ((Form)printPreviewDialog1).WindowState = FormWindowState.Maximized;
            if(printPreviewDialog1 .ShowDialog ()==DialogResult.OK )
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float margin = 40;
            
            Font f = new Font("Arial", 18, FontStyle.Bold);
            
            string strno = "#no" + txtnum.Text;
            string strdate= "التاريخ:" + txtdate.Text;
            string strname = "مطلوب من السيد :" + txtname.Text;

            SizeF fontsizeno = e.Graphics.MeasureString(strno, f);
            SizeF fontsizedate = e.Graphics.MeasureString(strdate, f);
            SizeF fontsizename = e.Graphics.MeasureString(strname, f);

            //e.Graphics.DrawImage(Properties.Resources.ahmed, 5, 5,200,200);
             
            e.Graphics.DrawString(strno, f, Brushes.Red, (e.PageBounds.Width - fontsizeno.Width) / 2, margin);
            e.Graphics.DrawString(strdate, f, Brushes.Black, e.PageBounds.Width - fontsizedate.Width - margin,margin +fontsizeno .Height);
            e.Graphics.DrawString(strname, f, Brushes.Navy, e.PageBounds.Width - fontsizename.Width - margin, margin + fontsizeno.Height + fontsizedate.Height);

            float preheights = margin + fontsizeno.Height + fontsizedate.Height + fontsizename.Height + 70;
            e.Graphics.DrawRectangle(Pens.Black, margin, preheights, e.PageBounds.Width - margin * 2, e.PageBounds.Height - margin - preheights);
            float colheight = 60;
            float col1width = 300;
            float col2width = 125+col1width ;
            float col3width = 125 + col2width;
            float col4width = 125+col3width ;
            e.Graphics.DrawLine(Pens.Black, margin, preheights+colheight, e.PageBounds.Width - margin, preheights + colheight);
            
            e.Graphics.DrawString("الصنف", f, Brushes.Black, e.PageBounds.Width - margin * 2 - col1width, preheights);


            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin * 2 - col1width, preheights, e.PageBounds.Width - margin * 2 - col1width, e.PageBounds.Height - margin);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin * 2 - col2width, preheights, e.PageBounds.Width - margin * 2 - col2width, e.PageBounds.Height - margin);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - margin * 2 - col3width, preheights, e.PageBounds.Width - margin * 2 - col3width, e.PageBounds.Height - margin);

            e.Graphics.DrawString("الكمية", f, Brushes.Black, e.PageBounds.Width - margin * 2 - col2width, preheights);
            e.Graphics.DrawString("السعر", f, Brushes.Black, e.PageBounds.Width - margin * 2 - col3width, preheights);
            e.Graphics.DrawString("الاجمالي", f, Brushes.Black, e.PageBounds.Width - margin * 2 - col4width, preheights);

            float rowsheight = 60;
            for (int x=0;x<dgv .Rows .Count;x+=1)
            {

                e.Graphics.DrawString(dgv.Rows[x].Cells[0].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin * 2 - col1width, preheights + rowsheight);
                e.Graphics.DrawString(dgv.Rows[x].Cells[1].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin * 2 - col2width, preheights + rowsheight);
                e.Graphics.DrawString(dgv.Rows[x].Cells[2].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin * 2 - col3width, preheights + rowsheight);
                e.Graphics.DrawString(dgv.Rows[x].Cells[3].Value.ToString(), f, Brushes.Navy, e.PageBounds.Width - margin * 2 - col4width, preheights + rowsheight);
                e.Graphics.DrawLine(Pens.Black, margin, preheights + rowsheight + colheight, e.PageBounds.Width - margin, preheights + rowsheight + colheight) ;

                rowsheight += 60;
            }
            e.Graphics.DrawString("الاجمالي", f, Brushes.Navy, e.PageBounds.Width - margin * 2 - col3width, preheights + rowsheight);
            e.Graphics.DrawString(txtall .Text , f, Brushes.Navy, e.PageBounds.Width - margin * 2 - col4width, preheights + rowsheight);
            e.Graphics.DrawLine(Pens.Black, margin, preheights + rowsheight + colheight, e.PageBounds.Width - margin, preheights + rowsheight + colheight);
        }

        private void comid_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtprice.Text = comid.SelectedValue.ToString();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
