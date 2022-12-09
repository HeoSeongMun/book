using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace 도서대출관리시스템
{
    public partial class admin_stock : Form
    {
        string ordsql;
        private int SelectedRowIndex;
        DBClass dbc = new DBClass();
        public admin_stock()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
        }

        private void admin_stock_Load(object sender, EventArgs e)
        {
            orderlist();
        }
        
        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        public void sql_execute(String sqlstr, DataSet dsstr)    //사용자 함수 정의
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "ord");
            dsstr.Tables["ord"].Clear();
            dbc.DA.Fill(dsstr, "ord"); 
            dataGridView1.DataSource = dsstr.Tables["ord"].DefaultView;
            ord_header();     //함수 호출 
        }
        public void orderlist()
        {
            ordsql = "select * from ord";
            sql_execute(ordsql, dbc.DS);
        }
        public void ord_header()
        {
            dataGridView1.Columns[0].HeaderText = "번호";
            dataGridView1.Columns[1].HeaderText = "제목";
            dataGridView1.Columns[2].HeaderText = "저자";
            dataGridView1.Columns[3].HeaderText = "출판사";
            dataGridView1.Columns[4].HeaderText = "주문날짜";

            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dbc.DS.Clear();
                dbc.DA.Fill(dbc.DS, "ord");
                DataTable bookTable = dbc.DS.Tables["ord"];
                DataRow newRow = bookTable.NewRow();

                newRow["ord_no"] = Convert.ToInt32(textBox4.Text);
                newRow["ord_nm"] = textBox1.Text;
                newRow["ord_auth"] = textBox2.Text;
                newRow["ord_pub"] = textBox3.Text;
                newRow["ord_date"] = System.DateTime.Today;

                bookTable.Rows.Add(newRow);
                dbc.DA.Update(dbc.DS, "ord");
                dbc.DS.AcceptChanges();
                ClearTextBoxes();
                dataGridView1.DataSource = dbc.DS.Tables["ord"].DefaultView;
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["ord"];
                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = bookTable.Columns["ord_no"];
                bookTable.PrimaryKey = PrimaryKey;
                
                DataRow currRow = bookTable.Rows.Find(SelectedRowIndex);
                currRow.Delete();

                dbc.DA.Update(dbc.DS.GetChanges(DataRowState.Deleted), "ord");
                dataGridView1.DataSource = dbc.DS.Tables["ord"].DefaultView;
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        private void 홈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_main admin_Main= new admin_main();
            admin_Main.ShowDialog();
        }

        private void 도서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_book admin_Book = new admin_book();
            admin_Book.ShowDialog();
        }

        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            login Login = new login();
            Login.ShowDialog();
        }

        private void 회원관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_user_management admin_User_Management = new admin_user_management();
            admin_User_Management.ShowDialog();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["ord"];
                if (e.RowIndex > bookTable.Rows.Count - 1)
                {
                    MessageBox.Show("해당하는 데이터가 존재하지 않습니다.");
                    return;
                }
                DataRow currRow = bookTable.Rows[e.RowIndex];
                textBox4.Text = currRow["ord_no"].ToString();
                textBox1.Text = currRow["ord_nm"].ToString();
                textBox2.Text = currRow["ord_auth"].ToString();
                textBox3.Text = currRow["ord_pub"].ToString();
                SelectedRowIndex = Convert.ToInt32(currRow["ord_no"]);
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
    }
}
