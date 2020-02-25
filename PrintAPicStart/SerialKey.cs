using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintAPicStart
{
    public partial class SerialKey : Form
    {
        string LICENSEFILE = "";
        string SERIALKEY = "";
       

        public SerialKey()
        {
            InitializeComponent();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            LICENSEFILE = File.ReadAllText(textBox1.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;
            SERIALKEY = File.ReadAllText(textBox2.Text);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Globals.StandardLicPath))
            {
                Directory.CreateDirectory(Globals.StandardLicPath);
            }
            File.Copy(textBox1.Text, Globals.StandardLicPath + "\\License.txt", true);
            File.Copy(textBox2.Text, Globals.StandardLicPath + "\\SerialKey.txt", true);
            this.Close();
        }
    }
}
