using PicsDirectoryDisplayWin.lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.lib_ImgIO
{
    class ImageIO
    {

        //int MaxThumbnailsToGenerate = 20;

        public async Task DirectConn_CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir)
        {
            await Wifi_CreateThumbnails(ImageDir);
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


        public void CreateImageListFromThumbnails(ChitraKiAlbumAurVivaran obj, ImageList imgs)
        {
            imgs.ImageSize = new Size(200, 200);
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            List<Image> images = new List<Image>();

            foreach (var item in obj.PeerImages)
            {
                using (Image im = Image.FromFile(item.ImageThumbnailFullName))
                {
                    //images.Add();
                    var imtemp = ResizeImage(im, 200, 200);
                    imgs.Images.Add(item.ImageKey, imtemp);
                    //im.Dispose();// = null;
                }
            }
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
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
                    //if (count >= MaxThumbnailsToGenerate)
                    //    break;
                    try
                    {
                        Image i = Image.FromFile(item.ImageFullName).GetThumbnailImage(200, 200, null, IntPtr.Zero);
                        if (!Directory.Exists(item.ImageDirName))
                            Directory.CreateDirectory(item.ImageDirName);
                        //i.Save(item.ImageDirName + "\\" + item.ImageName + ".jpg");
                        i.Save(item.ImageThumbnailFullName);
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
                await imgSearch.Search(progressIndicator, Parentform, InvokeRequired, 0);
                if (AllImages.Count>0)
                {
                     await Wifi_CreateThumbnails(AllImages[0]);
                }
                
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
