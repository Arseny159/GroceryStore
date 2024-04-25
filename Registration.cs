using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeViewLab2
{
    public partial class Registration : Form
    {
        public Registration()
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
                    NpgsqlCommand checkUserCommand = new NpgsqlCommand("SELECT * FROM users WHERE user_login = @username", conn);
                    checkUserCommand.Parameters.AddWithValue("@username", loginUser);
                    int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                    if (userCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!");
                    }
                    else
                    {
                        NpgsqlCommand registerUserCommand = new NpgsqlCommand("INSERT INTO users (user_id, user_login, user_password) VALUES (((select max(user_id) from users) + 1), @username, @password)", conn);
                        registerUserCommand.Parameters.AddWithValue("@username", loginUser);
                        registerUserCommand.Parameters.AddWithValue("@password", passwordUser);
                        int rowsAffected = registerUserCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Регистрация успешно завершена!");
                            Login frm = new Login();
                            this.Hide();
                            frm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при регистрации пользователя!");
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login frm = new Login();
            this.Hide();
            frm.ShowDialog();
        }
    }
}
