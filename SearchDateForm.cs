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
    public partial class SearchDateForm : Form
    {
        public List<string> SearchConditions { get; } = new List<string>();

        public string idx_temp = "";

        public SearchDateForm(string idx)
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
                string date = textBox1.Text;
                bool found = false;
                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
                {
                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand($"SELECT * FROM shipment WHERE shipment_date_time = @date", conn);
                    comm.Parameters.AddWithValue("@date", date);
                    NpgsqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        found = true;
                        while (reader.Read())
                        {
                            string foundDate = reader.GetString(reader.GetOrdinal("shipment_date_time"));
                            MessageBox.Show($"Элемент найден в базе данных. Дата отгрузки: {foundDate}");
                        }
                    }
                }

                if (!found)
                {
                    MessageBox.Show("Элемент с указанной датой отгрузки не найден в базе данных.");
                }
            }
            Close();
        }
    }
}
