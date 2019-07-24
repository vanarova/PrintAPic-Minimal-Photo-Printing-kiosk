using ImageProcessor;
using ImageProcessor.Imaging.Formats;
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
            await Wifi_RotateImageIfNeeded_CreateThumbnails(ImageDir);
        }

        public void DeleteAllFilesInDrectoryAndSubDirs(string path)
        {
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
                File.Delete(file);

        }

        public void DeleteAllNonImageFilesInDrectory(string path)
        {
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file.ToLower().Contains(".jpg") || file.ToLower().Contains(".jpeg"))
                    continue;

                File.Delete(file);
            }
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
                if (File.Exists(item.ImageThumbnailFullName))
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



        //public Image GetImageWithImageProcessorLib(string path)
        //{
        //    byte[] photoBytes = File.ReadAllBytes(path);
        //    // Format is automatically detected though can be changed.
        //    ISupportedImageFormat format = new JpegFormat { Quality = 70 };
        //    Size size = new Size(150, 0);
        //    using (MemoryStream inStream = new MemoryStream(photoBytes))
        //                {
        //                    using (MemoryStream outStream = new MemoryStream())
        //                    {
        //                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
        //                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
        //                        {
        //                            // Load, resize, set the format and quality and save an image.
        //                            imageFactory.Load(inStream)
        //                                        .Resize(size)
        //                                        .Format(format)
        //                                        .Save(outStream);
        //                        }
        //                        // Do something with the stream.
        //                    }
        //                }

        //}



        public void RotateImageWithImageProcessorLib(string path)
        {
            int exifOrientationID = 0x112;
            byte[] photoBytes = File.ReadAllBytes(path);
            // Format is automatically detected though can be changed.
            //ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            //Size size = new Size(150, 0);
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                       imageFactory.Load(inStream);
                        var prop = imageFactory.ExifPropertyItems[exifOrientationID];
                        //remove key if width is greater then height
                        //imageFactory.ExifPropertyItems.TryRemove()
                        //insert a rotate key if width is smaller then height
                        //imageFactory.ExifPropertyItems.TryUpdate()
                        int val = BitConverter.ToUInt16(prop.Value, 0);

                        var rot = RotateFlipType.RotateNoneFlipNone;

                        if (val == 3 || val == 4)
                            rot = RotateFlipType.Rotate180FlipNone;
                        else if (val == 5 || val == 6)
                            rot = RotateFlipType.Rotate90FlipNone;
                        else if (val == 7 || val == 8)
                            rot = RotateFlipType.Rotate270FlipNone;

                        if (val == 2 || val == 4 || val == 5 || val == 7)
                            rot |= RotateFlipType.RotateNoneFlipX;

                        //if (rot != RotateFlipType.RotateNoneFlipNone)
                            //img.RotateFlip(rot);


                    }
                    // Do something with the stream.
                }
            }

        }


        public Image GetImage(string path)
        {
            FileStream fread = null;
            try
            {
                using (fread = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.None))
                    return Image.FromStream(fread);
            }
            finally
            {
                if (fread != null)
                {
                    fread.Close(); fread.Dispose();
                }

            }

        }


        public async Task Wifi_RotateImageIfNeeded_CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir, bool forceCreateAll = false)
        {
            int count = 0; Image tempImg=null; Image thmImg=null;
            await Task.Run(() =>
            {
            //Lets try and create thumbnails
            foreach (var item in ImageDir.PeerImages)
            {
                if (forceCreateAll == false && Directory.Exists(item.ImageDirName) && File.Exists(item.ImageThumbnailFullName))
                {
                     continue; //thumbnail already exists , skip
                }
                else if (!Directory.Exists(item.ImageDirName))
                {
                    Directory.CreateDirectory(item.ImageDirName);
                }
                try
                {

                    int exifOrientationID = 0x112;
                    byte[] photoBytes = File.ReadAllBytes(item.ImageFullName);
                    using (MemoryStream inStream = new MemoryStream(photoBytes))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                // Load, resize, set the format and quality and save an image.
                                imageFactory.Load(inStream);
                                thmImg = imageFactory.Image.GetThumbnailImage(200, 200, null, IntPtr.Zero);
                                using (var fs = new FileStream(item.ImageThumbnailFullName, FileMode.Create))
                                {
                                   //fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                                    thmImg.Save(fs, ImageFormat.Jpeg);    
                                }
                               
                                PropertyItem prop;
                                thmImg.Save(item.ImageThumbnailFullName,ImageFormat.Jpeg);
                                imageFactory.ExifPropertyItems.TryGetValue(exifOrientationID,out prop);

                                //Image orientation is correct, remove orientation key, its not needed.
                                if (imageFactory.Image.Width > imageFactory.Image.Height && prop != null)
                                {
                                    PropertyItem tval ;
                                    imageFactory.ExifPropertyItems.TryRemove(exifOrientationID, out tval);
                                    imageFactory.Save(item.ImageFullName);
                                }
                                //Rotate image, remove orientation key, its not needed.
                                if (imageFactory.Image.Width < imageFactory.Image.Height && prop != null)
                                {
                                    imageFactory.Rotate(90);
                                    PropertyItem tval;
                                    imageFactory.ExifPropertyItems.TryRemove(exifOrientationID, out tval);
                                    imageFactory.Save(item.ImageFullName);
                                }
                                //Rotate image, no orientation key
                                else if (imageFactory.Image.Width < imageFactory.Image.Height && prop == null)
                                {
                                    imageFactory.Rotate(90);
                                    imageFactory.Save(item.ImageFullName);
                                }
                            }
                            // Do something with the stream.
                        }
                    }




                        
                        //}
                    }
                    catch (OutOfMemoryException o)
                    {
                        //TODO: Log o
                        //System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        //TODO: Log e
                    }finally
                    {
                        
                        if (tempImg !=null)
                            tempImg.Dispose();
                        if (thmImg!= null)
                            thmImg.Dispose();
                    }
                    count++;
                }
            });


            //imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName).GetThumbnailImage(250, 250, null, IntPtr.Zero));
        }






        //public async Task Wifi_CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir, bool forceCreateAll = false)
        //{
        //    int count = 0; Image tempImg = null; Image thmImg = null;
        //    await Task.Run(() =>
        //    {
        //        //Lets try and create thumbnails
        //        foreach (var item in ImageDir.PeerImages)
        //        {
        //            if (forceCreateAll == false && Directory.Exists(item.ImageDirName) && File.Exists(item.ImageThumbnailFullName))
        //            {
        //                continue; //thumbnail already exists , skip
        //            }
        //            else if (!Directory.Exists(item.ImageDirName))
        //            {
        //                Directory.CreateDirectory(item.ImageDirName);
        //            }
        //            try
        //            {
        //                using (tempImg = GetImage(item.ImageFullName))
        //                {
        //                    RotateImageWithImageProcessorLib(item.ImageFullName);
        //                    thmImg = tempImg.GetThumbnailImage(200, 200, null, IntPtr.Zero);
        //                    //if (!Directory.Exists(item.ImageDirName))
        //                    //    Directory.CreateDirectory(item.ImageDirName);
        //                    //i.Save(item.ImageDirName + "\\" + item.ImageName + ".jpg");
        //                    thmImg.Save(item.ImageThumbnailFullName);
        //                }
        //            }
        //            catch (OutOfMemoryException o)
        //            {
        //                //TODO: Log o
        //                //System.Threading.Thread.Sleep(100);
        //            }
        //            catch (Exception e)
        //            {
        //                //TODO: Log e
        //            }
        //            finally
        //            {
        //                if (tempImg != null)
        //                    tempImg.Dispose();
        //                if (thmImg != null)
        //                    thmImg.Dispose();
        //            }
        //            count++;
        //        }
        //    });


        //    //imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName).GetThumbnailImage(250, 250, null, IntPtr.Zero));
        //}




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
                     await Wifi_RotateImageIfNeeded_CreateThumbnails(AllImages[0]);
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
