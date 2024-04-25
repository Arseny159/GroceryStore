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
    public partial class AddSupplierForm : Form
    {
        public string idx_temp = "";

        public AddSupplierForm(string idx)
        {
            InitializeComponent();
            idx_temp = idx;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Значение поля не может быть пустым.");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Значение поля не может быть пустым.");
            }
            else if (string.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.SetError(textBox3, "Значение поля не может быть пустым.");
            }
            else if (string.IsNullOrEmpty(textBox4.Text))
            {
                errorProvider1.SetError(textBox4, "Значение поля не может быть пустым.");
            }
            else if (string.IsNullOrEmpty(textBox5.Text))
            {
                errorProvider1.SetError(textBox5, "Значение поля не может быть пустым.");
            }
            else
            {
                string address = textBox1.Text;
                string requisites = textBox2.Text;
                string phone_number = textBox3.Text;
                string email = textBox4.Text;
                string name = textBox5.Text;

                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
                {
                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand($"INSERT INTO supplier (supplier_id, supplier_address, supplier_payment_account, supplier_phone_number, supplier_email_address, supplier_name) VALUES (((select max(supplier_id) from supplier) + 1), @address, @requisites, @phone_number, @email, @name)", conn);
                    comm.Parameters.AddWithValue("@address", address);
                    comm.Parameters.AddWithValue("@requisites", requisites);
                    comm.Parameters.AddWithValue("@phone_number", phone_number);
                    comm.Parameters.AddWithValue("@email", email);
                    comm.Parameters.AddWithValue("@name", name);
                    comm.ExecuteNonQuery();
                }
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
