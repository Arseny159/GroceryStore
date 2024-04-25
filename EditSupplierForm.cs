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
    public partial class EditSupplierForm : Form
    {
        public string idx_temp = "";

        public EditSupplierForm(string idx)
        {
            InitializeComponent();
            idx_temp = idx;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string[] parts = idx_temp.Split(new[] { " (" }, StringSplitOptions.RemoveEmptyEntries);
            string supplierName = parts[0];
            string supplierAddress = parts[1].TrimEnd(')');

            errorProvider1.Clear();

            string address = textBox1.Text;
            string requisites = textBox2.Text;
            string phone_number = textBox3.Text;
            string email = textBox4.Text;
            string name = textBox5.Text;

            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand("UPDATE supplier SET ", conn);

                if (!string.IsNullOrEmpty(address))
                {
                    comm.CommandText += "supplier_address = @address";
                    comm.Parameters.AddWithValue("@address", address);
                }
                if (!string.IsNullOrEmpty(requisites))
                {
                    if (!string.IsNullOrEmpty(address))
                        comm.CommandText += ", ";
                    comm.CommandText += "supplier_payment_account = @requisites";
                    comm.Parameters.AddWithValue("@requisites", requisites);
                }
                if (!string.IsNullOrEmpty(phone_number))
                {
                    if (!string.IsNullOrEmpty(address) || !string.IsNullOrEmpty(requisites))
                        comm.CommandText += ", ";
                    comm.CommandText += "supplier_phone_number = @phone_number";
                    comm.Parameters.AddWithValue("@phone_number", phone_number);
                }
                if (!string.IsNullOrEmpty(email))
                {
                    if (!string.IsNullOrEmpty(address) || !string.IsNullOrEmpty(requisites) || !string.IsNullOrEmpty(phone_number))
                        comm.CommandText += ", ";
                    comm.CommandText += "supplier_email_address = @email";
                    comm.Parameters.AddWithValue("@email", email);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(address) || !string.IsNullOrEmpty(requisites) || !string.IsNullOrEmpty(phone_number) || !string.IsNullOrEmpty(email))
                        comm.CommandText += ", ";
                    comm.CommandText += "supplier_name = @name";
                    comm.Parameters.AddWithValue("@name", name);
                }

                comm.CommandText += $" WHERE supplier_name = @supplier_name AND supplier_address = @supplier_address;";
                comm.Parameters.AddWithValue("@supplier_name", supplierName);
                comm.Parameters.AddWithValue("@supplier_address", supplierAddress);

                comm.ExecuteNonQuery();
            }
            Close();
        }

        private void EditSupplierForm_Load(object sender, EventArgs e)
        {

        }
    }
}
