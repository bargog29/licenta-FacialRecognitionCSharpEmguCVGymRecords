using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections;

namespace GymSurveillanceSystem
{
    public partial class FaceRecognitionAdmin : Form
    {
        //initialize
        #region FacialRecognition
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        //now initialize a list to save recognized names
        List<string> NamePersons = new List<string>();
        string name, names = null;
        int t = 0, ContTrain = 0, NumLabels;
        #endregion
        #region Other
        private static ArrayList id_face = new ArrayList();
        private static ArrayList person_name = new ArrayList();
        private static ArrayList id_person = new ArrayList();
        private static ArrayList age = new ArrayList();
        private static ArrayList gender = new ArrayList();
        private static ArrayList start_date = new ArrayList();
        private static ArrayList end_date = new ArrayList();
        private static ArrayList address = new ArrayList();
        private static ArrayList id_pass = new ArrayList();
        private static ArrayList id_user = new ArrayList();

        string command = null;
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            grabber = new Capture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainPageAdmin f2 = new MainPageAdmin();
            f2.Show();
            Hide();
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Capture != null)
            {
                grabber.Dispose();
                Application.Idle -= FrameGrabber;
                imageBox1.Image = null;
            }
            else
            {
                MessageBox.Show("No session started");
            }
        }

        public FaceRecognitionAdmin()
        {
            
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                
                MySqlConnection con;
                String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
                String query = "SELECT *FROM faces";
                int totalRows = 0;
                int rowNumber = 0;
                con = new MySqlConnection(str);
                
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dbr;
                con.Open();
                dbr = cmd.ExecuteReader();
                int count = 0;
                while (dbr.Read())
                {
                    
                    count = count + 1;
                    string id = dbr[0].ToString();
                    string LabelsInfo = dbr.GetString(2);
                    var imageBytes = (byte[])dbr[1];
                    MemoryStream ms = new MemoryStream(imageBytes);
                    ms.Seek(0, SeekOrigin.Begin);
                    Image img = Image.FromStream(ms);
                    Bitmap bmp = new Bitmap(img);
                    trainingImages.Add(new Image<Gray, byte>(bmp));
                    labels.Add(LabelsInfo);
                    rowNumber = count + 1;
                    totalRows = count;
                    ContTrain = count;
                }
                con.Close();
                #region SaveImagesToFolderIfNeeded
                /*string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");*//*
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }*/
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show("no image trained");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            getData();
            
                if (id_face.Count >= 0)
                {
                    updateDataGrid(); dataGridView1.DataSource = null;
                }
                else
                {
                    MessageBox.Show("Data not found");
                }
           
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

            void FrameGrabber(object sender, EventArgs e)
            {

                NamePersons.Add("");
                label2.Text = "0";
                currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                gray = currentFrame.Convert<Gray, Byte>();
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
              face,
              1.2,
              10,
              Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
              new Size(25, 25));

                foreach (MCvAvgComp f in facesDetected[0])
                {
                    t = t + 1;
                    result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(125, 125, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    {
                        currentFrame.Draw(f.rect, new Bgr(Color.Red), 1);
                        if (trainingImages.ToArray().Length != 0)
                        {
                            MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                            EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                                     trainingImages.ToArray(),
                                     labels.ToArray(),
                                     3000,
                                     ref termCrit);
                            name = recognizer.Recognize(result);
                            currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                        }
                        NamePersons[t - 1] = name;
                        NamePersons.Add("");
                        label2.Text = facesDetected[0].Length.ToString();


                    }

                }
            
                t = 0;
                for (int i = 0; i < facesDetected[0].Length; i++)
                {
                    names = names + NamePersons[i];
                }

                imageBox1.Image = currentFrame;
                label3.Text = names;
            label3.Visible = false;
                names = "";
                NamePersons.Clear();
            }

        private void getData()
        {
            try
            {
                String str = "server=localhost;port = 3306; database = licenta;UID = root;password = oracle";
                if (label3.Text != "")
                {
                    String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user " +
                    "AND f.name = '"+label3.Text+"'";
                    MySqlConnection con = new MySqlConnection(str);
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(query, con);

                   

                    MySqlDataReader dbr;
                    dbr = cmd.ExecuteReader();
                    if (dbr.HasRows)
                    {
                        while (dbr.Read())
                        {
                            id_face.Add(dbr["id_face"]);
                            person_name.Add(dbr["name"].ToString());
                            id_person.Add(dbr["id_person"]);
                            age.Add(dbr["age"]);
                            gender.Add(dbr["gender"].ToString());
                            start_date.Add(dbr["start_date"].ToString());
                            end_date.Add(dbr["end_date"].ToString());
                            address.Add(dbr["address"].ToString());
                            id_pass.Add(dbr["id_pass"].ToString());
                            id_user.Add(dbr["id_user"].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data not found");
                    }
                    
                    con.Close();
                    con.Open();                    
                    command = "Person recognized, data taken: " + cmd.CommandText;
                    DateTime cmddate = DateTime.Now;
                    cmddate.ToString("yyyy-MM-dd H:mm:ss");
                    query = "INSERT INTO cmdlog(user_id, date, command) " +
                   "VALUES(@user_id, @date, @command)";
                    cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user_id", 2);
                    cmd.Parameters.AddWithValue("@date", cmddate);
                    cmd.Parameters.AddWithValue("@command", command);
                    cmd.ExecuteNonQuery();

                    command = "";
                    con.Close();
                }
              
                
               

                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
        private void updateDataGrid()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < id_face.Count; i++)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView1);
                newRow.Cells[0].Value = id_face[i];
                newRow.Cells[1].Value = person_name[i];
                newRow.Cells[2].Value = id_person[i];
                newRow.Cells[3].Value = age[i];
                newRow.Cells[4].Value = gender[i];
                newRow.Cells[5].Value = start_date[i];
                newRow.Cells[6].Value = end_date[i];
                newRow.Cells[7].Value = address[i];
                newRow.Cells[8].Value = id_pass[i];
                newRow.Cells[9].Value = id_user[i];
                dataGridView1.Rows.Add(newRow);

            }
            id_face.Clear();
            person_name.Clear();
            id_person.Clear();
            age.Clear();
            gender.Clear();
            start_date.Clear();
            end_date.Clear();
            address.Clear();
            id_pass.Clear();
            id_user.Clear();
        }

    }
}
