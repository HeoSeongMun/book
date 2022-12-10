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
        string bookcntsql;
        string lentbooksql;
        string membersql;
        string lentmemsql;
        public admin_main()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
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
            bookcnt();
            lentbookcnt();
            membercnt();
            lentmemcnt();
            odmemcnt();
            lentgridview();
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
        public void sql_execute2(String sqlstr, DataSet dsstr) //총 책 갯수 카운터
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "bookcnt");
            dsstr.Tables["bookcnt"].Clear();
            dbc.DA.Fill(dsstr, "bookcnt");
            label3.Text = dsstr.Tables["bookcnt"].Rows.Count.ToString() + "권";
        }
        public void bookcnt() //총 책 갯수 카운터(쿼리실행문)
        {
            bookcntsql = "select * from book";
            sql_execute2(bookcntsql, dbc.DS);
        }
        public void sql_execute3(String sqlstr, DataSet dsstr) //대여 책 갯수 카운터
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "bookcnt");
            dsstr.Tables["bookcnt"].Clear();
            dbc.DA.Fill(dsstr, "bookcnt");
            label7.Text = dsstr.Tables["bookcnt"].Rows.Count.ToString() + "권";
        }
        public void lentbookcnt() //대여 책 갯수 카운터(쿼리실행문)
        {
            lentbooksql = "select * from book where bo_user is not null";
            sql_execute3(lentbooksql, dbc.DS);
        }
        public void sql_execute4(String sqlstr, DataSet dsstr) //총 회원 카운터
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "member");
            dsstr.Tables["member"].Clear();
            dbc.DA.Fill(dsstr, "member");
            label5.Text = dsstr.Tables["member"].Rows.Count.ToString() + "명";
        }
        public void membercnt() //총 회원 갯수 카운터(쿼리실행문)
        {
            membersql = "select * from usinf";
            sql_execute4(membersql, dbc.DS);
        }
        public void sql_execute5(String sqlstr, DataSet dsstr) //대여 회원 카운터
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "lentmem");
            dsstr.Tables["lentmem"].Clear();
            dbc.DA.Fill(dsstr, "lentmem");
            label9.Text = (dsstr.Tables["lentmem"].Rows.Count - 1).ToString() + "명";
        }
        public void lentmemcnt() //대여 회원 카운터(쿼리실행문)
        {
            lentmemsql = "select bo_user, count(*) from book group by bo_user having count(*) >= 1";
            sql_execute5(lentmemsql, dbc.DS);
        }
        public void sql_execute6(String sqlstr, DataSet dsstr) //연체 회원 카운터
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "odmem");
            dsstr.Tables["odmem"].Clear();
            dbc.DA.Fill(dsstr, "odmem");
            label11.Text = dsstr.Tables["odmem"].Rows.Count.ToString() + "명";
        }
        public void odmemcnt() //연체 회원 카운터(쿼리실행문)
        {
            lentmemsql = "select * from usinf where user_overdue is not null";
            sql_execute6(lentmemsql, dbc.DS);
        }
        public void sql_execute7(String sqlstr, DataSet dsstr) //대여 목록 그리드뷰 출력
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "lent");
            dsstr.Tables["lent"].Clear();
            dbc.DA.Fill(dsstr, "lent");
            dataGridView2.DataSource = dsstr.Tables["lent"].DefaultView;
            lent_header();
        }
        public void lent_header()
        {
            dataGridView1.Columns[0].HeaderText = "도서코드";
            dataGridView1.Columns[1].HeaderText = "도서명";
            dataGridView1.Columns[2].HeaderText = "사용자";
            dataGridView1.Columns[3].HeaderText = "대여일자";
        }
        public void lentgridview() //대여목록 그리드뷰 쿼리문 출력
        {
            lentbooksql = "select bo_no, bo_nm, bo_user, bo_lent_date from book where bo_user is not null";
            sql_execute7(lentbooksql, dbc.DS);
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

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
