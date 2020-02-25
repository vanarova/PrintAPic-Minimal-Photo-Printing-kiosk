using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintAPicStart
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (CheckAlreadyRunningProcess() == true)
                return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        private static bool CheckAlreadyRunningProcess()
        {
            try
            {

                Process[] pa = Process.GetProcessesByName("PrintAPicStart");
                if (pa.Length > 1)
                {
                    MessageBox.Show("Program already running !!", "Warning",MessageBoxButtons.OK);
                    return true;
                }

            }
            catch
            {
                //TODO: #log this error
            }

            return false;
        }
    }
}
