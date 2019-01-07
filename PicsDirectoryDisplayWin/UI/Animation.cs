﻿using PicsDirectoryDisplayWin.lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin
{
    public partial class Animation : Form
    {
        private string TestSearchDir = @"C:\Users\Arunav\Pictures\Camera Roll";
        private string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        private Waiter waiter;
        int foundImageCount = 0;
        bool searchDone = false;
        int MaxThumbnailsToGenerate = 2; // set this to controls number max thumbnails t genertae and save for each found dir.
        public List<ChitraKiAlbumAurVivaran> AllImages { get; set; }
        public Animation()
        {
            InitializeComponent();
        }

        private void DirectConnectButton_Click(object sender, EventArgs e)
        {

            AllImages = new List<ChitraKiAlbumAurVivaran>();
            waiter = new Waiter();
            waiter.Show();
            var progressIndicator = new Progress<ChitraKiAlbumAurVivaran>(ReportProgressForImageSearch);
            lib.ChitraKhoj imgSearch = new ChitraKhoj(TestSearchDir);

            Task waitToComplete = new Task(async ()=>
            {
                await imgSearch.Search(progressIndicator);
                if (InvokeRequired)
                {
                    Invoke((Action<bool>)Done,false);
                    return;
                }
            });
            waitToComplete.Start();

            //Chowkidar ramu = new Chowkidar();
            //ramu.IskaamDekhteRahoAurKhatamHonePerSuchitKaro(longRunningWork, "Kaam khatam ho gaya hai");
        }

  
   
        private void Done(bool IsWeb)
        {
            //Bubble sort Images
            for (int write = 0; write < AllImages.Count; write++)
            {
                for (int sort = 0; sort < AllImages.Count - 1; sort++)
                {
                    if (AllImages[sort].ImageDirTotalImages > AllImages[sort + 1].ImageDirTotalImages)
                    {
                        var temp = AllImages[sort + 1];
                        AllImages[sort + 1] = AllImages[sort];
                        AllImages[sort] = temp;
                    }
                }
            }

            foreach (var item in AllImages)
            {
                //Create Thumbnails
                Task task = new Task(async () =>
                {
                    await CreateThumbnails(item);
                });
                task.Start();
                ReportProgressForThumbnails(item.ImageDirName);
            }
            
            AllImages.Reverse();
            waiter.Close();
            if (IsWeb)
            {
                SimpleGallery gallery = new SimpleGallery();
                gallery.AllImages = AllImages;
                gallery.Show();
            }
            else
            {
                DirectoryGallery gallery = new DirectoryGallery();
                gallery.AllImages = AllImages;
                gallery.Show();
            }
            
        }

        //TODO: group methods and write comments

        private async Task CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir)
        {
            int count = 0;
            await Task.Run(() =>
            {
                //Lets try and create thumbnails
                foreach (var item in ImageDir.PeerImages)
                {
                    if (count >= MaxThumbnailsToGenerate)
                        break;
                    try
                    {
                        Image i = Image.FromFile(item.ImageFullName).GetThumbnailImage(200, 200, null, IntPtr.Zero);
                        if (!Directory.Exists(item.ImageDirName))
                            Directory.CreateDirectory(item.ImageDirName);
                        i.Save(item.ImageDirName+"\\" +  item.ImageName + ".jpg");
                        i.Dispose();
                    }
                    catch (OutOfMemoryException o)
                    {
                        //TODO: Log o
                        System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        //TODO: Log e
                    }
                    count++;
                }
            });
           
           
            //imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName).GetThumbnailImage(250, 250, null, IntPtr.Zero));
        }

        private void ReportProgressForImageSearch(ChitraKiAlbumAurVivaran obj)
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

            AllImages = new List<ChitraKiAlbumAurVivaran>();
            waiter = new Waiter();
            waiter.Show();
            var progressIndicator = new Progress<ChitraKiAlbumAurVivaran>(ReportProgressForImageSearch);
            lib.ChitraKhoj imgSearch = new ChitraKhoj(WebSiteSearchDir);

            Task waitToComplete = new Task(async () =>
            {
                await imgSearch.Search(progressIndicator);
                if (InvokeRequired)
                {
                Invoke((Action<bool>)Done,true);
                    return;
                }
            });
            waitToComplete.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Animation_Load(object sender, EventArgs e)
        {
            WifiConnect.Text = ConfigurationManager.AppSettings["WIFIButton"];
            DirectConnectButton.Text = ConfigurationManager.AppSettings["USBButton"];
            
        }
    }

}