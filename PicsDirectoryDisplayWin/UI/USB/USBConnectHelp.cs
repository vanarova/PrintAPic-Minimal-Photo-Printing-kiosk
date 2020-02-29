using Dolinay;
using PicsDirectoryDisplayWin.lib;
using PicsDirectoryDisplayWin.lib_ImgIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.UI
{
    public partial class USBConnectHelp : Form
    {
       
        public List<TheImage> AllImages { get; set; }
        public Form AnimationForm { get; set; }
        //public bool IamAlreadyCalledOnce = false;


        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //private FileSystemWatcher fileSystemWatcher1;
        //private Waiter waiter = new Waiter();
        private ImageIO imageIO = new ImageIO();
        //private int foundImageCount = 0;
        //private string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        //int MaxThumbnailsToGenerate = 2; // set this to controls number max thumbnails t genertae and save for each found dir.

        private DriveDetector driveDetector = null;

        public USBConnectHelp()
        {
            InitializeComponent();
            //TODO : Memory leak was happeining from pic box, assign images like below, put urls in global file & resources

            driveDetector = new DriveDetector();
            driveDetector.DeviceArrived += new DriveDetectorEventHandler(OnDriveArrived);
            driveDetector.DeviceRemoved += new DriveDetectorEventHandler(OnDriveRemoved);
            driveDetector.QueryRemove += new DriveDetectorEventHandler(OnQueryRemove);


            //pictureBox7.BackgroundImage = GlobalImageCache.ArrowImg;
            pictureBox6.BackgroundImage = GlobalImageCache.ArrowImg;
            //tb.BackgroundImage = GlobalImageCache.TableBgImg;
            //pictureBox4.Image = GlobalImageCache.wifiStepImg;
            pictureBox3.Image = GlobalImageCache.BrowserStepImg;
            pictureBox2.Image = GlobalImageCache.WifiIconImg;

            //label14.Text = ConfigurationManager.AppSettings["ConnectToWIFIText"];
            label2.Text = ConfigurationManager.AppSettings["ConnectToUSBTextHindi_P1"];
            //label5.Text = ConfigurationManager.AppSettings["WIFIText"];
            //label4.Text = ConfigurationManager.AppSettings["PasswordKiZarooratText"];
            label1.Text = ConfigurationManager.AppSettings["ConnectToUSBTextEng"];
            //label6.Text = ConfigurationManager.AppSettings["PrintGoText"];
            label8.Text = ConfigurationManager.AppSettings["ConnectToUSBTextHindi_P2"];
            //label9.Text = ConfigurationManager.AppSettings["TransferPhotosText"];
            label10.Text = ConfigurationManager.AppSettings["WaitingForUSBEng"]; 
            label11.Text = ConfigurationManager.AppSettings["WaitingForUSBHindi"];
            tb.BackColor = Color.FromName(ConfigurationManager.AppSettings["AppBackgndColor"]);

            //fullscreen
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }


        // Called by DriveDetector when removable device in inserted 
        private void OnDriveArrived(object sender, DriveDetectorEventArgs e)
        {
            // Report the event in the listbox.
            // e.Drive is the drive letter for the device which just arrived, e.g. "E:\\"
            //string s = "Drive arrived " + e.Drive;
            ShowGallery(e.Drive);

            //MessageBox.Show(s);
            //listBox1.Items.Add(s);

            // If you want to be notified when drive is being removed (and be able to cancel it), 
            // set HookQueryRemove to true 
            //if (checkBoxAskMe.Checked)
            //    e.HookQueryRemove = true;
        }

        private void ShowGallery(string e)
        {
            this.Visible = false;
            SimpleGallery simpleGallery = new SimpleGallery(true);
            simpleGallery.AllImages = new List<TheImage>();
            simpleGallery.USBDriveLetter = e;
            simpleGallery.AnimationFormObject = AnimationForm;
            simpleGallery.Show();
        }

        // Called by DriveDetector after removable device has been unpluged 
        private void OnDriveRemoved(object sender, DriveDetectorEventArgs e)
        {
            // TODO: do clean up here, etc. Letter of the removed drive is in e.Drive;
            
            // Just add report to the listbox
            //string s = "Drive removed " + e.Drive;
            //MessageBox.Show(s);
            //listBox1.Items.Add(s);
        }

        // Called by DriveDetector when removable drive is about to be removed
        private void OnQueryRemove(object sender, DriveDetectorEventArgs e)
        {
            // Should we allow the drive to be unplugged?
            //if (checkBoxAskMe.Checked)
            //{
            //    if (MessageBox.Show("Allow remove?", "Query remove",
            //        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //        e.Cancel = false;       // Allow removal
            //    else
            //        e.Cancel = true;        // Cancel the removal of the device  
            //}
        }



        private void btn_Next_Click(object sender, EventArgs e)
        {
           
        }

              


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WifiConnectHelp_Load_1(object sender, EventArgs e)
        {
            
            DeleteAllImages();

        }

        public void DeleteAllImages()
        {
            //delete all pics from previous session. Before events are registed
            try
            {
                              

                imageIO.DeleteAllFilesInDrectoryAndSubDirs(ConfigurationManager.AppSettings["WebSiteSearchDir"]);
                if (imageIO.DoesAnyFileExists(ConfigurationManager.AppSettings["WebSiteSearchDir"]) > 0)
                    imageIO.DeleteAllFilesInDrectoryAndSubDirs(ConfigurationManager.AppSettings["WebSiteSearchDir"]);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //if (System.Diagnostics.Debugger.IsAttached)
                //    MessageBox.Show(ex.Message);
            }

        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            DeleteAllImages();
            AnimationForm.Visible = true;
            this.Visible = false;

        }

        private void pictureBox4_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void Ok_button_Click(object sender, EventArgs e)
        {
            string d1 = ConfigurationManager.AppSettings["DriveLetter1"];
            string d2 = ConfigurationManager.AppSettings["DriveLetter2"];
            string d3 = ConfigurationManager.AppSettings["DriveLetter3"];
            string d4 = ConfigurationManager.AppSettings["DriveLetter4"];
            string d5 = ConfigurationManager.AppSettings["DriveLetter5"];
            string d6 = ConfigurationManager.AppSettings["DriveLetter6"];
            //MessageBox.Show(DriveInfo.GetDrives());
            if (DriveInfo.GetDrives().Length > Convert.ToInt32(ConfigurationManager.AppSettings["LocalDrivesCount"]))
            {
                string driveletter = "";
                foreach (var item in DriveInfo.GetDrives())
                {
                    if(!(item.Name == d1 ||
                        item.Name == d2 ||
                        item.Name == d3 ||
                        item.Name == d4 ||
                        item.Name == d5 ||
                        item.Name == d6 ))
                    {
                        driveletter = item.Name;
                        break;
                    }
                    
                }
                if (!string.IsNullOrEmpty(driveletter))
                {
                    ShowGallery(driveletter);
                    warningLbl.Visible = false;
                }
                else
                {
                    throw (new Exception("No drive letter detected, but count of drive excedes mentioned drive count"));
                }

            }
            else
            {
                warningLbl.Visible = true;
            }
        }
    }
}
