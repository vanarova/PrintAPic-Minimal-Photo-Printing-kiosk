using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyGenrator_PrintAPic
{
    public partial class Form1 : Form
    {

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public Form1()
        {
            InitializeComponent();
            textBox3.Text = RandomString(6);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var text = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return text + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
        }

        public string standardPath { get
            {
                return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PrintAPicLicenses";
            }
                }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
            //check standard directory
            if (!Directory.Exists(standardPath))
            {
                Directory.CreateDirectory(standardPath);
            }
            //directory inside mydocuments
            string LICENSEDIR = standardPath + "\\" + textBox3.Text;
            Directory.CreateDirectory(LICENSEDIR);

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Load user license request file.");
                return;
            }

            string licFilePath = standardPath + "\\" + textBox3.Text + "\\" + textBox3.Text + ".txt";

            FileStream licFile = File.Create(licFilePath);
            licFile.Close();
            licFile.Dispose();
            string userSysInfo = textBox2.Text;


            var csp = new RSACryptoServiceProvider(2048);

            //how to get the private key
            var privKey = csp.ExportParameters(true);

            //and the public key ...
            var pubKey = csp.ExportParameters(false);

            //converting the public key into a string representation
            string pubKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, pubKey);
                //get the string from the stream
                pubKeyString = sw.ToString();
                pubKeyString = Base64Encode(pubKeyString);
                File.WriteAllText(LICENSEDIR + "\\" + "SerialKey.txt", pubKeyString);
                //pubKeyString = Convert.ToBase64String(sw.ToString());
            }


            //converting the private key into a string representation
            string privateKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, privKey);
                //get the string from the stream
                privateKeyString = sw.ToString();
                privateKeyString = Base64Encode(privateKeyString);
                File.WriteAllText(LICENSEDIR + "\\" + "privateKey.txt", privateKeyString);
                //pubKeyString = Convert.ToBase64String(sw.ToString());
            }

            //converting public key it back
            {
                pubKeyString = Base64Decode(pubKeyString);
                //get a stream from the string
                var sr = new System.IO.StringReader(pubKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }


            //converting private key it back
            {
                privateKeyString = Base64Decode(privateKeyString);
                //get a stream from the string
                var sr = new System.IO.StringReader(privateKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                privKey = (RSAParameters)xs.Deserialize(sr);
            }

            //conversion for the private key is no black magic either ... omitted

            //we have a public key ... let's get a new csp and load that key
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);

            //we need some data to encrypt
            var plainTextData = userSysInfo;

            //for encryption, always handle bytes...
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            //we might want a string representation of our cypher text... base64 will do
            var cypherText = Convert.ToBase64String(bytesCypherText);
            File.WriteAllText(licFilePath, cypherText);

            /*
             * some transmission / storage / retrieval
             * 
             * and we want to decrypt our cypherText
             */

            //first, get our bytes back from the base64 string ...
            bytesCypherText = Convert.FromBase64String(cypherText);

            //we want to decrypt, therefore we need a csp and load our private key
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);

            //decrypt and strip pkcs#1.5 padding
            bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            //get our original plainText back...
            plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
            textBox2.Text = plainTextData;
            statuslbl.Text = "License generation done..";

        }
            catch (Exception exp)
            {
                MessageBox.Show("Execption in program, error text:" + exp.Message);
            }

}

        private void button3_Click(object sender, EventArgs e)
        {
            Help hform = new Help();
            hform.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(textBox5.Text) && string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    MessageBox.Show("Load user license file & public key.");
                    return;
                }

                //Read both files
                string cypherText = File.ReadAllText(textBox5.Text);
                string privateKeyString = File.ReadAllText(textBox6.Text);


                privateKeyString = Base64Decode(privateKeyString);
                //get a stream from the string
                var sr = new System.IO.StringReader(privateKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                var privKey = (RSAParameters)xs.Deserialize(sr);

                //first, get our bytes back from the base64 string ...
                var bytesCypherText = Convert.FromBase64String(cypherText);

                //we want to decrypt, therefore we need a csp and load our private key
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(privKey);

                //decrypt and strip pkcs#1.5 padding
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

                //get our original plainText back...
                var plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
                textBox4.Text = plainTextData;
                label6.Text = "License read, please verify text contents";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Execption in program, error text:" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           openFileDialog1.ShowDialog();
           textBox1.Text = openFileDialog1.FileName;
            string userSysInfo = File.ReadAllText(textBox1.Text);
            textBox2.Text = userSysInfo;


        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox6.Text = openFileDialog1.FileName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox5.Text = openFileDialog1.FileName;
        }
    }
}
