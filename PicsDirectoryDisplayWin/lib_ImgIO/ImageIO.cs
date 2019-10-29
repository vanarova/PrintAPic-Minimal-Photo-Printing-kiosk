using ImageMagick;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using PicsDirectoryDisplayWin.lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //int MaxThumbnailsToGenerate = 20;

        public async Task DirectConn_CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir)
        {
            await Wifi_PostProcessImages(ImageDir);
        }

        public static void CheckNCreateDirectory(string DirName)
        {
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
        }


        public static string GetImageDirectoryPath(string imagePath)
        {
            imagePath = imagePath.Replace(Path.GetFileName(imagePath), "");
            imagePath = imagePath.TrimEnd('\\');
            imagePath = imagePath.TrimEnd('\\');
            imagePath = imagePath.TrimEnd('/');
            imagePath = imagePath.TrimEnd('/');
            return imagePath;
        }


        public void DeleteAllFilesInDrectoryAndSubDirs(string path)
        {
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
                File.Delete(file);

        }

        public void DeleteAllNonImageFilesInDrectory(string path)
        {
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside DeleteAllNonImageFilesInDrectory function");
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file.ToLower().Contains(".jpg"))
                    //|| file.ToLower().Contains(".jpeg"))
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

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside CreateImageListFromThumbnails function");
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
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside ResizeImage function");

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

        public async Task Wifi_PostProcessImages(ChitraKiAlbumAurVivaran ImageDir, bool forceCreateAll = false)
        {
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside Wifi_RotateImageIfNeeded_CreateThumbnails function");

            
            int count = 0; Image tempImg = null; Image thmImg = null;
            await Task.Run(() =>
            {
                //ResizeImagesIfNeeded(ImageDir);
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
                        //PostProcessImages(item);



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
                                    thmImg.Save(item.ImageThumbnailFullName, ImageFormat.Jpeg);

                                    //memAvailable = System.GC.GetTotalMemory(false);

                                    imageFactory.ExifPropertyItems.TryGetValue(exifOrientationID, out prop);

                                    //Image orientation is correct, remove orientation key, its not needed.
                                    if (imageFactory.Image.Width > imageFactory.Image.Height && prop != null)
                                    {
                                        PropertyItem tval;
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
                                    //memAvailable = GetMemoryUsage();

                                }
                                // Do something with the stream.
                            }
                            
                        }


                    }

                    catch (OutOfMemoryException o)
                    {
                        //TODO: Log o
                        //System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        //TODO: Log e
                    }
                    finally
                    {

                        if (tempImg != null)
                            tempImg.Dispose();
                        if (thmImg != null)
                            thmImg.Dispose();
                    }
                    count++;
                }
            });


            //imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName).GetThumbnailImage(250, 250, null, IntPtr.Zero));
        }



        private void ResizeImagesIfNeeded(ChitraKiAlbumAurVivaran ImageDir)
        {
            int AssumedSize_10Inch =10;
            //float shorterSide;
            int newWidth;int newHeight;
            //Lets try and create thumbnails
            foreach (var item in ImageDir.PeerImages)
            {
               
                try
                {
                    
                    //int exifOrientationID = 0x112;
                    byte[] photoBytes = File.ReadAllBytes(item.ImageFullName);
                    using (MemoryStream inStream = new MemoryStream(photoBytes))
                    {
                       
                            // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                            using (Image img = Image.FromStream(inStream))
                            {

                                float newImageShorterSide; float ratio;
                                // Load, resize, set the format and quality and save an image.
                                //factoryImage.Load(inStream);
                                //thmImg = imageFactory.Image.GetThumbnailImage(200, 200, null, IntPtr.Zero);


                                //Resize large images to save memory and fasten printing.
                                float actualWidth = img.Width / img.HorizontalResolution;
                                float actualHeight = img.Height / img.VerticalResolution;

                                if (actualHeight < actualWidth && actualHeight > AssumedSize_10Inch)
                                {
                                    //For 72ppi image, this size will be 720 px;
                                    newImageShorterSide = AssumedSize_10Inch * img.VerticalResolution;
                                    ratio = newImageShorterSide / img.Height;
                                    ResizeImageToRatio(img, ratio,item.ImageFullName);
                                    //newWidth = (int)(factoryImage.Image.Width * ratio);
                                    //newHeight = (int)(factoryImage.Image.Height * ratio);
                                    //factoryImage.Resize(new Size(newWidth, newHeight)).Save(item.ImageFullName);
                            }
                                else if(actualWidth < actualHeight && actualWidth > AssumedSize_10Inch)
                                {
                                    newImageShorterSide = AssumedSize_10Inch * img.HorizontalResolution;
                                    ratio = newImageShorterSide / img.Width;
                                    ResizeImageToRatio(img, ratio, item.ImageFullName);
                                //newWidth = (int)(factoryImage.Image.Width * ratio);
                                //newHeight = (int)(factoryImage.Image.Height * ratio);
                                //factoryImage.Resize(new Size(newWidth, newHeight)).Save(item.ImageFullName);
                                }

                            }
                        
                    }
                }
                catch (Exception e) {

                }
                GC.Collect(2, GCCollectionMode.Forced, true, true);
                if (GC.WaitForFullGCComplete() == GCNotificationStatus.Succeeded)
                    continue;
                else
                    System.Threading.Thread.Sleep(100);
            }
        }


        private  void ResizeImageToRatio(Image image,float ratio,string outImagePath)
        {

            var destRect = new Rectangle(0, 0, (int)(image.Width / ratio), (int)(image.Height / ratio));
            var destImage = new Bitmap((int)(image.Width / ratio), (int)(image.Height / ratio));    

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
            destImage.Save(outImagePath, ImageFormat.Jpeg);
            GC.Collect(2, GCCollectionMode.Forced, true, true);
            if (GC.WaitForFullGCComplete() == GCNotificationStatus.Succeeded)
                return;
            else
                System.Threading.Thread.Sleep(100);
        }



        private int GetMemoryUsage()
        {
            PerformanceCounter PC = new PerformanceCounter();
            PC.CategoryName = "Process";
            PC.CounterName = "Working Set - Private";
            PC.InstanceName = "PicsDirectoryDisplayWin";
            return (Convert.ToInt32(PC.NextValue()) / (int)(1024));
        }

        private void PostProcessImages(ChitraKiAlbumAurVivaran img)
        {
            //TODO: #PPFunc: Change image formats & save in "Formatter" dir.
            if(Path.GetExtension(img.ImageName).ToLower() == ".heic")
            {
                Final_ImageCovertHEICToJPG(img.ImageFullName,img.ImagePostProcessedFullName);

            }
            //TODO: #PPFunc: This can be done later ---- lossless Rotate images from Formatter + upload dir, & save in post processed dir.



            //TODO: #PPFunc: Edit images object and change dir names with new postprocessed dir.
            JPGImageRotateToHorizontalAndThumbnailCreate(img.ImageFullName, img.ImageThumbnailFullName, img.ImagePostProcessedFullName);



            //Now create Thumbnails, may be in parent function.
        }


        private void Final_ImageCovertHEICToJPG(string HEICimagePath, string outputImagePath)
        {
            using (MagickImage image = new MagickImage(HEICimagePath))
            {
                string FileName = Path.GetFileNameWithoutExtension(HEICimagePath);
                image.Write(outputImagePath);
            }
        }

        private void JPGImageRotateToHorizontalAndThumbnailCreate(string imagePath,string thumbnailPath, string outputImagePath)
        {
            //string FileName = Path.GetFileNameWithoutExtension(imagePath);
            using (MagickImage im = new MagickImage(imagePath))
            {
                //This reorients image back, if it was rotated by user
                im.AutoOrient();

                //rotates long image to horizontal
                if (im.Width < im.Height)
                {
                    im.Rotate(90);
                    im.Orientation = OrientationType.Undefined;
                }
                //if (!Directory.Exists(GetImageDirectoryPath(outputImagePath)))
                //{
                //    Directory.CreateDirectory(GetImageDirectoryPath(outputImagePath));
                //}
                im.Write(outputImagePath);
                im.Thumbnail(200, 200);
                im.Write(thumbnailPath);
            }

        }


        //public void Final_LossLessImageRotate(string imageToRotatePath, string outputDirectory)
        //{
        //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        //    encoder.FlipHorizontal = false;
        //    encoder.FlipVertical = false;
        //    encoder.QualityLevel = 100;
        //    //Rotate the bitmap clockwise by 90 degrees.
        //    encoder.Rotation = Rotation.Rotate90;
        //    string targetFile = Path.GetFileName(imageToRotatePath);
        //    encoder.Frames.Add(BitmapFrame.Create(new Uri(imageToRotatePath, UriKind.RelativeOrAbsolute)));
        //    FileStream image = new FileStream(outputDirectory + "\\" + targetFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        //    encoder.Save(image);
        //}


        //public async Task Wifi_RotateImageIfNeeded_CreateThumbnails(ChitraKiAlbumAurVivaran ImageDir, bool forceCreateAll = false)
        //{
        //    if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
        //        logger.Log(NLog.LogLevel.Info, "Inside Wifi_RotateImageIfNeeded_CreateThumbnails function");

        //    int count = 0; Image tempImg=null; Image thmImg=null;
        //    await Task.Run(() =>
        //    {
        //    //Lets try and create thumbnails
        //    foreach (var item in ImageDir.PeerImages)
        //    {
        //        if (forceCreateAll == false && Directory.Exists(item.ImageDirName) && File.Exists(item.ImageThumbnailFullName))
        //        {
        //             continue; //thumbnail already exists , skip
        //        }
        //        else if (!Directory.Exists(item.ImageDirName))
        //        {
        //            Directory.CreateDirectory(item.ImageDirName);
        //        }
        //        try
        //        {

        //            int exifOrientationID = 0x112;
        //            byte[] photoBytes = File.ReadAllBytes(item.ImageFullName);
        //            using (MemoryStream inStream = new MemoryStream(photoBytes))
        //            {
        //                using (MemoryStream outStream = new MemoryStream())
        //                {
        //                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
        //                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
        //                    {
        //                        // Load, resize, set the format and quality and save an image.
        //                        imageFactory.Load(inStream);
        //                        thmImg = imageFactory.Image.GetThumbnailImage(200, 200, null, IntPtr.Zero);
        //                        using (var fs = new FileStream(item.ImageThumbnailFullName, FileMode.Create))
        //                        {
        //                           //fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
        //                            thmImg.Save(fs, ImageFormat.Jpeg);    
        //                        }

        //                        PropertyItem prop;
        //                        thmImg.Save(item.ImageThumbnailFullName,ImageFormat.Jpeg);
        //                        imageFactory.ExifPropertyItems.TryGetValue(exifOrientationID,out prop);

        //                        //Image orientation is correct, remove orientation key, its not needed.
        //                        if (imageFactory.Image.Width > imageFactory.Image.Height && prop != null)
        //                        {
        //                            PropertyItem tval ;
        //                            imageFactory.ExifPropertyItems.TryRemove(exifOrientationID, out tval);
        //                            imageFactory.Save(item.ImageFullName);
        //                        }
        //                        //Rotate image, remove orientation key, its not needed.
        //                        if (imageFactory.Image.Width < imageFactory.Image.Height && prop != null)
        //                        {
        //                            imageFactory.Rotate(90);
        //                            PropertyItem tval;
        //                            imageFactory.ExifPropertyItems.TryRemove(exifOrientationID, out tval);
        //                            imageFactory.Save(item.ImageFullName);
        //                        }
        //                        //Rotate image, no orientation key
        //                        else if (imageFactory.Image.Width < imageFactory.Image.Height && prop == null)
        //                        {
        //                            imageFactory.Rotate(90);
        //                            imageFactory.Save(item.ImageFullName);
        //                        }
        //                    }
        //                    // Do something with the stream.
        //                }
        //            }


        //                //}
        //            }
        //            catch (OutOfMemoryException o)
        //            {
        //                //TODO: Log o
        //                //System.Threading.Thread.Sleep(100);
        //            }
        //            catch (Exception e)
        //            {
        //                //TODO: Log e
        //            }finally
        //            {

        //                if (tempImg !=null)
        //                    tempImg.Dispose();
        //                if (thmImg!= null)
        //                    thmImg.Dispose();
        //            }
        //            count++;
        //        }
        //    });


        //    //imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName).GetThumbnailImage(250, 250, null, IntPtr.Zero));
        //}






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

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside Wifi_CheckForImages function");
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
                     await Wifi_PostProcessImages(AllImages[0]);
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
