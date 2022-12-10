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
    public partial class admin_user_management : Form
    {
        //수정하거나 삭제하기 위해 선택된 행의 인덱스를 저장한다.
        private int SelectedRowIndex;

        string usersql;
        string oversql;
        DBClass dbc = new DBClass();
        public admin_user_management()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
        }

        private void admin_user_management_Load(object sender, EventArgs e)
        {
            userlist(); //대여 목록 출력
            overlist();
            this.Left = 0;
            this.Top = 0;
        }
        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
        public void user_header()
        {
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "이름";
            dataGridView1.Columns[2].HeaderText = "전화번호";
            dataGridView1.Columns[3].HeaderText = "비밀번호";
            dataGridView1.Columns[4].Visible = false;

            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
        }
        public void over_header()
        {
            dataGridView2.Columns[0].HeaderText = "제목";
            dataGridView2.Columns[1].HeaderText = "ID";
            dataGridView2.Columns[2].HeaderText = "대여일";
            dataGridView2.Columns[3].HeaderText = "반납일";
            dataGridView2.Columns[4].HeaderText = "벌금";

            dataGridView2.Columns[0].Width = 60;
            dataGridView2.Columns[1].Width = 60;
            dataGridView2.Columns[2].Width = 100;
            dataGridView2.Columns[3].Width = 100;
            dataGridView2.Columns[3].Width = 100;
        }
        public void userlist() //회원목록 조회 쿼리
        {
            usersql = "select * from usinf";
            sql_execute(usersql, dbc.DS);
        }
        public void overlist()  //연체회원 조회 쿼리
        {
            oversql = "select t1.bo_nm, t1.bo_user, TO_CHAR(t1.bo_lent_date,'yyyy/mm/dd'), TO_CHAR(t1.bo_rtndate,'yyyy/mm/dd'), t2.user_overdue from book t1, usinf t2 where t1.bo_rtndate > sysdate and t1.bo_user = user_id";
            sql_execute2(oversql, dbc.DS);
        }
        public void sql_execute(String sqlstr, DataSet dsstr)    //회원목록 실행
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "usinf");
            dsstr.Tables["usinf"].Clear();
            dbc.DA.Fill(dsstr, "usinf");

            dataGridView1.DataSource = dsstr.Tables["usinf"].DefaultView;

            user_header();
        }
        public void sql_execute2(String sqlstr, DataSet dsstr)
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "book");
            dsstr.Tables["book"].Clear();
            dbc.DA.Fill(dsstr, "book");

            dataGridView2.DataSource = dsstr.Tables["book"].DefaultView;

            over_header();
        }

        private void AppendBtn_Click(object sender, EventArgs e)    // 추가 버튼
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["usinf"];
                bookTable = dbc.DS.Tables["usinf"];//*
                DataRow newRow = bookTable.NewRow();
                newRow["user_id"] = Convert.ToInt32(textBox1.Text);
                newRow["user_nm"] = textBox2.Text;
                newRow["user_tel"] = textBox3.Text;
                newRow["user_pw"] = textBox4.Text;
                bookTable.Rows.Add(newRow);
                dbc.DA.Update(dbc.DS, "usinf");
                dbc.DS.AcceptChanges();//*
                ClearTextBoxes();  //*
                dataGridView1.DataSource = dbc.DS.Tables["usinf"].DefaultView;
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
        private void DeleteBtn_Click(object sender, EventArgs e)    // 삭제 버튼
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["usinf"];
                bookTable = dbc.DS.Tables["usinf"];
                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = bookTable.Columns["user_id"];
                bookTable.PrimaryKey = PrimaryKey;

                DataRow currRow = bookTable.Rows.Find(SelectedRowIndex);
                currRow.Delete();

                dbc.DA.Update(dbc.DS.GetChanges(DataRowState.Deleted), "usinf");
                dataGridView1.DataSource = dbc.DS.Tables["usinf"].DefaultView;
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
        private void UpdateBtn_Click(object sender, EventArgs e)    // 수정 버튼
        {
            try
            {
                DataTable bookTable = dbc.DS.Tables["usinf"];
                bookTable = dbc.DS.Tables["usinf"];//*
                DataColumn[] PrimaryKey = new DataColumn[1];
                PrimaryKey[0] = bookTable.Columns["user_id"];
                bookTable.PrimaryKey = PrimaryKey;

                DataRow currRow = bookTable.Rows.Find(SelectedRowIndex);


                currRow.BeginEdit();
                currRow["user_id"] = textBox1.Text;
                currRow["user_nm"] = textBox2.Text;
                currRow["user_tel"] = textBox3.Text;
                currRow["user_pw"] = textBox4.Text;
                currRow.EndEdit();

                DataSet UpdatedSet = dbc.DS.GetChanges(DataRowState.Modified);
                if (UpdatedSet.HasErrors)
                {
                    MessageBox.Show("변경된 데이터에 문제가 있습니다.");
                }
                else
                {
                    dbc.DA.Update(UpdatedSet, "usinf");
                    dbc.DS.AcceptChanges();
                }

                dataGridView1.DataSource = dbc.DS.Tables["usinf"].DefaultView;

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
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)    // 그리드뷰 셀 선택
        {
            try
            {
                DataTable usinfTable = dbc.DS.Tables["usinf"];
                if (e.RowIndex < 0)
                {
                    // DBGrid의 컬럼 헤더를 클릭하면 컬럼을 정렬하므로
                    // 아무 메시지도 띄우지 않습니다.
                    return;
                }
                else if (e.RowIndex > usinfTable.Rows.Count - 1)
                {
                    MessageBox.Show("해당하는 데이터가 존재하지 않습니다.");
                    return;
                }

                DataRow currRow = usinfTable.Rows[e.RowIndex];
                textBox1.Text = currRow["user_id"].ToString();
                textBox2.Text = currRow["user_nm"].ToString();
                textBox3.Text = currRow["user_tel"].ToString();
                textBox4.Text = currRow["user_pw"].ToString();
                SelectedRowIndex = Convert.ToInt32(currRow["user_id"]);
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
            admin_main admin_Main = new admin_main();
            admin_Main.ShowDialog();
        }

        private void 도서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_book admin_Book = new admin_book();
            admin_Book.ShowDialog();
        }

        private void 재고관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            admin_stock admin_stock = new admin_stock();
            admin_stock.ShowDialog();
        }

        private void 로그아웃ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            login Login = new login();
            Login.ShowDialog();
        }
    }
}
