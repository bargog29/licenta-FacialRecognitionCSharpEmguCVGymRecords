using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymSurveillanceSystem
{
    public partial class MainPageAdmin : Form
    {
        public MainPageAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FaceDetectionAdmin f4 = new FaceDetectionAdmin();
            f4.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FaceRecognitionAdmin f5 = new FaceRecognitionAdmin();
            f5.Show();
            Hide();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminRecords f6 = new AdminRecords();
            f6.Show();
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminStatistics f7 = new AdminStatistics();
            f7.Show();
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            Form1 f1 = new Form1();
            f1.Show();
            Hide();
        }
    }
}
