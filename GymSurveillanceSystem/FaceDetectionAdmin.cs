using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GymSurveillanceSystem
{
    public partial class FaceDetectionAdmin : Form
    {
        Capture grabber;
        Image<Bgr, byte> currentFrame;
        Image<Gray, byte> gray, result, TrainedFace = null;
        HaarCascade face = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        int NumLabels, ContTrain = 0;
        int t = 0;
        string command = null;
        public FaceDetectionAdmin()
        {
            face = new HaarCascade("haarcascade_frontalface_default.xml");

            InitializeComponent();
            try
            {
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
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
            catch (Exception e)
            {
                MessageBox.Show("Train item if its first time");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            textBox1.Visible = true;
            button3.Visible = true;
            TrainedFace = result.Resize(125, 125, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            imageBox2.Image = TrainedFace;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ContTrain = ContTrain + 1;

            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            
            MemoryStream ms = new MemoryStream();
            int width = 125;
            int height = 125;
            Bitmap bmp = new Bitmap(width, height);
            imageBox2.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] pic = (byte[])ms.ToArray();
            con = new MySqlConnection(str);
            con.Open();

            
            string query = "INSERT INTO faces(FaceData, name) " +
                "VALUES (@pic, @name)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pic", pic);
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.ExecuteNonQuery();

            query = "SELECT MAX(id_face) FROM faces";
            cmd = new MySqlCommand(query, con);
            var queryResult = cmd.ExecuteScalar();

            int last_id = Int32.Parse(queryResult.ToString());

            query = "INSERT INTO accounts(username, password, laccess) " +
                "VALUES (@username, @password, @laccess)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox1.Text);
            cmd.Parameters.AddWithValue("@laccess", 2);
            cmd.ExecuteNonQuery();

            query = "SELECT MAX(id_user) FROM accounts";
            cmd = new MySqlCommand(query, con);
            var queryResult2 = cmd.ExecuteScalar();

            int last_user_id = Int32.Parse(queryResult2.ToString());


           
            DateTime start_date = DateTime.Now;
            start_date.ToString("yyyy-MM-dd H:mm:ss");


            query = "INSERT INTO person(age, gender, start_date, end_date, address, id_face, id_pass, id_user) " +
                "VALUES (@age, @gender, @start_date, @end_date, @address, @id_face, @id_pass, @id_user)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@age", textBox2.Text);
            cmd.Parameters.AddWithValue("@gender", textBox3.Text);
            cmd.Parameters.AddWithValue("@start_date", start_date);
            cmd.Parameters.AddWithValue("@end_date", null);
            cmd.Parameters.AddWithValue("@address", textBox4.Text);
            cmd.Parameters.AddWithValue("@id_face", last_id);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@id_user", last_user_id);
            command = "Person added: "+cmd.CommandText;
            cmd.ExecuteNonQuery();

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

            

            query = "SELECT MAX(id_person) FROM person";
            cmd = new MySqlCommand(query, con);
            var queryResult1 = cmd.ExecuteScalar();

            int last_person_id = Int32.Parse(queryResult1.ToString());


            query = "INSERT INTO gphistory(id_person, start_date, end_date, id_pass) " +
                "VALUES(@id_person, @start_date, @end_date, @id_pass)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person",last_person_id);
            cmd.Parameters.AddWithValue("@start_date",start_date);
            cmd.Parameters.AddWithValue("@end_date",null);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.ExecuteNonQuery();


            query = "INSERT INTO payment(id_person, id_user, id_pass, payment_time, payment_fee) " +
                "VALUES (@id_person, @id_user, @id_pass, @payment_time, @payment_fee)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person", last_person_id);
            cmd.Parameters.AddWithValue("@id_user", last_user_id);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@payment_time", start_date);
            cmd.Parameters.AddWithValue("@payment_fee", null);
            cmd.ExecuteNonQuery();

            query = "SELECT MAX(id_payment) FROM payment";
            cmd = new MySqlCommand(query, con);
            var queryResult3 = cmd.ExecuteScalar();

            int last_payment_id = Int32.Parse(queryResult3.ToString());

            query = "UPDATE payment " +
                "SET payment_fee = (SELECT fee FROM gympass WHERE id_pass = @id_pass) " +
                "WHERE id_payment = @id_payment AND id_user = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@id_payment", last_payment_id);
            cmd.Parameters.AddWithValue("@id_user", last_user_id);
            cmd.ExecuteNonQuery();

            query = "UPDATE person SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                "WHERE id_person = @id_person";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@end_date", start_date);
            cmd.Parameters.AddWithValue("@id_person", last_person_id);
            cmd.ExecuteNonQuery();

            query = "UPDATE gphistory SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                "WHERE id_person = @id_person";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@end_date", start_date);
            cmd.Parameters.AddWithValue("@id_person", last_person_id);
            cmd.ExecuteNonQuery();

            MessageBox.Show(textBox1.Text + "'s Information added successfully");
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            imageBox2.Image = null;

            

            /*trainingImages.Add(TrainedFace);
            labels.Add(textBox1.Text);*/
            //File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");//add library to read/write to input file

            /*for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
            {
                trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");//sav faces to folder with name face(i)i is no. of face and .bmp extension of detected face image
                File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");//save names to text file
            }
            MessageBox.Show("Image trained and save to database");*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MainPageAdmin f2 = new MainPageAdmin();
            f2.Show();
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if(Capture != null)
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

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void imageBox2_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            grabber = new Capture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber);
            button2.Visible = true;
        }
        void FrameGrabber(object sender, EventArgs e)
        {

            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            
            gray = currentFrame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(25,25));
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(125, 125, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                currentFrame.Draw(f.rect, new Bgr(Color.Red), 1);
            }
            imageBox1.Image = currentFrame;
        }
    }
}
