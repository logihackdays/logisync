using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LedCSharp;
using System.Collections;
using System.Runtime.InteropServices;
using Hotkeys;
namespace Logisync
{
    public partial class MainForm : Form
    {

        int x = 0;
        int y = 0;
        int dx = 1;
        int dy = 1;
        keyboardNames[,] arrk = new keyboardNames[10, 11];


        const int TOTALBEHAVIOUR = 10;
        ArrayList behaviour = new ArrayList();
       
       
        keyboardNames[] xarr = new keyboardNames[9999];
        bool activeRemember = false;
        string temp = "";
        
        private GlobalKeyboardHook _globalKeyboardHook;


    public MainForm()
        {
          

            xarr['a'] = keyboardNames.A;
            xarr['b'] = keyboardNames.B;
            xarr['c'] = keyboardNames.C;
            xarr['d'] = keyboardNames.D;
            xarr['e'] = keyboardNames.E;
            xarr['f'] = keyboardNames.F;
            xarr['g'] = keyboardNames.G;
            xarr['h'] = keyboardNames.H;
            xarr['i'] = keyboardNames.I;
            xarr['j'] = keyboardNames.J;
            xarr['k'] = keyboardNames.K;
            xarr['l'] = keyboardNames.L;
            xarr['m'] = keyboardNames.M;
            xarr['n'] = keyboardNames.N;
            xarr['o'] = keyboardNames.O;
            xarr['p'] = keyboardNames.P;
            xarr['q'] = keyboardNames.Q;
            xarr['r'] = keyboardNames.R;
            xarr['s'] = keyboardNames.S;
            xarr['t'] = keyboardNames.T;
            xarr['u'] = keyboardNames.U;
            xarr['v'] = keyboardNames.V;
            xarr['w'] = keyboardNames.W;
            xarr['x'] = keyboardNames.X;
            xarr['y'] = keyboardNames.Y;
            xarr['z'] = keyboardNames.Z;
            xarr['1'] = keyboardNames.ONE;
            xarr['2'] = keyboardNames.TWO;
            xarr['3'] = keyboardNames.THREE;
            xarr['4'] = keyboardNames.FOUR;
            xarr['5'] = keyboardNames.FIVE;
            xarr['6'] = keyboardNames.SIX;
            xarr['7'] = keyboardNames.SEVEN;
            xarr['8'] = keyboardNames.EIGHT;
            xarr['9'] = keyboardNames.NINE;
            xarr['0'] = keyboardNames.ZERO;
            xarr['`'] = keyboardNames.TILDE;
            xarr['-'] = keyboardNames.MINUS;
            xarr['='] = keyboardNames.EQUALS;
            xarr['['] = keyboardNames.OPEN_BRACKET;
            xarr[']'] = keyboardNames.CLOSE_BRACKET;
            xarr['\\'] = keyboardNames.BACKSLASH;
            xarr[';'] = keyboardNames.SEMICOLON;
            xarr['\''] = keyboardNames.APOSTROPHE;
            xarr[','] = keyboardNames.COMMA;
            xarr['.'] = keyboardNames.PERIOD;
            xarr['/'] = keyboardNames.FORWARD_SLASH;
            xarr[' '] = keyboardNames.SPACE;

            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;

            LogitechGSDKLED.LogiLedInit();
            LogitechGSDKLED.LogiLedSaveCurrentLighting();
            LogitechGSDKLED.LogiLedSetLighting(0, 0, 100);

            arrk[0, 0] = keyboardNames.ONE;
            arrk[0, 1] = keyboardNames.TWO;
            arrk[0, 2] = keyboardNames.THREE;
            arrk[0, 3] = keyboardNames.FOUR;
            arrk[0, 4] = keyboardNames.FIVE;
            arrk[0, 5] = keyboardNames.SIX;
            arrk[0, 6] = keyboardNames.SEVEN;
            arrk[0, 7] = keyboardNames.EIGHT;
            arrk[0, 8] = keyboardNames.NINE;
            arrk[0, 9] = keyboardNames.ZERO;
            arrk[0, 10] = keyboardNames.MINUS;
            arrk[1, 0] = keyboardNames.Q;
            arrk[1, 1] = keyboardNames.W;
            arrk[1, 2] = keyboardNames.E;
            arrk[1, 3] = keyboardNames.R;
            arrk[1, 4] = keyboardNames.T;
            arrk[1, 5] = keyboardNames.Y;
            arrk[1, 6] = keyboardNames.U;
            arrk[1, 7] = keyboardNames.I;
            arrk[1, 8] = keyboardNames.O;
            arrk[1, 9] = keyboardNames.P;
            arrk[1, 10] = keyboardNames.OPEN_BRACKET;
            arrk[2, 0] = keyboardNames.A;
            arrk[2, 1] = keyboardNames.S;
            arrk[2, 2] = keyboardNames.D;
            arrk[2, 3] = keyboardNames.F;
            arrk[2, 4] = keyboardNames.G;
            arrk[2, 5] = keyboardNames.H;
            arrk[2, 6] = keyboardNames.J;
            arrk[2, 7] = keyboardNames.K;
            arrk[2, 8] = keyboardNames.L;
            arrk[2, 9] = keyboardNames.SEMICOLON;
            arrk[2, 10] = keyboardNames.APOSTROPHE;
            arrk[3, 0] = keyboardNames.Z;
            arrk[3, 1] = keyboardNames.X;
            arrk[3, 2] = keyboardNames.C;
            arrk[3, 3] = keyboardNames.V;
            arrk[3, 4] = keyboardNames.B;
            arrk[3, 5] = keyboardNames.N;
            arrk[3, 6] = keyboardNames.M;
            arrk[3, 7] = keyboardNames.COMMA;
            arrk[3, 8] = keyboardNames.PERIOD;
            arrk[3, 9] = keyboardNames.FORWARD_SLASH;
            arrk[3, 10] = keyboardNames.RIGHT_SHIFT;

            /* for (int i = 0; i < 4; i++)
             {
                 for (int j = 0; j < 11; j++)
                 {
                     LogitechGSDKLED.LogiLedFlashSingleKey(arrk[i, j], 0, 0, 100, 3000, 100);
                 }
             }*/
            
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            timer1.Start();
        }

