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
    public partial class admin_main : Form
    {
        DBClass dbc = new DBClass();
        string booksql;
        public admin_main()
        {
            InitializeComponent();
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void admin_main_Load(object sender, EventArgs e)
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
            booklist();
        }
        public void booklist()
        {
            booksql = "select * from book";
            sql_execute(booksql, dbc.DS);
        }
        public void book_header()
        {
            dataGridView1.Columns[0].HeaderText = "코드";
            dataGridView1.Columns[1].HeaderText = "제목";
            dataGridView1.Columns[2].HeaderText = "저자";
            dataGridView1.Columns[3].HeaderText = "출판사";
            dataGridView1.Columns[4].HeaderText = "출판연도";
            dataGridView1.Columns[5].HeaderText = "대여";
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;

            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 60;
        }
        public void list_search(String Find) //검색 기능
        {
            if (Find == "")
            {
                booksql = "select * from book ORDER BY bo_nm ASC";   // 정렬
                sql_execute(booksql, dbc.DS);
            }
            else if (Find != "")
            {
                booksql = "select * from book where bo_nm Like '%" + Find + "%'";   // 찾기
                sql_execute(booksql, dbc.DS);
                if (dbc.DS.Tables["book"].Rows.Count == 0)
                {
                    MessageBox.Show("해당 도서가 없습니다.");
                    booksql = "select * from book ORDER BY bo_nm ASC";
                    sql_execute(booksql, dbc.DS);
                }
            }
        }
        public void sql_execute(String sqlstr, DataSet dsstr)    //사용자 함수 정의
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "book");
            dsstr.Tables["book"].Clear();
            dbc.DA.Fill(dsstr, "book");
            int i;
            i = 0;
            while (i < dsstr.Tables["book"].Rows.Count)
            {
                DataRow currRow = dsstr.Tables["book"].Rows[i];
                if (currRow[5].ToString() == "")  //대여한 회원이 없으면 book테이블의 b_lent필드타입은 varchar(20)
                    currRow[5] = "가능";
                else    ////대여한 회원이 있으면 
                    currRow[5] = "불가능";

                i = i + 1;
            }
            dataGridView1.DataSource = dsstr.Tables["book"].DefaultView;
            book_header();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
                MessageBox.Show("검색 미입력");
            else
                list_search(textBox1.Text.Trim());
        }
        private void 도서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_book admin_Book = new admin_book();
            admin_Book.ShowDialog();
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

        private void 재고관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_stock admin_Stock = new admin_stock();
            admin_Stock.ShowDialog();
        }
    }
}
