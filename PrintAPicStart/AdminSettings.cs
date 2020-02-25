using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PrintAPicStart
{
    public partial class AdminSettings : Form
    {

        XmlDocument xDoc;
        public AdminSettings()
        {
            InitializeComponent();
        }

        private void AdminSettings_Load(object sender, EventArgs e)
        {
            CreateControlsBasedOnXmlElements();
        }

        private void CreateControlsBasedOnXmlElements()
        {
            xDoc = new XmlDocument();
            xDoc.Load("PicsDirectoryDisplayWin.exe.config");
            XmlNodeList adds = xDoc.GetElementsByTagName("add");
            foreach (XmlNode item in adds)
            {
                flowLayoutPanel1.Controls.Add(new AdminSettingsControl(item.Attributes["key"].Value, item.Attributes["value"].Value));
            }
        }

        private void SaveDocument()
        {
            //XmlNodeList adds = xDoc.GetElementsByTagName("add");
            foreach (var item in flowLayoutPanel1.Controls)
            {
                if (item is AdminSettingsControl  && ((AdminSettingsControl)item).GetChangedValue()!= null)
                {
                    AssignValueToXDocElement(((AdminSettingsControl)item).Property ,((AdminSettingsControl)item).GetChangedValue());
                }
               
            }
            xDoc.Save("PicsDirectoryDisplayWin.exe.config");
        }

        private void AssignValueToXDocElement(string prop,string value)
        {
            XmlNodeList adds = xDoc.GetElementsByTagName("add");
            foreach (XmlNode item in adds)
            {
                if (item.Attributes["key"].Value == prop)
                {
                    item.Attributes["value"].Value = value;
                }
                //flowLayoutPanel1.Controls.Add(new AdminSettingsControl(item.Attributes["key"].Value, item.Attributes["value"].Value));
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SaveDocument();
            this.Close();
        }
    }
}
