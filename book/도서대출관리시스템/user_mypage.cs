using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace 도서대출관리시스템
{
    public partial class user_mypage : Form
    {
        DBClass dbc = new DBClass();
        user_main parent;
        string param;
        string lentsql;

        public user_mypage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public user_mypage(user_main user_main)
        {
            InitializeComponent();
            parent = user_main;
        }

        private void user_mypage_Load(object sender, EventArgs e)
        {
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
            param = parent.getID;
            lent_search("");
        }
        public void sql_execute(String sqlstr, DataSet dsstr)
        {
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dsstr, "lent");
            dsstr.Tables["lent"].Clear();
            dbc.DA.Fill(dsstr, "lent");
            dataGridView1.DataSource = dsstr.Tables["lent"].DefaultView;
            row_counter();
            lent_header();
        }
        public void row_counter() //대여 건수 지정
        {
            label3.Text = (dataGridView1.RowCount - 1).ToString();
        }
        public void lent_header()
        {
            dataGridView1.Columns[0].HeaderText = "도서코드";
            dataGridView1.Columns[1].HeaderText = "사용자";
            dataGridView1.Columns[2].HeaderText = "대여일";
        }
        /*lent 테이블 구성 컬럼
        대여번호 : lent_no [varchar[20]타입]
        책번호 : lent_bo_no [varchar[20]타입]
        대여유저아이디 : lent_user [varchar[20]타입]
        대여 일자 : lent_date [date타입]
        */

        public void lent_search(String Find) //검색기능
        {
            if (Find == "")
            {
                lentsql = "select lent_bo_no, lent_user, lent_date from lent where lent_user = '" + param + "' ORDER BY lent_no ASC";   // 정렬
                sql_execute(lentsql, dbc.DS);
            }
            else if (Find != "")
            {
                lentsql = "select lent_bo_no, lent_user, lent_date from lent where lent_bo_no Like '%" + Find + "%'";   // 찾기
                sql_execute(lentsql, dbc.DS);
                if (dbc.DS.Tables["lent"].Rows.Count == 0)
                {
                    MessageBox.Show("해당 도서가 없습니다.");
                    lentsql = "select lent_bo_no, lent_user, lent_date from lent where lent_user = '" + param + "' ORDER BY lent_no ASC";
                    sql_execute(lentsql, dbc.DS);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e) //검색 버튼
        {
            lent_search(textBox1.Text.Trim());
        }
    }
}
