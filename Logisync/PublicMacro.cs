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
    public partial class PublicMacro : Form
    {
        public PublicMacro()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuTextbox1_OnTextChange(object sender, EventArgs e)
        {
            
        }

        private void bunifuTextbox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            MessageBox.Show("Enter key pressed");
        }

        private void bunifuTextbox1_KeyPress(object sender, EventArgs e)
        {
           
             KeyPressEventArgs ee = (KeyPressEventArgs)e;

             if (ee.KeyChar == 13)
             {
                panel2.Visible = true;
             }
            
        }

        private void bunifuTextbox1_KeyDown(object sender, EventArgs e)
        {
            
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
