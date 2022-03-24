using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SACTicketFormatter3._0
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            iniAbout();
        }

        private void iniAbout()
        {
            lbAbout.Text = "SAC Ticket Formatter Tool \n";
            lbAbout.Text += "Version: 3.3.1 \n";
            lbAbout.Text += "Created by: David Gullo \n"; //Feel free to add yourself to this project, if you contribute anything to this
        }

        private void About_Load(object sender, EventArgs e)
        {

        }
    }
}
