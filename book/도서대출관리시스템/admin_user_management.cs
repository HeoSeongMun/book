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
        public admin_user_management()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void admin_user_management_Load(object sender, EventArgs e)
        {

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

        private void 로그아웃ToolStripMenuItem1_Click(object sender, EventArgs e)
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
