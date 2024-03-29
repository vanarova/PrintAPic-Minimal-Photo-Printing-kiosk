﻿using PicsDirectoryDisplayWin.lib;
using PicsDirectoryDisplayWin.lib_ImgIO;
using PicsDirectoryDisplayWin.lib_Print;
using PicsDirectoryDisplayWin.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin
{
    public partial class PrintaPic : Form
    {
        private ImageIO imageIO = new ImageIO();
        //private string TestSearchDir = @"C:\Users\Arunav\Pictures\Camera Roll";
        //private string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        private WifiConnectHelp whelp;
        private Waiter waiter;
        int foundImageCount = 0;
        bool searchDone = false;
        int MaxThumbnailsToGenerate = 2; // set this to controls number max thumbnails t genertae and save for each found dir.
        public List<TheImage> AllImages { get; set; }


        public PrintaPic()
        {
            InitializeComponent();
            //this.tableLayoutPanel1.BackgroundImage = GlobalImageCache.TableBgImg;
            this.tableLayoutPanel1.BackColor = Color.FromName(ConfigurationManager.AppSettings["AppBackgndColor"]); ;
            this.tableLayoutPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            WifiConnect.Text = ConfigurationManager.AppSettings["WIFIButton"];
            DirectConnectButton.Text = ConfigurationManager.AppSettings["USBButton"];
            label4.Text = ConfigurationManager.AppSettings["HindiIntro"];
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            if (System.Diagnostics.Debugger.IsAttached)
                (new Info()).ShowDialog();
        }

       

        private void DirectConnectButton_Click(object sender, EventArgs e)
        {
            //clear print queues.
            PrintIO.AbortPrinting();
            imageIO.DeleteAllFilesInDrectoryAndSubDirs(Globals.PrintDir);


            //PickDropGallery pickDropGallery = new PickDropGallery();
            //pickDropGallery.Show();

            //Set default value;
            //Globals.PrintSelection = Globals.PrintSize.A5;
            //Set user selected value.
            new PicSizeSeletion().ShowDialog();

            this.Visible = false;

            USBConnectHelp usbform = new USBConnectHelp();
            usbform.AnimationForm = this;
            usbform.TopMost = true;
            usbform.ShowDialog();

            // Old implemetation of seacrhing images is commented.
            //AllImages = new List<ChitraKiAlbumAurVivaran>();
            //waiter = new Waiter();
            //waiter.Show();
            //var progressIndicator = new Progress<ChitraKiAlbumAurVivaran>(ReportProgressForImageSearch);
            //lib.ChitraKhoj imgSearch = new ChitraKhoj(Globals.USBSearchPath);

            //Task waitToComplete = new Task(async ()=>
            //{
            //    await imgSearch.Search(progressIndicator);
            //    if (InvokeRequired)
            //    {
            //        Invoke((Action<bool>)Done,false);
            //        return;
            //    }
            //});
            //waitToComplete.Start();

            
        }

  
   
        //private void Done(bool IsWeb)
        //{
        //    imageIO.BubbleSortImages(AllImages);
        //    //Bubble sort Images
        //    //for (int write = 0; write < AllImages.Count; write++)
        //    //{
        //    //    for (int sort = 0; sort < AllImages.Count - 1; sort++)
        //    //    {
        //    //        if (AllImages[sort].ImageDirTotalImages > AllImages[sort + 1].ImageDirTotalImages)
        //    //        {
        //    //            var temp = AllImages[sort + 1];
        //    //            AllImages[sort + 1] = AllImages[sort];
        //    //            AllImages[sort] = temp;
        //    //        }
        //    //    }
        //    //}

        //    foreach (var item in AllImages)
        //    {
        //        //Create Thumbnails
        //        Task task = new Task(async () =>
        //        {
        //            await imageIO.DirectConn_CreateThumbnails(item);
        //        });
        //        task.Start();
        //        ReportProgressForThumbnails(item.ImageDirName);
        //    }
            
        //    AllImages.Reverse();
        //    waiter.Close();
        //    if (IsWeb)
        //    {
        //        SimpleGallery gallery = new SimpleGallery();
        //        gallery.AllImages = AllImages;
        //        gallery.Show();
        //    }
        //    else
        //    {
        //        DirectoryGallery gallery = new DirectoryGallery();
        //        gallery.AllImages = AllImages;
        //        gallery.Show();
        //    }
            
        //}

        //TODO: group methods and write comments

               
           
        //    //imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName).GetThumbnailImage(250, 250, null, IntPtr.Zero));
        //}

        private void ReportProgressForImageSearch(TheImage obj)
        {
            foundImageCount = (foundImageCount + obj.ImageDirTotalImages);
            waiter.FileFoundLabelText = foundImageCount.ToString()  + " images found";
            AllImages.Add(obj);
        }

        private void ReportProgressForThumbnails(string dirName)
        {
            //foundImageCount = (foundImageCount + obj.ImageDirTotalImages);
            waiter.FileFoundLabelText = "Creating image thumbnails for : " + dirName;
            //AllImages.Add(obj);
        }

       


        private void WifiConnect_Click(object sender, EventArgs e)
        {
            //clear print queues.
            PrintIO.AbortPrinting();
            imageIO.DeleteAllFilesInDrectoryAndSubDirs(Globals.PrintDir);
            //Set default value;
            //Globals.PrintSelection = Globals.PrintSize.A5;
            //Set user selected value.
            new PicSizeSeletion().ShowDialog();

            this.Visible = false;
            if (whelp == null)
            {
                whelp = new WifiConnectHelp() { AnimationForm = this };
                whelp.ShowDialog();
            }
            else
            {
                whelp.Visible = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Animation_Load(object sender, EventArgs e)
        {
            
            //Clear receipts dir
            try
            {
                imageIO.DeleteAllFilesInDrectoryAndSubDirs(Globals.receiptDir);
                if (imageIO.DoesAnyFileExists(Globals.receiptDir) > 0)
                    imageIO.DeleteAllFilesInDrectoryAndSubDirs(Globals.receiptDir);

            }
            catch (Exception ex)
            {
               // logger.Error(ex.Message);
                //if (System.Diagnostics.Debugger.IsAttached)
                //    MessageBox.Show(ex.Message);
            }
            //Prints directory is overwritten with latest prints pdfs
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Print pf = new Print();
            //pf.Show();
        }

        private void Animation_VisibleChanged(object sender, EventArgs e)
        {
           
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None; this.ControlBox = false;
                return;
            }

            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.ControlBox = true;
                return;
            }
           
        }
    }

}
