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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TreeViewLab2
{
    public partial class SearchSupplierForm : Form
    {
        public List<string> SearchConditions { get; } = new List<string>();

        public string idx_temp = "";

        public SearchSupplierForm(string idx)
        {
            InitializeComponent();
            idx_temp = idx;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            string address = textBox1.Text;
            string requisites = textBox2.Text;
            string phone_number = textBox3.Text;
            string email = textBox4.Text;
            string name = textBox5.Text;

            bool found = false;

            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                conn.Open();
                NpgsqlCommand checkCommand = new NpgsqlCommand("SELECT * FROM supplier WHERE ", conn);

                if (!string.IsNullOrEmpty(address))
                {
                    checkCommand.CommandText += "supplier_address = @address";
                    checkCommand.Parameters.AddWithValue("@address", address);
                    found = true;
                }
                if (!string.IsNullOrEmpty(requisites))
                {
                    if (found)
                        checkCommand.CommandText += " AND ";
                    checkCommand.CommandText += "supplier_payment_account = @requisites";
                    checkCommand.Parameters.AddWithValue("@requisites", requisites);
                    found = true;
                }
                if (!string.IsNullOrEmpty(phone_number))
                {
                    if (found)
                        checkCommand.CommandText += " AND ";
                    checkCommand.CommandText += "supplier_phone_number = @phone_number";
                    checkCommand.Parameters.AddWithValue("@phone_number", phone_number);
                    found = true;
                }
                if (!string.IsNullOrEmpty(email))
                {
                    if (found)
                        checkCommand.CommandText += " AND ";
                    checkCommand.CommandText += "supplier_email_address = @email";
                    checkCommand.Parameters.AddWithValue("@email", email);
                    found = true;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    if (found)
                        checkCommand.CommandText += " AND ";
                    checkCommand.CommandText += "supplier_name = @name";
                    checkCommand.Parameters.AddWithValue("@name", name);
                    found = true;
                }

                if (found)
                {
                    NpgsqlDataReader reader = checkCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string foundSupplierName = reader.GetString(reader.GetOrdinal("supplier_name"));
                            MessageBox.Show($"Магазин {foundSupplierName} найден в базе данных.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Элемент с указанными параметрами не найден в базе данных.");
                    }
                }
                else
                {
                    MessageBox.Show("Не введены критерии поиска.");
                }
            }
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
