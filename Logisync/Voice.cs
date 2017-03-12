using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace Logisync
{
    public partial class Voice : Form
    {

        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public String text_to_send;
        public int flagConnectedClient;

        public TcpListener listener = null;
        public NetworkStream netStream = null;
        public int BUFSIZE = 100;
        public int servPort = 456;
        public static String ClientOrder = "alpha";
        public static int bytesRcvd;
        public byte[] rcvBuffer;
        //0 = alpha
        //1 = beta
        //2 = gamma
        int activeMacro = 0;
        //buat G1-G9 Alpha, Beta, Gamma
        String[,] arx = new String[3, 10];
        LogitechGSDK.logiGkeyCB cbInstance;
        int x;

        private GlobalKeyboardHook _globalKeyboardHook;
        public Voice()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;

            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            flagConnectedClient = 0;
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPServer.Text = address.ToString();
                }
            }

            // cbInstance = new LogitechGSDK.logiGkeyCB(this.GkeySDKCallback);
            // LogitechGSDK.LogiGkeyInitWithoutContext(cbInstance);
            LogitechGSDK.LogiGkeyInitWithoutCallback();

          //  timer1.Start();
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {

            x = e.KeyboardData.VirtualCode;
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                if (activeMacro == 0)
                {
                    macroG1 = text_AG1.Text;
                    macroG2 = text_AG2.Text;
                    macroG3 = text_AG3.Text;
                    macroG4 = text_AG4.Text;
                    macroG5 = text_AG5.Text;
                    macroG6 = text_AG6.Text;
                    macroG7 = text_AG7.Text;
                    macroG8 = text_AG8.Text;
                    macroG9 = text_AG9.Text;
                }
                else if (activeMacro == 1)
                {
                    macroG1 = text_BG1.Text;
                    macroG2 = text_BG2.Text;
                    macroG3 = text_BG3.Text;
                    macroG4 = text_BG4.Text;
                    macroG5 = text_BG5.Text;
                    macroG6 = text_BG6.Text;
                    macroG7 = text_BG7.Text;
                    macroG8 = text_BG8.Text;
                    macroG9 = text_BG9.Text;
                }
                else if (activeMacro == 2)
                {
                    macroG1 = text_GG1.Text;
                    macroG2 = text_GG2.Text;
                    macroG3 = text_GG3.Text;
                    macroG4 = text_GG4.Text;
                    macroG5 = text_GG5.Text;
                    macroG6 = text_GG6.Text;
                    macroG7 = text_GG7.Text;
                    macroG8 = text_GG8.Text;
                    macroG9 = text_GG9.Text;
                }
                // MessageBox.Show(x.ToString());
                if (x == 162)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 0] = macroG1;
                    SendKeys.Send(arx[activeMacro, 0]);
                }
                else if (x == 163)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 1] = macroG2;
                    SendKeys.Send(arx[activeMacro, 1]);
                }
                else if (x == 164)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 2] = macroG3;
                    SendKeys.Send(arx[activeMacro, 2]);
                }
                else if (x == 165)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 3] = macroG4;
                    SendKeys.Send(arx[activeMacro, 3]);
                }
                else if (x == 166)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 4] = macroG5;
                    SendKeys.Send(arx[activeMacro, 1]);
                }
                else if (x == 117)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 5] = macroG6;
                    SendKeys.Send(arx[activeMacro, 5]);
                }
                else if (x == 118)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 6] = macroG7;
                    SendKeys.Send(arx[activeMacro, 6]);
                }
                else if (x == 119)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 7] = macroG8;
                    SendKeys.Send(arx[activeMacro, 7]);
                }
                else if (x == 120)
                {
                    enabledButton(activeMacro, x);
                    arx[activeMacro, 8] = macroG9;
                    SendKeys.Send(arx[activeMacro, 8]);
                }
            }
            if (ClientOrder == "alpha")
            {
                activeMacro = 0;
                label10.ForeColor = System.Drawing.Color.Yellow;
                label11.ForeColor = System.Drawing.Color.White;
                label12.ForeColor = System.Drawing.Color.White;
            }
            else if (ClientOrder == "beta")
            {
                activeMacro = 1;
                label11.ForeColor = System.Drawing.Color.Yellow;
                label10.ForeColor = System.Drawing.Color.White;
                label12.ForeColor = System.Drawing.Color.White;
            }
            else if (ClientOrder == "gamma")
            {
                activeMacro = 2;
                label12.ForeColor = System.Drawing.Color.Yellow;
                label11.ForeColor = System.Drawing.Color.White;
                label10.ForeColor = System.Drawing.Color.White;
            }
            //set alpha beta or gamma buat tes aja
            /*if (textBox_voice.Text == "alpha")
            {
                activeMacro = 0;
            }
            else if (textBox_voice.Text == "beta")
            {
                activeMacro = 1;
            }
            else if (textBox_voice.Text == "gamma")
            {
                activeMacro = 2;
            }
            */
            //kombinasi macro button
            
            /* for (int index = 6; index <= LogitechGSDK.LOGITECH_MAX_MOUSE_BUTTONS; index++)
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
                         if (x == 112)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 0] = macroG1;
                             SendKeys.Send(arx[activeMacro, 0]);
                         }
                         else if (x == 113)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 1] = macroG2;
                             SendKeys.Send(arx[activeMacro, 1]);
                         }
                         else if (x == 114)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 2] = macroG3;
                             SendKeys.Send(arx[activeMacro, 2]);
                         }
                         else if (x == 115)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 3] = macroG4;
                             SendKeys.Send(arx[activeMacro, 3]);
                         }
                         else if (x == 116)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 4] = macroG5;
                             SendKeys.Send(arx[activeMacro, 1]);
                         }
                         else if (x == 117)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 5] = macroG6;
                             SendKeys.Send(arx[activeMacro, 5]);
                         }
                         else if (x == 118)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 6] = macroG7;
                             SendKeys.Send(arx[activeMacro, 6]);
                         }
                         else if (x == 119)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 7] = macroG8;
                             SendKeys.Send(arx[activeMacro, 7]);
                         }
                         else if (x == 120)
                         {
                             enabledButton(activeMacro, x);
                             arx[activeMacro, 8] = macroG9;
                             SendKeys.Send(arx[activeMacro, 8]);
                         }
                     }
                 }
             }
             */







        }



        String macroG1;
        String macroG2;
        String macroG3;
        String macroG4;
        String macroG5;
        String macroG6;
        String macroG7;
        String macroG8;
        String macroG9;
        void enabledButton(int aktifMakro, String buttonG)
        {

            if (aktifMakro == 0)
            {
                if (buttonG == "G1/M1") { btnAG1.Enabled = false; }
                if (buttonG == "G2/M1") { btnAG2.Enabled = false; }
                if (buttonG == "G3/M1") { btnAG3.Enabled = false; }
                if (buttonG == "G4/M1") { btnAG4.Enabled = false; }
                if (buttonG == "G5/M1") { btnAG5.Enabled = false; }
                if (buttonG == "G6/M1") { btnAG6.Enabled = false; }
                if (buttonG == "G7/M1") { btnAG7.Enabled = false; }
                if (buttonG == "G8/M1") { btnAG8.Enabled = false; }
                if (buttonG == "G9/M1") { btnAG9.Enabled = false; }
            }
            if (aktifMakro == 1)
            {
                if (buttonG == "G1/M1") { btnBG1.Enabled = false; }
                if (buttonG == "G2/M1") { btnBG2.Enabled = false; }
                if (buttonG == "G3/M1") { btnBG3.Enabled = false; }
                if (buttonG == "G4/M1") { btnBG4.Enabled = false; }
                if (buttonG == "G5/M1") { btnBG5.Enabled = false; }
                if (buttonG == "G6/M1") { btnBG6.Enabled = false; }
                if (buttonG == "G7/M1") { btnBG7.Enabled = false; }
                if (buttonG == "G8/M1") { btnBG8.Enabled = false; }
                if (buttonG == "G9/M1") { btnBG9.Enabled = false; }
            }
            if (aktifMakro == 2)
            {
                if (buttonG == "G1/M1") { text_GG1.Enabled = false; }
                if (buttonG == "G2/M1") { btnGG2.Enabled = false; }
                if (buttonG == "G3/M1") { btnGG3.Enabled = false; }
                if (buttonG == "G4/M1") { btnGG4.Enabled = false; }
                if (buttonG == "G5/M1") { btnGG5.Enabled = false; }
                if (buttonG == "G6/M1") { btnGG6.Enabled = false; }
                if (buttonG == "G7/M1") { btnGG7.Enabled = false; }
                if (buttonG == "G8/M1") { btnGG8.Enabled = false; }
                if (buttonG == "G9/M1") { btnGG9.Enabled = false; }
            }
        }

        void enabledButton(int aktifMakro, int btnG)
        {
            String buttonG = "";
            if (btnG == 112) buttonG = "G1/M1";
            if (btnG == 113) buttonG = "G2/M1";
            if (btnG == 114) buttonG = "G3/M1";
            if (btnG == 115) buttonG = "G4/M1";
            if (btnG == 116) buttonG = "G5/M1";
            if (btnG == 117) buttonG = "G6/M1";
            if (btnG == 118) buttonG = "G7/M1";
            if (btnG == 119) buttonG = "G8/M1";
            if (btnG == 120) buttonG = "G9/M1";

            if (aktifMakro == 0)
            {
                if (buttonG == "G1/M1") { btnAG1.Enabled = false; }
                if (buttonG == "G2/M1") { btnAG2.Enabled = false; }
                if (buttonG == "G3/M1") { btnAG3.Enabled = false; }
                if (buttonG == "G4/M1") { btnAG4.Enabled = false; }
                if (buttonG == "G5/M1") { btnAG5.Enabled = false; }
                if (buttonG == "G6/M1") { btnAG6.Enabled = false; }
                if (buttonG == "G7/M1") { btnAG7.Enabled = false; }
                if (buttonG == "G8/M1") { btnAG8.Enabled = false; }
                if (buttonG == "G9/M1") { btnAG9.Enabled = false; }
            }
            if (aktifMakro == 1)
            {
                if (buttonG == "G1/M1") { btnBG1.Enabled = false; }
                if (buttonG == "G2/M1") { btnBG2.Enabled = false; }
                if (buttonG == "G3/M1") { btnBG3.Enabled = false; }
                if (buttonG == "G4/M1") { btnBG4.Enabled = false; }
                if (buttonG == "G5/M1") { btnBG5.Enabled = false; }
                if (buttonG == "G6/M1") { btnBG6.Enabled = false; }
                if (buttonG == "G7/M1") { btnBG7.Enabled = false; }
                if (buttonG == "G8/M1") { btnBG8.Enabled = false; }
                if (buttonG == "G9/M1") { btnBG9.Enabled = false; }
            }
            if (aktifMakro == 2)
            {
                if (buttonG == "G1/M1") { text_GG1.Enabled = false; }
                if (buttonG == "G2/M1") { btnGG2.Enabled = false; }
                if (buttonG == "G3/M1") { btnGG3.Enabled = false; }
                if (buttonG == "G4/M1") { btnGG4.Enabled = false; }
                if (buttonG == "G5/M1") { btnGG5.Enabled = false; }
                if (buttonG == "G6/M1") { btnGG6.Enabled = false; }
                if (buttonG == "G7/M1") { btnGG7.Enabled = false; }
                if (buttonG == "G8/M1") { btnGG8.Enabled = false; }
                if (buttonG == "G9/M1") { btnGG9.Enabled = false; }
            }
        }
        void GkeySDKCallback(LogitechGSDK.GkeyCode gKeyCode, String gKeyOrButtonString, IntPtr context)
        {
            // TextBox_testing.Text = "Callback GKey.";
            if (gKeyCode.keyDown == 0)
            {
                if (gKeyCode.mouse == 1)
                {
                    Invoke(new Action(() =>
                    {
                        //TextBox_testing.Text = "nilai button : " + gKeyOrButtonString;
                        //	Code	to	handle	what	happens	on	gkey	released	on	mouse	
                    }));
                }
                else
                {
                    //button pressed
                    Invoke(new Action(() =>
                    {
                        gKeyOrButtonString = "";
                        btnAG1.Enabled = true; btnAG2.Enabled = true; btnAG3.Enabled = true; btnAG4.Enabled = true; btnAG5.Enabled = true;
                        btnAG6.Enabled = true; btnAG7.Enabled = true; btnAG8.Enabled = true; btnAG9.Enabled = true;
                        btnBG1.Enabled = true; btnBG2.Enabled = true; btnBG3.Enabled = true; btnBG4.Enabled = true; btnBG5.Enabled = true;
                        btnBG6.Enabled = true; btnBG7.Enabled = true; btnBG8.Enabled = true; btnBG9.Enabled = true;
                        text_GG1.Enabled = true; btnGG2.Enabled = true; btnGG3.Enabled = true; btnGG4.Enabled = true; btnGG5.Enabled = true;
                        btnGG6.Enabled = true; btnGG7.Enabled = true; btnGG8.Enabled = true; btnGG9.Enabled = true;
                    }));

                    //	Code	to	handle	what	happens	on	gkey	released	on	keyboard/headset	
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

                    Invoke(new Action(() =>
                    {

                        if (ClientOrder == "alpha")
                        {
                            activeMacro = 0;
                            label10.ForeColor = System.Drawing.Color.Yellow;
                            label11.ForeColor = System.Drawing.Color.White;
                            label12.ForeColor = System.Drawing.Color.White;
                        }
                        else if (ClientOrder == "beta")
                        {
                            activeMacro = 1;
                            label11.ForeColor = System.Drawing.Color.Yellow;
                            label10.ForeColor = System.Drawing.Color.White;
                            label12.ForeColor = System.Drawing.Color.White;
                        }
                        else if (ClientOrder == "gamma")
                        {
                            activeMacro = 2;
                            label12.ForeColor = System.Drawing.Color.Yellow;
                            label11.ForeColor = System.Drawing.Color.White;
                            label10.ForeColor = System.Drawing.Color.White;
                        }
                        //set alpha beta or gamma buat tes aja
                        /*if (textBox_voice.Text == "alpha")
                        {
                            activeMacro = 0;
                        }
                        else if (textBox_voice.Text == "beta")
                        {
                            activeMacro = 1;
                        }
                        else if (textBox_voice.Text == "gamma")
                        {
                            activeMacro = 2;
                        }
                        */
                        //kombinasi macro button
                        if (activeMacro == 0)
                        {
                            macroG1 = text_AG1.Text;
                            macroG2 = text_AG2.Text;
                            macroG3 = text_AG3.Text;
                            macroG4 = text_AG4.Text;
                            macroG5 = text_AG5.Text;
                            macroG6 = text_AG6.Text;
                            macroG7 = text_AG7.Text;
                            macroG8 = text_AG8.Text;
                            macroG9 = text_AG9.Text;
                        }
                        else if (activeMacro == 1)
                        {
                            macroG1 = text_BG1.Text;
                            macroG2 = text_BG2.Text;
                            macroG3 = text_BG3.Text;
                            macroG4 = text_BG4.Text;
                            macroG5 = text_BG5.Text;
                            macroG6 = text_BG6.Text;
                            macroG7 = text_BG7.Text;
                            macroG8 = text_BG8.Text;
                            macroG9 = text_BG9.Text;
                        }
                        else if (activeMacro == 2)
                        {
                            macroG1 = text_GG1.Text;
                            macroG2 = text_GG2.Text;
                            macroG3 = text_GG3.Text;
                            macroG4 = text_GG4.Text;
                            macroG5 = text_GG5.Text;
                            macroG6 = text_GG6.Text;
                            macroG7 = text_GG7.Text;
                            macroG8 = text_GG8.Text;
                            macroG9 = text_GG9.Text;
                        }
                        //check what button pressed is.

                      /*  if (gKeyOrButtonString == "G1/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 0] = macroG1;
                            SendKeys.Send(arx[activeMacro, 0]);
                        }
                        else if (gKeyOrButtonString == "G2/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 1] = macroG2;
                            SendKeys.Send(arx[activeMacro, 1]);
                        }
                        else if (gKeyOrButtonString == "G3/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 2] = macroG3;
                            SendKeys.Send(arx[activeMacro, 2]);
                        }
                        else if (gKeyOrButtonString == "G4/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 3] = macroG4;
                            SendKeys.Send(arx[activeMacro, 3]);
                        }
                        else if (gKeyOrButtonString == "G5/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 4] = macroG5;
                            SendKeys.Send(arx[activeMacro, 1]);
                        }
                        else if (gKeyOrButtonString == "G6/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 5] = macroG6;
                            SendKeys.Send(arx[activeMacro, 5]);
                        }
                        else if (gKeyOrButtonString == "G7/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 6] = macroG7;
                            SendKeys.Send(arx[activeMacro, 6]);
                        }
                        else if (gKeyOrButtonString == "G8/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 7] = macroG8;
                            SendKeys.Send(arx[activeMacro, 7]);
                        }
                        else if (gKeyOrButtonString == "G9/M1")
                        {
                            enabledButton(activeMacro, gKeyOrButtonString);
                            arx[activeMacro, 8] = macroG9;
                            SendKeys.Send(arx[activeMacro, 8]);
                        }*/



                    }));
                    //	Code	to	handle	what	happens	on	gkey	pressed	on	keyboard	 	 

                }

            }
        }
        void OnDestroy()
        {
            //Free	G-Keys	SDKs	before	quitting	the	game	
            LogitechGSDK.LogiGkeyShutdown();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Voice));
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.btnGG9 = new System.Windows.Forms.Button();
            this.text_GG9 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG8 = new System.Windows.Forms.Button();
            this.text_GG8 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG7 = new System.Windows.Forms.Button();
            this.text_GG7 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG6 = new System.Windows.Forms.Button();
            this.text_GG6 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG5 = new System.Windows.Forms.Button();
            this.text_GG5 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG4 = new System.Windows.Forms.Button();
            this.text_GG4 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG3 = new System.Windows.Forms.Button();
            this.text_GG3 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG2 = new System.Windows.Forms.Button();
            this.text_GG2 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnGG1 = new System.Windows.Forms.Button();
            this.text_GG1 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnBG9 = new System.Windows.Forms.Button();
            this.text_BG9 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG8 = new System.Windows.Forms.Button();
            this.text_BG8 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG7 = new System.Windows.Forms.Button();
            this.text_BG7 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG6 = new System.Windows.Forms.Button();
            this.text_BG6 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG5 = new System.Windows.Forms.Button();
            this.text_BG5 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG4 = new System.Windows.Forms.Button();
            this.text_BG4 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG3 = new System.Windows.Forms.Button();
            this.text_BG3 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG2 = new System.Windows.Forms.Button();
            this.text_BG2 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnBG1 = new System.Windows.Forms.Button();
            this.text_BG1 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnAG9 = new System.Windows.Forms.Button();
            this.text_AG9 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG8 = new System.Windows.Forms.Button();
            this.text_AG8 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG7 = new System.Windows.Forms.Button();
            this.text_AG7 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG6 = new System.Windows.Forms.Button();
            this.text_AG6 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG5 = new System.Windows.Forms.Button();
            this.text_AG5 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG4 = new System.Windows.Forms.Button();
            this.text_AG4 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG3 = new System.Windows.Forms.Button();
            this.text_AG3 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG2 = new System.Windows.Forms.Button();
            this.text_AG2 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.btnAG1 = new System.Windows.Forms.Button();
            this.text_AG1 = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button28 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.btn_Apply = new System.Windows.Forms.Button();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.IPServer = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.serverPort = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(1160, 192);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 33);
            this.label12.TabIndex = 140;
            this.label12.Text = "GAMMA";
            // 
            // btnGG9
            // 
            this.btnGG9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG9.FlatAppearance.BorderSize = 0;
            this.btnGG9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG9.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG9.Location = new System.Drawing.Point(1165, 573);
            this.btnGG9.Name = "btnGG9";
            this.btnGG9.Size = new System.Drawing.Size(46, 34);
            this.btnGG9.TabIndex = 139;
            this.btnGG9.Text = "G9";
            this.btnGG9.UseVisualStyleBackColor = false;
            // 
            // text_GG9
            // 
            this.text_GG9.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG9.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG9.ForeColor = System.Drawing.Color.White;
            this.text_GG9.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG9.HintText = "";
            this.text_GG9.isPassword = false;
            this.text_GG9.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG9.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG9.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG9.LineThickness = 3;
            this.text_GG9.Location = new System.Drawing.Point(1218, 573);
            this.text_GG9.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG9.Name = "text_GG9";
            this.text_GG9.Size = new System.Drawing.Size(73, 34);
            this.text_GG9.TabIndex = 138;
            this.text_GG9.Text = "GG9";
            this.text_GG9.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG8
            // 
            this.btnGG8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG8.FlatAppearance.BorderSize = 0;
            this.btnGG8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG8.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG8.Location = new System.Drawing.Point(1165, 531);
            this.btnGG8.Name = "btnGG8";
            this.btnGG8.Size = new System.Drawing.Size(46, 34);
            this.btnGG8.TabIndex = 137;
            this.btnGG8.Text = "G8";
            this.btnGG8.UseVisualStyleBackColor = false;
            // 
            // text_GG8
            // 
            this.text_GG8.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG8.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG8.ForeColor = System.Drawing.Color.White;
            this.text_GG8.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG8.HintText = "";
            this.text_GG8.isPassword = false;
            this.text_GG8.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG8.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG8.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG8.LineThickness = 3;
            this.text_GG8.Location = new System.Drawing.Point(1218, 531);
            this.text_GG8.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG8.Name = "text_GG8";
            this.text_GG8.Size = new System.Drawing.Size(73, 34);
            this.text_GG8.TabIndex = 136;
            this.text_GG8.Text = "GG8";
            this.text_GG8.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG7
            // 
            this.btnGG7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG7.FlatAppearance.BorderSize = 0;
            this.btnGG7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG7.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG7.Location = new System.Drawing.Point(1165, 489);
            this.btnGG7.Name = "btnGG7";
            this.btnGG7.Size = new System.Drawing.Size(46, 34);
            this.btnGG7.TabIndex = 135;
            this.btnGG7.Text = "G7";
            this.btnGG7.UseVisualStyleBackColor = false;
            // 
            // text_GG7
            // 
            this.text_GG7.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG7.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG7.ForeColor = System.Drawing.Color.White;
            this.text_GG7.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG7.HintText = "";
            this.text_GG7.isPassword = false;
            this.text_GG7.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG7.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG7.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG7.LineThickness = 3;
            this.text_GG7.Location = new System.Drawing.Point(1218, 489);
            this.text_GG7.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG7.Name = "text_GG7";
            this.text_GG7.Size = new System.Drawing.Size(73, 34);
            this.text_GG7.TabIndex = 134;
            this.text_GG7.Text = "GG7";
            this.text_GG7.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG6
            // 
            this.btnGG6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG6.FlatAppearance.BorderSize = 0;
            this.btnGG6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG6.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG6.Location = new System.Drawing.Point(1165, 447);
            this.btnGG6.Name = "btnGG6";
            this.btnGG6.Size = new System.Drawing.Size(46, 34);
            this.btnGG6.TabIndex = 133;
            this.btnGG6.Text = "G6";
            this.btnGG6.UseVisualStyleBackColor = false;
            // 
            // text_GG6
            // 
            this.text_GG6.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG6.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG6.ForeColor = System.Drawing.Color.White;
            this.text_GG6.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG6.HintText = "";
            this.text_GG6.isPassword = false;
            this.text_GG6.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG6.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG6.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG6.LineThickness = 3;
            this.text_GG6.Location = new System.Drawing.Point(1218, 447);
            this.text_GG6.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG6.Name = "text_GG6";
            this.text_GG6.Size = new System.Drawing.Size(73, 34);
            this.text_GG6.TabIndex = 132;
            this.text_GG6.Text = "GG6";
            this.text_GG6.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG5
            // 
            this.btnGG5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG5.FlatAppearance.BorderSize = 0;
            this.btnGG5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG5.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG5.Location = new System.Drawing.Point(1165, 405);
            this.btnGG5.Name = "btnGG5";
            this.btnGG5.Size = new System.Drawing.Size(46, 34);
            this.btnGG5.TabIndex = 131;
            this.btnGG5.Text = "G5";
            this.btnGG5.UseVisualStyleBackColor = false;
            // 
            // text_GG5
            // 
            this.text_GG5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG5.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG5.ForeColor = System.Drawing.Color.White;
            this.text_GG5.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG5.HintText = "";
            this.text_GG5.isPassword = false;
            this.text_GG5.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG5.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG5.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG5.LineThickness = 3;
            this.text_GG5.Location = new System.Drawing.Point(1218, 405);
            this.text_GG5.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG5.Name = "text_GG5";
            this.text_GG5.Size = new System.Drawing.Size(73, 34);
            this.text_GG5.TabIndex = 130;
            this.text_GG5.Text = "GG5";
            this.text_GG5.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG4
            // 
            this.btnGG4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG4.FlatAppearance.BorderSize = 0;
            this.btnGG4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG4.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG4.Location = new System.Drawing.Point(1165, 363);
            this.btnGG4.Name = "btnGG4";
            this.btnGG4.Size = new System.Drawing.Size(46, 34);
            this.btnGG4.TabIndex = 129;
            this.btnGG4.Text = "G4";
            this.btnGG4.UseVisualStyleBackColor = false;
            // 
            // text_GG4
            // 
            this.text_GG4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG4.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG4.ForeColor = System.Drawing.Color.White;
            this.text_GG4.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG4.HintText = "";
            this.text_GG4.isPassword = false;
            this.text_GG4.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG4.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG4.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG4.LineThickness = 3;
            this.text_GG4.Location = new System.Drawing.Point(1218, 363);
            this.text_GG4.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG4.Name = "text_GG4";
            this.text_GG4.Size = new System.Drawing.Size(73, 34);
            this.text_GG4.TabIndex = 128;
            this.text_GG4.Text = "GG4";
            this.text_GG4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG3
            // 
            this.btnGG3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG3.FlatAppearance.BorderSize = 0;
            this.btnGG3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG3.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG3.Location = new System.Drawing.Point(1165, 321);
            this.btnGG3.Name = "btnGG3";
            this.btnGG3.Size = new System.Drawing.Size(46, 34);
            this.btnGG3.TabIndex = 127;
            this.btnGG3.Text = "G3";
            this.btnGG3.UseVisualStyleBackColor = false;
            // 
            // text_GG3
            // 
            this.text_GG3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG3.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG3.ForeColor = System.Drawing.Color.White;
            this.text_GG3.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG3.HintText = "";
            this.text_GG3.isPassword = false;
            this.text_GG3.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG3.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG3.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG3.LineThickness = 3;
            this.text_GG3.Location = new System.Drawing.Point(1218, 321);
            this.text_GG3.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG3.Name = "text_GG3";
            this.text_GG3.Size = new System.Drawing.Size(73, 34);
            this.text_GG3.TabIndex = 126;
            this.text_GG3.Text = "GG3";
            this.text_GG3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG2
            // 
            this.btnGG2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG2.FlatAppearance.BorderSize = 0;
            this.btnGG2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG2.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG2.Location = new System.Drawing.Point(1165, 279);
            this.btnGG2.Name = "btnGG2";
            this.btnGG2.Size = new System.Drawing.Size(46, 34);
            this.btnGG2.TabIndex = 125;
            this.btnGG2.Text = "G2";
            this.btnGG2.UseVisualStyleBackColor = false;
            // 
            // text_GG2
            // 
            this.text_GG2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG2.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG2.ForeColor = System.Drawing.Color.White;
            this.text_GG2.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG2.HintText = "";
            this.text_GG2.isPassword = false;
            this.text_GG2.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG2.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG2.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG2.LineThickness = 3;
            this.text_GG2.Location = new System.Drawing.Point(1218, 279);
            this.text_GG2.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG2.Name = "text_GG2";
            this.text_GG2.Size = new System.Drawing.Size(73, 34);
            this.text_GG2.TabIndex = 124;
            this.text_GG2.Text = "GG2";
            this.text_GG2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnGG1
            // 
            this.btnGG1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnGG1.FlatAppearance.BorderSize = 0;
            this.btnGG1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGG1.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGG1.Location = new System.Drawing.Point(1165, 237);
            this.btnGG1.Name = "btnGG1";
            this.btnGG1.Size = new System.Drawing.Size(46, 34);
            this.btnGG1.TabIndex = 123;
            this.btnGG1.Text = "G1";
            this.btnGG1.UseVisualStyleBackColor = false;
            // 
            // text_GG1
            // 
            this.text_GG1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_GG1.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_GG1.ForeColor = System.Drawing.Color.White;
            this.text_GG1.HintForeColor = System.Drawing.Color.Empty;
            this.text_GG1.HintText = "";
            this.text_GG1.isPassword = false;
            this.text_GG1.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_GG1.LineIdleColor = System.Drawing.Color.Gray;
            this.text_GG1.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_GG1.LineThickness = 3;
            this.text_GG1.Location = new System.Drawing.Point(1218, 237);
            this.text_GG1.Margin = new System.Windows.Forms.Padding(4);
            this.text_GG1.Name = "text_GG1";
            this.text_GG1.Size = new System.Drawing.Size(73, 34);
            this.text_GG1.TabIndex = 122;
            this.text_GG1.Text = "GG1";
            this.text_GG1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(1012, 192);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 33);
            this.label11.TabIndex = 121;
            this.label11.Text = "BETA";
            // 
            // btnBG9
            // 
            this.btnBG9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG9.FlatAppearance.BorderSize = 0;
            this.btnBG9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG9.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG9.Location = new System.Drawing.Point(1017, 573);
            this.btnBG9.Name = "btnBG9";
            this.btnBG9.Size = new System.Drawing.Size(46, 34);
            this.btnBG9.TabIndex = 120;
            this.btnBG9.Text = "G9";
            this.btnBG9.UseVisualStyleBackColor = false;
            // 
            // text_BG9
            // 
            this.text_BG9.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG9.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG9.ForeColor = System.Drawing.Color.White;
            this.text_BG9.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG9.HintText = "";
            this.text_BG9.isPassword = false;
            this.text_BG9.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG9.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG9.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG9.LineThickness = 3;
            this.text_BG9.Location = new System.Drawing.Point(1070, 573);
            this.text_BG9.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG9.Name = "text_BG9";
            this.text_BG9.Size = new System.Drawing.Size(73, 34);
            this.text_BG9.TabIndex = 119;
            this.text_BG9.Text = "BG9";
            this.text_BG9.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG8
            // 
            this.btnBG8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG8.FlatAppearance.BorderSize = 0;
            this.btnBG8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG8.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG8.Location = new System.Drawing.Point(1017, 531);
            this.btnBG8.Name = "btnBG8";
            this.btnBG8.Size = new System.Drawing.Size(46, 34);
            this.btnBG8.TabIndex = 118;
            this.btnBG8.Text = "G8";
            this.btnBG8.UseVisualStyleBackColor = false;
            // 
            // text_BG8
            // 
            this.text_BG8.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG8.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG8.ForeColor = System.Drawing.Color.White;
            this.text_BG8.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG8.HintText = "";
            this.text_BG8.isPassword = false;
            this.text_BG8.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG8.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG8.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG8.LineThickness = 3;
            this.text_BG8.Location = new System.Drawing.Point(1070, 531);
            this.text_BG8.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG8.Name = "text_BG8";
            this.text_BG8.Size = new System.Drawing.Size(73, 34);
            this.text_BG8.TabIndex = 117;
            this.text_BG8.Text = "BG8";
            this.text_BG8.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG7
            // 
            this.btnBG7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG7.FlatAppearance.BorderSize = 0;
            this.btnBG7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG7.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG7.Location = new System.Drawing.Point(1017, 489);
            this.btnBG7.Name = "btnBG7";
            this.btnBG7.Size = new System.Drawing.Size(46, 34);
            this.btnBG7.TabIndex = 116;
            this.btnBG7.Text = "G7";
            this.btnBG7.UseVisualStyleBackColor = false;
            // 
            // text_BG7
            // 
            this.text_BG7.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG7.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG7.ForeColor = System.Drawing.Color.White;
            this.text_BG7.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG7.HintText = "";
            this.text_BG7.isPassword = false;
            this.text_BG7.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG7.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG7.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG7.LineThickness = 3;
            this.text_BG7.Location = new System.Drawing.Point(1070, 489);
            this.text_BG7.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG7.Name = "text_BG7";
            this.text_BG7.Size = new System.Drawing.Size(73, 34);
            this.text_BG7.TabIndex = 115;
            this.text_BG7.Text = "BG7";
            this.text_BG7.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG6
            // 
            this.btnBG6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG6.FlatAppearance.BorderSize = 0;
            this.btnBG6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG6.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG6.Location = new System.Drawing.Point(1017, 447);
            this.btnBG6.Name = "btnBG6";
            this.btnBG6.Size = new System.Drawing.Size(46, 34);
            this.btnBG6.TabIndex = 114;
            this.btnBG6.Text = "G6";
            this.btnBG6.UseVisualStyleBackColor = false;
            // 
            // text_BG6
            // 
            this.text_BG6.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG6.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG6.ForeColor = System.Drawing.Color.White;
            this.text_BG6.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG6.HintText = "";
            this.text_BG6.isPassword = false;
            this.text_BG6.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG6.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG6.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG6.LineThickness = 3;
            this.text_BG6.Location = new System.Drawing.Point(1070, 447);
            this.text_BG6.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG6.Name = "text_BG6";
            this.text_BG6.Size = new System.Drawing.Size(73, 34);
            this.text_BG6.TabIndex = 113;
            this.text_BG6.Text = "BG6";
            this.text_BG6.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG5
            // 
            this.btnBG5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG5.FlatAppearance.BorderSize = 0;
            this.btnBG5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG5.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG5.Location = new System.Drawing.Point(1017, 405);
            this.btnBG5.Name = "btnBG5";
            this.btnBG5.Size = new System.Drawing.Size(46, 34);
            this.btnBG5.TabIndex = 112;
            this.btnBG5.Text = "G5";
            this.btnBG5.UseVisualStyleBackColor = false;
            // 
            // text_BG5
            // 
            this.text_BG5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG5.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG5.ForeColor = System.Drawing.Color.White;
            this.text_BG5.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG5.HintText = "";
            this.text_BG5.isPassword = false;
            this.text_BG5.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG5.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG5.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG5.LineThickness = 3;
            this.text_BG5.Location = new System.Drawing.Point(1070, 405);
            this.text_BG5.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG5.Name = "text_BG5";
            this.text_BG5.Size = new System.Drawing.Size(73, 34);
            this.text_BG5.TabIndex = 111;
            this.text_BG5.Text = "BG5";
            this.text_BG5.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG4
            // 
            this.btnBG4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG4.FlatAppearance.BorderSize = 0;
            this.btnBG4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG4.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG4.Location = new System.Drawing.Point(1017, 363);
            this.btnBG4.Name = "btnBG4";
            this.btnBG4.Size = new System.Drawing.Size(46, 34);
            this.btnBG4.TabIndex = 110;
            this.btnBG4.Text = "G4";
            this.btnBG4.UseVisualStyleBackColor = false;
            // 
            // text_BG4
            // 
            this.text_BG4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG4.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG4.ForeColor = System.Drawing.Color.White;
            this.text_BG4.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG4.HintText = "";
            this.text_BG4.isPassword = false;
            this.text_BG4.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG4.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG4.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG4.LineThickness = 3;
            this.text_BG4.Location = new System.Drawing.Point(1070, 363);
            this.text_BG4.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG4.Name = "text_BG4";
            this.text_BG4.Size = new System.Drawing.Size(73, 34);
            this.text_BG4.TabIndex = 109;
            this.text_BG4.Text = "BG4";
            this.text_BG4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG3
            // 
            this.btnBG3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG3.FlatAppearance.BorderSize = 0;
            this.btnBG3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG3.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG3.Location = new System.Drawing.Point(1017, 321);
            this.btnBG3.Name = "btnBG3";
            this.btnBG3.Size = new System.Drawing.Size(46, 34);
            this.btnBG3.TabIndex = 108;
            this.btnBG3.Text = "G3";
            this.btnBG3.UseVisualStyleBackColor = false;
            // 
            // text_BG3
            // 
            this.text_BG3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG3.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG3.ForeColor = System.Drawing.Color.White;
            this.text_BG3.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG3.HintText = "";
            this.text_BG3.isPassword = false;
            this.text_BG3.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG3.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG3.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG3.LineThickness = 3;
            this.text_BG3.Location = new System.Drawing.Point(1070, 321);
            this.text_BG3.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG3.Name = "text_BG3";
            this.text_BG3.Size = new System.Drawing.Size(73, 34);
            this.text_BG3.TabIndex = 107;
            this.text_BG3.Text = "BG3";
            this.text_BG3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG2
            // 
            this.btnBG2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG2.FlatAppearance.BorderSize = 0;
            this.btnBG2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG2.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG2.Location = new System.Drawing.Point(1017, 279);
            this.btnBG2.Name = "btnBG2";
            this.btnBG2.Size = new System.Drawing.Size(46, 34);
            this.btnBG2.TabIndex = 106;
            this.btnBG2.Text = "G2";
            this.btnBG2.UseVisualStyleBackColor = false;
            // 
            // text_BG2
            // 
            this.text_BG2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG2.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG2.ForeColor = System.Drawing.Color.White;
            this.text_BG2.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG2.HintText = "";
            this.text_BG2.isPassword = false;
            this.text_BG2.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG2.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG2.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG2.LineThickness = 3;
            this.text_BG2.Location = new System.Drawing.Point(1070, 279);
            this.text_BG2.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG2.Name = "text_BG2";
            this.text_BG2.Size = new System.Drawing.Size(73, 34);
            this.text_BG2.TabIndex = 105;
            this.text_BG2.Text = "BG2";
            this.text_BG2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnBG1
            // 
            this.btnBG1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnBG1.FlatAppearance.BorderSize = 0;
            this.btnBG1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBG1.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBG1.Location = new System.Drawing.Point(1017, 237);
            this.btnBG1.Name = "btnBG1";
            this.btnBG1.Size = new System.Drawing.Size(46, 34);
            this.btnBG1.TabIndex = 104;
            this.btnBG1.Text = "G1";
            this.btnBG1.UseVisualStyleBackColor = false;
            // 
            // text_BG1
            // 
            this.text_BG1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_BG1.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_BG1.ForeColor = System.Drawing.Color.White;
            this.text_BG1.HintForeColor = System.Drawing.Color.Empty;
            this.text_BG1.HintText = "";
            this.text_BG1.isPassword = false;
            this.text_BG1.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_BG1.LineIdleColor = System.Drawing.Color.Gray;
            this.text_BG1.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_BG1.LineThickness = 3;
            this.text_BG1.Location = new System.Drawing.Point(1070, 237);
            this.text_BG1.Margin = new System.Windows.Forms.Padding(4);
            this.text_BG1.Name = "text_BG1";
            this.text_BG1.Size = new System.Drawing.Size(73, 34);
            this.text_BG1.TabIndex = 103;
            this.text_BG1.Text = "BG1";
            this.text_BG1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(865, 192);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 33);
            this.label10.TabIndex = 102;
            this.label10.Text = "ALPHA";
            // 
            // btnAG9
            // 
            this.btnAG9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG9.FlatAppearance.BorderSize = 0;
            this.btnAG9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG9.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG9.Location = new System.Drawing.Point(870, 573);
            this.btnAG9.Name = "btnAG9";
            this.btnAG9.Size = new System.Drawing.Size(46, 34);
            this.btnAG9.TabIndex = 101;
            this.btnAG9.Text = "G9";
            this.btnAG9.UseVisualStyleBackColor = false;
            // 
            // text_AG9
            // 
            this.text_AG9.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG9.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG9.ForeColor = System.Drawing.Color.White;
            this.text_AG9.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG9.HintText = "";
            this.text_AG9.isPassword = false;
            this.text_AG9.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG9.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG9.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG9.LineThickness = 3;
            this.text_AG9.Location = new System.Drawing.Point(923, 573);
            this.text_AG9.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG9.Name = "text_AG9";
            this.text_AG9.Size = new System.Drawing.Size(73, 34);
            this.text_AG9.TabIndex = 100;
            this.text_AG9.Text = "AG9";
            this.text_AG9.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG8
            // 
            this.btnAG8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG8.FlatAppearance.BorderSize = 0;
            this.btnAG8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG8.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG8.Location = new System.Drawing.Point(870, 531);
            this.btnAG8.Name = "btnAG8";
            this.btnAG8.Size = new System.Drawing.Size(46, 34);
            this.btnAG8.TabIndex = 99;
            this.btnAG8.Text = "G8";
            this.btnAG8.UseVisualStyleBackColor = false;
            // 
            // text_AG8
            // 
            this.text_AG8.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG8.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG8.ForeColor = System.Drawing.Color.White;
            this.text_AG8.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG8.HintText = "";
            this.text_AG8.isPassword = false;
            this.text_AG8.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG8.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG8.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG8.LineThickness = 3;
            this.text_AG8.Location = new System.Drawing.Point(923, 531);
            this.text_AG8.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG8.Name = "text_AG8";
            this.text_AG8.Size = new System.Drawing.Size(73, 34);
            this.text_AG8.TabIndex = 98;
            this.text_AG8.Text = "AG8";
            this.text_AG8.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG7
            // 
            this.btnAG7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG7.FlatAppearance.BorderSize = 0;
            this.btnAG7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG7.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG7.Location = new System.Drawing.Point(870, 489);
            this.btnAG7.Name = "btnAG7";
            this.btnAG7.Size = new System.Drawing.Size(46, 34);
            this.btnAG7.TabIndex = 97;
            this.btnAG7.Text = "G7";
            this.btnAG7.UseVisualStyleBackColor = false;
            // 
            // text_AG7
            // 
            this.text_AG7.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG7.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG7.ForeColor = System.Drawing.Color.White;
            this.text_AG7.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG7.HintText = "";
            this.text_AG7.isPassword = false;
            this.text_AG7.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG7.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG7.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG7.LineThickness = 3;
            this.text_AG7.Location = new System.Drawing.Point(923, 489);
            this.text_AG7.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG7.Name = "text_AG7";
            this.text_AG7.Size = new System.Drawing.Size(73, 34);
            this.text_AG7.TabIndex = 96;
            this.text_AG7.Text = "AG7";
            this.text_AG7.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG6
            // 
            this.btnAG6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG6.FlatAppearance.BorderSize = 0;
            this.btnAG6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG6.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG6.Location = new System.Drawing.Point(870, 447);
            this.btnAG6.Name = "btnAG6";
            this.btnAG6.Size = new System.Drawing.Size(46, 34);
            this.btnAG6.TabIndex = 95;
            this.btnAG6.Text = "G6";
            this.btnAG6.UseVisualStyleBackColor = false;
            // 
            // text_AG6
            // 
            this.text_AG6.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG6.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG6.ForeColor = System.Drawing.Color.White;
            this.text_AG6.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG6.HintText = "";
            this.text_AG6.isPassword = false;
            this.text_AG6.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG6.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG6.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG6.LineThickness = 3;
            this.text_AG6.Location = new System.Drawing.Point(923, 447);
            this.text_AG6.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG6.Name = "text_AG6";
            this.text_AG6.Size = new System.Drawing.Size(73, 34);
            this.text_AG6.TabIndex = 94;
            this.text_AG6.Text = "AG6";
            this.text_AG6.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG5
            // 
            this.btnAG5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG5.FlatAppearance.BorderSize = 0;
            this.btnAG5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG5.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG5.Location = new System.Drawing.Point(870, 405);
            this.btnAG5.Name = "btnAG5";
            this.btnAG5.Size = new System.Drawing.Size(46, 34);
            this.btnAG5.TabIndex = 93;
            this.btnAG5.Text = "G5";
            this.btnAG5.UseVisualStyleBackColor = false;
            // 
            // text_AG5
            // 
            this.text_AG5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG5.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG5.ForeColor = System.Drawing.Color.White;
            this.text_AG5.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG5.HintText = "";
            this.text_AG5.isPassword = false;
            this.text_AG5.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG5.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG5.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG5.LineThickness = 3;
            this.text_AG5.Location = new System.Drawing.Point(923, 405);
            this.text_AG5.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG5.Name = "text_AG5";
            this.text_AG5.Size = new System.Drawing.Size(73, 34);
            this.text_AG5.TabIndex = 92;
            this.text_AG5.Text = "AG5";
            this.text_AG5.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG4
            // 
            this.btnAG4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG4.FlatAppearance.BorderSize = 0;
            this.btnAG4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG4.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG4.Location = new System.Drawing.Point(870, 363);
            this.btnAG4.Name = "btnAG4";
            this.btnAG4.Size = new System.Drawing.Size(46, 34);
            this.btnAG4.TabIndex = 91;
            this.btnAG4.Text = "G4";
            this.btnAG4.UseVisualStyleBackColor = false;
            // 
            // text_AG4
            // 
            this.text_AG4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG4.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG4.ForeColor = System.Drawing.Color.White;
            this.text_AG4.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG4.HintText = "";
            this.text_AG4.isPassword = false;
            this.text_AG4.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG4.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG4.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG4.LineThickness = 3;
            this.text_AG4.Location = new System.Drawing.Point(923, 363);
            this.text_AG4.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG4.Name = "text_AG4";
            this.text_AG4.Size = new System.Drawing.Size(73, 34);
            this.text_AG4.TabIndex = 90;
            this.text_AG4.Text = "AG4";
            this.text_AG4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG3
            // 
            this.btnAG3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG3.FlatAppearance.BorderSize = 0;
            this.btnAG3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG3.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG3.Location = new System.Drawing.Point(870, 321);
            this.btnAG3.Name = "btnAG3";
            this.btnAG3.Size = new System.Drawing.Size(46, 34);
            this.btnAG3.TabIndex = 89;
            this.btnAG3.Text = "G3";
            this.btnAG3.UseVisualStyleBackColor = false;
            // 
            // text_AG3
            // 
            this.text_AG3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG3.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG3.ForeColor = System.Drawing.Color.White;
            this.text_AG3.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG3.HintText = "";
            this.text_AG3.isPassword = false;
            this.text_AG3.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG3.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG3.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG3.LineThickness = 3;
            this.text_AG3.Location = new System.Drawing.Point(923, 321);
            this.text_AG3.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG3.Name = "text_AG3";
            this.text_AG3.Size = new System.Drawing.Size(73, 34);
            this.text_AG3.TabIndex = 88;
            this.text_AG3.Text = "AG3";
            this.text_AG3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG2
            // 
            this.btnAG2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG2.FlatAppearance.BorderSize = 0;
            this.btnAG2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG2.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG2.Location = new System.Drawing.Point(870, 279);
            this.btnAG2.Name = "btnAG2";
            this.btnAG2.Size = new System.Drawing.Size(46, 34);
            this.btnAG2.TabIndex = 87;
            this.btnAG2.Text = "G2";
            this.btnAG2.UseVisualStyleBackColor = false;
            // 
            // text_AG2
            // 
            this.text_AG2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG2.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG2.ForeColor = System.Drawing.Color.White;
            this.text_AG2.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG2.HintText = "";
            this.text_AG2.isPassword = false;
            this.text_AG2.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG2.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG2.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG2.LineThickness = 3;
            this.text_AG2.Location = new System.Drawing.Point(923, 279);
            this.text_AG2.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG2.Name = "text_AG2";
            this.text_AG2.Size = new System.Drawing.Size(73, 34);
            this.text_AG2.TabIndex = 86;
            this.text_AG2.Text = "AG2";
            this.text_AG2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnAG1
            // 
            this.btnAG1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.btnAG1.FlatAppearance.BorderSize = 0;
            this.btnAG1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAG1.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAG1.Location = new System.Drawing.Point(870, 237);
            this.btnAG1.Name = "btnAG1";
            this.btnAG1.Size = new System.Drawing.Size(46, 34);
            this.btnAG1.TabIndex = 85;
            this.btnAG1.Text = "G1";
            this.btnAG1.UseVisualStyleBackColor = false;
            // 
            // text_AG1
            // 
            this.text_AG1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.text_AG1.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.text_AG1.ForeColor = System.Drawing.Color.White;
            this.text_AG1.HintForeColor = System.Drawing.Color.Empty;
            this.text_AG1.HintText = "";
            this.text_AG1.isPassword = false;
            this.text_AG1.LineFocusedColor = System.Drawing.Color.Blue;
            this.text_AG1.LineIdleColor = System.Drawing.Color.Gray;
            this.text_AG1.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.text_AG1.LineThickness = 3;
            this.text_AG1.Location = new System.Drawing.Point(923, 237);
            this.text_AG1.Margin = new System.Windows.Forms.Padding(4);
            this.text_AG1.Name = "text_AG1";
            this.text_AG1.Size = new System.Drawing.Size(73, 34);
            this.text_AG1.TabIndex = 84;
            this.text_AG1.Text = "AG1";
            this.text_AG1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.text_AG1.OnValueChanged += new System.EventHandler(this.text_AG1_OnValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(51, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 33);
            this.label1.TabIndex = 144;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(272, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 33);
            this.label2.TabIndex = 145;
            this.label2.Text = "PORT";
            // 
            // button28
            // 
            this.button28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.button28.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(196)))), ((int)(((byte)(240)))));
            this.button28.FlatAppearance.BorderSize = 0;
            this.button28.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button28.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button28.Location = new System.Drawing.Point(57, 291);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(168, 34);
            this.button28.TabIndex = 146;
            this.button28.Text = "Start Server";
            this.button28.UseVisualStyleBackColor = false;
            this.button28.Click += new System.EventHandler(this.button28_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(864, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 33);
            this.label9.TabIndex = 83;
            this.label9.Text = "MACRO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(51, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 33);
            this.label3.TabIndex = 147;
            this.label3.Text = "VOICE SERVER";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // btn_Apply
            // 
            this.btn_Apply.Location = new System.Drawing.Point(871, 631);
            this.btn_Apply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new System.Drawing.Size(168, 40);
            this.btn_Apply.TabIndex = 148;
            this.btn_Apply.Text = "Apply";
            this.btn_Apply.UseVisualStyleBackColor = true;
            this.btn_Apply.Click += new System.EventHandler(this.btn_Apply_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Location = new System.Drawing.Point(1123, 631);
            this.btn_Edit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(168, 39);
            this.btn_Edit.TabIndex = 149;
            this.btn_Edit.Text = "Edit";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(271, 293);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 33);
            this.label4.TabIndex = 150;
            this.label4.Text = "Not Connected";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Open Sans", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(28, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(218, 41);
            this.label6.TabIndex = 8;
            this.label6.Text = "VOICE MACRO";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1246, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(88, 82);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1346, 99);
            this.panel1.TabIndex = 151;
            // 
            // IPServer
            // 
            this.IPServer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.IPServer.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.IPServer.ForeColor = System.Drawing.Color.White;
            this.IPServer.HintForeColor = System.Drawing.Color.Empty;
            this.IPServer.HintText = "";
            this.IPServer.isPassword = false;
            this.IPServer.LineFocusedColor = System.Drawing.Color.Blue;
            this.IPServer.LineIdleColor = System.Drawing.Color.Gray;
            this.IPServer.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.IPServer.LineThickness = 3;
            this.IPServer.Location = new System.Drawing.Point(57, 237);
            this.IPServer.Margin = new System.Windows.Forms.Padding(4);
            this.IPServer.Name = "IPServer";
            this.IPServer.Size = new System.Drawing.Size(168, 34);
            this.IPServer.TabIndex = 152;
            this.IPServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // serverPort
            // 
            this.serverPort.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.serverPort.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.serverPort.ForeColor = System.Drawing.Color.White;
            this.serverPort.HintForeColor = System.Drawing.Color.Empty;
            this.serverPort.HintText = "";
            this.serverPort.isPassword = false;
            this.serverPort.LineFocusedColor = System.Drawing.Color.Blue;
            this.serverPort.LineIdleColor = System.Drawing.Color.Gray;
            this.serverPort.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.serverPort.LineThickness = 3;
            this.serverPort.Location = new System.Drawing.Point(278, 237);
            this.serverPort.Margin = new System.Windows.Forms.Padding(4);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(73, 34);
            this.serverPort.TabIndex = 153;
            this.serverPort.Text = "456";
            this.serverPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(51, 377);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 33);
            this.label5.TabIndex = 154;
            this.label5.Text = "Instructions";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(51, 423);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(420, 23);
            this.label7.TabIndex = 155;
            this.label7.Text = "1. Modify and apply the macro in alpha, beta, gamma";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(53, 489);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(151, 23);
            this.label8.TabIndex = 156;
            this.label8.Text = "3. Start the server";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(53, 458);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(565, 23);
            this.label13.TabIndex = 157;
            this.label13.Text = "2. Fill the IP and port in the Android Application with the application port";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(53, 518);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(218, 23);
            this.label14.TabIndex = 158;
            this.label14.Text = "4. Speak for macro shifting";
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Open Sans", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(29, 630);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 33);
            this.label15.TabIndex = 159;
            this.label15.Text = "BACK";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // Voice
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(31)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1346, 693);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.serverPort);
            this.Controls.Add(this.IPServer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_Edit);
            this.Controls.Add(this.btn_Apply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button28);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnGG9);
            this.Controls.Add(this.text_GG9);
            this.Controls.Add(this.btnGG8);
            this.Controls.Add(this.text_GG8);
            this.Controls.Add(this.btnGG7);
            this.Controls.Add(this.text_GG7);
            this.Controls.Add(this.btnGG6);
            this.Controls.Add(this.text_GG6);
            this.Controls.Add(this.btnGG5);
            this.Controls.Add(this.text_GG5);
            this.Controls.Add(this.btnGG4);
            this.Controls.Add(this.text_GG4);
            this.Controls.Add(this.btnGG3);
            this.Controls.Add(this.text_GG3);
            this.Controls.Add(this.btnGG2);
            this.Controls.Add(this.text_GG2);
            this.Controls.Add(this.btnGG1);
            this.Controls.Add(this.text_GG1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnBG9);
            this.Controls.Add(this.text_BG9);
            this.Controls.Add(this.btnBG8);
            this.Controls.Add(this.text_BG8);
            this.Controls.Add(this.btnBG7);
            this.Controls.Add(this.text_BG7);
            this.Controls.Add(this.btnBG6);
            this.Controls.Add(this.text_BG6);
            this.Controls.Add(this.btnBG5);
            this.Controls.Add(this.text_BG5);
            this.Controls.Add(this.btnBG4);
            this.Controls.Add(this.text_BG4);
            this.Controls.Add(this.btnBG3);
            this.Controls.Add(this.text_BG3);
            this.Controls.Add(this.btnBG2);
            this.Controls.Add(this.text_BG2);
            this.Controls.Add(this.btnBG1);
            this.Controls.Add(this.text_BG1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnAG9);
            this.Controls.Add(this.text_AG9);
            this.Controls.Add(this.btnAG8);
            this.Controls.Add(this.text_AG8);
            this.Controls.Add(this.btnAG7);
            this.Controls.Add(this.text_AG7);
            this.Controls.Add(this.btnAG6);
            this.Controls.Add(this.text_AG6);
            this.Controls.Add(this.btnAG5);
            this.Controls.Add(this.text_AG5);
            this.Controls.Add(this.btnAG4);
            this.Controls.Add(this.text_AG4);
            this.Controls.Add(this.btnAG3);
            this.Controls.Add(this.text_AG3);
            this.Controls.Add(this.btnAG2);
            this.Controls.Add(this.text_AG2);
            this.Controls.Add(this.btnAG1);
            this.Controls.Add(this.text_AG1);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Voice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Voice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button28_Click(object sender, EventArgs e)
        {
            listener = null;
            client = null;
            netStream = null;
            listener = new TcpListener(IPAddress.Any, servPort);
            listener.Start();
            rcvBuffer = new byte[BUFSIZE];
            label4.Text = "STARTED";
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {

                client = listener.AcceptTcpClient(); // Get clien
                netStream = client.GetStream();
                {
                    rcvBuffer = new byte[BUFSIZE];
                    bytesRcvd = netStream.Read(rcvBuffer, 0, rcvBuffer.Length);
                    ClientOrder = (Encoding.ASCII.GetString(rcvBuffer)).Substring(0, bytesRcvd);
                    ClientOrder = ClientOrder.ToLower();
                    if (ClientOrder == "alpha")
                    {
                        label10.ForeColor = System.Drawing.Color.Yellow;
                        label11.ForeColor = System.Drawing.Color.White;
                        label12.ForeColor = System.Drawing.Color.White;
                    }
                    else if (ClientOrder == "beta")
                    {
                        label11.ForeColor = System.Drawing.Color.Yellow;
                        label10.ForeColor = System.Drawing.Color.White;
                        label12.ForeColor = System.Drawing.Color.White;
                    }
                    else if (ClientOrder == "gamma")
                    {
                        label12.ForeColor = System.Drawing.Color.Yellow;
                        label11.ForeColor = System.Drawing.Color.White;
                        label10.ForeColor = System.Drawing.Color.White;
                    }
                    netStream.Close();
                    client.Close();
                }
            }


        }
        //send data
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.Write(text_to_send);

            }
            else
            {
                MessageBox.Show("Send failed.");
            }
            backgroundWorker2.CancelAsync();
        }



        private void btn_Apply_Click(object sender, EventArgs e)
        {
            text_AG1.Enabled = false; text_AG2.Enabled = false; text_AG3.Enabled = false;
            text_AG4.Enabled = false; text_AG5.Enabled = false; text_AG6.Enabled = false;
            text_AG7.Enabled = false; text_AG8.Enabled = false; text_AG9.Enabled = false;
            text_BG1.Enabled = false; text_BG2.Enabled = false; text_BG3.Enabled = false;
            text_BG4.Enabled = false; text_BG5.Enabled = false; text_BG6.Enabled = false;
            text_BG7.Enabled = false; text_BG8.Enabled = false; text_BG9.Enabled = false;
            text_GG1.Enabled = false; text_GG2.Enabled = false; text_GG3.Enabled = false;
            text_GG4.Enabled = false; text_GG5.Enabled = false; text_GG6.Enabled = false;
            text_GG7.Enabled = false; text_GG8.Enabled = false; text_GG9.Enabled = false;
        
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            text_AG1.Enabled = true; text_AG2.Enabled = true; text_AG3.Enabled = true;
            text_AG4.Enabled = true; text_AG5.Enabled = true; text_AG6.Enabled = true;
            text_AG7.Enabled = true; text_AG8.Enabled = true; text_AG9.Enabled = true;
            text_BG1.Enabled = true; text_BG2.Enabled = true; text_BG3.Enabled = true;
            text_BG4.Enabled = true; text_BG5.Enabled = true; text_BG6.Enabled = true;
            text_BG7.Enabled = true; text_BG8.Enabled = true; text_BG9.Enabled = true;
            text_GG1.Enabled = true; text_GG2.Enabled = true; text_GG3.Enabled = true;
            text_GG4.Enabled = true; text_GG5.Enabled = true; text_GG6.Enabled = true;
            text_GG7.Enabled = true; text_GG8.Enabled = true; text_GG9.Enabled = true;
        }

        private void Voice_Load(object sender, EventArgs e)
        {

        }

        private void text_AG1_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
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
                        if (x == 112)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 0] = macroG1;
                            SendKeys.Send(arx[activeMacro, 0]);
                        }
                        else if (x == 113)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 1] = macroG2;
                            SendKeys.Send(arx[activeMacro, 1]);
                        }
                        else if (x == 114)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 2] = macroG3;
                            SendKeys.Send(arx[activeMacro, 2]);
                        }
                        else if (x == 115)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 3] = macroG4;
                            SendKeys.Send(arx[activeMacro, 3]);
                        }
                        else if (x == 116)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 4] = macroG5;
                            SendKeys.Send(arx[activeMacro, 1]);
                        }
                        else if (x == 117)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 5] = macroG6;
                            SendKeys.Send(arx[activeMacro, 5]);
                        }
                        else if (x == 118)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 6] = macroG7;
                            SendKeys.Send(arx[activeMacro, 6]);
                        }
                        else if (x == 119)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 7] = macroG8;
                            SendKeys.Send(arx[activeMacro, 7]);
                        }
                        else if (x == 120)
                        {
                            enabledButton(activeMacro, x);
                            arx[activeMacro, 8] = macroG9;
                            SendKeys.Send(arx[activeMacro, 8]);
                        }
                    }
                }
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
