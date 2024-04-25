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
    public partial class AddDateForm : Form
    {
        public string idx_temp = "";

        public AddDateForm(string idx)
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
            else
            {
                string date = textBox1.Text;
                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
                {
                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand($"INSERT INTO shipment (shipment_id, supplier_id, assortment_id, shipment_date_time) VALUES (((select max(shipment_id) from shipment) + 1), (select supplier_id from shipment where shipment_date_time = '{idx_temp}'), (select assortment_id from shipment where shipment_date_time = '{idx_temp}'), @date)", conn);
                    comm.Parameters.AddWithValue("@date", date);
                    comm.ExecuteNonQuery();
                }
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
