using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GymSurveillanceSystem
{
    public partial class AdminStatistics : Form
    {

        public AdminStatistics()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainPageAdmin f2 = new MainPageAdmin();
            f2.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.Series.Add("PaymentsPerDay");

            string Query = "SELECT DATE(payment_time) AS PaymentDate, COUNT(*) AS NumPayments FROM payment GROUP BY PaymentDate;";
            MySqlConnection con;
            string str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(str);

            MySqlCommand cmdDataBase = new MySqlCommand(Query, con);
            MySqlDataReader myReader;

            try
            {
                con.Open();
                myReader = cmdDataBase.ExecuteReader();
                while (myReader.Read())
                {
                    string paymentDate = myReader.GetDateTime("PaymentDate").ToString("yyyy-MM-dd");
                    int numPayments = myReader.GetInt32("NumPayments");
                    this.chart1.Series["PaymentsPerDay"].Points.AddXY(paymentDate, numPayments);
                }

                chart1.Series["PaymentsPerDay"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                chart1.Series["PaymentsPerDay"].IsValueShownAsLabel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

            chart3.Series.Clear();

            string query = "SELECT gender, COUNT(*) AS NumGymPassHolders FROM person GROUP BY gender;";

            string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        chart3.Series.Add("GymPassHolders");
                        chart3.Series["GymPassHolders"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

                        while (reader.Read())
                        {
                            string gender = reader.GetString("gender");
                            int numGymPassHolders = reader.GetInt32("NumGymPassHolders");

                            chart3.Series["GymPassHolders"].Points.AddXY(gender, numGymPassHolders);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            chart3.ChartAreas[0].AxisX.Title = "Gender";
            chart3.ChartAreas[0].AxisY.Title = "Number of Gym Pass Holders";
            chart3.ChartAreas[0].AxisX.Interval = 1;

            chart3.ChartAreas[0].AxisX.Interval = 1;
            chart3.ChartAreas[0].AxisX.Title = "Gender";
            chart3.ChartAreas[0].AxisY.Title = "Number of Gym Pass Holders";
            chart2.Series.Clear();
            chart2.Series.Add("Gym Pass Name");
            Query = "select g.pass_name AS PassName, p.id_pass, COUNT(id_person) AS NumPers " +
                "FROM person p, gympass g " +
                "WHERE p.id_pass = g.id_pass " +
                "GROUP BY id_pass;";
            str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(str);
            cmdDataBase = new MySqlCommand(Query, con);

            try
            {
                con.Open();
                myReader = cmdDataBase.ExecuteReader();
                while (myReader.Read())
                {
                    this.chart2.Series["Gym Pass Name"].Points.AddXY(myReader.GetString("PassName"), myReader.GetString("NumPers"));
                }
myReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            MySqlConnection connection1 = new MySqlConnection(connectionString);

            try
            {
                connection1.Open();

                query = "SELECT id_user, SUM(elapsed_time) FROM gymtime GROUP BY id_user";
                MySqlCommand command = new MySqlCommand(query, connection1);
                MySqlDataReader reader = command.ExecuteReader();

                List<int> userIDs = new List<int>();
                List<int> totalTimes = new List<int>();

                while (reader.Read())
                {
                    int userID = reader.GetInt32(0);
                    int totalTime = reader.GetInt32(1);

                    userIDs.Add(userID);
                    totalTimes.Add(totalTime);
                }

                reader.Close();

                chart4.Series.Clear();
                chart4.ChartAreas.Clear();

                ChartArea chartArea = new ChartArea();
                chart4.ChartAreas.Add(chartArea);

                Series series = new Series();
                series.ChartType = SeriesChartType.Pie;
                series.Points.DataBindXY(userIDs, totalTimes);

                chart4.Series.Add(series);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                connection1.Close();
            }
        }

        private void AdminStatistics_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        public class PageNumberHandler : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                int pageNumber = writer.PageNumber;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                PdfContentByte pdfContentByte = writer.DirectContent;

                string pageNumberText = pageNumber.ToString();
                float fontSize = 10;
                float textWidth = baseFont.GetWidthPoint(pageNumberText, fontSize);
                float textHeight = baseFont.GetFontDescriptor(BaseFont.ASCENT, fontSize);
                float textX = (pageSize.Left + pageSize.Right - textWidth) / 2;
                float textY = pageSize.GetBottom(30) + (textHeight / 2);

                pdfContentByte.BeginText();
                pdfContentByte.SetFontAndSize(baseFont, fontSize);
                pdfContentByte.SetTextMatrix(textX, textY);
                pdfContentByte.ShowText(pageNumberText);
                pdfContentByte.EndText();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection con;
            string str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(str);
            string query = "SELECT g.pass_name, SUM(p.payment_fee) AS total_revenue " +
                               "FROM gympass g " +
                               "JOIN payment p ON g.id_pass = p.id_pass " +
                               "GROUP BY g.pass_name " +
                               "ORDER BY total_revenue DESC;";

                MySqlCommand command = new MySqlCommand(query, con);
                con.Open();
                MySqlDataReader reader = command.ExecuteReader();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save Revenue Analysis Report";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {


                string filePath = saveFileDialog.FileName;

                Document document = new Document();
                PdfPageEventHelper pageEventHelper = new PdfPageEventHelper();
                
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.PageEvent = new PageNumberHandler();

                document.Open();

                Chunk dateChunk = new Chunk(DateTime.Now.ToString("dd-MM-yyyy"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD));
                Paragraph dateParagraph = new Paragraph(dateChunk);
                dateParagraph.Alignment = Element.ALIGN_LEFT;
                document.Add(dateParagraph);
                document.Add(new Paragraph("\n"));

                Paragraph doctitle = new Paragraph("GENERAL REPORT", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD));
                doctitle.Alignment = Element.ALIGN_CENTER;
                doctitle.SpacingAfter = 15f; 
                document.Add(doctitle);
                document.Add(new Paragraph("\n"));

                Paragraph title = new Paragraph("Revenue Analysis Report", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 15f; 
                document.Add(title);

                PdfPTable table = new PdfPTable(2);
                table.AddCell("Pass Name");
                table.AddCell("Total Revenue");

                while (reader.Read())
                {
                    string passName = reader.GetString("pass_name");
                    decimal totalRevenue = reader.GetDecimal("total_revenue");

                    table.AddCell(passName);
                    table.AddCell(totalRevenue.ToString());
                }

                document.Add(table);
                con.Close();
                document.Add(new Paragraph(" ")); 
                con.Open();
                MySqlCommand ageGroupCommand = new MySqlCommand("SELECT gp.pass_name, " +
            "SUM(CASE WHEN p.age BETWEEN 0 AND 2 THEN 1 ELSE 0 END) AS age_group_baby, " +
            "SUM(CASE WHEN p.age BETWEEN 3 AND 39 THEN 1 ELSE 0 END) AS age_group_young_adults, " +
            "SUM(CASE WHEN p.age BETWEEN 40 AND 59 THEN 1 ELSE 0 END) AS age_group_middle_aged_adults, " +
            "SUM(CASE WHEN p.age >= 60 THEN 1 ELSE 0 END) AS age_group_old_adults " +
            "FROM gympass gp " +
            "INNER JOIN person p ON gp.id_pass = p.id_pass " +
            "GROUP BY gp.pass_name", con);
                MySqlDataReader ageGroupReader = ageGroupCommand.ExecuteReader();

                Paragraph ageGroupTitle = new Paragraph("Age Group Analysis Report", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD));
                ageGroupTitle.Alignment = Element.ALIGN_CENTER;
                ageGroupTitle.SpacingAfter = 15f; 
                document.Add(ageGroupTitle);

                PdfPTable ageGroupTable = new PdfPTable(5);
                ageGroupTable.AddCell("Gym Pass");
                ageGroupTable.AddCell("Age Group: Baby (0-2)");
                ageGroupTable.AddCell("Age Group: Young Adults (3-39)");
                ageGroupTable.AddCell("Age Group: Middle-Aged Adults (40-59)");
                ageGroupTable.AddCell("Age Group: Old Adults (60+)");

                while (ageGroupReader.Read())
                {
                    ageGroupTable.AddCell(ageGroupReader.GetString("pass_name"));
                    ageGroupTable.AddCell(ageGroupReader.GetInt32("age_group_baby").ToString());
                    ageGroupTable.AddCell(ageGroupReader.GetInt32("age_group_young_adults").ToString());
                    ageGroupTable.AddCell(ageGroupReader.GetInt32("age_group_middle_aged_adults").ToString());
                    ageGroupTable.AddCell(ageGroupReader.GetInt32("age_group_old_adults").ToString());
                }

                document.Add(ageGroupTable);
                con.Close();
                con.Open();

                MySqlCommand passComparisonCommand = new MySqlCommand("SELECT gp.pass_name, COUNT(DISTINCT p.id_person) AS num_members, " +
            "SUM(pm.payment_fee) AS total_revenue, COUNT(*) AS num_payments " +
            "FROM gympass gp " +
            "INNER JOIN person p ON gp.id_pass = p.id_pass " +
            "INNER JOIN payment pm ON p.id_person = pm.id_person " +
            "GROUP BY gp.pass_name", con);
                MySqlDataReader passComparisonReader = passComparisonCommand.ExecuteReader();

                Paragraph passComparisonTitle = new Paragraph("Pass Comparison Report", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD));
                passComparisonTitle.Alignment = Element.ALIGN_CENTER;
                passComparisonTitle.SpacingAfter = 15f; 
                document.Add(passComparisonTitle);

                PdfPTable passComparisonTable = new PdfPTable(4);
                passComparisonTable.AddCell("Gym Pass");
                passComparisonTable.AddCell("Number of Members");
                passComparisonTable.AddCell("Total Revenue");
                passComparisonTable.AddCell("Number of Payments");

                while (passComparisonReader.Read())
                {
                    passComparisonTable.AddCell(passComparisonReader.GetString("pass_name"));
                    passComparisonTable.AddCell(passComparisonReader.GetInt32("num_members").ToString());
                    passComparisonTable.AddCell(passComparisonReader.GetDecimal("total_revenue").ToString());
                    passComparisonTable.AddCell(passComparisonReader.GetInt32("num_payments").ToString());
                }

                document.Add(passComparisonTable);
                con.Close();
                con.Open();

                document.Add(new Paragraph("\n"));

                MySqlCommand genderDistributionCommand = new MySqlCommand("SELECT gp.pass_name, " +
                    "SUM(CASE WHEN p.gender = 'male' THEN 1 ELSE 0 END) AS num_male, " +
                    "SUM(CASE WHEN p.gender = 'female' THEN 1 ELSE 0 END) AS num_female, " +
                    "SUM(CASE WHEN p.gender = 'other' THEN 1 ELSE 0 END) AS num_other " +
                    "FROM gympass gp " +
                    "INNER JOIN person p ON gp.id_pass = p.id_pass " +
                    "GROUP BY gp.pass_name", con);
                MySqlDataReader genderDistributionReader = genderDistributionCommand.ExecuteReader();

                Paragraph genderDistributionTitle = new Paragraph("Gender Distribution Report", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD));
                genderDistributionTitle.Alignment = Element.ALIGN_CENTER;
                genderDistributionTitle.SpacingAfter = 15f;
                document.Add(genderDistributionTitle);

                PdfPTable genderDistributionTable = new PdfPTable(4);
                genderDistributionTable.AddCell("Gym Pass");
                genderDistributionTable.AddCell("Number of Males");
                genderDistributionTable.AddCell("Number of Females");
                genderDistributionTable.AddCell("Number of Others");

                while (genderDistributionReader.Read())
                {
                    genderDistributionTable.AddCell(genderDistributionReader.GetString("pass_name"));
                    genderDistributionTable.AddCell(genderDistributionReader.GetInt32("num_male").ToString());
                    genderDistributionTable.AddCell(genderDistributionReader.GetInt32("num_female").ToString());
                    genderDistributionTable.AddCell(genderDistributionReader.GetInt32("num_other").ToString());
                }

                document.Add(genderDistributionTable);

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));


                PdfPTable chartTable = new PdfPTable(2);

                Paragraph chart1Title = new Paragraph("Payments/Day Chart", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD));
                chart1Title.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart1Title);
                MemoryStream chart1Stream = new MemoryStream();
                chart1.SaveImage(chart1Stream, ChartImageFormat.Png);
                iTextSharp.text.Image chart1Image = iTextSharp.text.Image.GetInstance(chart1Stream.GetBuffer());
                chart1Image.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart1Image);

                Paragraph chart2Title = new Paragraph("Person NO/Gympass Chart", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD));
                chart2Title.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart2Title);
                MemoryStream chart2Stream = new MemoryStream();
                chart2.SaveImage(chart2Stream, ChartImageFormat.Png);
                iTextSharp.text.Image chart2Image = iTextSharp.text.Image.GetInstance(chart2Stream.GetBuffer());
                chart2Image.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart2Image);

                Paragraph chart3Title = new Paragraph("Number of gympass holders Chart", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD));
                chart3Title.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart3Title);
                MemoryStream chart3Stream = new MemoryStream();
                chart3.SaveImage(chart3Stream, ChartImageFormat.Png);
                iTextSharp.text.Image chart3Image = iTextSharp.text.Image.GetInstance(chart3Stream.GetBuffer());
                chart3Image.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart3Image);

                Paragraph chart4Title = new Paragraph("Time spent in gym by each user", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD));
                chart4Title.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart4Title);
                MemoryStream chart4Stream = new MemoryStream();
                chart4.SaveImage(chart4Stream, ChartImageFormat.Png);
                iTextSharp.text.Image chart4Image = iTextSharp.text.Image.GetInstance(chart4Stream.GetBuffer());
                chart4Image.Alignment = Element.ALIGN_CENTER;
                chartTable.AddCell(chart4Image);

                document.Add(chartTable);

                document.Close();
                writer.Close();
                reader.Close();
            }
        }
    }
}
