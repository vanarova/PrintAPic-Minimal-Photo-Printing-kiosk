using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintAPicStart
{
    public partial class AdminSettingsControl : UserControl
    {
        public string Property { get; set; }
        public string InitialValue { get; set; }
        public AdminSettingsControl()
        {
            InitializeComponent();

        }

        public AdminSettingsControl(string labelText, string ValueText):this()
        {
            Property = labelText;
            InitialValue = ValueText;
            label1.Text = labelText;
            textBox1.Text = ValueText;
        }

        public string GetChangedValue()
        {
            if (InitialValue.Equals(textBox1.Text))
             return null;
            else
             return textBox1.Text;

        }

    }
}
