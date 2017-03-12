using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;


namespace Logisync
{
    public partial class Login : Form
    {
        bool isLogin = false;
        string whosLogin = "";
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;
        int numDetected = 0;

        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;


            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                if (Labels.Length>0)
                {
                    if (Labels[0]!="")
                    {
                        NumLabels = Convert.ToInt16(Labels[0]);
                        ContTrain = NumLabels;
                        string LoadFaces;

                        for (int tf = 1; tf < NumLabels + 1; tf++)
                        {
                            LoadFaces = "face" + tf + ".bmp";
                            trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                            labels.Add(Labels[tf]);
                        }
                    }
                  
                }
               

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
               // MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            timer1.Start();
            grabber = new Capture(0);
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            grabber.Dispose();
           
            MainForm mainForm = new Logisync.MainForm(whosLogin);
            mainForm.Show();
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            //label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            imageBoxFrameGrabber.Image = currentFrame;
            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       1850,
                       ref termCrit);

                    name = recognizer.Recognize(result);

                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");

                numDetected = facesDetected[0].Length;

                //Set the number of faces detected on the scene
                if (numDetected>0)
                {
                    label1.Text = "You are not recognized";
                }
               

                /*
                //Set the region of interest on the faces

                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */

            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            //imageBoxFrameGrabber.Image = currentFrame;
            //label4.Text = names;
            string xnames = "";
            if (numDetected>0)
            {
                names = names.Trim();
                if (names.Length > 0)
                {
                    xnames = names.Substring(0, names.Length - 1);
                }

                if ((xnames == "") && (numDetected == 1 ))
                {
                    label4.Text = "Please register first";
                    label4.Visible = true;
                }
                else if ((xnames != "") && (numDetected == 1))
                {
                    isLogin = true;

                    names = names.Substring(0, names.Length - 1);
                    whosLogin = names;
                    label1.Text = "Welcome " + names;
                    label5.Visible = true;
                    label2.Visible = true;
                    label3.Text = "It is not you?";
                    label2.Text = "Login Here";
                    label3.Visible = true;
                    label4.Visible = false;
                    //label4.Text = "Welcome,"+names;
                    // label4.Visible = true;
                    Application.Idle -= new EventHandler(FrameGrabber);
                }
            }
           
           // label4.Text = names;
            names = "";
            
            
            //Clear the list(vector) of names
            NamePersons.Clear();

        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            grabber.Dispose();
            Application.Idle -= new EventHandler(FrameGrabber);
            MainForm mainForm = new Logisync.MainForm();
            mainForm.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bunifuCircleProgressbar1.Value = ((bunifuCircleProgressbar1.Value + 1) % 100);
        }
    }
}
