using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MetroFramework.Controls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymSurveillanceSystem
{
    public partial class MainPageUser : Form
    {
        #region ChangePicture
        Capture grabber;
        Image<Bgr, byte> currentFrame;
        Image<Gray, byte> gray, result, TrainedFace = null;
        HaarCascade face = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        int NumLabels, ContTrain = 0;
        int t = 0;
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

        private Emgu.CV.Image<Bgr, byte> emguImage;

        private bool isHovering = false;

        private DateTime startWorkout;
        private string type = null;
        private string muscle = null;
        private string command = null;
        #endregion


        private int userId;
        public int UserId 
        {
            get { return userId; }
            set { userId = value; }
        }
        public class Exercise
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string MuscleGroup { get; set; }
            public string Equipment { get; set; }
            public string Difficulty { get; set; }
            public string Instructions { get; set; }
        }



        public MainPageUser()
        {
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            InitializeComponent();
            
        }

        private void MainPageUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void MainPageUser_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            button10.BackColor = Color.Green;
            button11.Visible = false;
            MsgHello();
            GetPic();
            GetProfileData();
            CheckIfActive();
            GetPassword();
            GetTimeSpent();
            GetCheatDays();
            GetPaymentData();
            if (textBox7.Text == "fitness" || textBox7.Text == "aerobic" || textBox7.Text == "pump"
                || textBox7.Text == "pilates")
            {
                button11.Visible = true;
            }
            if (label10.Text == "INACTIVE")
            {
                button9.Enabled = false;
                button11.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }
        }

        private void GetPaymentData()
        {
            int currentUserId = userId;
            try
            {
                String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";

                String query = "SELECT id_payment AS ID_Payment, f.name AS Customer_Name, pass_name AS Pass_Name, payment_time AS Payment_Time, payment_fee AS Payment_Fee " +
                    "FROM accounts a, person p, payment py, faces f, gympass gp " +
                    "WHERE a.id_user = p.id_user " +
                    "AND p.id_user = py.id_user " +
                    "AND a.id_user = py.id_user " +
                    "AND p.id_face = f.id_face " +
                    "AND py.id_pass = gp.id_pass  " + 
                    "AND a.id_user = @id_user";
                MySqlConnection con = new MySqlConnection(str);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dbr;
                cmd.Parameters.AddWithValue("@id_user", currentUserId);
                dbr = cmd.ExecuteReader();


                DataTable dataTable = new DataTable();
                dataTable.Load(dbr);

                metroGrid1.DataSource = dataTable;
                metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                con.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        public void GetTimeSpent()
        {
            int currentUserId = userId;
            MySqlConnection con;
            string str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            string query = "SELECT SUM(elapsed_time) AS total_seconds " +
                "FROM gymtime " +
                "WHERE id_user = @id_user;";
            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            var queryResult = cmd.ExecuteScalar();

            if (queryResult != DBNull.Value && queryResult != null)
            {
                int totalTimeSeconds = Convert.ToInt32(queryResult);
                int hours = totalTimeSeconds / 3600;
                int minutes = (totalTimeSeconds % 3600) / 60;
                int seconds = totalTimeSeconds % 60;

                string formattedTime = $"{hours} hours, {minutes} minutes, {seconds} seconds";
                label11.Text = formattedTime;
            }
            else
            {
                label11.Text = "N/A";
            }

            con.Close();
        }

        public void GetCheatDays()
        {
            int currentUserId = userId;
            MySqlConnection con;
            string str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            string query = "SELECT DATEDIFF(CURDATE(), t.date) AS days_passed " +
                "FROM gymtime t " +
                "WHERE t.id_user = @id_user " +
                "ORDER BY t.date DESC " +
                "LIMIT 1; ";
            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            var queryResult = cmd.ExecuteScalar();

            if (queryResult != DBNull.Value && queryResult != null)
            {
                int daysPassed = Convert.ToInt32(queryResult);
                label12.Text = daysPassed.ToString() + " day(s)";
            }
            else
            {
                label12.Text = "N/A days";
            }

            con.Close();
        }

        public void MsgHello()
        {
            int currentUserId = userId;
            string username = null;
            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            String query = "SELECT f.name FROM person p, faces f " +
                "WHERE p.id_user = @id_user AND p.id_face = f.id_face";
            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            MySqlDataReader dbr;
            dbr = cmd.ExecuteReader();
            if (dbr.Read())
            {
                username = dbr["name"].ToString();
            }

            label1.Text = "Hello, " + username;
            con.Close();
        }

        public void GetPic()
        {
            int currentUserId = userId;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            using (MySqlConnection connection = new MySqlConnection(str))
            {
                connection.Open();

                string query = "SELECT FaceData FROM faces f, person p " +
                    "WHERE p.id_user = @id_user " +
                    "AND p.id_face = f.id_face";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_user", currentUserId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            byte[] imageData = (byte[])reader["FaceData"];

                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                                Emgu.CV.IImage emguImage = (Emgu.CV.IImage)new Emgu.CV.Image<Bgr, byte>(new Bitmap(image));

                                imageBox1.Image = emguImage;
                                imageBox1.Refresh();
                            }
                        }
                    }
                }
            }
        }

        public void GetProfileData()
        {
            int currentUserId = userId;
            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            String query = "SELECT name, age, gender, start_date, end_date, address, pass_name " +
                " FROM faces f, person p, accounts a, gympass gp" +
                " WHERE f.id_face = p.id_face " +
                "AND p.id_pass = gp.id_pass " +
                "AND p.id_user = a.id_user " +
                "AND a.id_user = p.id_user " +
                "AND a.id_user = @id_user";
            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            MySqlDataReader dbr;
            dbr = cmd.ExecuteReader();
            dbr.Read();
            textBox1.Text = dbr["name"].ToString();
            textBox2.Text = dbr["age"].ToString();
            textBox3.Text = dbr["gender"].ToString();
            textBox6.Text = dbr["address"].ToString();
            textBox4.Text = dbr["start_date"].ToString();
            textBox5.Text = dbr["end_date"].ToString();
            textBox7.Text = dbr["pass_name"].ToString();
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox7.Enabled = false;
        }

        public void CheckIfActive()
        {
            int currentUserId = userId;
            string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT start_date, end_date FROM person WHERE id_user = @id_user;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_user", currentUserId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime startDate = reader.GetDateTime("start_date");
                            DateTime endDate = reader.GetDateTime("end_date");
                            DateTime currentTime = DateTime.Now;
                            if (currentTime >= startDate && currentTime <= endDate)
                            {
                                label10.Text = "ACTIVE";
                                label10.ForeColor = Color.Green;
                            }
                            else
                            {
                                label10.Text = "INACTIVE";
                                label10.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        public void GetPassword()
        {
            int currentUserId = userId;
            string password = null;
            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            String query = "SELECT password FROM accounts " +
                "WHERE id_user = @id_user";
            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            MySqlDataReader dbr;
            dbr = cmd.ExecuteReader();
            if (dbr.Read())
            {
                password = dbr["password"].ToString();
            }

            metroTextBox1.Text = password;
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            UserId = 0;
            Form1 f1 = new Form1();
            f1.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime cmddate = DateTime.Now;
            cmddate.ToString("yyyy-MM-dd H:mm:ss");

            int currentUserId = userId;
            Int32 age;
            Int32.TryParse(textBox2.Text, out age);

            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            String query = "UPDATE faces " +
                "SET name = @name " +
                "WHERE id_face = (SELECT id_face FROM person WHERE id_user = @id_user)";


            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "INSERT INTO cmdlog(user_id, date, command) " +
                "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", currentUserId);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "UPDATE person " +
                "SET age = @age, gender = @gender, address = @address " +
                "WHERE id_user = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@age", age);
            cmd.Parameters.AddWithValue("@gender", textBox3.Text);
            cmd.Parameters.AddWithValue("@address", textBox6.Text);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "INSERT INTO cmdlog(user_id, date, command) " +
                "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", currentUserId);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "UPDATE accounts " +
                "SET username = @username, password = @password " +
                "WHERE id_user = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox1.Text);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "INSERT INTO cmdlog(user_id, date, command) " +
                "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", currentUserId);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";
            MessageBox.Show("Profile Updated!");
            MsgHello();
            GetPic();
            GetProfileData();
            GetPassword();
            GetPaymentData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.FileName = "output.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    document.Open();

                    Paragraph title = new Paragraph("Exported Profile", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD));
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    document.Add(new Paragraph(" "));

                    Bitmap imageBitmap = (Bitmap)imageBox1.Image.Bitmap;
                    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imageBitmap, System.Drawing.Imaging.ImageFormat.Jpeg);
                    pdfImage.Alignment = Element.ALIGN_CENTER;
                    document.Add(pdfImage);

                    document.Add(new Paragraph(" "));

                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = Element.ALIGN_CENTER;

                    int labelIndex = 2;
                    int textBoxIndex = 1;

                    while (labelIndex <= groupBox1.Controls.Count && textBoxIndex <= groupBox1.Controls.Count)
                    {
                        Label label = groupBox1.Controls.OfType<Label>().FirstOrDefault(lbl => lbl.Name == $"label{labelIndex}");
                        TextBox textBox = groupBox1.Controls.OfType<TextBox>().FirstOrDefault(txt => txt.Name == $"textBox{textBoxIndex}");

                        if (label != null && textBox != null)
                        {
                            table.AddCell(new Phrase(label.Text));
                            table.AddCell(new Phrase(textBox.Text));

                            labelIndex += 1;
                            textBoxIndex += 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    document.Add(table);

                    document.Close();
                    MessageBox.Show("PDF exported successfully!");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            int currentUserId = userId;
            if(metroTextBox2.Text == metroTextBox3.Text)
            {
                MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            String query = "UPDATE accounts " +
                    "SET password = @password " +
                "WHERE id_user = @id_user";
            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@password", metroTextBox3.Text);
                cmd.Parameters.AddWithValue("@id_user", currentUserId);
                command = cmd.CommandText;
            cmd.ExecuteNonQuery();

                DateTime cmddate = DateTime.Now;
                cmddate.ToString("yyyy-MM-dd H:mm:ss");
                query = "INSERT INTO cmdlog(user_id, date, command) " +
               "VALUES(@user_id, @date, @command)";
                cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", currentUserId);
                cmd.Parameters.AddWithValue("@date", cmddate);
                cmd.Parameters.AddWithValue("@command", command);
                cmd.ExecuteNonQuery();

                command = "";


                con.Close();
                metroTextBox2.Text = "";
                metroTextBox3.Text = "";
                MessageBox.Show("Password changed! You'll be logged out!");
                button1_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Different passwords");
            }
        }

        private void metroTextBox1_MouseEnter(object sender, EventArgs e)
        {
            isHovering = true;
            metroTextBox1.UseSystemPasswordChar = false;
        }

        private void metroTextBox1_MouseLeave(object sender, EventArgs e)
        {
            isHovering = false;
            metroTextBox1.UseSystemPasswordChar = true;
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isHovering)
            {
                metroTextBox1.UseSystemPasswordChar = true;
            }
        }

        private void metroTextBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            TrainedFace = result.Resize(125, 125, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            imageBox2.Image = TrainedFace;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int currentUserId = userId;
            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";

            MemoryStream ms = new MemoryStream();
            int width = 125;
            int height = 125;
            Bitmap bmp = new Bitmap(width, height);
            imageBox2.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, width, height));
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] pic = (byte[])ms.ToArray();
            con = new MySqlConnection(str);
            con.Open();

            String query = "UPDATE faces " +
                "SET FaceData = @face " +
                "WHERE id_face = (SELECT id_face FROM person WHERE id_user = @id_user)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@face", pic);
            cmd.Parameters.AddWithValue("@id_user", currentUserId);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();
            imageBox2.Image = null;
            MessageBox.Show("Profile Picture Updated");

            DateTime cmddate = DateTime.Now;
            cmddate.ToString("yyyy-MM-dd H:mm:ss");
            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", currentUserId);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            GetPic();
            tabControl1.SelectedIndex = 0;
            tabControl1.TabPages.Remove(tabPage2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int currentUserId = userId;
            if (button9.Text == "Start")
            {
                startWorkout = DateTime.Now;
                button9.Text = "Stop";
                button10.BackColor = Color.Red;
            }
            else
            {
                TimeSpan elapsedTime = DateTime.Now - startWorkout;
                int timeInSeconds = (int)elapsedTime.TotalSeconds;

                DateTime workoutEndDate = DateTime.Now;
                workoutEndDate.ToString("yyyy-MM-dd H:mm:ss");
                MySqlConnection con;
                string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
                con = new MySqlConnection(connectionString);
                con.Open();
                string query = "INSERT INTO gymtime " +
                    "VALUES(@id_user, @elapsed_time, @date)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id_user", currentUserId);
                cmd.Parameters.AddWithValue("@elapsed_time", timeInSeconds);
                cmd.Parameters.AddWithValue("@date", workoutEndDate);
                cmd.ExecuteNonQuery();

                button9.Text = "Start";
                button10.BackColor = Color.Green;
                GetTimeSpent();
                GetCheatDays();
            }
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Add(tabPage3);
            if(textBox7.Text == "fitness")
            {
                type = "strength";
            }
            else if(textBox7.Text == "aerobic")
            {
                type = "plyometrics";
            }
            else if (textBox7.Text == "pump")
            {
                type = "powerlifting";
            }
            else if (textBox7.Text == "pilates")
            {
                type = "stretching";
            }
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://exercises-by-api-ninjas.p.rapidapi.com/v1/exercises?" +
                "type="+type),
                Headers =
        {
            { "X-RapidAPI-Key", "698c29201emshddf440b92eedac3p11c9f2jsn5e56bbc67dcf" },
            { "X-RapidAPI-Host", "exercises-by-api-ninjas.p.rapidapi.com" },
        },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                var exercises = JsonConvert.DeserializeObject<List<Exercise>>(body);

                var formattedData = new StringBuilder();

                foreach (var exercise in exercises)
                {
                    formattedData.AppendLine($"Exercise Name: {exercise.Name}");
                    formattedData.AppendLine($"Type: {exercise.Type}");
                    formattedData.AppendLine($"Muscle Group: {exercise.MuscleGroup}");
                    formattedData.AppendLine($"Equipment: {exercise.Equipment}");
                    formattedData.AppendLine($"Difficulty: {exercise.Difficulty}");
                    formattedData.AppendLine($"Instructions: {exercise.Instructions}");
                    formattedData.AppendLine();
                }

                richTextBox1.AppendText(formattedData.ToString());
            }
            tabControl1.SelectedIndex = 2;
            label13.Text = "Personalized workout for: " + textBox1.Text;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            tabControl1.TabPages.Remove(tabPage3);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            tabControl1.TabPages.Remove(tabPage2);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (Capture != null)
            {
                grabber.Dispose();
                Application.Idle -= FrameGrabber;
                imageBox3.Image = null;
            }
            else
            {
                MessageBox.Show("No session started");
            }
            GetPic();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Add(tabPage2);
            tabControl1.SelectedIndex = 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            grabber = new Capture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber);
        }
        void FrameGrabber(object sender, EventArgs e)
        {

            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            gray = currentFrame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(25, 25));
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(125, 125, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                currentFrame.Draw(f.rect, new Bgr(Color.Red), 1);
            }
            imageBox3.Image = currentFrame;
        }
    }
}
