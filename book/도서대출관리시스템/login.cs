using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Oracle.DataAccess.Client;

namespace 도서대출관리시스템
{
    public partial class login : Form
    {
        string sqlstr; //쿼리문 저장 변수
        string sqlstr1;
        DBClass dbc = new DBClass();  //*****DBClass 객체 생성
        public login()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            dbc.DB_ObjCreate();
            dbc.DB_Open();
            dbc.DB_Access();
        }
        public string getID
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value.ToString(); }
        }

        private void login_Load(object sender, EventArgs e)
        {
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "")
            {
                MessageBox.Show("ID 또는 Passwrd를 입력하세요");
                return;
            }
            sqlstr = "Select user_id, user_pw From usinf Where user_id ='" + textBox2.Text + "'and user_pw ='" + textBox3.Text + "'";
            dbc.DCom.CommandText = sqlstr;
            dbc.DA.SelectCommand = dbc.DCom;
            dbc.DA.Fill(dbc.DS, "usinf");
            dbc.DS.Tables["usinf"].Clear();
            dbc.DA.Fill(dbc.DS, "usinf");
            if (dbc.DS.Tables["usinf"].Rows.Count == 0)
            {
                MessageBox.Show("등록되지 않은 회원 또는 비밀번호가 맞지 않습니다.");
                textBox3.Text = "";
                textBox3.Focus();
            }
            else if (dbc.DS.Tables["usinf"].Rows.Count == 1)
            {
                this.Visible = false;
                user_main user_main = new user_main(this);
                user_main.ShowDialog();
            }
            else
            {
                MessageBox.Show("입력한 정보가 맞지 않습니다.");
                textBox3.Text = "";
                textBox3.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "")
            {
                MessageBox.Show("ID 또는 Passwrd를 입력하세요");
                return;
            }
                sqlstr = "Select adm_nm,adm_pwd From adm Where adm_nm ='" + textBox2.Text + "'and adm_pwd ='" + textBox3.Text + "'";
                dbc.DCom.CommandText = sqlstr;
                dbc.DA.SelectCommand = dbc.DCom;
                dbc.DA.Fill(dbc.DS, "adm");
                dbc.DS.Tables["adm"].Clear();
                dbc.DA.Fill(dbc.DS, "adm");
                if (dbc.DS.Tables["adm"].Rows.Count == 1)
                {
                    this.Visible = false;
                    admin_main admin_Main = new admin_main();
                    admin_Main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("입력한 정보가 맞지 않습니다.");
                    textBox3.Text = "";
                    textBox3.Focus();
                }
          }
    }
}
