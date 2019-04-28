using PicsDirectoryDisplayWin.lib;
using PicsDirectoryDisplayWin.lib_ImgIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin
{
    /// <summary>
    /// this class shows 2 pages, 1 is gallery shown after images are 
    /// searched from mobile, 2 is selected images which is a subset of all images available
    /// </summary>
    public partial class SimpleGallery : Form
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public List<ChitraKiAlbumAurVivaran> AllImages { get; set; }
        public Form WifiConnectHelpObject { get; set; }
        public Form AnimationFormObject { get; set; }
        public List<string> SelectedImageKeys { get; set; }

        public bool IS_USBConnection = false;
        public string USBDriveLetter;
        public bool FilesChanged
        {
            set
            {
                if (fileChangedNotifer != null)
                    fileChangedNotifer(this, new EventArgs());
            }
        }

        public bool RefreshGalleryNotify
        {
            set
            {
                if (fileChangedNotifer != null)
                    refreshGalleryNotifier(this, new EventArgs());
            }
        }


        private int foundImageCount = 0;
        private Timer timer;
        //TODO : Remove this timer, make a queue to process multiple requests, process only, first and last request.
        private int RefreshResponseDelay = 1000; //milisec
        private string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        private Waiter waiter = new Waiter();
        private bool SelectionChanged = false;
        private readonly ImageIO imageIO = new ImageIO();
        //gallery preview listview
        private ListView galleryPreview;
        private UI.Print print;
        private bool OnGalleryPreviewPage = false;
        private readonly string CheckSymbol;
        private readonly Color SelectedColor = Color.Silver;
        private readonly Font SelectedFont = new Font(new Font("Arial", 10.0f), FontStyle.Bold);
        private readonly Font UnSelectedFont = new Font(new Font("Arial", 8.0f), FontStyle.Regular);
       // private int fileChangedCounter = 0;
        private event EventHandler fileChangedNotifer;
        private event EventHandler refreshGalleryNotifier;
     
        public SimpleGallery()
        {
            InitializeComponent();
            fileChangedNotifer += SimpleGallery_fileChangedNotifer;
            refreshGalleryNotifier += SimpleGallery_refreshGalleryNotifier;
            imglist.MultiSelect = false;
            //imglist.View = View.LargeIcon;
            imglist.LabelWrap = true;
            imglist.Font = UnSelectedFont;
            imglist.ItemSelectionChanged += Imglist_ItemSelectionChanged;
            imglist.Click += Imglist_Click;
            SelectedImageKeys = new List<string>();
            //UploadButton.Visible = IS_USBConnection;
            string checkUnicode = "2714"; // ballot box -1F5F9
            int value = int.Parse(checkUnicode, System.Globalization.NumberStyles.HexNumber);
            CheckSymbol = char.ConvertFromUtf32(value).ToString();

            UploadButton.Text = ConfigurationManager.AppSettings["UploadButton"];
            btn_Next.Text = ConfigurationManager.AppSettings["NextButton"];
            btn_Back.Text = ConfigurationManager.AppSettings["BackButton"];
            label6.Text = ConfigurationManager.AppSettings["BillInfo"];
            label12.Text = ConfigurationManager.AppSettings["PrintSizeText"];
            label11.Text = ConfigurationManager.AppSettings["PrintSizeValue"];
            label4.Text = ConfigurationManager.AppSettings["CostText"] ;
            label8.Text = ConfigurationManager.AppSettings["CostValue"]; 
            label5.Text = ConfigurationManager.AppSettings["NoOfPicsText"];
            label_PicsCount.Text = ConfigurationManager.AppSettings["NoOfPicsInitialValue"];
            label10.Text = ConfigurationManager.AppSettings["AmountText"];
            label9.Text = ConfigurationManager.AppSettings["AmountInitialValue"];
            label2.Text = ConfigurationManager.AppSettings["GSTText"];
            label1.Text = ConfigurationManager.AppSettings["GSTValue"];
            label14.Text = ConfigurationManager.AppSettings["TotalText"];
            label13.Text = ConfigurationManager.AppSettings["TotalValue"];
            warningTxt.Text = ConfigurationManager.AppSettings["WarningText"];

            UploadButton.Visible = IS_USBConnection;
            tb.BackgroundImage = GlobalImageCache.TableBgImg;
            tb.BackgroundImageLayout = ImageLayout.Stretch;
            
        }

        private void SimpleGallery_refreshGalleryNotifier(object sender, EventArgs e)
        {
            if (AllImages.Count>0)
            {
                ShowGallerySelectionImages(AllImages[0]);
            }
            
        }

        private void SimpleGallery_fileChangedNotifer(object sender, EventArgs e)
        {
            if (InvokeRequired && System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show("file watcher, Invoke needed");
            }
            AllImages = new List<ChitraKiAlbumAurVivaran>();
            waiter = new Waiter();
            imageIO.Wifi_CheckForImages(AllImages, InvokeRequired, WebSiteSearchDir,
                this, waiter, ReportProgress, Done);
        }

        private void PrepareFormForGalleryPreview()
        {
            btn_Next.Text = "Done";
            OnGalleryPreviewPage = true;
            //folder_list.Visible = false;
            imglist.Visible = false;
            galleryPreview = new ListView();
            galleryPreview.Dock = DockStyle.Fill;
            tb.Controls.Add(galleryPreview,1,0);
            galleryPreview.Enabled = false;
        }


        private void PrepareFormForGallerySelection()
        {
            btn_Next.Text = "Next";
            OnGalleryPreviewPage = false;
            //folder_list.Visible = true;
            imglist.Visible = true;
            galleryPreview.Visible = false;
          
        }

        private void Imglist_Click(object sender, EventArgs e)
        {
            if (!SelectionChanged)
            {
                ListViewItem item = imglist.SelectedItems[0];
                if (item != null)
                    UnSelectImage(item);
            }
            SelectionChanged = false;
        
        }

        private void Imglist_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            SelectionChanged = true;
            if (e.IsSelected)
            {
                if (!SelectedImageKeys.Contains((e.Item).ImageKey))
                {
                    SelectImage(e.Item);
                }
                else if (SelectedImageKeys.Contains(e.Item.ImageKey))
                {
                    UnSelectImage(e.Item);
                }
            }

        }

        private void UnSelectImage(ListViewItem item)
        {
            if (item.Checked)
            {
                SelectedImageKeys.Remove(item.ImageKey);

                item.Checked = false;
                item.BackColor = Color.White;
                item.Focused = false;
                //string copyrightUnicode = "2714"; // ballot box -1F5F9
                //int value = int.Parse(copyrightUnicode, System.Globalization.NumberStyles.HexNumber);
                //string symbol = char.ConvertFromUtf32(value).ToString();
                item.Font = UnSelectedFont;
                item.Text = item.Text.Replace("[" + CheckSymbol + "] ", "");
                UpdateBillDetails(SelectedImageKeys.Count);
            }
           

        }

        private void SelectImage(ListViewItem item)
        {
            if (item.Checked ==false)
            {
                SelectedImageKeys.Add(item.ImageKey);
                SelectedImageChecked(item);
                UpdateBillDetails(SelectedImageKeys.Count);
            }
            //item.Bounds.Inflate(60, 60);
            //item.ForeColor = Color.White;

        }

        private void SelectedImageChecked(ListViewItem item)
        {
            item.Checked = true;
            item.BackColor = SelectedColor;
            item.Focused = true;
            //string copyrightUnicode = "2714"; // ballot box -1F5F9
            //int value = int.Parse(copyrightUnicode, System.Globalization.NumberStyles.HexNumber);
            //string symbol = char.ConvertFromUtf32(value).ToString();
            item.Font = SelectedFont;
            item.Text = "[" + CheckSymbol + "] " + item.Text;
        }

        private void SimpleGallery_Load(object sender, EventArgs e)
        {
            //UploadButton.Visible = IS_USBConnection;
            //tb.BackgroundImage = GlobalImageCache.TableBgImg;
            //tb.BackgroundImageLayout = ImageLayout.Stretch;
            //TODO : fix, below line, all images [1] is wrong, it shud only detect images and not go in subdirectory
            if (AllImages != null && AllImages.Count > 0)
                ShowGallerySelectionImages(AllImages[0]);

            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += Timer_Tick;
            timer.Start();
            //FileSystemWatcher WebSiteUploadsWatcher = new FileSystemWatcher
            //{
            //    Path = WebSiteSearchDir,
            //    EnableRaisingEvents = true
            //};
            ////fileSystemWatcher1.Changed += FileSystemWatcher1_Changed;
            //WebSiteUploadsWatcher.Created += WebSiteUploadsWatcher_Changed;
            //WebSiteUploadsWatcher.Deleted += WebSiteUploadsWatcher_Changed;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           int FilesInWebSearchDir = new DirectoryInfo(WebSiteSearchDir).GetFiles().Length;
           int FilesInThumbsDir = new DirectoryInfo(ConfigurationManager.AppSettings["WebSiteSearchDir"] + "\\thumbs").GetFiles().Length;
            bool isloading = false;

           


            if (AllImages != null && AllImages.Count>0 &&
                AllImages[0].PeerImages.Count != FilesInWebSearchDir && AllImages[0].PeerImages.Count < 20)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { FilesChanged = true; }));
                    isloading = true;
                }
                else
                {
                    FilesChanged = true;
                    isloading = true;
                }
                return;
            }

            if (imglist.LargeImageList != null && imglist.Items.Count != imglist.LargeImageList.Images.Count)
            {
                RefreshGalleryNotify = true;
                isloading = true;
            }

            if (FilesInWebSearchDir != FilesInThumbsDir && FilesInThumbsDir < 20)
            {
                RefreshThumbnails();
                isloading = true;
            }

            if (IS_USBConnection && FilesInWebSearchDir != FilesInThumbsDir && FilesInThumbsDir < 20)
            {
                FilesChanged = true;
                isloading = true;
                RefreshThumbnails();
                
            }


            loadingImageslabel.Visible = isloading;
            LoadingImagesPBar.Visible = isloading;

            //if (SelectedImageKeys.Count==0)
            //{
            //    for (int i = 0; i < imglist.Items.Count; i++)
            //    {
            //        SelectImage(imglist.Items[i]);
            //    }
            //}
            ValidateSelectedImages();

            UpdateBillDetails(SelectedImageKeys.Count);
        }


        private void ValidateSelectedImages()
        {
            if (SelectedImageKeys.Count == 0)
            {
                for (int i = 0; i < imglist.Items.Count; i++)
                {
                    SelectImage(imglist.Items[i]);
                }
            }
            //Is selected image also marked with check.
            for (int i = 0; i < imglist.Items.Count; i++)
            {
                if(SelectedImageKeys.Contains(imglist.Items[i].ImageKey) && 
                    !imglist.Items[i].Text.Contains("[" + CheckSymbol + "] "))
                {
                    SelectedImageChecked(imglist.Items[i]);
                }
            }

        }


    //     if (item.Checked ==false)
    //        {
    //            SelectedImageKeys.Add(item.ImageKey);
    //            item.Checked = true;
    //            item.BackColor = SelectedColor;
    //            item.Focused = true;
    //            //string copyrightUnicode = "2714"; // ballot box -1F5F9
    //            //int value = int.Parse(copyrightUnicode, System.Globalization.NumberStyles.HexNumber);
    //            //string symbol = char.ConvertFromUtf32(value).ToString();
    //            item.Font = SelectedFont;
    //            item.Text = "[" + CheckSymbol + "] " + item.Text;
    //            UpdateBillDetails(SelectedImageKeys.Count);
    //}


    private void UpdateBillDetails(int count)
        {
            if (!string.IsNullOrEmpty(label8.Text) &&
                !string.IsNullOrEmpty(label1.Text) )
            {
           
            label_PicsCount.Text = count.ToString();
            label9.Text = (Convert.ToInt16(label8.Text) * count).ToString();
            label13.Text = (((Convert.ToInt16(label8.Text) * count) * Convert.ToInt16(label1.Text) / 100)
                            + (Convert.ToInt16(label8.Text) * count)).ToString();
            }
        }


        /// <summary>
        /// Shows fianll selection, gallery preview
        /// </summary>
        /// <param name="imageKeys"></param>
        private void ShowSelectedImages(List<string> imageKeys)
        {
            previewImages.Images.Clear();
            if (galleryPreview.LargeImageList != null && galleryPreview.LargeImageList.Images.Count>0)
                galleryPreview.LargeImageList.Images.Clear();
            galleryPreview.Clear(); //galleryPreview.LargeImageList.Images.Clear();
            foreach (var item in imageKeys)
            {
               
                string[] imgDetails = item.Split('|');
                string tempImg = imgDetails[0].Replace(imgDetails[1], "thumbs/") + imgDetails[1];

                //string imgName = item.Split('|')[1];
                previewImages.Images.Add(tempImg, imageIO.GetImage(tempImg));
                //previewImages.Images.Add(tempImg, Image.FromFile(tempImg));
                logger.Log(NLog.LogLevel.Info, "thumbnail size 80,80 should be in a config file.");
                previewImages.ImageSize = new Size(80, 80);
                galleryPreview.LargeImageList = previewImages;
                // image key is the image sleected from imagelist collection, key must present in imagelist above\
                galleryPreview.Items.Add(imgDetails[1], tempImg);
                galleryPreview.Show();
            }
            
        }

        //private Image ResizeHighQualityImage(System.Drawing.Image image, int width, int height)
        //{
          
        //    Bitmap result;
        //    // the resized result bitmap
        //    result = new Bitmap(width, height);
            
        //        // get the graphics and draw the passed image to the result bitmap
        //        using (Graphics grphs = Graphics.FromImage(result))
        //        {
        //            grphs.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //            grphs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //            grphs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //            grphs.DrawImage(image, 0, 0, result.Width, result.Height);
        //        }
        //        //Image i = new Bitmap(result);
        //        //// check the quality passed in
        //        //if ((quality < 0) || (quality > 100))
        //        //{
        //        //    string error = string.Format("quality must be 0, 100", quality);
        //        //    throw new ArgumentOutOfRangeException(error);
        //        //}

        //        //EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //        //string lookupKey = "image/jpeg";
        //        //var jpegCodec = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupKey)).FirstOrDefault();
                
        //        ////create a collection of EncoderParameters and set the quality parameter
        //        //var encoderParams = new EncoderParameters(1);
        //        //encoderParams.Param[0] = qualityParam;
        //        ////save the image using the codec and the encoder parameter
        //        //result.Save(pathToSave, jpegCodec, encoderParams);
            
        //    return result;
        //}


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        //private static Bitmap ResizeImage(Image image, int width, int height)
        //{
        //    var destRect = new Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }

        //    return destImage;
        //}


        //private Image resizeImage(Image imgToResize, Size size)
        //{
        //    if (imgToResize.Width < 100 | imgToResize.Height < 100)
        //    {
        //        return imgToResize;
        //    }
        //    int sourceWidth = imgToResize.Width;
        //    int sourceHeight = imgToResize.Height;

        //    float nPercent = 0;
        //    float nPercentW = 0;
        //    float nPercentH = 0;

        //    nPercentW = ((float)size.Width / (float)sourceWidth);
        //    nPercentH = ((float)size.Height / (float)sourceHeight);

        //    if (nPercentH < nPercentW)
        //        nPercent = nPercentH;
        //    else
        //        nPercent = nPercentW;

        //    int destWidth = (int)(sourceWidth * nPercent);
        //    int destHeight = (int)(sourceHeight * nPercent);

        //    Bitmap b = new Bitmap(destWidth, destHeight);
        //    Graphics g = Graphics.FromImage((Image)b);
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //    g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        //    g.Dispose();

        //    return (Image)b;
        //}

        //private void CreateImageList(ChitraKiAlbumAurVivaran obj)
        //{
        //    imgs.ImageSize = new Size(200, 200);
        //    imgs.ColorDepth = ColorDepth.Depth32Bit;
        //    List<Image> images = new List<Image>();

        //    foreach (var item in obj.PeerImages)
        //    {
        //        using (Image im = Image.FromFile(item.ImageFullName))
        //        {
        //            //images.Add();
        //            var imtemp = ResizeImage(im, 200, 200);
        //            imgs.Images.Add(item.ImageKey, imtemp);
        //            //im.Dispose();// = null;
        //        }
        //    }
        //}



        private void ShowGallerySelectionImages(ChitraKiAlbumAurVivaran obj)
        {
            if (imglist.LargeImageList != null && imglist.LargeImageList.Images.Count >0)
            {
                imglist.LargeImageList.Images.Clear();
            }
            
            imglist.Clear();//imglist.LargeImageList.Images.Clear();
            imageIO.CreateImageListFromThumbnails(obj,imgs);
            imglist.LargeImageList = imgs;
            CheckForMaxImageWarning();
            foreach (var item in obj.PeerImages)
            {
                // image key is the image sleected from imagelist collection, key must present in imagelist above
                imglist.Items.Add(item.ImageName, item.ImageKey);
                imglist.Show();
            }

            // Dont show folder icon..
            ////imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName).GetThumbnailImage(200, 200, null, IntPtr.Zero));
            ////imgs.ImageSize = new Size(200, 200);
            
            ////imglist.Items.Add(obj.ImageName, obj.ImageKey);
            ////imglist.Show();
        }

        private void CheckForMaxImageWarning()
        {
            if (AllImages.Count != 0 && AllImages[0].PeerImages.Count == Globals.IncludeMaxImages)
            {
                warningTxt.Text = "Max Image count:" + Globals.IncludeMaxImages + ".";
                pictureBox4.Visible = true;
            }
            else if (AllImages.Count != 0 && AllImages[0].PeerImages.Count < Globals.IncludeMaxImages)
            {
                warningTxt.Text = "";
                pictureBox4.Visible = false;
            }
        }



        /// <summary>
        /// Shows sleected images, first convert images to a collection. then remove extra controls from form and'
        /// show only images selcted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (SelectedImageKeys.Count > 0)
            {
                //gallery preview is page where all final pics are shown before print
                if (!OnGalleryPreviewPage)
                {
                    PrepareFormForGalleryPreview();
                    ShowSelectedImages(SelectedImageKeys);
                }
                else
                {
                    PrepareForPrinting();
                }
            }
            else
            {
                //TODO: show error, no image selected
            }
        }

        private void PrepareForPrinting()
        {
            //this.Visible = false;
            timer.Stop();
            timer.Dispose();
            this.Close();
            print = new UI.Print(this, WifiConnectHelpObject, AnimationFormObject, waiter, SelectedImageKeys.Count);
            //print.SelectedImages = imgs;
            print.SelectedImages = SelectedImageKeys;
            print.ShowDialog();

        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
           // Fix this

            if (SelectedImageKeys.Count > 0)
            {
                //gallery preview is page where all final pics are shown before print
                if (!OnGalleryPreviewPage)
                {
                    //PrepareFormForGallerySelection();
                    BackTOAnimation();
                }
                else
                {
                    PrepareFormForGallerySelection();
                    ///PrepareFormForGalleryPreview();
                    //ShowSelectedImages(SelectedImageKeys);
                }
            }
            else
            {
                //TODO: show error, no image selected
                BackTOAnimation();
            }

            
        }

        private void BackTOAnimation()
        {
            Application.Exit();
            //if (WifiConnectHelpObject != null)
            //{
            //    WifiConnectHelpObject.Visible = false;
            //    ((UI.WifiConnectHelp)(this.WifiConnectHelpObject)).DeleteAllImages();
            //    ((UI.WifiConnectHelp)(this.WifiConnectHelpObject)).IamAlreadyCalledOnce = false;
            //}
            //if (imglist != null && imglist.LargeImageList != null && imglist.LargeImageList.Images != null)
            //    imglist.LargeImageList.Images.Clear();
            //imglist.Clear();
            //timer.Stop();
            //timer.Dispose();
            //this.Clos3e();
            //this.Dispose();
            //AnimationFormObject.Visible = true;
        }

        private void WebSiteUploadsWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            
            
            //if (fileChangedCounter == 0 && InvokeRequired)
            //{
            //    //System.Threading.Thread.Sleep(1000);
            //    Invoke(new Action(()=>{ FilesChanged = true; }));
            //}
           
            //fileChangedCounter++;

        
        }

        private void Done(bool IsWeb)
        {

            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    if (AllImages.Count == 0)
            //    {
            //        MessageBox.Show("No Image Found, Error in Image search function.");
            //        //return;
            //    }
            //}

            foreach (var item in AllImages)
            {
                //Create Thumbnails
                Task task = new Task(async () =>
                {
                    await imageIO.DirectConn_CreateThumbnails(item);
                });
                task.Start();
                if (InvokeRequired)
                {
                    Invoke(new Action(() => ReportProgressForThumbnails(item.ImageDirName)));
                }
            }

            //if (AllImages.Count != 0)
            //{
            //    imageIO.BubbleSortImages(AllImages);
            //    AllImages.Reverse();
            //}

            if (InvokeRequired)
            {
                Invoke(new Action(() => { waiter.Close(); waiter.Dispose(); }));
                Invoke(new Action(() => RefreshGalleryNotify = true));
                //if (fileChangedCounter > 1)
                //{// again raise event.
                //    Invoke(new Action(() => FilesChanged = true));
                //}
                //Invoke(new Action(() => fileChangedCounter = 0));
                Invoke(new Action(() => CheckForMaxImageWarning()));
                
            }
            else
            {
                if (waiter!=null)
                {
                    waiter.Close(); waiter.Dispose();
                }
               
                RefreshGalleryNotify = true;
                CheckForMaxImageWarning();
                //if (fileChangedCounter > 1)
                //{// again raise event.
                //    FilesChanged = true;
                //}
                //fileChangedCounter = 0;
            }
            
        }

        private void ReportProgressForThumbnails(string dirName)
        {
            //foundImageCount = (foundImageCount + obj.ImageDirTotalImages);
            waiter.FileFoundLabelText = "Creating image thumbnails for : " + dirName;
            //AllImages.Add(obj);
        }

        private void ReportProgress(ChitraKiAlbumAurVivaran obj)
        {
            foundImageCount = (foundImageCount + obj.ImageDirTotalImages);
            //TODO : write here invoke required and invoke to display images found count on form
            //if(InvokeRequired)
            //    Invoke(new Action(() => label13.Text = foundImageCount.ToString() + " images found"));
            AllImages.Add(obj);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void RefreshThumbnails()
        {
            if (AllImages==null || AllImages.Count==0)
                return;
            //Create Thumbnails
            Task task = new Task(async () =>
            {
                await imageIO.DirectConn_CreateThumbnails(AllImages[0]);
            });
            task.Start();
            if (InvokeRequired)
            {
                Invoke(new Action(() => ReportProgressForThumbnails(AllImages[0].ImageDirName)));
            }
            RefreshGalleryNotify = true;
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            //RefreshThumbnails();
            UploadUSBFilesDialog.InitialDirectory = USBDriveLetter;
            UploadUSBFilesDialog.ShowDialog();
            UploadUSBFilesDialog.DefaultExt = ".jpg";
            UploadUSBFilesDialog.Multiselect = true;
            for (int i = 0; i < UploadUSBFilesDialog.FileNames.Count(); i++)
            {
                //if (!UploadUSBFilesDialog.SafeFileNames[i].ToLower().Contains(".jpg")
                //    || !UploadUSBFilesDialog.SafeFileNames[i].ToLower().Contains(".jpeg")
                //    || !UploadUSBFilesDialog.SafeFileNames[i].ToLower().Contains(".png")
                //    )
                //{
                //    continue;
                //}
                File.Copy(UploadUSBFilesDialog.FileNames[i], ConfigurationManager.AppSettings["WebSiteSearchDir"] + "\\"+ 
                    UploadUSBFilesDialog.SafeFileNames[i]);
            }
            
        }

        
    }


}
