using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace GymSurveillanceSystem
{

    public partial class AdminRecords : Form
    {
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

        private int userId;
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private string command = null;
        #endregion
        public AdminRecords()
        {
            InitializeComponent();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getData();
            if (id_face.Count >= 0)
            {
                updateDataGrid();
            }
            else
            {
                MessageBox.Show("Data not found");
            }

        }

        private void getFilteredDataAscByName()
        {
            try
            {
                String str = "server=localhost;port = 3306; database = licenta;UID = root;password = oracle";

                String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user ORDER BY name ASC";
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
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void getFilteredDataDescByName()
        {
            try
            {
                String str = "server=localhost;port = 3306; database = licenta;UID = root;password = oracle";

                String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user ORDER BY name DESC";
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
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void getData()
        {
            try
            {
                String str = "server=localhost;port = 3306; database = licenta;UID = root;password = oracle";

                String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user ";
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
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void getDataByName()
        {
            try
            {
                String str = "server=localhost;port = 3306; database = licenta;UID = root;password = oracle";

                String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user AND name = @name";
                MySqlConnection con = new MySqlConnection(str);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", textBox11.Text);

                

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
                command = "Data searched: " + cmd.CommandText;
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
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void getFilteredDataByAgeASC()
        {
            try
            {
                String str = "server=localhost;port = 3306; database = licenta;UID = root;password = oracle";

                String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user ORDER BY age ASC";
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
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void getFilteredDataByAgeDESC()
        {
            try
            {
                String str = "server=localhost;port = 3306; database = licenta;UID = root;password = oracle";

                String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user ORDER BY age DESC";
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

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            textBox6.Visible = false;
            textBox9.Visible = false;
            textBox10.Visible = false;
            label1.Visible = false;
            label8.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;
            textBox12.Visible = false;
            textBox13.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Update?", "Are You Sure", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Int32 id;
                Int32.TryParse(textBox6.Text, out id);
                tabControl1.SelectedIndex = 1;
                MySqlConnection con;
                String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
                String query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, id_pass, a.id_user" +
                    " FROM faces f, person p, accounts a" +
                    " WHERE f.id_face = p.id_face " +
                    "AND p.id_user = a.id_user " +
                    "AND p.id_person = @id_person";
                con = new MySqlConnection(str);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id_person", id);
                MySqlDataReader dbr;
                dbr = cmd.ExecuteReader();
                dbr.Read();
                textBox1.Text = dbr["name"].ToString();
                textBox2.Text = dbr["age"].ToString();
                textBox3.Text = dbr["gender"].ToString();
                textBox4.Text = dbr["address"].ToString();
                textBox5.Text = dbr["id_pass"].ToString();
                textBox7.Text = dbr["id_person"].ToString();
                textBox8.Text = dbr["start_date"].ToString();


            }
            else if (dialogResult == DialogResult.No)
            {
                dataGridView1.ClearSelection();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime end_date = DateTime.Now;
            end_date.ToString("yyyy-MM-dd H:mm:ss");
            Int32 age;
            Int32.TryParse(textBox2.Text, out age);
            Int32 id;
            Int32.TryParse(textBox6.Text, out id);
            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(str);
            con.Open();

            String query = "UPDATE person " +
                "SET end_date = @end_date " +
                "WHERE id_face = @id";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@end_date", end_date);
            cmd.Parameters.AddWithValue("@id", id);
            command = cmd.CommandText;
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

            query = "SELECT id_person FROM person WHERE id_face = @id_face";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_face", id);
            var queryResult3 = cmd.ExecuteScalar();

            textBox13.Text = queryResult3.ToString();

            Int32 id_person;
            Int32.TryParse(textBox13.Text, out id_person);

            /*string dateString = textBox8.Text;
            DateTime dateValue;
            DateTime.TryParse(dateString, out dateValue);*/
            query = "SELECT MAX(id_gph) FROM gphistory " +
                "WHERE id_person = @id_person";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person", id_person);
            var queryResult5 = cmd.ExecuteScalar();

            int last_gph_id = Int32.Parse(queryResult5.ToString());
            query = "UPDATE gphistory " +
                "SET end_date = @end_date " +
                "WHERE id_person = @id_person " +
                "AND id_gph = @id_gph";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@end_date", end_date);
            cmd.Parameters.AddWithValue("@id_person", id_person);
            cmd.Parameters.AddWithValue("@id_gph", last_gph_id);
            cmd.ExecuteNonQuery();

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            MessageBox.Show("Customer " + id.ToString() + " updated");
            tabControl1.SelectedIndex = 0;
            textBox6.Text = "";
            button1_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime end_date = DateTime.Now;

            Int32 age;
            Int32.TryParse(textBox2.Text, out age);
            Int32 id;
            Int32.TryParse(textBox6.Text, out id);
            /*String query = "UPDATE produs " +
                            "SET categorie = '" + textBox14.Text + "',denumire = '" + textBox13.Text + "'," +
                            "greutate = '" + greutate1 + "',garantie='" + textBox11.Text + "'," +
                            "pret = '" + pret1 + "'" +
                        "WHERE id_produs = '" + id + "';";*/
            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            String query = "UPDATE faces " +
                "SET name = @name " +
                "WHERE id_face = @id";


            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            query = "UPDATE person " +
                "SET age = @age, gender = @gender, address = @address, id_pass = @id_pass " +
                "WHERE id_face = @id";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@age", age);
            cmd.Parameters.AddWithValue("@gender", textBox3.Text);
            cmd.Parameters.AddWithValue("@address", textBox4.Text);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@id", id);
            command = cmd.CommandText;
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

            query = "SELECT id_user FROM person WHERE id_face = @id_face";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_face", id);
            var queryResult1 = cmd.ExecuteScalar();

            textBox9.Text = queryResult1.ToString();

            Int32 id_user;
            Int32.TryParse(textBox9.Text, out id_user);

            query = "UPDATE accounts " +
                "SET username = @username, password = @password " +
                "WHERE id_user = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox1.Text);
            cmd.Parameters.AddWithValue("@id_user", id_user);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            
            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            DateTime payment_time = DateTime.Parse(textBox8.Text);
            query = "UPDATE payment " +
                "SET id_pass = @id_pass " +
                "WHERE id_person = @id_person AND payment_time = @payment_time";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            /*cmd.Parameters.AddWithValue("@payment_time", textBox8.Text);*/
            cmd.Parameters.Add("@payment_time", MySqlDbType.Datetime).Value = payment_time;
            cmd.Parameters.AddWithValue("@id_person", textBox7.Text);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "SELECT id_person FROM person WHERE id_face = @id_face";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_face", id);
            var queryResult3 = cmd.ExecuteScalar();

            textBox13.Text = queryResult3.ToString();

            Int32 id_person;
            Int32.TryParse(textBox13.Text, out id_person);


            /*string dateString = textBox8.Text;
            DateTime dateValue;
            DateTime.TryParse(dateString, out dateValue);*/
            
            
            query = "SELECT MAX(id_gph) FROM gphistory " +
                "WHERE id_person = @id_person";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person", id_person);
            var queryResult5 = cmd.ExecuteScalar();

            int last_gph_id = Int32.Parse(queryResult5.ToString());
            query = "UPDATE gphistory " +
                "SET id_pass = @id_pass " +
                "WHERE id_person = @id_person " +
                "AND id_gph = @id_gph";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@id_person", id_person);
            cmd.Parameters.AddWithValue("@id_gph", last_gph_id);
            cmd.ExecuteNonQuery();

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            MessageBox.Show("Customer " + id.ToString() + " updated");
            tabControl1.SelectedIndex = 0;
            textBox6.Text = "";
            button1_Click(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Int32 id;
            Int32.TryParse(textBox6.Text, out id);

            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(str);
            con.Open();

            String query = "DELETE FROM payment " +
                "WHERE id_person = (SELECT id_person FROM person WHERE id_face = @id_face)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_face", id);
            command = cmd.CommandText;
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

            query = "SELECT id_person FROM person WHERE id_face = @id_face";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_face", id);
            var queryResult2 = cmd.ExecuteScalar();

            textBox9.Text = queryResult2.ToString();

            Int32 id_person;
            Int32.TryParse(textBox9.Text, out id_person);

            query = "DELETE FROM gphistory " +
                "WHERE id_person = @id_person";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person", id_person);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "SELECT id_user FROM person WHERE id_face = @id_face";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_face", id);
            var queryResult1 = cmd.ExecuteScalar();

            textBox9.Text = queryResult1.ToString();

            Int32 id_user;
            Int32.TryParse(textBox9.Text, out id_user);

            query = "DELETE FROM gymtime " +
                "WHERE id_user = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", id_user);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "DELETE FROM cmdlog " +
                "WHERE user_id = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", id_user);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "DELETE FROM person " +
                "WHERE id_face = @id";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            
            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "DELETE FROM faces " +
                "WHERE id_face = @id";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            
           
            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            

            query = "DELETE FROM accounts " +
                "WHERE id_user = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_user", id_user);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            
            
            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            MessageBox.Show("Customer " + id.ToString() + " deleted");
            textBox6.Text = "";
            button1_Click(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Customer's data exported to xls");
            // creating Excel Application  
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];
            worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";
            // storing header part in Excel  
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
            // save the application  
            workbook.SaveAs("C:\\Users\\georg\\Desktop\\Licenta\\customers.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application  
            //app.Quit();

            MySqlConnection con;
            string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(connectionString);
            con.Open();
            command = "Data exported in csv file: " + "C:\\Users\\georg\\Desktop\\Licenta\\customers.xls";
            DateTime cmddate = DateTime.Now;
            cmddate.ToString("yyyy-MM-dd H:mm:ss");
            string query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";
            con.Close();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                //Add the Header row for CSV file.
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    csv += column.HeaderText + ',';
                }
                //Add new line.
                csv += "\r\n";

                //Adding the Rows

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                        {
                            //Add the Data rows.
                            csv += cell.Value.ToString().Replace(",", ";") + ',';
                        }
                        // break;
                    }
                    //Add new line.
                    csv += "\r\n";
                }

                //Exporting to CSV.
                string folderPath = "C:\\Users\\georg\\Desktop\\Licenta\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                File.WriteAllText(folderPath + "customers.csv", csv);
                MessageBox.Show("Customers exported in CSV - " + folderPath.ToString());

                MySqlConnection con;
                string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
                con = new MySqlConnection(connectionString);
                con.Open();
                command = "Data exported in csv file: " + folderPath;
                DateTime cmddate = DateTime.Now;
                cmddate.ToString("yyyy-MM-dd H:mm:ss");
                string query = "INSERT INTO cmdlog(user_id, date, command) " +
               "VALUES(@user_id, @date, @command)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", 2);
                cmd.Parameters.AddWithValue("@date", cmddate);
                cmd.Parameters.AddWithValue("@command", command);
                cmd.ExecuteNonQuery();

                command = "";
                con.Close();

            }
            catch
            {
                MessageBox.Show("");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Assuming you have a MySqlConnection object
            MySqlConnection con;
            string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(connectionString);
            con.Open();
            MessageBox.Show("Choose the csv file you'd like to upload :)");
            // Open file dialog to select the CSV file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;

                try
                {
                    using (StreamReader reader = new StreamReader(System.IO.Path.GetFullPath(fileName)))
                    {
                        var lineNumber = 0;

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (lineNumber == 0)
                            {
                                lineNumber++;
                                continue; // Skip the header line
                            }
                            var values = line.Split(',');
                            Console.WriteLine("Line: " + line);
                            if (values.Length >= 5)
                            {


                                string name = values[0].Trim();
                                string ageStr = values[1].Trim();
                                string gender = values[2].Trim();
                                string address = values[3].Trim();
                                string id_pass = values[4].Trim();

                                // Convert age to integer
                                int age = int.Parse(ageStr);

                                // Convert start date to DateTime
                                DateTime startDate = DateTime.Now;
                                startDate.ToString("yyyy-MM-dd H:mm:ss");
                                // Convert end date to DateTime
                                DateTime? endDate = null;

                                MessageBox.Show("Choose the bmp format picture :)");
                                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                                openFileDialog1.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                                DialogResult dialogResult1 = openFileDialog.ShowDialog();

                                if (dialogResult == DialogResult.OK)
                                {
                                    string imagePath = openFileDialog.FileName;
                                    // Rest of your code for processing the image file
                                    int width = 125;
                                    int height = 125;
                                    byte[] pic;
                                    using (Bitmap bmp = new Bitmap(imagePath))
                                    {
                                        using (Bitmap resizedBmp = new Bitmap(bmp, width, height))
                                        {
                                            using (MemoryStream ms = new MemoryStream())
                                            {
                                                resizedBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                                                pic = ms.ToArray();
                                            }
                                        }
                                    }
                                    string query = "INSERT INTO faces(FaceData, name) " +
                                            "VALUES (@pic, @name)";
                                    MySqlCommand cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@pic", pic);
                                    cmd.Parameters.AddWithValue("@name", name);
                                    cmd.ExecuteNonQuery();

                                    query = "SELECT MAX(id_face) FROM faces";
                                    cmd = new MySqlCommand(query, con);
                                    var queryResult = cmd.ExecuteScalar();

                                    int last_id = Int32.Parse(queryResult.ToString());

                                    query = "INSERT INTO accounts(username, password, laccess) " +
                                        "VALUES (@username, @password, @laccess)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@username", name);
                                    cmd.Parameters.AddWithValue("@password", name);
                                    cmd.Parameters.AddWithValue("@laccess", 2);
                                    cmd.ExecuteNonQuery();

                                    query = "SELECT MAX(id_user) FROM accounts";
                                    cmd = new MySqlCommand(query, con);
                                    var queryResult2 = cmd.ExecuteScalar();

                                    int last_user_id = Int32.Parse(queryResult2.ToString());



                                    query = "INSERT INTO person(age, gender, start_date, end_date, address, id_face, id_pass, id_user) " +
                                        "VALUES (@age, @gender, @start_date, @end_date, @address, @id_face, @id_pass, @id_user)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@age", age);
                                    cmd.Parameters.AddWithValue("@gender", gender);
                                    cmd.Parameters.AddWithValue("@start_date", startDate);
                                    cmd.Parameters.AddWithValue("@end_date", null);
                                    cmd.Parameters.AddWithValue("@address", address);
                                    cmd.Parameters.AddWithValue("@id_face", last_id);
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.Parameters.AddWithValue("@id_user", last_user_id);
                                    cmd.ExecuteNonQuery();

                                    query = "SELECT MAX(id_person) FROM person";
                                    cmd = new MySqlCommand(query, con);
                                    var queryResult1 = cmd.ExecuteScalar();

                                    int last_person_id = Int32.Parse(queryResult1.ToString());

                                    query = "INSERT INTO gphistory(id_person, start_date, end_date, id_pass) " +
                "VALUES(@id_person, @start_date, @end_date, @id_pass)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.Parameters.AddWithValue("@start_date", startDate);
                                    cmd.Parameters.AddWithValue("@end_date", null);
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.ExecuteNonQuery();

                                    query = "INSERT INTO payment(id_person, id_user, id_pass, payment_time, payment_fee) " +
                                        "VALUES (@id_person, @id_user, @id_pass, @payment_time, @payment_fee)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.Parameters.AddWithValue("@id_user", last_user_id);
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.Parameters.AddWithValue("@payment_time", startDate);
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
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.Parameters.AddWithValue("@id_payment", last_payment_id);
                                    cmd.Parameters.AddWithValue("@id_user", last_user_id);
                                    cmd.ExecuteNonQuery();

                                    query = "UPDATE person SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                    "WHERE id_person = @id_person";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@end_date", startDate);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.ExecuteNonQuery();

                                    query = "UPDATE gphistory SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                "WHERE id_person = @id_person";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@end_date", startDate);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.ExecuteNonQuery();

                                }
                                //string imagePath = "C:\\Users\\georg\\Desktop\\Licenta\\GymSurveillanceSystem\\bin\\Debug\\TrainedFaces\\face20.bmp";





                            }
                            else
                            {
                                Console.WriteLine("Invalid line format: " + line);
                            }


                            lineNumber++;
                        }

                        con.Close();
                        
                    }

                    MessageBox.Show("Customer's data has been updated");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing the CSV file: " + ex.Message);
                }
                
            }
            MySqlConnection con1;
            String str1 = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con1 = new MySqlConnection(str1);
            con1.Open();
            command = "Data imported from csv file";
            DateTime cmddate = DateTime.Now;
            cmddate.ToString("yyyy-MM-dd H:mm:ss");
            string query1 = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            MySqlCommand cmd1 = new MySqlCommand(query1, con1);
            cmd1.Parameters.AddWithValue("@user_id", 2);
            cmd1.Parameters.AddWithValue("@date", cmddate);
            cmd1.Parameters.AddWithValue("@command", command);
            cmd1.ExecuteNonQuery();

            command = "";
            con1.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MainPageAdmin f2 = new MainPageAdmin();
            f2.Show();
            Hide();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            getFilteredDataAscByName();
            if (id_face.Count >= 0)
            {
                updateDataGrid();
            }
            else
            {
                MessageBox.Show("Data not found");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            getFilteredDataDescByName();
            if (id_face.Count >= 0)
            {
                updateDataGrid();
            }
            else
            {
                MessageBox.Show("Data not found");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            getFilteredDataByAgeDESC();
            if (id_face.Count >= 0)
            {
                updateDataGrid();
            }
            else
            {
                MessageBox.Show("Data not found");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            getFilteredDataByAgeASC();
            if (id_face.Count >= 0)
            {
                updateDataGrid();
            }
            else
            {
                MessageBox.Show("Data not found");
            }
        }

        private void AdminRecords_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            getDataByName();
            if (id_face.Count >= 0)
            {
                updateDataGrid();
            }
            else
            {
                MessageBox.Show("Data not found");
            }
            textBox11.Text = "";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (object item in listBox1.SelectedItems)
            {
                string itemData = item.ToString();
                sb.Append(itemData[0]);
                sb.Append(",");
            }

            if (sb.Length > 0)
            {
                sb.Length--; // Remove the last character (comma)
            }

            textBox12.Text = sb.ToString();
        }

        private void getDataFilteredByDateAndPass()
        {
            /*string theDate = dateTimePicker1.Value.ToString("yyyy-MM-dd ");*/
            /*DateTime selectedDateTime = dateTimePicker1.Value;*/
            DateTime selectedDateTime = dateTimePicker1.Value;
            string formattedDate = selectedDateTime.ToString("yyyy:MM:dd HH:mm:ss");

            try
            {
                /*MessageBox.Show(selectedDateTime.ToString() + " " + textBox12.Text);*/
                string str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";

                string idPassValue = textBox12.Text;
                string[] idPassArray = idPassValue.Split(',');
                List<int> idPassList = new List<int>();

                foreach (string id in idPassArray)
                {
                    if (int.TryParse(id.Trim(), out int parsedId))
                    {
                        idPassList.Add(parsedId);
                    }
                }

                string query = "SELECT f.id_face, name, id_person, age, gender, start_date, end_date, address, p.id_pass, a.id_user" +
                                " FROM faces f, person p, accounts a" +
                                " WHERE f.id_face = p.id_face " +
                                " AND p.id_user = a.id_user " +
                                " AND start_date >= @start_date ";

                MySqlConnection con = new MySqlConnection(str);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);

                

                if (idPassList.Count > 0)
                {
                    string idPassParameter = string.Join(",", idPassList.Select(i => i.ToString()));
                    query += " AND id_pass IN (" + idPassParameter + ")";
                }

                cmd.Parameters.AddWithValue("@start_date", formattedDate);
                cmd.CommandText = query;

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
                        id_pass.Add(dbr["id_pass"]);
                        id_user.Add(dbr["id_user"]);
                    }
                }
                else
                {
                    MessageBox.Show("Data not found");
                }

                con.Close();
                con.Open();
                command = "Data filtered";
                DateTime cmddate = DateTime.Now;
                cmddate.ToString("yyyy-MM-dd H:mm:ss");
                string query1 = "INSERT INTO cmdlog(user_id, date, command) " +
               "VALUES(@user_id, @date, @command)";
                MySqlCommand cmd1 = new MySqlCommand(query1, con);
                cmd1.Parameters.AddWithValue("@user_id", 2);
                cmd1.Parameters.AddWithValue("@date", cmddate);
                cmd1.Parameters.AddWithValue("@command", command);
                cmd1.ExecuteNonQuery();

                command = "";
                con.Close();
                listBox1.ClearSelected();
                textBox12.Text = "";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            getDataFilteredByDateAndPass();
            if (id_face.Count >= 0)
            {
                updateDataGrid();

            }
            else
            {
                MessageBox.Show("Data not found");
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            MySqlConnection con;
            string connectionString = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con = new MySqlConnection(connectionString);
            con.Open();
            MessageBox.Show("Choose the excel file you'd like to upload from:)");
            // Set the EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Open file dialog to select the Excel file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx";
            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string excelFilePath = openFileDialog.FileName;

                try
                {
                    // Load the Excel file using EPPlus
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"]; // Replace "Sheet1" with the actual worksheet name

                        if (worksheet != null)
                        {
                            // Get the number of rows and columns in the worksheet
                            int rowCount = worksheet.Dimension.Rows;
                            int columnCount = worksheet.Dimension.Columns;

                            // Process the data from each row
                            for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip the header
                            {
                                string name = worksheet.Cells[row, 1].Value?.ToString(); // Assuming name is in column 1
                                int age = Convert.ToInt32(worksheet.Cells[row, 2].Value); // Assuming age is in column 2
                                string gender = worksheet.Cells[row, 3].Value?.ToString(); // Assuming gender is in column 3
                                string address = worksheet.Cells[row, 4].Value?.ToString(); // Assuming address is in column 4
                                string id_pass = worksheet.Cells[row, 5].Value?.ToString(); // Assuming id_pass is in column 5

                                // Rest of your code for processing the data

                                Console.WriteLine("Name: " + name);
                                Console.WriteLine("Age: " + age);
                                Console.WriteLine("Gender: " + gender);
                                Console.WriteLine("Address: " + address);
                                Console.WriteLine("ID_Pass: " + id_pass);
                                Console.WriteLine();
                                // Convert start date to DateTime
                                DateTime startDate = DateTime.Now;
                                startDate.ToString("yyyy-MM-dd H:mm:ss");
                                // Convert end date to DateTime
                                DateTime? endDate = null;

                                MessageBox.Show("Choose the bmp format picture :)");
                                OpenFileDialog imageFileDialog = new OpenFileDialog();
                                imageFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                                DialogResult imageDialogResult = imageFileDialog.ShowDialog();

                                if (imageDialogResult == DialogResult.OK)
                                {
                                    string imagePath = imageFileDialog.FileName;
                                    // Rest of your code for processing the image file
                                    int width = 125;
                                    int height = 125;
                                    byte[] pic;
                                    using (Bitmap bmp = new Bitmap(imagePath))
                                    {
                                        using (Bitmap resizedBmp = new Bitmap(bmp, width, height))
                                        {
                                            using (MemoryStream ms = new MemoryStream())
                                            {
                                                resizedBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                                                pic = ms.ToArray();
                                            }
                                        }
                                    }
                                    string query = "INSERT INTO faces(FaceData, name) " +
                                            "VALUES (@pic, @name)";
                                    MySqlCommand cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@pic", pic);
                                    cmd.Parameters.AddWithValue("@name", name);
                                    cmd.ExecuteNonQuery();

                                    query = "SELECT MAX(id_face) FROM faces";
                                    cmd = new MySqlCommand(query, con);
                                    var queryResult = cmd.ExecuteScalar();

                                    int last_id = Int32.Parse(queryResult.ToString());

                                    query = "INSERT INTO accounts(username, password, laccess) " +
                                        "VALUES (@username, @password, @laccess)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@username", name);
                                    cmd.Parameters.AddWithValue("@password", name);
                                    cmd.Parameters.AddWithValue("@laccess", 2);
                                    cmd.ExecuteNonQuery();

                                    query = "SELECT MAX(id_user) FROM accounts";
                                    cmd = new MySqlCommand(query, con);
                                    var queryResult2 = cmd.ExecuteScalar();

                                    int last_user_id = Int32.Parse(queryResult2.ToString());



                                    query = "INSERT INTO person(age, gender, start_date, end_date, address, id_face, id_pass, id_user) " +
                                        "VALUES (@age, @gender, @start_date, @end_date, @address, @id_face, @id_pass, @id_user)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@age", age);
                                    cmd.Parameters.AddWithValue("@gender", gender);
                                    cmd.Parameters.AddWithValue("@start_date", startDate);
                                    cmd.Parameters.AddWithValue("@end_date", null);
                                    cmd.Parameters.AddWithValue("@address", address);
                                    cmd.Parameters.AddWithValue("@id_face", last_id);
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.Parameters.AddWithValue("@id_user", last_user_id);
                                    cmd.ExecuteNonQuery();

                                    query = "SELECT MAX(id_person) FROM person";
                                    cmd = new MySqlCommand(query, con);
                                    var queryResult1 = cmd.ExecuteScalar();

                                    int last_person_id = Int32.Parse(queryResult1.ToString());

                                    query = "INSERT INTO gphistory(id_person, start_date, end_date, id_pass) " +
                "VALUES(@id_person, @start_date, @end_date, @id_pass)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.Parameters.AddWithValue("@start_date", startDate);
                                    cmd.Parameters.AddWithValue("@end_date", null);
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.ExecuteNonQuery();

                                    query = "INSERT INTO payment(id_person, id_user, id_pass, payment_time, payment_fee) " +
                                        "VALUES (@id_person, @id_user, @id_pass, @payment_time, @payment_fee)";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.Parameters.AddWithValue("@id_user", last_user_id);
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.Parameters.AddWithValue("@payment_time", startDate);
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
                                    cmd.Parameters.AddWithValue("@id_pass", id_pass);
                                    cmd.Parameters.AddWithValue("@id_payment", last_payment_id);
                                    cmd.Parameters.AddWithValue("@id_user", last_user_id);
                                    cmd.ExecuteNonQuery();

                                    query = "UPDATE person SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                    "WHERE id_person = @id_person";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@end_date", startDate);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.ExecuteNonQuery();

                                    query = "UPDATE gphistory SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                "WHERE id_person = @id_person";
                                    cmd = new MySqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@end_date", startDate);
                                    cmd.Parameters.AddWithValue("@id_person", last_person_id);
                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Data imported from your excel file");

                                    //con.Close();

                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Worksheet not found in the Excel file.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing the Excel file: " + ex.Message);
                }
            }

            MySqlConnection con1;
            String str1 = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";
            con1 = new MySqlConnection(str1);
            con1.Open();
            command = "Data imported from excel file";
            DateTime cmddate = DateTime.Now;
            cmddate.ToString("yyyy-MM-dd H:mm:ss");
            string query1 = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            MySqlCommand cmd1 = new MySqlCommand(query1, con1);
            cmd1.Parameters.AddWithValue("@user_id", 2);
            cmd1.Parameters.AddWithValue("@date", cmddate);
            cmd1.Parameters.AddWithValue("@command", command);
            cmd1.ExecuteNonQuery();

            command = "";
            con1.Close();
            con.Close();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Int32 id;
            Int32.TryParse(textBox6.Text, out id);

            MySqlConnection con;
            String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";

            DateTime start_date = DateTime.Now;
            start_date.ToString("yyyy-MM-dd H:mm:ss");


            string query = "UPDATE person " +
                "SET start_date = @start_date, end_date = @end_date, id_pass = @id_pass " +
                "WHERE id_person = @id_person";
            con = new MySqlConnection(str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);

            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@start_date", start_date);
            cmd.Parameters.AddWithValue("@end_date", null);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@id_person", id);
            command = cmd.CommandText;
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

            query = "SELECT id_user FROM person " +
                "WHERE id_person = @id_person";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person", id);
            var queryResult3 = cmd.ExecuteScalar();

            int user_id = Int32.Parse(queryResult3.ToString());

            /*(SELECT fee FROM gympass WHERE id_pass = @id_pass)*/

            query = "INSERT INTO payment(id_person, id_user, id_pass, payment_time, payment_fee) " +
                "VALUES(@id_person, @id_user, @id_pass, @payment_time, @payment_fee)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person", id);
            cmd.Parameters.AddWithValue("@id_user", user_id);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@payment_time", start_date);
            cmd.Parameters.AddWithValue("@payment_fee", null);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            
            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "SELECT MAX(id_payment) FROM payment";
            cmd = new MySqlCommand(query, con);
            var queryResult4 = cmd.ExecuteScalar();

            int last_payment_id = Int32.Parse(queryResult4.ToString());
            query = "UPDATE payment " +
                "SET payment_fee = (SELECT fee FROM gympass WHERE id_pass = @id_pass) " +
                "WHERE id_payment = @id_payment AND id_user = @id_user";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.Parameters.AddWithValue("@id_payment", last_payment_id);
            cmd.Parameters.AddWithValue("@id_user", user_id);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            
            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "UPDATE person SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                "WHERE id_person = @id_person";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@end_date", start_date);
            cmd.Parameters.AddWithValue("@id_person", id);
            command = cmd.CommandText;
            cmd.ExecuteNonQuery();

            query = "INSERT INTO cmdlog(user_id, date, command) " +
           "VALUES(@user_id, @date, @command)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", 2);
            cmd.Parameters.AddWithValue("@date", cmddate);
            cmd.Parameters.AddWithValue("@command", command);
            cmd.ExecuteNonQuery();

            command = "";

            query = "INSERT INTO gphistory(id_person, start_date, end_date, id_pass) " +
                "VALUES(@id_person, @start_date, @end_date, @id_pass)";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id_person", id);
            cmd.Parameters.AddWithValue("@start_date", start_date);
            cmd.Parameters.AddWithValue("@end_date", null);
            cmd.Parameters.AddWithValue("@id_pass", textBox5.Text);
            cmd.ExecuteNonQuery();


            query = "SELECT MAX(id_gph) FROM gphistory";
            cmd = new MySqlCommand(query, con);
            var queryResult5 = cmd.ExecuteScalar();

            int last_gph_id = Int32.Parse(queryResult5.ToString());
            query = "UPDATE gphistory SET end_date = ADDDATE(@end_date, INTERVAL 30 DAY) " +
                "WHERE id_person = @id_person " +
                "AND id_gph = @id_gph";
            cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@end_date", start_date);
            cmd.Parameters.AddWithValue("@id_person", id);
            cmd.Parameters.AddWithValue("@id_gph", last_gph_id);
            cmd.ExecuteNonQuery();


            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            MessageBox.Show("Customer " + id.ToString() + " updated");
            tabControl1.SelectedIndex = 0;
            textBox6.Text = "";
            button1_Click(sender, e);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";

                String query = "SELECT *FROM cmdlog";
                MySqlConnection con = new MySqlConnection(str);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dbr;
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

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                String str = "server=localhost;port=3306;database=licenta;UID=root;password=oracle";

                String query = "SELECT *FROM gphistory";
                MySqlConnection con = new MySqlConnection(str);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dbr;
                dbr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(dbr);

                metroGrid2.DataSource = dataTable;
                metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                con.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
