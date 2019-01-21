using PicsDirectoryDisplayWin.lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.lib_ImgIO
{
    class ImageIO
    {

        int MaxThumbnailsToGenerate = 20;

        public async Task DirectConn_CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir)
        {
            await Wifi_CreateThumbnails(ImageDir);
        }

        //TODO : Not using thumbnails mechanism, use thumbnail, after done, get thumbnails and display.
        public async Task Wifi_CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir)
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
                        i.Save(item.ImageDirName + "\\" + item.ImageName + ".jpg");
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


        public void Wifi_CheckForImages(List<ChitraKiAlbumAurVivaran> AllImages, bool InvokeRequired, string WebSiteSearchDir,
            Form Parentform, Form waiter, Action<ChitraKiAlbumAurVivaran> ReportProgressForImageSearch, 
            Action<bool> Done)
        {
            //AllImages = new List<ChitraKiAlbumAurVivaran>();
            if (InvokeRequired) // this is invoked, coz file system watcher triggers an event which trigger this func.
                                // Probably filesystemwatcher works on another thread.
                Parentform.Invoke(new Action(() => waiter.Show()));
            else
                waiter.Show();

            var progressIndicator = new Progress<ChitraKiAlbumAurVivaran>(ReportProgressForImageSearch);
            lib.ChitraKhoj imgSearch = new ChitraKhoj(WebSiteSearchDir);

            Task waitToComplete = new Task(async () =>
            {
                await imgSearch.Search(progressIndicator, Parentform, InvokeRequired);
                if (InvokeRequired)
                {
                    Parentform.Invoke((Action<bool>)Done, true);
                    return;
                }
                else
                    Done(true);
            });
            waitToComplete.Start();
        }

        public void BubbleSortImages(List<ChitraKiAlbumAurVivaran> AllImages)
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

        }


    }
}
