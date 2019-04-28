using PicsDirectoryDisplayWin.lib;
using PicsDirectoryDisplayWin.lib_ImgIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.UI
{
    public partial class WifiConnectHelp : Form
    {

        public List<ChitraKiAlbumAurVivaran> AllImages { get; set; }
        public Form AnimationForm { get; set; }
        public bool IamAlreadyCalledOnce = false;


        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private FileSystemWatcher fileSystemWatcher1;
        private Waiter waiter = new Waiter();
        private ImageIO imageIO = new ImageIO();
        private int foundImageCount = 0;
        private string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        //int MaxThumbnailsToGenerate = 2; // set this to controls number max thumbnails t genertae and save for each found dir.



        public WifiConnectHelp()
        {
            InitializeComponent();
            //TODO : Memory leak was happeining from pic box, assign images like below, put urls in global file & resources
            fileSystemWatcher1 = new FileSystemWatcher
            {
                Path = WebSiteSearchDir,
                EnableRaisingEvents = true
            };
            //DeleteAllImages();

            pictureBox7.BackgroundImage = GlobalImageCache.ArrowImg;
            pictureBox6.BackgroundImage = GlobalImageCache.ArrowImg;
            tb.BackgroundImage = GlobalImageCache.TableBgImg;
            pictureBox4.Image = GlobalImageCache.wifiStepImg;
            pictureBox3.Image = GlobalImageCache.BrowserStepImg;
            pictureBox2.Image = GlobalImageCache.WifiIconImg;

            label14.Text = ConfigurationManager.AppSettings["ConnectToWIFIText"];
            label2.Text = ConfigurationManager.AppSettings["PasswordNotNeededText"];
            label5.Text = ConfigurationManager.AppSettings["WIFIText"];
            label4.Text = ConfigurationManager.AppSettings["PasswordKiZarooratText"];
            label1.Text = ConfigurationManager.AppSettings["TypeInBrowserText"];
            label6.Text = ConfigurationManager.AppSettings["PrintGoText"];
            label8.Text = ConfigurationManager.AppSettings["BrowserMeinLikhenText"];
            label9.Text = ConfigurationManager.AppSettings["TransferPhotosText"];
            label10.Text = ConfigurationManager.AppSettings["WaitingForPics"];
            label11.Text = ConfigurationManager.AppSettings["PhotosKiPratikchaText"];
            fileSystemWatcher1.Created += FileSystemWatcher1_Changed;
        }



        private void btn_Next_Click(object sender, EventArgs e)
        {
            CheckForImages();
        }


        private void CheckForImages()
        {
            if (IamAlreadyCalledOnce)
                return;
            IamAlreadyCalledOnce = true;
            AllImages = new List<ChitraKiAlbumAurVivaran>();
            imageIO.Wifi_CheckForImages(AllImages, InvokeRequired, WebSiteSearchDir,
                this, waiter, ReportProgressForImageSearch, Done);
        }


        private void Done(bool IsWeb)
        {
            imageIO.BubbleSortImages(AllImages);

            foreach (var item in AllImages)
            {
                //Create Thumbnails
                Task task = new Task(async () => { await imageIO.Wifi_RotateImageIfNeeded_CreateThumbnails(item); });
                task.Start();
                task.Wait();
                //ReportProgressForThumbnails(item.ImageDirName);
            }

            AllImages.Reverse();
            if (InvokeRequired)
                Invoke(new Action(() => waiter.Visible = false));
            else
                waiter.Visible = false;
            if (IsWeb)
            {
                SimpleGallery gallery = new SimpleGallery
                {
                    WifiConnectHelpObject = this,
                    AnimationFormObject = AnimationForm,
                    AllImages = AllImages
                };
                this.Visible = false;
                gallery.Show();

            }
            else
            {
                DirectoryGallery gallery = new DirectoryGallery { AllImages = AllImages };
                this.Visible = false;
                gallery.Show();
            }

        }

        private void ReportProgressForImageSearch(ChitraKiAlbumAurVivaran obj)
        {
            foundImageCount = (foundImageCount + obj.ImageDirTotalImages);
            //TODO : write here invoke required and invoke to display images found count on form
            //if(InvokeRequired)
            //    Invoke(new Action(() => label13.Text = foundImageCount.ToString() + " images found"));
            AllImages.Add(obj);
        }



        //private void ReportProgressForThumbnails(string dirName)
        //{
        //    //foundImageCount = (foundImageCount + obj.ImageDirTotalImages);
        //    //waiter.FileFoundLabelText = "Creating image thumbnails for : " + dirName;
        //    //AllImages.Add(obj);
        //}


        private void FileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            CheckForImages();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WifiConnectHelp_Load_1(object sender, EventArgs e)
        {


            //pictureBox1.Image = GlobalImageCache.HorseAnimImg;

            
            DeleteAllImages();

            //fileSystemWatcher1.Created += FileSystemWatcher1_Changed;
            //fileSystemWatcher1.Deleted += FileSystemWatcher1_Changed;

        }

        public void DeleteAllImages()
        {
            if ((new DirectoryInfo(ConfigurationManager.AppSettings["WebSiteSearchDir"])).GetFiles().Length==0)
            {
                return;
            }
            //delete all pics from previous session. Before events are registed
            try
            {

                //fileSystemWatcher1.Created -= FileSystemWatcher1_Changed;
                //fileSystemWatcher1.Deleted -= FileSystemWatcher1_Changed;

                imageIO.DeleteAllFilesInDrectoryAndSubDirs(ConfigurationManager.AppSettings["WebSiteSearchDir"]);
                if (imageIO.DoesAnyFileExists(ConfigurationManager.AppSettings["WebSiteSearchDir"]) > 0)
                    imageIO.DeleteAllFilesInDrectoryAndSubDirs(ConfigurationManager.AppSettings["WebSiteSearchDir"]);


                //fileSystemWatcher1.Created += FileSystemWatcher1_Changed;
                //fileSystemWatcher1.Deleted += FileSystemWatcher1_Changed;
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
    }
}
