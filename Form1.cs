using System;
using System.Drawing;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Windows.Forms;

/*
 *  Git Requirements to push
 * 
 *   PM> git config --global http.proxy http://domain\-A:-A Password@URL:80
 *   PM> git config --global http.sslVerify false
 *   git config --global http.sslVerify true
 * 
 */


namespace SACTicketFormatter3._0
{
    
    public partial class Form1 : Form
    {
        //Variables and Methods
        public int formatType = 1;
        public string genPassword = "";
        public string supTeam = "";
        public string[] strContext = new string[999];
        public int check = 0;
        public Timer t = new Timer();
        public string TempPath { get; set; }
        IniFile MyIni; //Color Profile Save
        IniFile Saves; //Multiple Save Files

        private void setPath()
        {
            //Setup file Path to temp folder IE: %TEMP%
            string result = Path.GetTempPath();
            TempPath = result;
        }
        public void CopyClipboard(int index)
        {
            Clipboard.SetText(strContext[index]);
        }
        //Initialize Timer
        Timer timer = new Timer();
        //Actual Forms
        public Form1()
        {
            InitializeComponent(); //Core App Configuration
            setPath(); //Gets a file path to TEMP

            //Set password Default Length
            comboBox1.SelectedIndex = 2;

            // Multiple Save initialization
            Saves = new IniFile(TempPath + "SACSaves.ini"); //Creates a ini file which will store Saves for SAC Tickets
            try
            {
                load1ToolStripMenuItem.Text = Saves.Read("SaveName", "Save 1");
                load2ToolStripMenuItem.Text = Saves.Read("SaveName", "Save 2");
                load3ToolStripMenuItem.Text = Saves.Read("SaveName", "Save 3");
                load4ToolStripMenuItem.Text = Saves.Read("SaveName", "Save 4");
                loadToolStripMenuItem1.Text = Saves.Read("SaveName", "Save 5");
            }
            catch(Exception ehh)
            {
                Console.WriteLine(ehh);
            }
            if (!File.Exists(TempPath + "SACSaves.ini"))
            {
                //Indexing for Saves
                string[] Save = { "Save 1", "Save 2", "Save 3", "Save 4", "Save 5"};
                int saves = 0;
                //Default Saves
                for(saves = 0; saves <6; saves++)
                {
                    Saves.Write("SaveName", Save[saves], Save[saves]);
                    Saves.Write("SacTicket", "", Save[saves]);
                    Saves.Write("PrimaryFirstName", "", Save[saves]);
                    Saves.Write("PrimaryLastName", "", Save[saves]);
                    Saves.Write("PrimaryID", "", Save[saves]);
                    Saves.Write("PrimarySNum", "", Save[saves]);
                    Saves.Write("Department", "", Save[saves]);
                    Saves.Write("PrimaryEmail", "", Save[saves]);
                    Saves.Write("PrimaryEmail2", "", Save[saves]);
                    Saves.Write("MirroredFirstName", "", Save[saves]);
                    Saves.Write("MirroredLastName", "", Save[saves]);
                    Saves.Write("MirroredID", "", Save[saves]);
                    Saves.Write("MirroredEmail", "", Save[saves]);
                }

                MessageBox.Show("We have refreshed the save files, either you are a new user or you cleared your Temp Folder.");
            }

            MyIni = new IniFile(TempPath + "Settings.ini"); //Creates a ini file which will store Color for forms
            if (File.Exists(TempPath + "Settings.ini"))
            {
                Console.WriteLine(MyIni.KeyExists("BackgroundColor", "Design"));
                Console.WriteLine(MyIni.KeyExists("ForegroundColor", "Design"));
                if (MyIni.KeyExists("BackgroundColor", "Design"))
                {
                    //Background color
                    string backgroundR = MyIni.Read("BackgroundColorR", "Design");
                    string backgroundG = MyIni.Read("BackgroundColorG", "Design");
                    string backgroundB = MyIni.Read("BackgroundColorB", "Design");
                    Color background = new Color();
                    background = Color.FromArgb(Convert.ToInt32(backgroundR), Convert.ToInt32(backgroundG), Convert.ToInt32(backgroundB));
                    panel1.BackColor = background;
                    groupBox1.BackColor = background;
                    groupBox2.BackColor = background;
                }            
                if (MyIni.KeyExists("ForegroundColor", "Design"))
                {
                    //Foreground color
                    string foregroundR = MyIni.Read("ForegroundColorR", "Design");
                    string foregroundG = MyIni.Read("ForegroundColorG", "Design");
                    string foregroundB = MyIni.Read("ForegroundColorB", "Design");
                    Color foreground = new Color();
                    foreground = Color.FromArgb(Convert.ToInt32(foregroundR), Convert.ToInt32(foregroundG), Convert.ToInt32(foregroundB));
                    panel1.ForeColor = foreground;
                    groupBox1.ForeColor = foreground;
                    groupBox2.ForeColor = foreground;
                    button3.ForeColor = Color.Black;
                }
            }
            else
            {
                try
                {
                    panel1.BackColor = Color.LightGray;
                    groupBox1.BackColor = Color.LightGray;
                    groupBox2.BackColor = Color.LightGray;
                    panel1.ForeColor = Color.Black;
                    groupBox1.ForeColor = Color.Black;
                    groupBox2.ForeColor = Color.Black;
                    button3.ForeColor = Color.Black;
                }
                catch(Exception ee)
                {
                    Console.WriteLine("Oops: " + ee.Message);
                }
            }

            setListBox3(); //Adds all the different forms for SAC's to go to
            

            //Refresh Loop

            timer.Interval = (5 * 100); // 0.5 secs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timeTick(object sender, EventArgs e)
        {
            //Color Changing Randomizer
            int red, green, blue;
            red = 0;
            green = 0;
            blue = 0;
            Random ran = new Random();
            red = ran.Next(255);
            green = ran.Next(255);
            blue = ran.Next(255);

            Color colorRGB = new Color();
            colorRGB = Color.FromArgb(red, green, blue);
            Console.WriteLine(colorRGB);
            panel1.BackColor = colorRGB;
            groupBox1.BackColor = colorRGB;
            groupBox2.BackColor = colorRGB;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //Refresh Handler
            listBox1.Items.Clear();
            if (formatType == 0)
            {
                listBox1.Items.Add(tb_primFirst.Text + " " + tb_primLast.Text + " (" + tb_primID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primEmail.Text);
                listBox1.Items.Add(tb_primEmail2.Text);
                listBox1.Items.Add(tb_mirrorFirst.Text + " " + tb_mirrorLast.Text + " (" + tb_mirrorID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primSNum.Text.ToLower());
                listBox1.Items.Add(genPassword);

            }
            else if (formatType == 1)
            {
                listBox1.Items.Add(tb_primLast.Text + ", " + tb_primFirst.Text + " (" + tb_primID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primEmail.Text);
                listBox1.Items.Add(tb_primEmail2.Text);
                listBox1.Items.Add(tb_mirrorLast.Text + ", " + tb_mirrorFirst.Text + " (" + tb_mirrorID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primSNum.Text.ToLower());
                listBox1.Items.Add(genPassword);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Simply closes program
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            //Clear on Change
            listBox1.Items.Clear();
            //Format Option for Name
            if (formatType == 0)
            {
                listBox1.Items.Add(tb_primFirst.Text + " " + tb_primLast.Text + " (" + tb_primID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primEmail.Text);
                listBox1.Items.Add(tb_primEmail2.Text);
                listBox1.Items.Add(tb_mirrorFirst.Text + " " + tb_mirrorLast.Text + " (" + tb_mirrorID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primSNum.Text.ToLower());
                listBox1.Items.Add(genPassword);
                
            }
            else if (formatType == 1)
            {
                listBox1.Items.Add(tb_primLast.Text + ", " + tb_primFirst.Text + " (" + tb_primID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primEmail.Text);
                listBox1.Items.Add(tb_primEmail2.Text);
                listBox1.Items.Add(tb_mirrorLast.Text + ", " + tb_mirrorFirst.Text + " (" + tb_mirrorID.Text.ToLower() + ")");
                listBox1.Items.Add(tb_primSNum.Text.ToLower());
                listBox1.Items.Add(genPassword);
            }
            //Index Controls
            int index = listBox2.SelectedIndex;
            listBox1.SelectedIndex = index;
            if (checkBox1.Checked == true)
            {
                try
                {
                    Clipboard.SetText(listBox1.Items[index].ToString());
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Error: " + ee.Message);
                }
            }
            else
            {

            }
        }

        private void firstNameLastNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formatType = 0; // Changes it from LastName, FirstName to FirstName LastName
        }

        private void lastNameFirstNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formatType = 1; // Changes it from FirstName LastName to LastName, FirstName
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Clear Function
            tb_sacTicket.Text = "";
            tb_primLast.Text = "";
            tb_primFirst.Text = "";
            tb_primID.Text = "";
            tb_primEmail.Text = "";
            tb_primEmail2.Text = "";
            tb_mirrorLast.Text = "";
            tb_mirrorFirst.Text = "";
            tb_mirrorID.Text = "";
            tb_mirrorEmail.Text = "";
            tb_primSNum.Text = "";
            genPassword = "";
            tb_Department.Text = "";
            tb_job.Text = "";
            tb_RequesterEmail.Text = "";
            tb_copy.Text = "";
            //Set to Default
            listBox1.Items.Clear();
            listBox1.Items.Add("");
            listBox1.Items.Add("");
            listBox1.Items.Add("");
            listBox1.Items.Add("");
            listBox1.Items.Add("");
            listBox1.Items.Add("");
            //Sync
            checkBox1.Checked = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int pLength = Convert.ToInt32(comboBox1.Text); //Grabs Length from Combo Box  
            genPassword = CreatePassword(pLength); //Creates a password at 'X' Character Length
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; //Available Characters to Pick from
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Removed
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Removed
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Matches Listbox 2 Selection
            checkBox1.Checked = true;
            int index = listBox1.SelectedIndex;
            listBox2.SelectedIndex = index;
            if(checkBox1.Checked == true)
            {
                try
                {
                    Clipboard.SetText(listBox1.Items[index].ToString());
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Error: " + ee.Message);
                }
            }
            else
            {
                
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Brings you to the SAC Ticket
            if (tb_sacTicket.Text == "")
            {
                System.Diagnostics.Process.Start("https://itcoe.snapon.com/");
            }
            else
            {
                System.Diagnostics.Process.Start("https://itcoe.snapon.com/browse/SAC-" + tb_sacTicket.Text);
            }
        }
        public void setListBox3()
        {
            //List of Macros
            listBox3.Items.Add("ITCOE");
            listBox3.Items.Add("LN");
            listBox3.Items.Add("Zoom");
            listBox3.Items.Add("CIM");
            listBox3.Items.Add("Tableau");
            listBox3.Items.Add("Supply Web");
            listBox3.Items.Add("Connect Ship");
            listBox3.Items.Add("PAM Groups");
            listBox3.Items.Add("CPS");
            listBox3.Items.Add("Catia");
            listBox3.Items.Add("Autoplot");
            listBox3.Items.Add("CCM"); // Added in 3.3.04
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {   /*
             *  In order to add a new Application you must
             *  1. Add a new Item in Listbox3
             *  2. Add a new strContext
             *  3. Add a what it should say in the strContext
             *  4. Add a Case that also includes what the Support Team is
             */

            string jobdepartment;
            if(tb_job.TextLength>0 && tb_Department.TextLength >1)
            {
                jobdepartment = " is a new " + tb_job.Text + " in " + tb_Department.Text;
            }
            else { jobdepartment = ""; }
            strContext[0] = "Please add user access to ITCOE: " + listBox1.Items[0] + " Email: " + listBox1.Items[1];
            strContext[1] = "Please add user access to LN: " + listBox1.Items[0] + " | Mirror User: " + listBox1.Items[3] + "\n\nUser is Posix enabled.";
            strContext[2] = "Please add user to Zoom: " + listBox1.Items[1] + " | Department: " + tb_Department.Text;
            strContext[3] = "Please add user access to CIM: " + listBox1.Items[0] + " | Mirror User: " + listBox1.Items[3];
            strContext[4] = "Please add user access to Tableau: " + listBox1.Items[0] + " | Mirror User: " + listBox1.Items[3];
            strContext[5] = "Please add user access to Supply Web: " + listBox1.Items[0] + " | Mirror User: " + listBox1.Items[3];
            strContext[6] = "Please add user to Connect Ship: " + listBox1.Items[1];
            strContext[7] = "Please approve access to PAM Group c-Paperwise-CLAK for new user " + listBox1.Items[0];
            strContext[8] = "New associate " + listBox1.Items[0] + jobdepartment + ". Needs access to CPS. Their permissions should mirror existing employee " + listBox1.Items[3] + " do you approve of adding " + listBox1.Items[0] + " to the group(s):";
            strContext[9] = "New associate " + listBox1.Items[0] + jobdepartment + ". Needs access to Catia. Their permissions should mirror existing employee " + listBox1.Items[3] + " do you approve of adding " + listBox1.Items[0] + " to the group(s):";
            strContext[10] = "New associate " + listBox1.Items[0] + jobdepartment + ". Needs access to Autoplot. Their permissions should mirror existing employee " + listBox1.Items[3] + " do you approve of adding " + listBox1.Items[0] + " to the group(s):";
            strContext[11] = "Please add user access to CCM: " + listBox1.Items[0] + " | Mirror User: " + listBox1.Items[3] + "\n\nUser is Posix enabled."; // Added in 3.3.04
            int index = listBox3.SelectedIndex + 1;
            switch (index)
            {
                case 1:
                    supTeam = "IT Support";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 2:
                    supTeam = "Unix Admin | LN Support";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 3:
                    supTeam = "IT Support";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 4:
                    supTeam = "CIM";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 5:
                    supTeam = "Information Warehouse";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 6:
                    supTeam = "Supply Web";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 7:
                    supTeam = "BoltOnSupport";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 8:
                    supTeam = "*Depends on PAM*";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 9:
                    supTeam = "CPS";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 10:
                    supTeam = "Catia";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 11:
                    supTeam = "Autoplot";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                case 12:
                    // Added in 3.3.04
                    supTeam = "CCM Admin";
                    CopyClipboard(index - 1);
                    tb_copy.Text = strContext[index - 1];
                    break;
                default:
                    CopyClipboard(index - 1);
                    break;
            }
            lbSupportTeam.Text = "Support Team: " + supTeam;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        //Email

        private void button4_Click(object sender, EventArgs e)
        {
            string requester = "user@example.com";
            if (tb_RequesterEmail.Text.Contains("@"))
            {
                requester = tb_RequesterEmail.Text;
            }
            string body = ("Below you will find the ID And password that has been created for this request. Please have the new user change the password by using the password provided below to log into the following site: https://qpss.snapon.com/pmuser there they will be prompted to enter a new one.%0d%0a%0d%0a" +
                                "E-mail Address: " + tb_primEmail.Text + "%0d%0a" +
                                "Username: " + tb_primID.Text + "%0d%0a" +
                                "Password: " + genPassword + "%0d%0a%0d%0a" +
                                "If you experience problems logging in, please respond to this e-mail or contact the corporate help desk at: (800)762-7638. %0d%0a" +
                                "Please allow 24 hours for all access to replicate."
                          );



            System.Diagnostics.Process.Start("mailto:"+requester+"?cc="+tb_primEmail.Text+ "&subject=New Hire - " + listBox1.Items[0] + "&body="+body);
        }
        //End Email

       
        private void colorToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void rGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (check == 0)
            {
                //Rainbow
                
                t.Interval = (1 * 1000); // 1 secs
                t.Tick += new EventHandler(timeTick);
                t.Start();
                check = 1;
            }
            else if (check>0)
            {
                t.Enabled = false;
                check = 0;
            }
        }

        private void colorPickerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorpanel = new ColorDialog();
            colorpanel.FullOpen = true;
            colorpanel.ShowHelp = true;
            if (colorpanel.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorpanel.Color;
                groupBox1.BackColor = colorpanel.Color;
                groupBox2.BackColor = colorpanel.Color;

                MyIni.Write("BackgroundColor", "True", "Design");
                MyIni.Write("BackgroundColorR", colorpanel.Color.R.ToString(), "Design");
                MyIni.Write("BackgroundColorG", colorpanel.Color.G.ToString(), "Design");
                MyIni.Write("BackgroundColorB", colorpanel.Color.B.ToString(), "Design");
                Console.WriteLine("Saved to .ini: " + colorpanel.Color.R.ToString());
            }

        }
        

        private void btn_Sync_Click(object sender, EventArgs e)
        {
            
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) //Default State
            {
                timer.Start();
                lb_sync.Text = "Sync";
            }
            else
            {
                timer.Stop();
                lb_sync.Text = "Pause";
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            timer.Stop();
            lb_sync.Text = "Pause";
        }

        private void listBox2_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            timer.Stop();
            lb_sync.Text = "Pause";
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorpanel = new ColorDialog();
            colorpanel.FullOpen = true;
            colorpanel.ShowHelp = true;
            if (colorpanel.ShowDialog() == DialogResult.OK)
            {
                panel1.ForeColor = colorpanel.Color;
                groupBox1.ForeColor = colorpanel.Color;
                groupBox2.ForeColor = colorpanel.Color;
                button3.ForeColor = Color.Black;
                MyIni.Write("ForegroundColor", "True", "Design");
                MyIni.Write("ForegroundColorR", colorpanel.Color.R.ToString(), "Design");
                MyIni.Write("ForegroundColorG", colorpanel.Color.G.ToString(), "Design");
                MyIni.Write("ForegroundColorB", colorpanel.Color.B.ToString(), "Design");
                Console.WriteLine("Saved to .ini: " + colorpanel.Color.R.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://idm.snapon.com/#/list/managed/employee");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tb_primID.Text.ToLower());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // SAVE SLOTS
        private void save1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMethod("Save 1", save1ToolStripMenuItem, load1ToolStripMenuItem);
        }

        private void save2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMethod("Save 2", save2ToolStripMenuItem, load2ToolStripMenuItem);
        }

        private void save3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMethod("Save 3", save3ToolStripMenuItem, load3ToolStripMenuItem);
        }

