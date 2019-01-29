using PicsDirectoryDisplayWin.lib;
using PicsDirectoryDisplayWin.lib_ImgIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.UI
{
    public partial class WifiConnectHelp : Form
    {

        public List<ChitraKiAlbumAurVivaran> AllImages { get; set; }
        private Waiter waiter = new Waiter();
        private ImageIO imageIO = new ImageIO();
        private int foundImageCount = 0;
        private string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        //int MaxThumbnailsToGenerate = 2; // set this to controls number max thumbnails t genertae and save for each found dir.
        private bool IamAlreadyCalledOnce = false;
        

        public WifiConnectHelp()
        {
            InitializeComponent();
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
                Task task = new Task(async () => { await imageIO.Wifi_CreateThumbnails(item); });
                task.Start();
                task.Wait();
                //ReportProgressForThumbnails(item.ImageDirName);
            }

            AllImages.Reverse();
            if (InvokeRequired)
                Invoke(new Action(() => waiter.Close()));
            else
                waiter.Close();
            if (IsWeb)
            {
                SimpleGallery gallery = new SimpleGallery {   AllImages = AllImages };
                gallery.Show();
            }
            else
            {
                DirectoryGallery gallery = new DirectoryGallery { AllImages = AllImages };
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
            FileSystemWatcher fileSystemWatcher1 = new FileSystemWatcher
            {
                Path = WebSiteSearchDir,
                EnableRaisingEvents = true
            };
            //delete all pics from previous session. Before events are registed
            try
            {
                imageIO.DeleteAllFilesInDrectoryAndSubDirs(Globals.WebSiteSearchDir);
                if (imageIO.DoesAnyFileExists(Globals.WebSiteSearchDir) > 0)
                    imageIO.DeleteAllFilesInDrectoryAndSubDirs(Globals.WebSiteSearchDir);
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    MessageBox.Show(ex.Message);
            }
           
                

            fileSystemWatcher1.Created += FileSystemWatcher1_Changed;
            fileSystemWatcher1.Deleted += FileSystemWatcher1_Changed;
           
        }
    }
}
