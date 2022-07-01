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
            /*
             *  Naming Conventions for Updates
             *  
             *  v3.0 is the major build this is the 3 Version of this program, likely v4.0 will come out with Net 6.0 becomes more common
             *  v3.3 is the minor build this is the 3rd version of the 3 Version of the program, generally these bulids are when new features come out
             *  v3.3.1 is the general fix path, I increase this up to 9, after 9 there is a new feature build at that point but you are welcome to continue it 3.3.9,3.3.10,3.3.11,etc.
             *  v3.3.01 The 0 represents that this will be a test build, 3.3.01 - 3.3.0100 would mean there was 100 Test Builds
             * 
             */
            lbAbout.Text = "SAC Ticket Formatter Tool \n";
            lbAbout.Text += "Version: 3.3.11 \n";
            lbAbout.Text += "Created by: David Gullo \n"; //Feel free to add yourself to this project, if you contribute anything to this
        }

        private void About_Load(object sender, EventArgs e)
        {

        }
    }
}
