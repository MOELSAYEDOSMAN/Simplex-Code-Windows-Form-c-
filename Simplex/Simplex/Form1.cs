using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simplex
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void InsertValuseInGrideView1_2(int Varibles, int equales)
        {
            DataTable table1 = new DataTable();
            for (int i = 1; i <= Varibles; i++)
            {
                table1.Columns.Add($"x{i}");
            }
            table1.Columns.Add("sign");
            table1.Columns.Add("R.H.P");
            for (int i = 1; i < equales; i++)
            {
                table1.Rows.Add();
            }
            dataGridView1.DataSource = table1;

            DataTable table2 = new DataTable();
            for (int i = 1; i <= Varibles; i++)
            {
                table2.Columns.Add($"x{i}");
            }

            dataGridView2.DataSource = table2;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Varibles.Value != 0 && equal.Value != 0)
            {
                InsertValuseInGrideView1_2(int.Parse(Varibles.Value.ToString()), int.Parse(equal.Value.ToString()));
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
                Simple simple = new Simple();
                DataTable table = new DataTable();
                double sum = simple.sim(dataGridView1, dataGridView2, comboBox1.Items[comboBox1.SelectedIndex].ToString(), ref table);
                label5.Text = sum.ToString();
                dataGridView3.DataSource = table;
           
        }
    }
}
