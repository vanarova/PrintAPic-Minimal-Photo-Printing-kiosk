using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace PicsDirectoryDisplayWin.UI
{
    public partial class Print : Form
    {
        string receiptDir = "Receipt\\";
        string PrintDir = "Prints\\";

        string taxinvoicenumber =
        DateTime.Now.Day.ToString()
        + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.Hours.ToString()
        + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString();

        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public enum AspectRatio {S4x3,LessThanS4x3, GreaterThanS4x3 };
        public List<string> SelectedImages { get; set; }
        public Print()
        {
            InitializeComponent();
            SelectedImages = new List<string>();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            //(new PicsDirectoryDisplayWin.lib_ImgIO.ImageIO()).DeleteAllFilesInDrectoryAndSubDirs(PrintDir);
            PicsbgWorker.RunWorkerAsync();

        }

        private void GeneratePicsPDF()
        {
            //Start();
            PdfImage image1 = null;
            PdfImage image2 = null;
            AspectRatio aspectRatio1 = AspectRatio.S4x3;
            AspectRatio aspectRatio2 = AspectRatio.S4x3;
            //DetectImageAspectRatio();
            //SelectedImages.Add(@"C:\inetpub\wwwroot\ps\Uploads\22.jpg");
            //SelectedImages.Add(@"C:\inetpub\wwwroot\ps\Uploads\ww.jpg");

            //PdfImage image2 = PdfImage.FromFile(@"C:\inetpub\wwwroot\ps\Uploads\20160530_200327.jpg");

            //Call below code in pair, insode a for loop.
            //List<PdfImage> pdfImages = new List<PdfImage>();

            for (int i = 0; i < SelectedImages.Count; i = i + 2)
            {
                image1 = null; image2 = null;
                //first image on page
                image1 = PdfImage.FromFile(SelectedImages[i].Split('|')[0]); //Take Image name from image key
                aspectRatio1 = DetectImageAspectRatio(image1);
                //second image on page
                if (!(i + 1 >= SelectedImages.Count))
                {
                    image2 = PdfImage.FromFile(SelectedImages[i + 1].Split('|')[0]);
                    aspectRatio2 = DetectImageAspectRatio(image2);
                }
                SetPageMarginGeneratePDF_ImageRatio4x3(PrintDir + "Print" + i + ".pdf", image1, image2, aspectRatio1, aspectRatio2);

            }
        }

        private AspectRatio DetectImageAspectRatio(PdfImage image)
        {
            logger.Info("Printing image");
            float width = image.Width;
            float height = image.Height;
            if (width<height)
              logger.Error("Image rotation is needed");

            float ap = width / height;
            if (Math.Abs(Math.Round(ap,2) - 1.33)<0.1) //compare with 4/3 aspect ratio, if equal
                return AspectRatio.S4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1 
                && (Math.Round(ap, 2) - 1.33) <0) // -ve value means aspect ratio is lower than 4x3
                return AspectRatio.LessThanS4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1
                && (Math.Round(ap, 2) - 1.33) > 0) // +ve value means aspect ratio is higher than 4x3, could be 3x2 = 1.5
                return AspectRatio.GreaterThanS4x3;
            return AspectRatio.S4x3;
        }


        private void Generate_receipt(string ImageFileName)
        {
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

            PdfMargins margin = new PdfMargins();
            margin.Top = InPoints(0.5f);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            margin.Left = InPoints(0.5f); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Right = margin.Left;

            //PdfPageSettings settings = new PdfPageSettings();
            //settings.Size = new SizeF(InPoints(7.2f), InPoints(7.2f)); //here should 72mmx height
            //PaperSize psizee = new PaperSize();
            //psizee.Height = (int)settings.Size.Height;
            //psizee.Width = (int)settings.Size.Width;
            //doc.PrintSettings.PaperSize = psizee;
            //Create one page
            PdfPageBase page = doc.Pages.Add(new SizeF(InPoints(7.2f), InPoints(7.2f)),margin);
            PdfFont font12 = new PdfFont(PdfFontFamily.Helvetica, 12f);
            PdfFont font10 = new PdfFont(PdfFontFamily.Helvetica, 10f);
            PdfFont font8 = new PdfFont(PdfFontFamily.Helvetica, 8f);
            PdfSolidBrush brush1 = new PdfSolidBrush(Color.Black);
            PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            PdfStringFormat rightAlignment = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);


            string total = (((Convert.ToInt16(ConfigurationManager.AppSettings["CostValue"]) * SelectedImages.Count) * Convert.ToInt16(ConfigurationManager.AppSettings["GSTValue"]) / 100)
                            + (Convert.ToInt16(ConfigurationManager.AppSettings["CostValue"]) * SelectedImages.Count)).ToString();


            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line1"],font12, brush1, page.Canvas.ClientSize.Width, 10, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line2"] + taxinvoicenumber, font10, brush1, page.Canvas.ClientSize.Width, 25, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line3"]+SelectedImages.Count+"/-",font10, brush1, page.Canvas.ClientSize.Width, 40, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line4"]+ ConfigurationManager.AppSettings["CostValue"] + "/-"
                , font10, brush1, page.Canvas.ClientSize.Width, 55, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line5"]+
                ConfigurationManager.AppSettings["GSTValue"]+ "%", font10, brush1, page.Canvas.ClientSize.Width, 70, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line6"] ,font10, brush1, page.Canvas.ClientSize.Width, 85, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line7"] + total + "/-", font10, brush1, page.Canvas.ClientSize.Width, 100,rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line8"],font8, brush1,  page.Canvas.ClientSize.Width, 120, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line9"],font8, brush1,  page.Canvas.ClientSize.Width, 130, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line10"],font8, brush1, page.Canvas.ClientSize.Width, 140,rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line11"],font10, brush1, page.Canvas.ClientSize.Width, 155,rightAlignment);
            //Save the document
            doc.SaveToFile(receiptDir + ImageFileName);
            doc.SaveToFile(ConfigurationManager.AppSettings["ReceiptBackupDir"] + "\\" + ImageFileName);
            doc.Close();

            //Launch the Pdf file
            PDFDocumentViewer(receiptDir + ImageFileName);
        }

        private void SetPageMarginGeneratePDF_ImageRatio4x3(string ImageFileName, PdfImage img1, PdfImage img2, 
            AspectRatio aspectRatioImage1 = AspectRatio.S4x3, AspectRatio aspectRatioImage2 = AspectRatio.S4x3)
        {
            float MARGINTOP_BOTTOM = 0.75f; float MARGINLEFT_RIGHT = 1.25f;
            float DISTANCE_BTW_IMAGES = 0.5F;
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = InPoints(MARGINTOP_BOTTOM);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            margin.Left = InPoints(MARGINLEFT_RIGHT); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Right = margin.Left;

            //Create one page
            PdfPageBase page = doc.Pages.Add(PdfPageSize.A4, margin);

            float A4Height = unitCvtr.ConvertUnits(PdfPageSize.A4.Height, PdfGraphicsUnit.Point, PdfGraphicsUnit.Centimeter);
            float A4Width = unitCvtr.ConvertUnits(PdfPageSize.A4.Width, PdfGraphicsUnit.Point, PdfGraphicsUnit.Centimeter);

            //After margin
            float A4PostMarginWidth = PdfPageSize.A4.Width - (margin.Left * 2); // left,right margins
            //In centimeter :
            //label6.Text = InCentimeter(A4PostMarginWidth);

            //Adjust width, height with 4:3 ratio in PDF points :
            float A4PostMarginWidthPts4x3 = PdfPageSize.A4.Width - (margin.Left * 2); // left,right margins
            float A4PostMarginHeightPts4x3 = A4PostMarginWidthPts4x3 * 0.75f;  // height = 3/4 * width

            //In centimeter = 
            //label8.Text = InCentimeter(A4PostMarginWidthPts4x3);
            //label9.Text = InCentimeter(A4PostMarginHeightPts4x3);

            //TransformText(page);
            DrawImagesInPage(img1,img2,page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3,
                DISTANCE_BTW_IMAGES, aspectRatioImage1, aspectRatioImage2);
            //TransformImage(page);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            //Launch the Pdf file
            PDFDocumentViewer( ImageFileName);
        }

        private string InCentimeter(float value)
        {
           return new PdfUnitConvertor().ConvertUnits(value, PdfGraphicsUnit.Point, PdfGraphicsUnit.Centimeter).ToString();
        }

        private float InPoints(float value)
        {
            return new PdfUnitConvertor().ConvertUnits(value, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        }




        private void DrawImagesInPage(PdfImage img1, PdfImage img2, PdfPageBase page, float w, float h,
            float distance_btw_images, AspectRatio aspectRatioImage1 , AspectRatio aspectRatioImage2 )
        {
            float heightBtwImages = InPoints(distance_btw_images);
            //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            DrawImageInFrame_wxh(img1, page, 0, w, h, aspectRatioImage1);
            DrawImageInFrame_wxh(img2, page, h+heightBtwImages, w, h, aspectRatioImage2);
            //if (img2 != null)
            //{
            //    if (aspectRatioImage2 == AspectRatio.S4x3)
            //    {

            //        page.Canvas.DrawImage(img2, new System.Drawing.RectangleF(0, h + heightBtwImages, w, h));
            //    }

            //}



            //float width = image.Width * 0.75f;
            //float height = image.Height * 0.75f;
            //float x = (page.Canvas.ClientSize.Width - width) / 2;

            //Draw the image
            //page.Canvas.DrawImage(image, x, 60, width, height);
            //float rectangleHeight = page.Canvas.ClientSize.Height/2- heightBtwImages;
            ////unitCvtr.ConvertUnits(rectangleHeight,PdfGraphicsUnit.)
            //page.Canvas.DrawImage(image, new System.Drawing.RectangleF(0, 0, page.Canvas.ClientSize.Width, rectangleHeight));
            //page.Canvas.DrawImage(image2, new System.Drawing.RectangleF(0, rectangleHeight + heightBtwImages, page.Canvas.ClientSize.Width, rectangleHeight));

            //if (image2.Height > image2.Width)
            //{
            //    //Rotate image
            //    PdfTemplate template = new PdfTemplate(page.Canvas.ClientSize.Width, rectangleHeight);
            //    template.Graphics.RotateTransform(90);
            //    template.Graphics.DrawImage(image2, new System.Drawing.RectangleF(0, rectangleHeight + heightBtwImages, page.Canvas.ClientSize.Width, rectangleHeight));

            //}


        }

        private void DrawImageInFrame_wxh(PdfImage img, PdfPageBase page,float y, float w, float h, AspectRatio aspectRatioImage1)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            
            //Load an image
            //PdfImage image = PdfImage.FromFile(@"C:\inetpub\wwwroot\ps\Uploads\nasa-89125-unsplash - Copy.jpg");
            //PdfImage image2 = PdfImage.FromFile(@"C:\inetpub\wwwroot\ps\Uploads\20160530_200327.jpg");
            if (img != null)
            {
                if (aspectRatioImage1 == AspectRatio.S4x3)
                {
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, 0, w, h));
                }
                else
                {
                    page.Canvas.DrawRectangle(brush, new Rectangle(new Point(0, (int)y), new Size(Convert.ToInt32(w), Convert.ToInt32(h))));
                }
                if (aspectRatioImage1 == AspectRatio.GreaterThanS4x3)
                {
                    float new_h = w * img.Height / img.Width;
                    float new_MarginTopBottom = (h - new_h) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0,y+ new_MarginTopBottom, w, new_h)); //use width of frame, adjust height.

                }
                if (aspectRatioImage1 == AspectRatio.LessThanS4x3)
                {
                    float new_w = h * img.Width / img.Height;
                    float new_MarginLeftRight = (w - new_w) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(new_MarginLeftRight, y, new_w, h)); //use width of frame, adjust height.
                    //Graphics.FromImage(img.);

                }
            }

        }

        private void PDFDocumentViewer(string fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch { }
        }

        public void Start()
        {
           
           
           string filename = String.Format("{0}_tempfile.pdf", Guid.NewGuid().ToString("D").ToUpper());
            var s_document = new PdfSharp.Pdf.PdfDocument();
            s_document.Info.Title = "PDFsharp XGraphic Sample";
            s_document.Info.Author = "Stefan Lange";
            s_document.Info.Subject = "Created with code snippets that show the use of graphical functions";
            s_document.Info.Keywords = "PDFsharp, XGraphics";
            DrawPageTransformed(s_document.AddPage());
            // Save the s_document...
            s_document.Save(filename);
        }

        public void DrawPageTransformed(PdfPage page)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //DrawTitle(page, gfx, "Images");

            //DrawImage(gfx, 1);

            DrawImageTransformed(gfx, 2, SelectedImages[0]);
            //DrawImageRotated(gfx, 3, SelectedImages[0]);
           
        }


        //public void DrawPageRedrawn(PdfPage page)
        //{
        //    XGraphics gfx = XGraphics.FromPdfPage(page);

        //    //DrawTitle(page, gfx, "Images");

        //    //DrawImage(gfx, 1);

        //    DrawImageScaled(gfx, 2, SelectedImages[0]);
        //    //DrawImageRotated(gfx, 3, SelectedImages[0]);
        //    //DrawImageSheared(gfx, 4);
        //    //DrawGif(gfx, 5);
        //    //DrawPng(gfx, 6);
        //    //DrawTiff(gfx, 7);
        //    //DrawFormXObject(gfx, 8);
        //}

        //void DrawImageScaled(XGraphics gfx, int number, string jpegSamplePath)
        //{
            
        //    //BeginBox(gfx, number, "DrawImage (scaled)");

        //    XImage image = XImage.FromFile(jpegSamplePath.Split('|')[0]);
        //    gfx.DrawImage(image, 0, 0, image.PixelWidth/2, image.PixelHeight/2);

        //    //EndBox(gfx);
        //}

        void DrawImageTransformed(XGraphics gfx, int number, string jpegSamplePath)
        {
            XImage image = XImage.FromFile(jpegSamplePath.Split('|')[0]);

            const double dx = 250, dy = 140;

            //gfx.TranslateTransform(dx / 2, dy / 2);
            gfx.ScaleTransform(0.5);
            //gfx.RotateTransform(-25);
            //gfx.TranslateTransform(-dx / 2, -dy / 2);

            //XMatrix matrix = new XMatrix();  //XMatrix.Identity;

            //double width = image.PixelWidth * 72 / image.HorizontalResolution;
            //double height = image.PixelHeight * 72 / image.HorizontalResolution;

            gfx.DrawImage(image, image.PixelHeight/4 + 50, 0);
        }

        void DrawImageRotated(XGraphics gfx, int number, string jpegSamplePath)
        {
            //BeginBox(gfx, number, "DrawImage (rotated)");

            XImage image = XImage.FromFile(jpegSamplePath.Split('|')[0]);

            const double dx = 250, dy = 140;

            gfx.TranslateTransform(dx / 2, dy / 2);
            gfx.ScaleTransform(0.7);
            gfx.RotateTransform(-25);
            gfx.TranslateTransform(-dx / 2, -dy / 2);

            //XMatrix matrix = new XMatrix();  //XMatrix.Identity;

            double width = image.PixelWidth * 72 / image.HorizontalResolution;
            double height = image.PixelHeight * 72 / image.HorizontalResolution;

            gfx.DrawImage(image, (dx - width) / 2, 0, width, height);

            //EndBox(gfx);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintSettings pst = new PrintSettings();
            
            pst.ShowDialog();
        }

        private void PicsbgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
            PicsbgWorker.ReportProgress(25);

            Generate_receipt(taxinvoicenumber+".pdf");
            PrintReceipt();
            PicsbgWorker.ReportProgress(50);
           
            GeneratePicsPDF();
            PrintPicsPDF();
            PicsbgWorker.ReportProgress(75);
            
            CheckPrinterQueue();
            PicsbgWorker.ReportProgress(100);
            
        }

        private void PrintPicsPDF()
        {
            PdfDocument pdf = new PdfDocument();
            pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["PhotoPrinterName"];
            //search all files prresent in "printdir"
            //load and print them one by one.
            FileInfo[] files = new DirectoryInfo(PrintDir).GetFiles();
            foreach (var item in files)
            {
                pdf.LoadFromFile(item.FullName);
                pdf.Print();
            }
            

        }

        private void CheckPrinterQueue()
        {
            System.Threading.Thread.Sleep(2000);
            //throw new NotImplementedException();
        }

        private void PrintReceipt()
        {

           PdfDocument pdf = new PdfDocument();
           pdf.LoadFromFile(receiptDir + taxinvoicenumber+".pdf");
           pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["ReceiptPrinterName"];
           pdf.Print();


            //System.Threading.Thread.Sleep(2000);
            //throw new NotImplementedException();
        }

        private void PicsbgWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 25)
                PrintStatusLbl.Text = "Printing receipt";
            if (e.ProgressPercentage == 50)
                PrintStatusLbl.Text = "Printing pics";
            if (e.ProgressPercentage == 75)
                PrintStatusLbl.Text = "Checking print status";
            if (e.ProgressPercentage == 100)
                PrintStatusLbl.Text = "Finished";

        }

        private void PicsbgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Generate_receipt(taxinvoicenumber+".pdf");
        }




        //public void BeginBox(XGraphics gfx, int number, string title)
        //{
        //    const int dEllipse = 15;
        //    XRect rect = new XRect(0, 20, 300, 200);
        //    if (number % 2 == 0)
        //        rect.X = 300 - 5;
        //    rect.Y = 40 + ((number - 1) / 2) * (200 - 5);
        //    rect.Inflate(-10, -10);
        //    XRect rect2 = rect;
        //    rect2.Offset(this.borderWidth, this.borderWidth);
        //    gfx.DrawRoundedRectangle(new XSolidBrush(this.shadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
        //    XLinearGradientBrush brush = new XLinearGradientBrush(rect, this.backColor, this.backColor2, XLinearGradientMode.Vertical);
        //    gfx.DrawRoundedRectangle(this.borderPen, brush, rect, new XSize(dEllipse, dEllipse));
        //    rect.Inflate(-5, -5);

        //    XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
        //    gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);

        //    rect.Inflate(-10, -5);
        //    rect.Y += 20;
        //    rect.Height -= 20;

        //    this.state = gfx.Save();
        //    gfx.TranslateTransform(rect.X, rect.Y);
        //}

        //public void EndBox(XGraphics gfx)
        //{
        //    gfx.Restore(this.state);
        //}

    }
}
