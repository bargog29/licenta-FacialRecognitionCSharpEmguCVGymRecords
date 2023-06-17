using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GymSurveillanceSystem
{
    public partial class Form1 : Form
    {
        // Public user ID variable
        public int UserId { get; set; }

        private int originalWidth;
        private int originalHeight;

        public Form1()
        {
            InitializeComponent();
            originalWidth = this.Width;
            originalHeight = this.Height;
        }

        private void RetrieveUserIdFromDatabase()
        {
            string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            string query = "SELECT id_user FROM accounts WHERE username = @username and password = @password AND laccess = 2";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", textBox4.Text);
                        command.Parameters.AddWithValue("@password", textBox3.Text);

                        var result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            UserId = Convert.ToInt32(result);
                        }
                        else
                        {
                            // Handle the case when the result is null or DBNull.Value
                            UserId = -1; // Assign a default value or handle it as needed
                            MessageBox.Show("User ID not found");
                        }

                       // MessageBox.Show(result.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = originalWidth - 390;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                MySqlConnection con;
                if (!(textBox1.Text == string.Empty))
                {
                    if (!(textBox2.Text == string.Empty))
                    {
                        String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
                        String query = "SELECT *FROM accounts WHERE username = '" + textBox1.Text + "' and password = '" + textBox2.Text + "' AND laccess=1;";
                        con = new MySqlConnection(str);
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        MySqlDataReader dbr;
                        dbr = cmd.ExecuteReader();
                        int count = 0;
                        while (dbr.Read())
                        {
                            count = count + 1;
                        }
                        con.Close();
                        if (count == 1)
                        {
                            MessageBox.Show("Connected Succesfully");
                            MainPageAdmin f2 = new MainPageAdmin();
                            f2.Show();
                            Hide();
                        }
                        else if (count > 1)
                        {
                            MessageBox.Show("Duplicate user and pass", "ReLogin");
                        }
                        else
                        {
                            MessageBox.Show("Wrong user or pass", "ReLogin");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Complete Password Field", "ReLogin");
                    }
                }
                else
                {
                    MessageBox.Show("Complete Username Field", "ReLogin");
                }
                //   con.Close();  
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Width = originalWidth;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = false;
        }

        private void label4_MouseHover(object sender, EventArgs e)
        {
            label4.ForeColor = System.Drawing.Color.Blue;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = System.Drawing.Color.White;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           RetrieveUserIdFromDatabase();
            this.Width = originalWidth - 390;
            MainPageUser f2 = new MainPageUser();
            if(UserId == -1)
            {
                MessageBox.Show("There is no account using those credentials");
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                textBox3.Text = "";
                textBox4.Text = "";
            }
            else
            {
                f2.UserId = UserId; // Assign the UserId value to the property
                f2.Show();
                Hide();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.PasswordChar = '*';
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
