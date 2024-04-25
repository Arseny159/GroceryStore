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
    public partial class AddAssortmentForm : Form
    {
        public string idx_temp = "";

        public AddAssortmentForm(string idx)
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
                return;
            }
            else
            {
                string name = textBox1.Text;
                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
                {
                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand($"INSERT INTO assortment (assortment_id, assortment_name, type_id, unit_of_measure_id) VALUES (((select max(assortment_id) from assortment) + 1), @name, (select type_id from assortment where assortment_name = '{idx_temp}'), (select type_id from assortment where assortment_name = '{idx_temp}'))", conn);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
