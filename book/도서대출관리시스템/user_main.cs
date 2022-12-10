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
        string idsql;
        string booksql;
        string lentsql;
        string rtnsql;
        private int SelectedRowIndex;
        private string SelectedRowString;
        private string SelectedRowStringGrid1;
        login parent;       //login타입의 parent 객체 생성
        DBClass dbc = new DBClass();
        public user_main() //매개 변수없는 기본생성자
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public user_main(login loginform) // 로그인 타입의 매개변수를 갖는 생성자
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            parent = loginform;
        }
        public string getID // 유저 마이페이지로 접속 유저 아이디를 전달하는 메서드
        {
            get { return param; }
        }
        private void main_Load(object sender, EventArgs e)
        {
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
            param = parent.getID; //로그인 폼에서 로그인 한 유저 ID값 저장
            list_search(""); //도서 목록 출력
            lentlist(); //대여 목록 출력
            idSelect(); //로그인한 유저 신상정보 출력
        }
        public void sql_execute(String sqlstr, DataSet dsstr)    //도서 목록 실행
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
        public void lentlist() //대여목록 조회 쿼리
        {
            booksql = "select bo_nm, TO_CHAR(bo_lent_date,'yyyy/mm/dd'), TO_CHAR(bo_rtndate,'yyyy/mm/dd') from book where bo_user ='" + param + "'";
            sql_execute2(booksql, dbc.DS);
        }
        public void sql_execute2(String sqlstr, DataSet dsstr)    //대여목록 실행
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
        private void button3_Click(object sender, EventArgs e) //대여 버튼
        {
            if (SelectedRowStringGrid1 == "가능")
              {
                DialogResult rtyes = MessageBox.Show("해당 도서를 대여하시겠습니까?", "확인", MessageBoxButtons.YesNo);
                if (rtyes == DialogResult.Yes)
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
            else
            {
                MessageBox.Show("이미 대여중인 도서입니다.");
                return;
            }         
        }
        private void button2_Click(object sender, EventArgs e) //반납 버튼
        {
            DialogResult rtyes = MessageBox.Show("해당 도서를 반납하시겠습니까?", "확인", MessageBoxButtons.YesNo);
            if (rtyes == DialogResult.Yes)
            {
                lentsql = "insert into lent(lent_no, lent_bo_nm, lent_user, lent_date, lent_rtndate) select seq_lent.NEXTVAL, bo_nm, bo_user, bo_lent_date, bo_rtndate from book where bo_nm ='" + SelectedRowString+ "'";
                dbc.DCom.CommandText = lentsql;
                dbc.DCom.ExecuteNonQuery();
                dbc.DA.SelectCommand = dbc.DCom;

                rtnsql = "update book set bo_user = null, bo_lent_date = null, bo_rtndate = null where bo_nm = '" + SelectedRowString + "'";
                dbc.DCom.CommandText = rtnsql;
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
        private void button4_Click(object sender, EventArgs e) //연기 버튼 클릭
        {
            DialogResult rtyes = MessageBox.Show("해당 도서 대여를 연장하시겠습니까?", "확인", MessageBoxButtons.YesNo);
            if (rtyes == DialogResult.Yes)
            {
                rtnsql = "update book set bo_rtndate = bo_rtndate + 7 where bo_nm = '" + SelectedRowString + "'";
                dbc.DCom.CommandText = rtnsql;
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
        private void button1_Click(object sender, EventArgs e) //검색버튼
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
        private void button5_Click(object sender, EventArgs e) //초기화 버튼
        {
            list_search("");
        }
        public void idSelect() //로그인 폼에서 전달받은 param값을 통해 접속한 유저의 신상정보를 추출
        {
            idsql = "select * from usinf where user_id = " + param;
            sql_execute3(idsql, dbc.DS);
        }
        public void sql_execute3(String sqlstr, DataSet dsstr) //폼 내 라벨값에 유저의 신상정보 출력
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "idinf");
            dsstr.Tables["idinf"].Clear();
            dbc.DA.Fill(dsstr, "idinf");
            DataRow currRow = dsstr.Tables["idinf"].Rows[0];
            label5.Text = currRow[0].ToString();
            label6.Text = currRow[1].ToString();
            label7.Text = currRow[2].ToString();
            label8.Text = currRow[1].ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //셀 클릭하여 특정 데이터값 추출
        {
            SelectedRowStringGrid1 = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(); //내가 선택한 행의 5번째 열의 값 추출
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
            user_mypage user_mypage = new user_mypage(this);
            user_mypage.ShowDialog();
        }
        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            login login = new login();
            login.ShowDialog();
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) //대여목록 데이터그리드뷰 데이터값 도출
        {
            try
            {
                DataTable lentTable = dbc.DS.Tables["lent"];
                if (e.RowIndex > lentTable.Rows.Count - 1)
                {
                    MessageBox.Show("해당하는 데이터가 존재하지 않습니다.");
                    return;
                }
                DataRow currRow = lentTable.Rows[e.RowIndex];
                SelectedRowString = currRow["bo_nm"].ToString();
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
