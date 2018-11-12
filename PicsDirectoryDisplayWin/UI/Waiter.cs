using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin
{
    public partial class Waiter : Form
    {
        public string FileFoundLabelText { set {
                FileFoundLabel.Text = value;
            } }
        public Waiter()
        {
            InitializeComponent();
        }
    }
}
