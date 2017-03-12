using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logisync
{
    public partial class Form1 : Form
    {
        int counter = 0;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Visible = false;
            //bunifuTransition1.ShowSync(this, true);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter>10)
            {
                //bunifuTransition1.HideSync(this, true);
                this.Hide();
                 Login loginForm = new Logisync.Login();
                 loginForm.Show();
                //RegisterForm rf = new RegisterForm();
               // rf.Show();
                // Sync syncForm = new Sync();
                // syncForm.Show();
                //MainForm mainForm = new MainForm();
                //mainForm.Show();
                timer1.Stop();
               
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
