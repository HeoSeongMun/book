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
        DateTime datetime;
        string dt1;
        string dt2;
        public user_mypage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public user_mypage(user_main user_main)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
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
            dataGridView1.Columns[0].HeaderText = "제목";
            dataGridView1.Columns[1].HeaderText = "대여일";
            dataGridView1.Columns[2].HeaderText = "반납일";
        }
        /*lent 테이블 구성 컬럼
        대출번호   lent_bo_no NUMBER NOT NULL, 
        책 제목    bo_nm VARCHAR2(40) NOT NULL,
        대출사용자 lent_user VARCHAR2(40) NOT NULL,
        대출일     bo_lent_date DATE NOT NULL,
        반납일     bo_rtndate DATE NOT NULL,
                   PRIMARY KEY(lent_bo_no) 기본키
        */

        public void lent_search(String Find) //검색기능
        {
            if (Find == "")
            {
                lentsql = "select bo_nm, TO_CHAR(bo_lent_date,'yyyy/mm/dd'), TO_CHAR(bo_rtndate,'yyyy/mm/dd') from lent where lent_user = '" + param + "' ORDER BY lent_bo_no ASC";   // 정렬
                sql_execute(lentsql, dbc.DS);
            }
            else if (Find != "")
            {
                lentsql = "select bo_nm, TO_CHAR(bo_lent_date,'yyyy/mm/dd'), TO_CHAR(bo_rtndate,'yyyy/mm/dd') from lent where bo_nm Like '%" + Find + "%'";   // 찾기
                sql_execute(lentsql, dbc.DS);
                if (dbc.DS.Tables["lent"].Rows.Count == 0)
                {
                    MessageBox.Show("해당 도서가 없습니다.");
                    lentsql = "select bo_nm, TO_CHAR(bo_lent_date,'yyyy/mm/dd'), TO_CHAR(bo_rtndate,'yyyy/mm/dd') from lent where lent_user = '" + param + "' ORDER BY lent_bo_no ASC";
                    sql_execute(lentsql, dbc.DS);
                }
            }
        }

        public void lent_datesearch() //날짜 검색기능
        {
            
        }
        private void button1_Click(object sender, EventArgs e) //검색 버튼
        {
            lent_search(textBox1.Text.Trim());
        }


        private void button2_Click(object sender, EventArgs e)
        {
            lent_search("");
        }
        private void button3_Click(object sender, EventArgs e) //날짜 검색 버튼
        {
            lentsql = "select bo_nm, TO_CHAR(bo_lent_date,'yyyy/mm/dd'), TO_CHAR(bo_rtndate,'yyyy/mm/dd') from lent where TO_CHAR(bo_lent_date,'yyyy-mm-dd') >= '" + dt1 + "' and TO_CHAR(bo_lent_date,'yyyy-mm-dd') <= '" + dt2 + "'"; // 찾기
            sql_execute(lentsql, dbc.DS);
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dt1 = dateTimePicker1.Value.ToShortDateString();
        }


        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dt2 = dateTimePicker2.Value.ToShortDateString();
        }
    }
}
