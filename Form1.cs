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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TreeViewLab2
{
    public partial class Form1 : Form
    {
        public string idx;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand(@"SELECT * FROM supplier", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        void LoadGroup(TreeNode parent, int assortment_id)
        {
            string t = "";
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand($"SELECT * FROM assortment WHERE type_id = {assortment_id}", conn);
                comm.Parameters.AddWithValue("assortment_id", assortment_id);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var node = new TreeNode(dr["assortment_name"].ToString());
                    string e = node.ToString();
                    if (e != t)
                    {
                        parent.Nodes.Add(node);
                        t = e;
                        LoadGroup2(node, (int)dr["assortment_id"]);
                        node.ContextMenuStrip = ctmAssortment;
                    }
                }
            }
        }

        void LoadGroup2(TreeNode parent, int assortment_id)
        {
            string t = "";
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand($"SELECT * FROM shipment WHERE assortment_id = {assortment_id}", conn);
                comm.Parameters.AddWithValue("assortment_id", assortment_id);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var node = new TreeNode(dr["shipment_date_time"].ToString());
                    string e = node.ToString();
                    if (e != t)
                    {
                        parent.Nodes.Add(node);
                        t = e;
                        node.ContextMenuStrip = ctmDate;
                    }
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            idx = e.Node.Text;
        }

        private void ctmSupplier_Opening(object sender, CancelEventArgs e)
        {

        }

        private void ctmAssortment_Opening(object sender, CancelEventArgs e)
        {

        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode != null)
            {
                string[] nodeTextParts = selectedNode.Text.Split(new[] { " (" }, StringSplitOptions.RemoveEmptyEntries);
                string nodeName = nodeTextParts[0];
                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
                {
                    conn.Open();
                    NpgsqlCommand deleteComm = new NpgsqlCommand($"DELETE FROM supplier WHERE supplier_name = @name", conn);
                    deleteComm.Parameters.AddWithValue("@name", nodeName);
                    deleteComm.ExecuteNonQuery();
                }
                treeView1.Nodes.Remove(selectedNode);
            }
        }

        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection connDelete = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                connDelete.Open();
                NpgsqlCommand deleteComm = new NpgsqlCommand($"DELETE FROM assortment WHERE assortment_name = @name", connDelete);
                deleteComm.Parameters.AddWithValue("@name", idx);
                deleteComm.ExecuteNonQuery();
            }
            treeView1.Nodes.Clear();
            using (NpgsqlConnection connRefresh = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                connRefresh.Open();
                NpgsqlCommand refreshComm = new NpgsqlCommand(@"SELECT * FROM supplier", connRefresh);
                NpgsqlDataReader dr = refreshComm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void удалитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection connDelete = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                connDelete.Open();
                NpgsqlCommand deleteComm = new NpgsqlCommand($"DELETE FROM shipment WHERE shipment_date_time = @dateTime", connDelete);
                deleteComm.Parameters.AddWithValue("@dateTime", idx);
                deleteComm.ExecuteNonQuery();
            }
            treeView1.Nodes.Clear();
            using (NpgsqlConnection connRefresh = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                connRefresh.Open();
                NpgsqlCommand refreshComm = new NpgsqlCommand(@"SELECT * FROM supplier", connRefresh);
                NpgsqlDataReader dr = refreshComm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new EditSupplierForm(idx);
            frm.ShowDialog();
            treeView1.Nodes.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand(@"select * from supplier", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void изменитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frm = new EditAssortmentForm(idx);
            frm.ShowDialog();
            treeView1.Nodes.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand(@"select * from supplier", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void изменитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var frm = new EditDateForm(idx);
            frm.ShowDialog();
            treeView1.Nodes.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand(@"select * from supplier", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new AddSupplierForm(idx);
            frm.ShowDialog();
            treeView1.Nodes.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand(@"select * from supplier", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void добавитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frm = new AddAssortmentForm(idx);
            frm.ShowDialog();
            treeView1.Nodes.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand(@"select * from supplier", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void добавитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var frm = new AddDateForm(idx);
            frm.ShowDialog();
            treeView1.Nodes.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; Database=GroceryStore; User Id=postgres; Password=123456;"))
            {
                treeView1.Visible = true;
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand(@"select * from supplier", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    var nodeName = dr["supplier_name"].ToString();
                    var nodeAddress = dr["supplier_address"].ToString();
                    var node = new TreeNode($"{nodeName} ({nodeAddress})");
                    treeView1.Nodes.Add(node);
                    LoadGroup(node, (int)dr["supplier_id"]);
                    node.ContextMenuStrip = ctmSupplier;
                }
            }
        }

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new SearchSupplierForm(idx);
            frm.ShowDialog();
        }

        private void поискToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frm = new SearchAssortmentForm(idx);
            frm.ShowDialog();
        }

        private void поискToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var frm = new SearchDateForm(idx);
            frm.ShowDialog();
        }
    }
}
