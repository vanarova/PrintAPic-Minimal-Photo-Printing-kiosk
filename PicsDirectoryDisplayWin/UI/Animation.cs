using PicsDirectoryDisplayWin.lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin
{
    public partial class Animation : Form
    {
        Waiter waiter;
        int foundImageCount = 0;
        bool searchDone = false;
        int MaxThumbnailsToGenerate = 100; // set this to controls number max thumbnails t genertae and save for each found dir.
        public List<ChitraKiAlbumAurVivaran> AllImages { get; set; }
        public Animation()
        {
            InitializeComponent();
        }

        private void DirectConnectButton_Click(object sender, EventArgs e)
        {
            //uint deviceCount = 0;
            //PortableDeviceManager manager = new PortableDeviceManager();

            //manager.RefreshDeviceList();


            //manager.GetDevices(null, ref deviceCount)
            //string strVal = string.Empty;
            //string[] deviceIdDescriptionList = new string[deviceCount];
            //manager.GetDevices(ref deviceIdDescriptionList[0], ref deviceCount);
            //if (deviceCount > 0)
            //{
            //    string[] deviceIDs = new string[deviceCount];
            //    manager.GetDevices(ref deviceIDs[0], ref deviceCount);
            //    for (int ndxDevices = 0; ndxDevices < cDevices; ndxDevices++)
            //    {
            //        MessageBox.Show("Device[{0}]: " +
            //                  ndxDevices + 1 + "          " + deviceIDs[0].ToString());
            //        uint nameLength = 30;
            //        ushort[] nameBuffer = new ushort[nameLength];
            //        devMgr.GetDeviceFriendlyName(deviceIDs[0], ref nameBuffer[0], ref nameLength);

            //        //convert to string
            //        string friendlyName = "";
            //        foreach (ushort letter in nameBuffer)
            //            if (letter != 0) friendlyName += (char)letter;
            //        MessageBox.Show("FriendlyName is " + friendlyName);
            //    }






                AllImages = new List<ChitraKiAlbumAurVivaran>();
            waiter = new Waiter();
            waiter.Show();
            var progressIndicator = new Progress<ChitraKiAlbumAurVivaran>(ReportProgressForImageSearch);
            lib.ChitraKhoj imgSearch = new ChitraKhoj();

            Task waitToComplete = new Task(async ()=>
            {
                await imgSearch.Search(progressIndicator);
                if (InvokeRequired)
                {
                    Invoke((Action)Done);
                    return;
                }
            });
            waitToComplete.Start();

            //Chowkidar ramu = new Chowkidar();
            //ramu.IskaamDekhteRahoAurKhatamHonePerSuchitKaro(longRunningWork, "Kaam khatam ho gaya hai");
        }

   
        private void longRunningWork(string value)
        {
            System.Threading.Thread.Sleep(2000);
            MessageBox.Show(value);
            System.Threading.Thread.Sleep(2000);
        }

   
        private void Done()
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
            Gallery gallery = new Gallery();
            gallery.AllImages = AllImages;
            gallery.Show();
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


    }

}