        private void save4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMethod("Save 4", save4ToolStripMenuItem, load4ToolStripMenuItem);
        }

        private void save5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Fixed in 3.3.03
            SaveMethod("Save 5", save5ToolStripMenuItem, loadToolStripMenuItem1);
        }

        private void load1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMethod("Save 1");
        }

        private void load2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMethod("Save 2");
        }

        private void load3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMethod("Save 3");
        }

        private void load4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMethod("Save 4");
        }

        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadMethod("Save 5");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //CSSR Email Alias merge button
            tb_copy.Text = "";
            string var = ("Please add CSSR email address of "+ tb_primSNum.Text + "@snapon.com to primary account: "+ listBox1.Items[0] +" | "+tb_primEmail.Text);
            Clipboard.SetText(var);
            tb_copy.Text = var;
        }

        private void SaveMethod(string SaveIndex, ToolStripMenuItem SaveTool, ToolStripMenuItem LoadTool)
        {
            string Name = "";
            Name = Prompt.ShowSACDialog("Name this Save.", "Save Dialog");
            SaveTool.Text = Name;
            LoadTool.Text = Name;
            Saves.Write("SaveName", Name, SaveIndex);

            Saves.Write("SacTicket", tb_sacTicket.Text, SaveIndex);
            Saves.Write("PrimaryFirstName", tb_primFirst.Text, SaveIndex);
            Saves.Write("PrimaryLastName", tb_primLast.Text, SaveIndex);
            Saves.Write("PrimaryID", tb_primID.Text, SaveIndex);
            Saves.Write("PrimarySNum", tb_primSNum.Text, SaveIndex);
            Saves.Write("Department", tb_Department.Text, SaveIndex);
            Saves.Write("PrimaryEmail", tb_primEmail.Text, SaveIndex);
            Saves.Write("PrimaryEmail2", tb_primEmail2.Text, SaveIndex);
            Saves.Write("MirroredFirstName", tb_mirrorFirst.Text, SaveIndex);
            Saves.Write("MirroredLastName", tb_mirrorLast.Text, SaveIndex);
            Saves.Write("MirroredID", tb_mirrorID.Text, SaveIndex);
            Saves.Write("MirroredEmail", tb_mirrorEmail.Text, SaveIndex);
            //Patch 3.3.03 Password Save
            Saves.Write("Password", genPassword, SaveIndex);
        }
        private void LoadMethod(string LoadIndex)
        {
            tb_sacTicket.Text = Saves.Read("SacTicket", LoadIndex);
            tb_primFirst.Text = Saves.Read("PrimaryFirstName", LoadIndex);
            tb_primLast.Text = Saves.Read("PrimaryLastName", LoadIndex);
            tb_primID.Text = Saves.Read("PrimaryID", LoadIndex);
            tb_primSNum.Text = Saves.Read("PrimarySNum", LoadIndex);
            tb_Department.Text = Saves.Read("Department", LoadIndex);
            tb_primEmail.Text = Saves.Read("PrimaryEmail", LoadIndex);
            tb_primEmail2.Text = Saves.Read("PrimaryEmail2", LoadIndex);
            tb_mirrorFirst.Text = Saves.Read("MirroredFirstName", LoadIndex);
            tb_mirrorLast.Text = Saves.Read("MirroredLastName", LoadIndex);
            tb_mirrorID.Text = Saves.Read("MirroredID", LoadIndex);
            tb_mirrorEmail.Text = Saves.Read("MirroredEmail", LoadIndex);
            //Patch 3.3.03 Password Save
            genPassword = Saves.Read("Password", LoadIndex);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About newAbout = new About();
            newAbout.Show();
        }

        //Added 3/11/22 in 3.3.01
        private void findADGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //User must have RSAT installed to use
            string username = "";
            string userid = tb_primID.Text;
            username = tb_mirrorID.Text;
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                using (var powerShell = PowerShell.Create())
                {
                    //PowerShell Command
                    string script = "Get-ADPrincipalGroupMembership " + username + " | select name";

                    //start powershell script
                    powerShell.Runspace = runspace;
                    powerShell.Runspace.Open();
                    powerShell.AddScript(script);
                    int i = 0;

                    //Window Open
                    Groups GroupWin = new Groups(username, userid, powerShell.Invoke().Count);
                    GroupWin.Icon = this.Icon;
                    
                    //Compile the Data to Groups.cs
                    foreach (PSObject result in powerShell.Invoke())
                    {
                        string temp = result.ToString().Trim(new char[] { '@', '{', '}', '=' });
                        temp = temp.Remove(0, 5);
                        GroupWin.temp[i] = temp;
                        i++;
                    } // End foreach.

                    GroupWin.Show();
                }
            }
            
        }

        //Added in 3.3.02
        private void button8_Click(object sender, EventArgs e)
        {
            Form1 newPage = new Form1();
            newPage.Show();
        }
    }
}
