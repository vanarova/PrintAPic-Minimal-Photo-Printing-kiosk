using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintAPicStart
{
    public partial class Diagnostics : Form
    {
        public object client { get; private set; }

        public Diagnostics()
        {
            InitializeComponent();
        }

        private void Diagnostics_Load(object sender, EventArgs e)
        {

        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            bool IsNetworkAvailable = false;
            IsNetworkAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            textBox1.Text = "Checking avlble network connections ---";
            if (IsNetworkAvailable)
            {
                textBox1.Text = textBox1.Text + " STATUS -- OK" + Environment.NewLine;
            }
            else
            {
                textBox1.Text = textBox1.Text + " STATUS -- NO NETWORK FOUND" + Environment.NewLine;
                textBox1.Text = textBox1.Text + " Aborting" + Environment.NewLine;
                return;
            }

            try
            {
                await RequestHTTP();

            }
            catch (Exception)
            {
                HTTPChkFlag = false;
            }
            if (!HTTPChkFlag)
            {
                textBox1.Text = textBox1.Text + " STATUS -- WEBSITE DOWN" + Environment.NewLine;
                textBox1.Text = textBox1.Text + " Aborting with Error" + Environment.NewLine;
                return;
            }


            textBox1.Text = textBox1.Text + "Pinging Router 10.3.141.1 --- " + Environment.NewLine;
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send("10.3.141.1", 1000);
                if (reply != null)
                {
                    textBox1.Text = textBox1.Text + ("STATUS :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address) + Environment.NewLine;
                    //Console.WriteLine(reply.ToString());

                }
            }
            catch
            {
                Console.WriteLine("ERROR: You have Some TIMEOUT issue");
            }

            textBox1.Text = textBox1.Text + "Checking static IP 10.3.141.61 --- ";
            if (CheckLocalIPAddress("10.3.141.61"))
            {
                textBox1.Text = textBox1.Text + "STATUS -- OK" + Environment.NewLine;
            }
            else
            {
                textBox1.Text = textBox1.Text + " STATUS -- NO STATIC IP FOUND" + Environment.NewLine;
                textBox1.Text = textBox1.Text + "ABORTING WITH ERROR" + Environment.NewLine;
                return;
            }



            //textBox1.Text = textBox1.Text + "FINISHED, No Error Found." + Environment.NewLine;
        }

        bool HTTPChkFlag = false;
        private async Task<string> RequestHTTP()
        {
            HttpClient client = new HttpClient();
            var responseString = await client.GetStringAsync("http://localhost");
            textBox1.Text = textBox1.Text + "Checking Website --- ";
            if (responseString.Contains("<title>Print-a-Pic</title>"))
            {
                textBox1.Text = textBox1.Text + "STATUS -- OK" + Environment.NewLine;
                HTTPChkFlag = true;
            }
            else
            {
                textBox1.Text = textBox1.Text + " STATUS -- WEBSITE DOWN" + Environment.NewLine;
                textBox1.Text = textBox1.Text + "ABORTING WITH ERROR" + Environment.NewLine;
                HTTPChkFlag = false;
            }
            return responseString;
        }


        public bool CheckLocalIPAddress(string ipToChk)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ipToChk.Equals(ip.ToString()))
                    return true;
            }
            return false;
        }
    }
}
