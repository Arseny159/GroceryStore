using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TreeViewLab2
{
    public partial class SearchAssortmentForm : Form
    {
        public List<string> SearchConditions { get; } = new List<string>();

        public string idx_temp = "";

        public SearchAssortmentForm(string idx)
        {
            InitializeComponent();
            idx_temp = idx;
        }

        private void label2_Click(object sender, EventArgs e)
        {

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
                bool found = false;
                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
                {
                    conn.Open();
                    NpgsqlCommand checkCommand = new NpgsqlCommand($"SELECT * FROM assortment WHERE assortment_name = @name", conn);
                    checkCommand.Parameters.AddWithValue("@name", name);
                    NpgsqlDataReader reader = checkCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        found = true;
                        while (reader.Read())
                        {
                            string foundItemName = reader.GetString(reader.GetOrdinal("assortment_name"));
                            MessageBox.Show($"Элемент {foundItemName} найден в базе данных.");
                        }
                    }
                }

                if (!found)
                {
                    MessageBox.Show("Элемент не найден в базе данных.");
                }
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
