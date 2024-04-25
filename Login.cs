using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TreeViewLab2
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(textBox_login.Text))
            {
                errorProvider1.SetError(textBox_login, "Значение поля не может быть пустым.");
            }
            else if (string.IsNullOrEmpty(textBox_password.Text))
            {
                errorProvider1.SetError(textBox_password, "Значение поля не может быть пустым.");
            }
            else
            {
                var loginUser = textBox_login.Text;
                var passwordUser = textBox_password.Text;
                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
                {
                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM users WHERE user_login = @username AND user_password = @password", conn);
                    comm.Parameters.AddWithValue("@username", loginUser);
                    comm.Parameters.AddWithValue("@password", passwordUser);
                    object result = comm.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int count = Convert.ToInt32(result);
                        if (count > 0)
                        {
                            MessageBox.Show("Доступ разрешен!");
                            Form1 frm = new Form1();
                            this.Hide();
                            frm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверное имя пользователя или пароль!");
                    }
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '*';
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration frm = new Registration();
            frm.Show();
            this.Hide();
        }
    }
}
