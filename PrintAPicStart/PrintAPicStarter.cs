using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PrintAPicStart
{
    public partial class Main : Form
    {

        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref tagLASTINPUTINFO plii);
        Int32 IdleTime;
        public struct tagLASTINPUTINFO
        {
            public uint cbSize;
            public Int32 dwTime;
        }

        Process process;
        Timer timer;
        //UserSettings uSettings;
        public Main()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
            timer.Interval = 1000;


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (process != null && process.HasExited)
                process.Start();

            string timeout = ConfigurationManager.AppSettings["TimeOut"];
            ///Idle time detection
            tagLASTINPUTINFO LastInput = new tagLASTINPUTINFO();

            LastInput.cbSize = (uint)Marshal.SizeOf(LastInput);
            LastInput.dwTime = 0;

            if (GetLastInputInfo(ref LastInput))
            {
                IdleTime = System.Environment.TickCount - LastInput.dwTime;
                IdleTimelbl.Text = IdleTime / 1000 + " sec";
                if ((IdleTime / 1000) != 0  && (IdleTime / 1000)% Convert.ToInt16(timeout) == 0)
                {
                    RestartProcess();
                }
            }
        }

        private void StartStop_Button_Click(object sender, EventArgs e)
        {
            timer.Start();
            //PicsDirectoryDisplayWin.Animation ani = new PicsDirectoryDisplayWin.Animation();
            //ani.Show();
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //string FileName = "../../../PicsDirectoryDisplayWin/bin/Release/PicsDirectoryDisplayWin.exe";
            string FileName = "PicsDirectoryDisplayWin.exe";
            if (System.IO.File.Exists(FileName))
            {
                System.IO.FileInfo finfo = new System.IO.FileInfo(FileName);
                //startInfo.Arguments = file;
                process = Process.Start(finfo.FullName);

                FreezeUI();
            }
        }



        private void FreezeUI()
        {
            foreach (Control item in this.Controls)
            {
                Start_Button.Enabled = false;
                CloseExit_button.Enabled = false;
            }
        }

        private void DeFreezeUI()
        {
            Start_Button.Enabled = true;
            CloseExit_button.Enabled = true;
        }

     

        private void main_Load(object sender, EventArgs e)
        {
            LicenseCheck();

            //uSettings = Helper.DeserializeText();

            //First time run only.
            //if (uSettings == null)
            //{
            //    uSettings = new UserSettings();
            //    Helper.SerializeText(uSettings);
            //}


            //freguency_comboBox.SelectedIndex = 3;

            //freguency_comboBox.SelectedItem = userSettingsBindingSource.
            //userSettingsBindingSource.DataSource = uSettings;

        }

        private void LicenseCheck()
        {
            string error = "";
            bool isvalid = Licensing.validateLicense(out error);
            Start_Button.Enabled = isvalid;
            Stop_Button.Enabled = isvalid;
            if (isvalid)
            {
                button1.BackColor = System.Drawing.Color.LightGreen;
                button1.Text = "Registered";
            }
            else
            {
                button1.BackColor = System.Drawing.Color.LightSalmon;
                button1.Text = "Register";
            }
        }

        private void main_notifyIcon_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                this.Show();
            }

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
                e.Cancel = true;
            }

        }

        private void CloseExit_button_Click(object sender, EventArgs e)
        {
            StopProcess();
            if (this.Visible)
            {
                this.Visible = false;
                this.Close();
            }
            Application.Exit();
            //TODO: Ask to delete logs, Delete all logs as well.
        }

        private void hide_button_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
            }
        }

        private void Stop_Button_Click(object sender, EventArgs e)
        {
            StopProcess();
        }

        public void DeleteAllFilesInDrectoryAndSubDirs(string path)
        {
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
                File.Delete(file);

        }

        public int DoesAnyFileExists(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;

        }

        private void DeleteAllImgs()
        {
            
            //NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                //ImageIO imgs = new ImageIO();
                DeleteAllFilesInDrectoryAndSubDirs(ConfigurationManager.AppSettings["WebSiteSearchDir"]);
                if (DoesAnyFileExists(ConfigurationManager.AppSettings["WebSiteSearchDir"]) > 0)
                    DeleteAllFilesInDrectoryAndSubDirs(ConfigurationManager.AppSettings["WebSiteSearchDir"]);

            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                //if (System.Diagnostics.Debugger.IsAttached)
                //    MessageBox.Show(ex.Message);
            }
        }


        private void RestartProcess()
        {
            //timer.Stop();
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
            System.Threading.Thread.Sleep(1000);
            try
            {
                Process[] pa = Process.GetProcessesByName("PicsDirectoryDisplayWin");
                if (pa.Length != 0)
                {
                    for (int i = 0; i < pa.Length; i++)
                    {
                        pa[i].Kill();
                    }
                }
            }
            catch
            {
                //TODO: #log this error
            }
            finally
            {
                DeleteAllImgs();
            }

            //Process.GetProcessById(matlabProcess.Id).Kill();
            //Start_Button.Text = "Start scheduler";
            //DeFreezeUI();
        }



        private void StopProcess()
        {
            timer.Stop();
            if (process != null && !process.HasExited)
            {
                process.Kill();
                
            }

            System.Threading.Thread.Sleep(1000);

            try
            {

                Process[] pa = Process.GetProcessesByName("PicsDirectoryDisplayWin");
                if (pa.Length != 0)
                {
                    for (int i = 0; i < pa.Length; i++)
                    {
                        pa[i].Kill();
                    }
                }

            }
            catch
            {
                //TODO: #log this error
            }finally
            {
                DeleteAllImgs();
            }

            //Process.GetProcessById(matlabProcess.Id).Kill();
            //Start_Button.Text = "Start scheduler";
            DeFreezeUI();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            reg.ShowDialog();
            LicenseCheck();
        }

       
    }
}
