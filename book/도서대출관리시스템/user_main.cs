using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 도서대출관리시스템
{
    public partial class user_main : Form
    {
        string param;
        string booksql;
        string lentsql;
        private int SelectedRowIndex;
        login parent;
        DBClass dbc = new DBClass();
        public user_main()
        {
            InitializeComponent();
        }
        public user_main(login loginform)
        {
            InitializeComponent();
            parent = loginform;
        }
        public string getID
        {
            get { return label8.Text; }
            set { label8.Text = value.ToString(); }
        }
        private void main_Load(object sender, EventArgs e)
        {
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
            param = parent.getID;
            label8.Text = param;
            list_search("");
            lentlist();
            this.Left = 0;
            this.Top = 0;
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
                if (currRow[4].ToString() == "")  //대여한 회원이 없으면 book테이블의 b_lent필드타입은 varchar(20)
                    currRow[4] = "가능";
                else    ////대여한 회원이 있으면
                    currRow[4] = "불가능";

                i = i + 1;
            }
            dataGridView1.DataSource = dsstr.Tables["book"].DefaultView;
            book_header();     //함수 호출 
        }
        public void book_header()
        {
            dataGridView1.Columns[0].HeaderText = "코드";
            dataGridView1.Columns[1].HeaderText = "제목";
            dataGridView1.Columns[2].HeaderText = "저자";
            dataGridView1.Columns[3].HeaderText = "출판사";
            dataGridView1.Columns[4].HeaderText = "대출 상태";

            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 80;
        }
        public void lentlist()
        {
            booksql = "select bo_nm, TO_CHAR(bo_lent_date,'yyyy/mm/dd'), TO_CHAR(bo_rtndate,'yyyy/mm/dd') from book where bo_user ='" + param + "'";
            sql_execute2(booksql, dbc.DS);
        }
        public void sql_execute2(String sqlstr, DataSet dsstr)    //사용자 함수 정의
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "lent");
            dsstr.Tables["lent"].Clear();
            dbc.DA.Fill(dsstr, "lent");

            dataGridView2.DataSource = dsstr.Tables["lent"].DefaultView;

            lentm_header();
        }
        public void lentm_header()
        {
            dataGridView2.Columns[0].HeaderText = "제목";
            dataGridView2.Columns[1].HeaderText = "대여일";
            dataGridView2.Columns[2].HeaderText = "반납일";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DialogResult rtyes = MessageBox.Show("해당 도서를 대여하시겠습니까?", "확인", MessageBoxButtons.YesNo);
            if(rtyes == DialogResult.Yes)
            {
                lentsql = "Update book Set bo_user = '" + param + "', bo_lent_date = sysdate, bo_rtndate = sysdate+7 where bo_no = '" + SelectedRowIndex + "'";
                dbc.DCom.CommandText = lentsql;
                dbc.DCom.ExecuteNonQuery();
                dbc.DA.SelectCommand = dbc.DCom;
                dbc.DA.Fill(dbc.DS, "lent");
                dbc.DS.Tables["lent"].Clear();
                dbc.DA.Fill(dbc.DS, "lent");
                list_search("");
                lentlist();
            }
            else
            {
                return;
            }
                       
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
                MessageBox.Show("검색 미입력");
            else
                list_search(textBox1.Text.Trim());
        }
        public void list_search(String Find) //검색 기능
        {
            if (Find == "")
            {
                booksql = "select bo_no, bo_nm, bo_auth, bo_pub, bo_user from book ORDER BY bo_no ASC";   // 정렬
                sql_execute(booksql, dbc.DS);
            }
            else if (Find != "")
            {
                booksql = "select bo_no, bo_nm, bo_auth, bo_pub, bo_user from book where bo_nm Like '%" + Find + "%'";   // 찾기
                sql_execute(booksql, dbc.DS);
                if (dbc.DS.Tables["book"].Rows.Count == 0)
                {
                    MessageBox.Show("해당 도서가 없습니다.");
                    booksql = "select bo_no, bo_nm, bo_auth, bo_pub, bo_user from book ORDER BY bo_nm ASC";
                    sql_execute(booksql, dbc.DS);
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            list_search("");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["book"];
                if (e.RowIndex > bookTable.Rows.Count - 1)
                {
                    MessageBox.Show("해당하는 데이터가 존재하지 않습니다.");
                    return;
                }
                DataRow currRow = bookTable.Rows[e.RowIndex];
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

        private void 마이페이지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            user_mypage user_mypage = new user_mypage();
            user_mypage.ShowDialog();
        }
        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            login login = new login();
            login.ShowDialog();
        }
    }
}
