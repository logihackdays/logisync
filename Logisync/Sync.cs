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
    public partial class Sync : Form
    {
        LogitechGSDK.logiGkeyCB cbInstance;
        bool usingCallback = false;
        public Sync()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //Value	used	to	show	the	two	different	ways	to	implement	g-keys	support	in	your	game
            //change	it	to	false	to	try	the	non-callback	version
            usingCallback = true; //or	false,	depending	on	your	implementation
            if (usingCallback)
            {
                cbInstance = new LogitechGSDK.logiGkeyCB(this.GkeySDKCallback);
                LogitechGSDK.LogiGkeyInitWithoutContext(cbInstance);
            }
            else
                LogitechGSDK.LogiGkeyInitWithoutCallback();

            timer1.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!usingCallback)
            {
                for (int index = 6; index <= LogitechGSDK.LOGITECH_MAX_MOUSE_BUTTONS; index++)
                {
                    if (LogitechGSDK.LogiGkeyIsMouseButtonPressed(index) == 1)
                    {
                        //	Code	to	handle	what	happens	on	gkey	pressed	on	mouse
                    }
                }
                for (int index = 1; index <= LogitechGSDK.LOGITECH_MAX_GKEYS; index++)
                {
                    for (int mKeyIndex = 1; mKeyIndex <= LogitechGSDK.LOGITECH_MAX_M_STATES;
                    mKeyIndex++)
                    {
                        if (LogitechGSDK.LogiGkeyIsKeyboardGkeyPressed(index, mKeyIndex) == 1)
                        {
                            //	Code	to	handle	what	happens	on	gkey	pressed	on	keyboard/headset
                        }
                    }
                }
            }
        }

        void GkeySDKCallback(LogitechGSDK.GkeyCode gKeyCode, String gKeyOrButtonString, IntPtr context)
        {
            if (gKeyCode.keyDown == 0)
            {
                if (gKeyCode.mouse == 1)
                {
                    //	Code	to	handle	what	happens	on	gkey	released	on	mouse
                }
                else
                {
                    //	Code	to	handle	what	happens	on	gkey released	on	keyboard/headset
                }
            }
            else
            {
                if (gKeyCode.mouse == 1)
                {
                    //	Code	to	handle	what	happens	on	gkey	pressed	on	mouse
                }
                else
                {
                    //	Code	to	handle	what	happens	on	gkey	pressed	on	keyboard
                }
            }

        }
        void OnDestroy()
        {
            //Free G-Keys	SDKs	before	quitting	the	game
            LogitechGSDK.LogiGkeyShutdown();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {
            PublicMacro publicMacroForm = new PublicMacro();
            publicMacroForm.Show();

            bunifuMaterialTextbox1.Text = "QQQREEQRT";
            bunifuMaterialTextbox2.Text = "QWWREQQRT";
            bunifuMaterialTextbox3.Text = "QEQRWWWRT";
            bunifuMaterialTextbox4.Text = "WWWREWQRT";
            bunifuMaterialTextbox5.Text = "WWEREQQRT";
        }

        private void Sync_Load(object sender, EventArgs e)
        {

        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {
            if (bunifuDropdown1.selectedIndex == 0)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Logitech G910");
            }
            else if (bunifuDropdown1.selectedIndex == 1)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Logitech G430");
            }
            else if (bunifuDropdown1.selectedIndex == 2)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Logitech G910");
            }
            else
            {

            }
        }
    }
}
