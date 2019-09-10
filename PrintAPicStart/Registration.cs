using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintAPicStart
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
            comboBox1.SelectedIndex =0;
        }

       
        public bool IS_LICENSE_VALID { get; set; }

        //string LICENSEdIR = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PrintAPic";

        private void Button1_Click(object sender, EventArgs e)
        {
            //check standard directory
            if (!Directory.Exists(Globals.StandardLicPath))
            {
                Directory.CreateDirectory(Globals.StandardLicPath);
            }

            label5.Text = "";
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                label5.Text = "Please fill all fields";
                return;
            }
            string requestText = Licensing.GetUniqueSystemIDs(
                textBox1.Text,textBox2.Text,textBox4.Text, comboBox1.Items[comboBox1.SelectedIndex].ToString());

            File.WriteAllText(Globals.StandardLicPath + "\\" + "LicenseReq.txt", requestText);



            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            button2.Visible = true;
            label8.Text = Globals.StandardLicPath;

        }

      

        //Dont use mac, it can change with netwrk adaptor
        private string GetMAC()
        {
             string mac = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();
            return mac;
        }  

        

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Globals.StandardLicPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cant open directory, Please try again");
            }
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SerialKey sform = new SerialKey();
            sform.ShowDialog();
            string error = "";
            IS_LICENSE_VALID = Licensing.validateLicense(out error);

            if (IS_LICENSE_VALID)
                label5.Text = "License is valid";
            else
            {
                label5.Text = "No license available";
                MessageBox.Show(error);
            }
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            string error="";
            IS_LICENSE_VALID = Licensing.validateLicense(out error);
            if (IS_LICENSE_VALID)
                label5.Text = "License is valid";
            else
                label5.Text = "No license available";
        }

       
    }
}
