using ImageMagick;
using ImageProcessor;
using PicsDirectoryDisplayWin.lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.lib_ImgIO
{
    internal class ImageIO
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //int MaxThumbnailsToGenerate = 20;

        public static void CheckNCreateDirectory(string DirName)
        {
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
        }

        //public static string GetImageDirectoryPath(string imagePath)
        //{
        //    imagePath = imagePath.Replace(Path.GetFileName(imagePath), "");
        //    imagePath = imagePath.TrimEnd('\\');
        //    imagePath = imagePath.TrimEnd('\\');
        //    imagePath = imagePath.TrimEnd('/');
        //    imagePath = imagePath.TrimEnd('/');
        //    return imagePath;
        //}

        internal void BubbleSortImages(List<TheImage> AllImages)
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

        internal void CreateImageListFromThumbnails(TheImage obj, ImageList imgs)
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

        internal void DeleteAllFilesInDrectoryAndSubDirs(string path)
        {
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
                File.Delete(file);
        }

        internal void DeleteAllNonImageFilesInDrectory(string path)
        {
            string[] files;
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside DeleteAllNonImageFilesInDrectory function");

            //In pdf scenario, dont only images from top dir, in thumbs, there is pdf icon jpg file.
            if (Globals.PrintSelection == Globals.PrintSize.pdf)
            {
                files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (file.IndexOf(".pdf", StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;
                    File.Delete(file);
                }
            }
            else
            {
                files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (file.IndexOf(".jpg", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        file.IndexOf(".jpeg", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        file.IndexOf(".heic", StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;

                    File.Delete(file);
                }
            }
        }

        internal async Task DirectConn_CreateThumbnails(TheImage ImageDir)
        {
            await Wifi_PostProcessImages(ImageDir);
        }

        internal int DoesAnyFileExists(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
        }

        internal Image GetImage(string path)
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

        internal void Wifi_CheckForImages(List<TheImage> AllImages, bool InvokeRequired, string WebSiteSearchDir,
            Form Parentform, Form waiter, Action<TheImage> ReportProgressForImageSearch,
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

            var progressIndicator = new Progress<TheImage>(ReportProgressForImageSearch);
            lib.ImgSearch imgSearch = new ImgSearch(WebSiteSearchDir);

            Task waitToComplete = new Task(async () =>
            {
                await imgSearch.Search(progressIndicator, Parentform, InvokeRequired, 0);
                if (AllImages.Count > 0)
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

        //}
        internal async Task Wifi_PostProcessImages(TheImage ImageDir, bool forceCreateAll = false)
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
                        if (Globals.PrintSelection == Globals.PrintSize.pdf && item.ImageName.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //For PDFs, copy a readymade icon file of pdf.
                            File.Copy(Globals.pdfIcon, item.ImageThumbnailFullName); //replaces thumbnail .jpg ext.
                        }
                        else
                        {
                            ImageCovertHEICToJPG_WriteBackToSameFile(item);
                            (new ImageResizing(item.ImageFullName)).ResizeIfNeeded(item.ImageFullName);
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
                                        if (Globals.PrintSelection == Globals.PrintSize.A5)
                                        {
                                            AdjustImageOrientation_A5(item, exifOrientationID, imageFactory, prop);
                                        }
                                        else if (Globals.PrintSelection == Globals.PrintSize.A4)
                                        {
                                            AdjustImageOrientation_A4(item, exifOrientationID, imageFactory, prop);
                                        }
                                        else if (Globals.PrintSelection == Globals.PrintSize.Passport)
                                        {
                                            //Will work for passport as well
                                            AdjustImageOrientation_A4(item, exifOrientationID, imageFactory, prop);
                                        }
                                        else if (Globals.PrintSelection == Globals.PrintSize.Postcard)
                                        {
                                            //will work for postcard as well
                                            AdjustImageOrientation_A5(item, exifOrientationID, imageFactory, prop);
                                        }

                                        //memAvailable = GetMemoryUsage();
                                    }
                                    // Do something with the stream.
                                }
                            }
                        }
                    }
                    catch (OutOfMemoryException o)
                    {
                        logger.Log(NLog.LogLevel.Error, o.Message + "Inner ex:" + o.InnerException.Message);
                    }
                    catch (Exception e)
                    {
                        //TODO: Log e
                        logger.Log(NLog.LogLevel.Error, e.Message + "Inner ex:" + e.InnerException.Message);
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

        internal static void AdjustImageOrientation_A4(TheImage item, int exifOrientationID, ImageFactory imageFactory, PropertyItem prop)
        {
            //Image orientation is correct, remove orientation key, its not needed.
            if (imageFactory.Image.Width > imageFactory.Image.Height && prop != null)
            {
                imageFactory.Rotate(90);
                PropertyItem tval;
                imageFactory.ExifPropertyItems.TryRemove(exifOrientationID, out tval);
                imageFactory.Save(item.ImageFullName);
            }
            if (imageFactory.Image.Width > imageFactory.Image.Height && prop == null)
            {
                imageFactory.Rotate(90);
                imageFactory.Save(item.ImageFullName);
            }
            //Rotate image, remove orientation key, its not needed.
            if (imageFactory.Image.Width < imageFactory.Image.Height && prop != null)
            {
                //imageFactory.Rotate(90);
                PropertyItem tval;
                imageFactory.ExifPropertyItems.TryRemove(exifOrientationID, out tval);
                imageFactory.Save(item.ImageFullName);
            }
            //Rotate image, no orientation key
            //else if (imageFactory.Image.Width < imageFactory.Image.Height && prop == null)
            //{
            //    //imageFactory.Rotate(90);
            //    imageFactory.Save(item.ImageFullName);
            //}
        }

        private static void AdjustImageOrientation_A5(TheImage item, int exifOrientationID, ImageFactory imageFactory, PropertyItem prop)
        {
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
        }

        private void ImageCovertHEICToJPG_WriteBackToSameFile(TheImage HEICimagePath)
        {
            if (!Path.GetExtension(HEICimagePath.ImageFullName).ToLower().Contains("heic"))
                return;
            byte[] photoBytes = File.ReadAllBytes(HEICimagePath.ImageFullName);
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MagickImage image = new MagickImage(inStream, MagickFormat.Heic))
                {
                    //string FileName = Path.GetFileNameWithoutExtension(HEICimagePath.ImageFullName);
                    image.Write(HEICimagePath.ImageFullName, MagickFormat.Jpg);
                    string prevName = HEICimagePath.ImageFullName;
                    //Now change name in collection
                    HEICimagePath.ImageFullName = Path.ChangeExtension(HEICimagePath.ImageFullName, ".jpg");
                    HEICimagePath.ImageName = Path.GetFileName(HEICimagePath.ImageFullName);
                    //Rename physical file.
                    File.Move(prevName, HEICimagePath.ImageFullName);
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
    }
}