        public MainForm(String user) : this()
        {
            label3.Visible = false;
            label8.Visible = true;
            label8.Text = "Welcome "+user;
            timer1.Start();
            x = 0;
            y = 0;

        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {


            if (activeRemember)
            {
                LogitechGSDKLED.LogiLedSetLighting(0, 0, 100);
                //lbInput.Text = lbInput.Text + e.KeyChar.ToString();
                
                char x = Convert.ToChar(e.KeyboardData.VirtualCode+32);
              
                
                temp = temp + x;
                if (temp.Length == 10)
                {
                    behaviour.Add(temp);
                    temp = "";
                }

                foreach (string behave in behaviour)
                {

                    for (int i = 0; i < behave.Length - 1; i++)
                    {
                        if (x == behave[i])
                        {
                            char y = behave[i + 1];
                            LogitechGSDKLED.LogiLedSetLightingForKeyWithKeyName(xarr[y], 99, 71, 8);
                        }
                    }
                }


            }

            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
            {
               // MessageBox.Show("Print Screen");
                //e.Handled = true;
            }
        }

 


      
        void RememberKeypress(char e)
        {
            
            if (activeRemember)
            {
                LogitechGSDKLED.LogiLedSetLighting(0, 0, 100);
                //lbInput.Text = lbInput.Text + e.KeyChar.ToString();
                char x = e;

                temp = temp + e;
                if (temp.Length == 10)
                {
                    behaviour.Add(temp);
                    temp = "";
                }
                //behaviour[behaviourCounter] = lbInput.Text;

               /* if (behaviourCounter < TOTALBEHAVIOUR - 1)
                {
                    behaviourCounter++;
                }
                else
                {
                    behaviourCounter = 0;
                }*/



                foreach (string behave in behaviour)
                {

                    for (int i = 0; i < behave.Length - 1; i++)
                    {
                        if (x == behave[i])
                        {
                            char y = behave[i + 1];
                            LogitechGSDKLED.LogiLedSetLightingForKeyWithKeyName(xarr[y], 99, 71, 8);
                        }
                    }
                }

            
            }
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
        {
            if (bunifuCheckbox1.Checked)
            {
                activeRemember = true;
                timer1.Stop();
                LogitechGSDKLED.LogiLedSetLighting(0, 0, 100);
            }
            else
            {
                activeRemember = false;
                timer1.Start();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            RegisterForm rf = new RegisterForm();
            rf.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LogitechGSDKLED.LogiLedSetLighting(100, 0, 0);

            //LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(arrk[y, x], 0, 0, 100, 1000, 100);
            LogitechGSDKLED.LogiLedSetLightingForKeyWithKeyName(arrk[y, x], 100, 100, 0);

            x += dx;
            y += dy;

            if (x > 9)
            {
                dx = -1 * dx;
            }
            else if (x < 1)
            {
                dx = -1 * dx;
            }
            if (y > 2)
            {
                dy = -1 * dy;
            }
            else if (y < 1)
            {
                dy = -1 * dy;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Voice v = new Logisync.Voice();
            v.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Sync syncForm = new Sync();
            syncForm.Show();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "D:\\Logitech Hackathon\\Fatigue Detector\\go.bat";
            proc.StartInfo.WorkingDirectory = "D:\\Logitech Hackathon\\Fatigue Detector";
            proc.Start();
        }
    }
}
