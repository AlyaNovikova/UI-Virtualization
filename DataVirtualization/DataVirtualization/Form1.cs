using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataVirtualization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            ListBox listbox = new ListBox();
            listbox.Location = new System.Drawing.Point(10, 10);
            listbox.Name = "ListBox";
            listbox.Size = new System.Drawing.Size(300, 300);
            listbox.BackColor = Color.White;
            listbox.ForeColor = Color.Black;
            listbox.BorderStyle = BorderStyle.FixedSingle;

            Controls.Add(listbox);

            for (int i = 0; i < 1000; i++)
            {
                listbox.Items.Add("Dog");
                listbox.Items.Add("Cat");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
