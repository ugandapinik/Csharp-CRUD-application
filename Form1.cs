using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;     //we will create a connection to our PROJECT using ODBC
using System.Data.SqlClient;

namespace CRUDAppDesktop
{
    public partial class Form1 : Form
    {
        //create connection before
        SqlConnection con = new SqlConnection();
        public Boolean newData;

        public Form1()
        {
            InitializeComponent();
            
        }

        



        private void Form1_Load(object sender, EventArgs e)
        {

            //declaration for new data is true
            //if newData is true, we will insert new data to database
            //if new data is false, so we will update into database

            newData = true;
            LoadData();
            id.Enabled = false;
        }

        //load data from database
        //you must have a database (mysql database) before.
        //i have a database in my localhost
        private void LoadData()
        {
            //create connection before
            con.ConnectionString = @"Data Source=Developer-Lab;Initial Catalog=crud;Integrated Security=True";

            con.Open();

            String sql = "SELECT * FROM users order by ID";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);


            DataTable dt = new DataTable();
            adapter.Fill(dt);
            //bind data into the grid
            dataGridView1.DataSource = dt;
            //close connection
            con.Close();
            dt.Dispose();
            adapter.Dispose();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //value form the data grid where clicked celled.
            DataGridViewRow rows = dataGridView1.Rows[e.RowIndex];

            id.Text = rows.Cells[0].Value.ToString();
            fullname.Text = rows.Cells[1].Value.ToString();
            email.Text = rows.Cells[2].Value.ToString();
            phone.Text = rows.Cells[3].Value.ToString();
            address.Text = rows.Cells[4].Value.ToString();

            //new data false if textbox is not null
            newData = false;
        }


        private void ClearText()
        {
            id.Text = "";
            fullname.Text = "";
            email.Text = "";
            phone.Text = "";
            address.Text = "";
            id.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newData = true;
            ClearText();
        }

        //function save update, delete into database;
        public void updateData(String sql)
        {
            try
            {
                //open connection
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                //excute
                cmd.ExecuteNonQuery();
                //show mesage 
                MessageBox.Show("Data has been update!", "Information");
                con.Close();
                cmd.Dispose();

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogresult;
            String SaveData = "";
            //if new data == true , create query update
            //if newdata == false, create query INSERT
            if (newData)
            {
                dialogresult = MessageBox.Show("Are you sure to add new data into database", "Information", MessageBoxButtons.YesNo);

                if(dialogresult == DialogResult.No)
                {
                    return;
                }

                SaveData = "INSERT INTO users(fullname, email, phone, address) VALUES('" + fullname.Text + "','" + email.Text + "','" + phone.Text + "','" + address.Text +"')";


            }
            else {
                SaveData = "UPDATE users SET fullname='" + fullname.Text + "', email='" + email.Text + "',phone='" + phone.Text+ "',address='" + address.Text + "' WHERE id='" + id.Text + "'";
            }

            updateData(SaveData);
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            String delete = "";
            dialogResult = MessageBox.Show("Are you sure to deelte this data?", "Warning", MessageBoxButtons.YesNo);

            if(dialogResult == DialogResult.No)
            {
                return;
            }
            else
            {
                delete = "DELETE FROM users WHERE id='" + id.Text + "'";

                updateData(delete);
                LoadData();

            }
        }
    }
}
