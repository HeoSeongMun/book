using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 도서대출관리시스템
{
    public partial class admin_book : Form
    {
        DBClass dbc = new DBClass();
        private int SelectedRowIndex;
        public admin_book()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
        }
        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dbc.DS.Clear();
                dbc.DA.Fill(dbc.DS, "book");
                dataGridView1.DataSource = dbc.DS.Tables["book"].DefaultView;
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
            book_header();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["book"];
                if (e.RowIndex < 0)
                {
                    // DBGrid의 컬럼 헤더를 클릭하면 컬럼을 정렬하므로
                    // 아무 메시지도 띄우지 않습니다.
                    return;
                }
                else if (e.RowIndex > bookTable.Rows.Count - 1)
                {
                    MessageBox.Show("해당하는 데이터가 존재하지 않습니다.");
                    return;
                }
                DataRow currRow = bookTable.Rows[e.RowIndex];
                textBox1.Text = currRow["bo_no"].ToString();
                textBox2.Text = currRow["bo_nm"].ToString();
                textBox3.Text = currRow["bo_auth"].ToString();
                textBox4.Text = currRow["bo_pub"].ToString();
                textBox5.Text = currRow["bo_year"].ToString();
                SelectedRowIndex = Convert.ToInt32(currRow["bo_no"]);
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
        public void book_header()
        {
            dataGridView1.Columns[0].HeaderText = "코드";
            dataGridView1.Columns[1].HeaderText = "제목";
            dataGridView1.Columns[2].HeaderText = "저자";
            dataGridView1.Columns[3].HeaderText = "출판사";
            dataGridView1.Columns[4].HeaderText = "출판연도";
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;

            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
        }
        private void 홈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_main admin_Main = new admin_main();
            admin_Main.ShowDialog();
        }

        private void 재고관리StripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_stock admin_Stock = new admin_stock();
            admin_Stock.ShowDialog();
        }

        private void 회원관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_user_management admin_User_Management = new admin_user_management();
            admin_User_Management.ShowDialog();
        }

        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            login Login = new login();
            Login.ShowDialog();
        }
        private void AppendBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["book"];
                bookTable = dbc.DS.Tables["book"];//*
                DataRow newRow = bookTable.NewRow();
                newRow["bo_no"] = Convert.ToInt32(textBox1.Text);
                newRow["bo_nm"] = textBox2.Text;
                newRow["bo_auth"] = textBox3.Text;
                newRow["bo_pub"] = textBox4.Text;
                newRow["bo_year"] = textBox5.Text;
                bookTable.Rows.Add(newRow);
                dbc.DA.Update(dbc.DS, "book");
                dbc.DS.AcceptChanges();//*
                ClearTextBoxes();  //*
                dataGridView1.DataSource = dbc.DS.Tables["book"].DefaultView;
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
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["book"];
                bookTable = dbc.DS.Tables["book"];//*
                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = bookTable.Columns["bo_no"];
                bookTable.PrimaryKey = PrimaryKey;

                DataRow currRow = bookTable.Rows.Find(SelectedRowIndex);
                currRow.Delete();

                dbc.DA.Update(dbc.DS.GetChanges(DataRowState.Deleted), "book");
                dataGridView1.DataSource = dbc.DS.Tables["book"].DefaultView;
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

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["book"];
                bookTable = dbc.DS.Tables["book"];//*
                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = bookTable.Columns["bo_no"];
                bookTable.PrimaryKey = PrimaryKey;

                DataRow currRow = bookTable.Rows.Find(SelectedRowIndex);


                currRow.BeginEdit();
                currRow["bo_no"] = textBox1.Text;
                currRow["bo_nm"] = textBox2.Text;
                currRow["bo_auth"] = textBox3.Text;
                currRow["bo_pub"] = textBox4.Text;
                currRow["bo_year"] = textBox5.Text;
                currRow.EndEdit();

                DataSet UpdatedSet = dbc.DS.GetChanges(DataRowState.Modified);
                if (UpdatedSet.HasErrors)
                {
                    MessageBox.Show("변경된 데이터에 문제가 있습니다.");
                }
                else
                {
                    dbc.DA.Update(UpdatedSet, "book");
                    dbc.DS.AcceptChanges();
                }

                dataGridView1.DataSource = dbc.DS.Tables["book"].DefaultView;

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