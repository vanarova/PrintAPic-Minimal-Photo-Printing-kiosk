using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.UI
{
    public partial class FlexMessageBox : Form
    {
        public DialogResult Result  { get; set; }
        public FlexMessageBox()
        {
            InitializeComponent();
     
           
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Yes;
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Result = DialogResult.No;
            this.Close();
        }

        private void FlexMessageBox_Load(object sender, EventArgs e)
        {
            //tableLayoutPanel1.BackColor = Color.FromName(ConfigurationManager.AppSettings["AppBackgndColor"]);
        }
    }
}
