using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SQLiter
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        bool connectionFailed = false;
        SQLiteConnection conn;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                MessageBox.Show("You must specify a table name.", "Errör");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = ofd.FileName;
                TestConnection(ofd.FileName);
            }
            else
            {
                MessageBox.Show("Couldn't load file.", "Errör");
            }
        }

        private void TestConnection(string strFileName)
        {
            conn = new SQLiteConnection();

            if (!connectionFailed && string.IsNullOrEmpty(txtPassword.Text))
            {
                conn = new SQLiteConnection(string.Concat("Data Source=", strFileName));
            }
            else
            {
                conn = new SQLiteConnection(string.Concat("Data Source=", txtPath.Text, ";Password=", txtPassword.Text, ";"));
            }

            conn.Open();
            try
            {
                var command = conn.CreateCommand();
                command.CommandText = string.Format("select * from {0}", txtTableName.Text);
                command.ExecuteNonQuery();
                command.ExecuteScalar();
                if (conn.State == ConnectionState.Open)
                {
                    lblStatus.Text = "Connected";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    MessageBox.Show("Couldn't establish a connection with the database. If it's password protected, please specify the password in the password field.", "Errör");
                    connectionFailed = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't establish a connection with the database. If it's password protected, please specify the password in the password field.", ex.Message);
                lblStatus.Text = "Disconnected";
                lblStatus.ForeColor = Color.Red;
                connectionFailed = true;
            }

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                conn.ChangePassword(txtPassword.Text);
                MessageBox.Show("Password has been succesfully changed to " + txtPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Close();
                conn = new SQLiteConnection(string.Concat("Data Source=", txtPath.Text));
                conn.SetPassword(txtPassword.Text);
                conn.Open();
                conn.Close();
                conn.OpenAndReturn();
                // Here is the trick -->
                conn.ChangePassword(txtPassword.Text);
                conn.Close();
                MessageBox.Show("Password '" + txtPassword.Text + "' has been succesfully set to the database.", "Success");
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                conn.ChangePassword("");
                MessageBox.Show("Password has been succesfully nulled.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            // I know, it could be better :)
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < 15; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            txtPassword.Text = builder.ToString().ToLower();
        }
    }
}
