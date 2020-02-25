
using PicsDirectoryDisplayWin.lib_Print;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Printing;
using System.Reflection;
using System.Windows.Forms;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace PicsDirectoryDisplayWin.UI
{
    public partial class Print : Form
    {
        //string receiptDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +"\\Receipt\\";
        //string PrintDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Prints\\";
        private PrinterState PrintState = new PrinterState();
        Form galleryFormObject = null; Form wifiHelpFormObject = null;
        Form AnimationFormObject = null; Form waiterObject = null;
        

        string taxinvoicenumber =
        DateTime.Now.Day.ToString()
        + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.Hours.ToString()
        + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString();

        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public enum AspectRatio {S4x3,LessThanS4x3, GreaterThanS4x3, S6x4, LessThanS6x4, GreaterThanS6x4 };
        public List<string> SelectedImages { get; set; }
        public int selectedImagesCount;

        public Print(Form gallery,Form wifiHelp,Form animation, Form waiter, int SelectedImagesCount)
        {
            InitializeComponent();
            PrintWatch.Stop();
            SelectedImages = new List<string>();
            galleryFormObject = gallery;
            wifiHelpFormObject = wifiHelp;
            AnimationFormObject = animation;
            waiterObject = waiter;
            button2.Visible = false;
            button3.Visible = false;
            label5.Visible = false;
            label1.Visible = false;
            selectedImagesCount = SelectedImagesCount;


            //fullscreen
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            string checkUnicode = "2714"; // ballot box -1F5F9
            int value = int.Parse(checkUnicode, System.Globalization.NumberStyles.HexNumber);

            label24.Text = ConfigurationManager.AppSettings["PrintReady1Eng"];
            label4.Text = ConfigurationManager.AppSettings["PrintReady1Hindi"];
            label6.Text = ConfigurationManager.AppSettings["BillInfo"];
            label13.Text = ConfigurationManager.AppSettings["PrintSizeText"];
            label14.Text = Globals.PrintSelection.ToString(); //ConfigurationManager.AppSettings["PrintSizeValue"];
            label8.Text = ConfigurationManager.AppSettings["CostValue"+ Globals.PrintSelection.ToString()];
            label6.Text = ConfigurationManager.AppSettings["NoOfPicsText"];
            label_PicsCount.Text = ConfigurationManager.AppSettings["NoOfPicsInitialValue"];
            label11.Text = ConfigurationManager.AppSettings["AmountText"];
            label12.Text = ConfigurationManager.AppSettings["AmountInitialValue"];
            label9.Text = ConfigurationManager.AppSettings["GSTValue"];
            label16.Text = ConfigurationManager.AppSettings["TotalText"];
            label15.Text = ConfigurationManager.AppSettings["TotalValue"];
            tb.BackColor = Color.FromName(ConfigurationManager.AppSettings["AppBackgndColor"]);
            UpdateBillDetails(selectedImagesCount);
        }

        private void UpdateBillDetails(int count)
        {
            label3.Text = count.ToString();
            label23.Text = count.ToString();
            if (!string.IsNullOrEmpty(label8.Text) &&
                !string.IsNullOrEmpty(label1.Text))
            {

                label_PicsCount.Text = count.ToString();
                label12.Text = (Convert.ToInt16(label8.Text) * count).ToString();
                
                label15.Text = (((Convert.ToInt16(label8.Text) * count) * Convert.ToInt16(label9.Text) / 100)
                                + (Convert.ToInt16(label8.Text) * count)).ToString();
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            btn_print.Enabled = false;
            label3.Visible = false;
            label4.Visible = false;
            label23.Visible = false;
            label24.Visible = false;

            label5.Visible = true;
            label1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            progressBar1.Visible = true;
            PrintWatch.Start();
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


            //Call below code in pair, insode a for loop.
            //List<PdfImage> pdfImages = new List<PdfImage>();
            //Globals.PrintSelection = Globals.PrintSize.A4;

            if (Globals.PrintSelection == Globals.PrintSize.A5)
            {
                for (int i = 0; i < SelectedImages.Count; i = i + 2)
                {
                    image1 = null; image2 = null;
                    //first image on page
                    image1 = PdfImage.FromFile(SelectedImages[i].Split('|')[0]); //Take Image name from image key
                    aspectRatio1 = DetectImageAspectRatio_A5(image1);
                    //second image on page
                    if (!(i + 1 >= SelectedImages.Count))
                    {
                        image2 = PdfImage.FromFile(SelectedImages[i + 1].Split('|')[0]);
                        aspectRatio2 = DetectImageAspectRatio_A5(image2);
                    }
                    SetPageMarginGeneratePDF_ImageRatio4x3_A5(Globals.PrintDir + "Print" + i + ".pdf", image1, image2, aspectRatio1, aspectRatio2);
                    if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                        logger.Log(NLog.LogLevel.Info, "Generating Image PDF..");
                }
            }
            else if (Globals.PrintSelection == Globals.PrintSize.A4)
            {
                for (int i = 0; i < SelectedImages.Count; i++)
                {
                    image1 = null; image2 = null;
                    //first image on page
                    image1 = PdfImage.FromFile(SelectedImages[i].Split('|')[0]); //Take Image name from image key
                    aspectRatio1 = DetectImageAspectRatio_A4(image1);
                    SetPageMarginGeneratePDF_ImageRatio4x3_A4(Globals.PrintDir + "Print" + i + ".pdf", image1, aspectRatio1);

                }


            }
            else if (Globals.PrintSelection == Globals.PrintSize.Passport)
            {
                image1 = PdfImage.FromFile(SelectedImages[0].Split('|')[0]); //Take Image name from image key
                SetPageMarginGeneratePDF_ImageRatio4x3_Passport(Globals.PrintDir + "Print" + 0 + ".pdf", image1);
            }
            else if (Globals.PrintSelection == Globals.PrintSize.Postcard)
            {
                for (int i = 0; i < SelectedImages.Count; i = i + 2)
                {
                    image1 = null; image2 = null;
                    //first image on page
                    image1 = PdfImage.FromFile(SelectedImages[i].Split('|')[0]); //Take Image name from image key
                    aspectRatio1 = DetectImageAspectRatio_Postcard(image1);
                    //second image on page
                    if (!(i + 1 >= SelectedImages.Count))
                    {
                        image2 = PdfImage.FromFile(SelectedImages[i + 1].Split('|')[0]);
                        aspectRatio2 = DetectImageAspectRatio_Postcard(image2);
                    }
                    SetPageMarginGeneratePDF_ImageRatio6x4_Postcard(Globals.PrintDir + "Print" + i + ".pdf", image1, image2, aspectRatio1, aspectRatio2);
                    if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                        logger.Log(NLog.LogLevel.Info, "Generating Image PDF..");
                }
            }
            else if (Globals.PrintSelection == Globals.PrintSize.pdf)
            {
                //TODO: If PDF file is large and have many page, a dialog to choose pages shud be displayed
                //Also, rejected files with more then 15 pages.
                for (int i = 0; i < SelectedImages.Count; i = i + 2)
                {
                    File.Copy(SelectedImages[i].Split('|')[0], Globals.PrintDir + "Print" + i + ".pdf");
                    if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                        PrintIO.PDFDocumentViewer(Globals.PrintDir + "Print" + i + ".pdf");
                }
            }


        }

        private AspectRatio DetectImageAspectRatio_A5(PdfImage image)
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

        private AspectRatio DetectImageAspectRatio_A4(PdfImage image)
        {
            logger.Info("Printing image");
            float height = image.Width;// Swap width and height in case of A4
            float width = image.Height;
            //if (width < height)
            //    logger.Error("Image rotation is needed");

            float ap = width / height;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) < 0.1) //compare with 4/3 aspect ratio, if equal
                return AspectRatio.S4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1
                && (Math.Round(ap, 2) - 1.33) < 0) // -ve value means aspect ratio is lower than 4x3
                return AspectRatio.LessThanS4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1
                && (Math.Round(ap, 2) - 1.33) > 0) // +ve value means aspect ratio is higher than 4x3, could be 3x2 = 1.5
                return AspectRatio.GreaterThanS4x3;

            return AspectRatio.S4x3;
        }


        private AspectRatio DetectImageAspectRatio_Postcard(PdfImage image)
        {
            logger.Info("Printing image");
            float height = image.Width;// Swap width and height in case of A4
            float width = image.Height;
            //if (width < height)
            //    logger.Error("Image rotation is needed");

            float ap = width / height;
            if (Math.Abs(Math.Round(ap, 2) - 1.5) < 0.1) //compare with 4/3 aspect ratio, if equal
                return AspectRatio.S6x4;
            if (Math.Abs(Math.Round(ap, 2) - 1.5) > 0.1
                && (Math.Round(ap, 2) - 1.5) < 0) // -ve value means aspect ratio is lower than 4x3
                return AspectRatio.LessThanS6x4;
            if (Math.Abs(Math.Round(ap, 2) - 1.5) > 0.1
                && (Math.Round(ap, 2) - 1.5) > 0) // +ve value means aspect ratio is higher than 4x3, could be 3x2 = 1.5
                return AspectRatio.GreaterThanS6x4;

            return AspectRatio.S6x4;
        }

        //private void Generate_receipt(string ImageFileName)
        //{
        //    ///Create a pdf document
        //    Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

        //    //Set the margin
        //    PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

        //    PdfMargins margin = new PdfMargins();
        //    margin.Top =  PrintIO.InPoints(0.5f);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        //    margin.Bottom = margin.Top;
        //    //TODO: Add margins in global settings xml file.
        //    //margin.Left = InPoints(0.5f); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        //    //margin.Right = margin.Left;

        //    //PdfPageSettings settings = new PdfPageSettings();
        //    //settings.Size = new SizeF(InPoints(7.2f), InPoints(7.2f)); //here should 72mmx height
        //    //PaperSize psizee = new PaperSize();
        //    //psizee.Height = (int)settings.Size.Height;
        //    //psizee.Width = (int)settings.Size.Width;
        //    //doc.PrintSettings.PaperSize = psizee;
        //    //Create one page
        //    PdfPageBase page = doc.Pages.Add(new SizeF(PrintIO.InPoints(float.Parse(ConfigurationManager.AppSettings["ReceiptWidth"])),
        //        PrintIO.InPoints(float.Parse(ConfigurationManager.AppSettings["ReceiptHeight"]))), margin);
        //    //PdfFont font12 = new PdfFont(PdfFontFamily.Helvetica, 12f);
        //    PdfFont font10 = new PdfFont(PdfFontFamily.Helvetica, 10f);
        //    //PdfFont font8 = new PdfFont(PdfFontFamily.Helvetica, 8f);
        //    PdfSolidBrush brush1 = new PdfSolidBrush(Color.Black);
        //    PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
        //    PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //    PdfStringFormat rightAlignment = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);




        //    //Launch the Pdf file
        //    if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
        //    {
        //        logger.Log(NLog.LogLevel.Info, "Inside Generate_receipt function.");
        //       PrintIO.PDFDocumentViewer(Globals.receiptDir + ImageFileName);
        //    }
        //}

        //private void Generate_receipt(string ImageFileName)
        //{
        //    ///Create a pdf document
        //    Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

        //    //Set the margin
        //    PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

        //    PdfMargins margin = new PdfMargins();
        //    margin.Top = InPoints(0.5f);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        //    margin.Bottom = margin.Top;
        //    //TODO: Add margins in global settings xml file.
        //    margin.Left = InPoints(0.5f); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        //    margin.Right = margin.Left;

        //    //PdfPageSettings settings = new PdfPageSettings();
        //    //settings.Size = new SizeF(InPoints(7.2f), InPoints(7.2f)); //here should 72mmx height
        //    //PaperSize psizee = new PaperSize();
        //    //psizee.Height = (int)settings.Size.Height;
        //    //psizee.Width = (int)settings.Size.Width;
        //    //doc.PrintSettings.PaperSize = psizee;
        //    //Create one page
        //    PdfPageBase page = doc.Pages.Add(new SizeF(InPoints(7.2f), InPoints(7.2f)),margin);
        //    PdfFont font12 = new PdfFont(PdfFontFamily.Helvetica, 12f);
        //    PdfFont font10 = new PdfFont(PdfFontFamily.Helvetica, 10f);
        //    PdfFont font8 = new PdfFont(PdfFontFamily.Helvetica, 8f);
        //    PdfSolidBrush brush1 = new PdfSolidBrush(Color.Black);
        //    PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
        //    PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //    PdfStringFormat rightAlignment = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);


        //    page.Canvas.DrawString(ConfigurationManager.AppSettings["Line5"]+
        //        ConfigurationManager.AppSettings["GSTValue"]+ "%", font10, brush1, page.Canvas.ClientSize.Width, 70, rightAlignment);
        //    page.Canvas.DrawString(ConfigurationManager.AppSettings["Line6"] ,font10, brush1, page.Canvas.ClientSize.Width, 85, rightAlignment);
        //    page.Canvas.DrawString(ConfigurationManager.AppSettings["Line7"] + total + "/-", font10, brush1, page.Canvas.ClientSize.Width, 100,rightAlignment);
        //    page.Canvas.DrawString(ConfigurationManager.AppSettings["Line8"],font8, brush1,  page.Canvas.ClientSize.Width, 120, rightAlignment);
        //    page.Canvas.DrawString(ConfigurationManager.AppSettings["Line9"],font8, brush1,  page.Canvas.ClientSize.Width, 130, rightAlignment);
        //    page.Canvas.DrawString(ConfigurationManager.AppSettings["Line10"],font8, brush1, page.Canvas.ClientSize.Width, 140,rightAlignment);
        //    page.Canvas.DrawString(ConfigurationManager.AppSettings["Line11"],font10, brush1, page.Canvas.ClientSize.Width, 155,rightAlignment);
        //    //Save the document
        //    doc.SaveToFile(receiptDir + ImageFileName);
        //    doc.SaveToFile(ConfigurationManager.AppSettings["ReceiptBackupDir"] + "\\" + ImageFileName);
        //    doc.Close();

        //    //Launch the Pdf file
        //    if (System.Diagnostics.Debugger.IsAttached)
        //        PDFDocumentViewer(receiptDir + ImageFileName);
        //}

        private void SetPageMarginGeneratePDF_ImageRatio4x3_A4(string ImageFileName, PdfImage img1,
       AspectRatio aspectRatioImage1 = AspectRatio.S4x3)
        {
            float MARGINTOP_BOTTOM = 1.25f; float MARGINLEFT_RIGHT = 1f;
            float DISTANCE_BTW_IMAGES = 0.5F;
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = PrintIO.InPoints(MARGINTOP_BOTTOM);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            margin.Left = PrintIO.InPoints(MARGINLEFT_RIGHT); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
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
            float A4PostMarginHeightPts4x3 = A4PostMarginWidthPts4x3 * 1.34f;  // height = 4/3 * width

            //In centimeter = 
            //label8.Text = InCentimeter(A4PostMarginWidthPts4x3);
            //label9.Text = InCentimeter(A4PostMarginHeightPts4x3);
            DrawImagesInPage_A4(img1, page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3, DISTANCE_BTW_IMAGES, aspectRatioImage1);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                PrintIO.PDFDocumentViewer(ImageFileName);

            ////Launch the Pdf file
            //if (System.Diagnostics.Debugger.IsAttached)
            //    PDFDocumentViewer( ImageFileName);
        }


        private void SetPageMarginGeneratePDF_ImageRatio6x4_Postcard(string ImageFileName, PdfImage img1, PdfImage img2,
            AspectRatio aspectRatioImage1 = AspectRatio.S6x4, AspectRatio aspectRatioImage2 = AspectRatio.S6x4)
        {
            float MARGINTOP_BOTTOM = 0.75f; float MARGINLEFT_RIGHT = 1.25f;
            float DISTANCE_BTW_IMAGES = 0.5F;
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = PrintIO.InPoints(MARGINTOP_BOTTOM);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            margin.Left = PrintIO.InPoints(MARGINLEFT_RIGHT); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
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
            float A4PostMarginHeightPts4x3 = A4PostMarginWidthPts4x3 * 0.67f;  // height = 3/4 * width

            //In centimeter = 
            //label8.Text = InCentimeter(A4PostMarginWidthPts4x3);
            //label9.Text = InCentimeter(A4PostMarginHeightPts4x3);

            //TransformText(page);
            DrawImagesInPage_Postcard(img1, img2, page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3,
                DISTANCE_BTW_IMAGES, aspectRatioImage1, aspectRatioImage2);
            //TransformImage(page);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                PrintIO.PDFDocumentViewer(ImageFileName);

            ////Launch the Pdf file
            //if (System.Diagnostics.Debugger.IsAttached)
            //    PDFDocumentViewer( ImageFileName);
        }

        private void SetPageMarginGeneratePDF_ImageRatio4x3_Passport(string ImageFileName, PdfImage img1)
        {
            float MARGINTOP_BOTTOM = 1.25f; float MARGINLEFT_RIGHT = 1.5f;
            float DISTANCE_BTW_IMAGES = 0.5F;
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = PrintIO.InPoints(MARGINTOP_BOTTOM);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            margin.Left = PrintIO.InPoints(MARGINLEFT_RIGHT); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Right = margin.Left;

            //Create one page
            PdfPageBase page = doc.Pages.Add(PdfPageSize.A4, margin);

            //float A4Height = unitCvtr.ConvertUnits(PdfPageSize.A4.Height, PdfGraphicsUnit.Point, PdfGraphicsUnit.Centimeter);
            //float A4Width = unitCvtr.ConvertUnits(PdfPageSize.A4.Width, PdfGraphicsUnit.Point, PdfGraphicsUnit.Centimeter);

            //After margin
            //float A4PostMarginWidth = PdfPageSize.A4.Width - (margin.Left * 2); // left,right margins
            //In centimeter :
            //label6.Text = InCentimeter(A4PostMarginWidth);

            //divde A4 210mm page to 6 parts to get 35mm image width
            float passport_35x45_width = PdfPageSize.A4.Width/6; // left,right margins
            float passport_35x45_height = passport_35x45_width * 1.28f;  // calc height image size = 4.5 x 3.5 cm, width is known


            //Passport size image
            float postcard_101_1x152_4_width = PdfPageSize.A4.Width / 2.08f; //101.1
            float postcard_101_1x152_4_height = postcard_101_1x152_4_width * 1.5f; //151.6

            //In centimeter = 
            //label8.Text = InCentimeter(A4PostMarginWidthPts4x3);
            //label9.Text = InCentimeter(A4PostMarginHeightPts4x3);
            //DrawImagesInPage_Passport(img1, page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3, DISTANCE_BTW_IMAGES, aspectRatioImage1);

            //Firt row
            float DistanceBtwSecondImg = passport_35x45_width + PrintIO.InPoints(1f);
            DrawImageInFrame_wxh_Passport(img1, page, 0,0, passport_35x45_width, passport_35x45_height);
            DrawImageInFrame_wxh_Passport(img1, page, DistanceBtwSecondImg, 0, passport_35x45_width, passport_35x45_height);
            DrawImageInFrame_wxh_Passport(img1, page, 2 * DistanceBtwSecondImg, 0, passport_35x45_width, passport_35x45_height);
            DrawImageInFrame_wxh_Passport(img1, page, 3 * DistanceBtwSecondImg, 0, passport_35x45_width, passport_35x45_height);


            //second row
            float DistanceBtwSecondRowofImg = passport_35x45_height + PrintIO.InPoints(1f);
            DrawImageInFrame_wxh_Passport(img1, page, 0, DistanceBtwSecondRowofImg, passport_35x45_width, passport_35x45_height);
            DrawImageInFrame_wxh_Passport(img1, page, DistanceBtwSecondImg, DistanceBtwSecondRowofImg, passport_35x45_width, passport_35x45_height);
            DrawImageInFrame_wxh_Passport(img1, page, 2 * DistanceBtwSecondImg, DistanceBtwSecondRowofImg, passport_35x45_width, passport_35x45_height);
            DrawImageInFrame_wxh_Passport(img1, page, 3 * DistanceBtwSecondImg, DistanceBtwSecondRowofImg, passport_35x45_width, passport_35x45_height);

            //third row
            //float DistanceBtwThirdRowofImg = 2 * (passport_35x45_height + PrintIO.InPoints(1f));
            //DrawImageInFrame_wxh_Passport(img1, page, 0, DistanceBtwThirdRowofImg, postcard_101_1x152_4_width, postcard_101_1x152_4_height);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                PrintIO.PDFDocumentViewer(ImageFileName);

            ////Launch the Pdf file
            //if (System.Diagnostics.Debugger.IsAttached)
            //    PDFDocumentViewer( ImageFileName);
        }


        private void SetPageMarginGeneratePDF_ImageRatio4x3_A5(string ImageFileName, PdfImage img1, PdfImage img2, 
            AspectRatio aspectRatioImage1 = AspectRatio.S4x3, AspectRatio aspectRatioImage2 = AspectRatio.S4x3)
        {
            float MARGINTOP_BOTTOM = 0.75f; float MARGINLEFT_RIGHT = 1.25f;
            float DISTANCE_BTW_IMAGES = 0.5F;
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = PrintIO.InPoints(MARGINTOP_BOTTOM);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            margin.Left = PrintIO.InPoints(MARGINLEFT_RIGHT); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
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
            DrawImagesInPage_A5(img1,img2,page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3,
                DISTANCE_BTW_IMAGES, aspectRatioImage1, aspectRatioImage2);
            //TransformImage(page);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                PrintIO.PDFDocumentViewer(ImageFileName);

            ////Launch the Pdf file
            //if (System.Diagnostics.Debugger.IsAttached)
            //    PDFDocumentViewer( ImageFileName);
        }

        private string InCentimeter(float value)
        {
           return new PdfUnitConvertor().ConvertUnits(value, PdfGraphicsUnit.Point, PdfGraphicsUnit.Centimeter).ToString();
        }

        //private float InPoints(float value)
        //{
        //    return new PdfUnitConvertor().ConvertUnits(value, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        //}


        //private void DrawImagesInPage_Passport(PdfImage img1, PdfPageBase page, float w, float h,
        //float distance_btw_images, AspectRatio aspectRatioImage1)
        //{
        //    float heightBtwImages = PrintIO.InPoints(distance_btw_images);
        //    //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
        //    DrawImageInFrame_wxh_A4(img1, page, 0, w, h, aspectRatioImage1);
        //    //DrawImageInFrame_wxh(img2, page, h + heightBtwImages, w, h, aspectRatioImage2);

        //}


        private void DrawImagesInPage_A4(PdfImage img1, PdfPageBase page, float w, float h,
            float distance_btw_images, AspectRatio aspectRatioImage1)
        {
            //float heightBtwImages = PrintIO.InPoints(distance_btw_images);
            //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            DrawImageInFrame_wxh_A4(img1, page, 0, w, h, aspectRatioImage1);
            //DrawImageInFrame_wxh(img2, page, h + heightBtwImages, w, h, aspectRatioImage2);

        }


        private void DrawImagesInPage_A5(PdfImage img1, PdfImage img2, PdfPageBase page, float w, float h,
            float distance_btw_images, AspectRatio aspectRatioImage1 , AspectRatio aspectRatioImage2 )
        {
            float heightBtwImages = PrintIO.InPoints(distance_btw_images);
            //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            DrawImageInFrame_wxh_A5(img1, page, 0, w, h, aspectRatioImage1);
            DrawImageInFrame_wxh_A5(img2, page, h+heightBtwImages, w, h, aspectRatioImage2);
            //if (img2 != null)
            //{
            //    if (aspectRatioImage2 == AspectRatio.S4x3)
            //    {

        }


        private void DrawImagesInPage_Postcard(PdfImage img1, PdfImage img2, PdfPageBase page, float w, float h,
           float distance_btw_images, AspectRatio aspectRatioImage1, AspectRatio aspectRatioImage2)
        {
            float heightBtwImages = PrintIO.InPoints(distance_btw_images);
            //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            DrawImageInFrame_wxh_Postcard(img1, page, 0, w, h, aspectRatioImage1);
            DrawImageInFrame_wxh_Postcard(img2, page, h + heightBtwImages, w, h, aspectRatioImage2);
            //if (img2 != null)
            //{
            //    if (aspectRatioImage2 == AspectRatio.S4x3)
            //    {

        }


        private void DrawImageInFrame_wxh_Passport(PdfImage img, PdfPageBase page,float x, float y, float w, float h)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            if (img != null)
            {
                page.Canvas.DrawImage(img, new System.Drawing.RectangleF(x, y, w, h));
            }

        }

        private void DrawImageInFrame_wxh_A4(PdfImage img, PdfPageBase page, float y, float w, float h, AspectRatio aspectRatioImage1)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

            //Load an image
            if (img != null)
            {
                if (aspectRatioImage1 == AspectRatio.S4x3)
                {
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y, w, h));
                }
                else
                {
                    page.Canvas.DrawRectangle(brush, new Rectangle(new Point(0, (int)y), new Size(Convert.ToInt32(w), Convert.ToInt32(h))));
                }
                if (aspectRatioImage1 == AspectRatio.GreaterThanS4x3)
                {

                    float new_w = h * img.Width / img.Height;
                    float new_MarginLeftRight = (w - new_w) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(new_MarginLeftRight, y, new_w, h)); //use width of frame, adjust height.
                    //Graphics.FromImage(img.);

                }
                if (aspectRatioImage1 == AspectRatio.LessThanS4x3)
                {

                    float new_h = w * img.Height / img.Width;
                    float new_MarginTopBottom = (h - new_h) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y + new_MarginTopBottom, w, new_h)); //use width of frame, adjust height.


                }
            }

        }
        private void DrawImageInFrame_wxh_A5(PdfImage img, PdfPageBase page,float y, float w, float h, AspectRatio aspectRatioImage1)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            
            //Load an image
            if (img != null)
            {
                if (aspectRatioImage1 == AspectRatio.S4x3)
                {
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y, w, h));
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



        private void DrawImageInFrame_wxh_Postcard(PdfImage img, PdfPageBase page, float y, float w, float h, AspectRatio aspectRatioImage1)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

            //Load an image

            if (img != null)
            {
                if (aspectRatioImage1 == AspectRatio.S6x4)
                {
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y, w, h));
                }
                else
                {
                    page.Canvas.DrawRectangle(brush, new Rectangle(new Point(0, (int)y), new Size(Convert.ToInt32(w), Convert.ToInt32(h))));
                }
                if (aspectRatioImage1 == AspectRatio.GreaterThanS6x4)
                {
                    float new_h = w * img.Height / img.Width;
                    float new_MarginTopBottom = (h - new_h) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y + new_MarginTopBottom, w, new_h)); //use width of frame, adjust height.

                }
                if (aspectRatioImage1 == AspectRatio.LessThanS6x4)
                {
                    float new_w = h * img.Width / img.Height;
                    float new_MarginLeftRight = (w - new_w) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(new_MarginLeftRight, y, new_w, h)); //use width of frame, adjust height.
                    //Graphics.FromImage(img.);

                }
            }

        }

        //private void PDFDocumentViewer(string fileName)
        //{
        //    try
        //    {
        //        System.Diagnostics.Process.Start(fileName);
        //    }
        //    catch { }
        //}





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



        private void button1_Click(object sender, EventArgs e)
        {
            PrintSettings pst = new PrintSettings(SelectedImages);
            pst.PrintDir = Globals.PrintDir;
            //pst.SelectedImages = SelectedImages;
            pst.ShowDialog();
        }

        private void PicsbgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
            PicsbgWorker.ReportProgress(25);

            PrintIO.Generate_receipt(SelectedImages, taxinvoicenumber+".pdf", taxinvoicenumber);
            PrintReceipt();
            PicsbgWorker.ReportProgress(50);
           
            GeneratePicsPDF();


            //PrintWatch.Interval = 500;
            //PrintWatch.Enabled = true;
            
            PicsbgWorker.ReportProgress(75);
            PrintPicsPDF();
            //CheckPrinterQueue();
            PicsbgWorker.ReportProgress(100);
            
        }
        
        private void PrintPicsPDF()
        {
            if (CheckPhotoPrinterStatus())
            PrintIO.PrintPDFs(Globals.PrintDir);
        }

        private bool CheckPhotoPrinterStatus()
        {
            //throw new NotImplementedException();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (ConfigurationManager.AppSettings["PhotoPrinterName"].Equals(printer))
                    return true;
            }
            return false;
        }

        private void PrintReceipt()
        {
            if(CheckReceiptPrinterStatus())
            PrintIO.PrintReceipt(Globals.receiptDir, taxinvoicenumber);
        }

        private bool CheckReceiptPrinterStatus()
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (ConfigurationManager.AppSettings["ReceiptPrinterName"].Equals(printer))
                    return true;
            }
            return false;
        }

        private void PicsbgWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (PrintState.alert && MessageBox.Show("Printing issue: " + ((PrinterState)e.UserState).status, "Print Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
            {
                PrintIO.AbortPrinting();
            }

            if (e.ProgressPercentage == 25 )
            {
                PrintStatusLbl.Text = "Printing receipt";
               
            }
                
            if (e.ProgressPercentage == 50)
                PrintStatusLbl.Text = "Printing pics";
            if (e.ProgressPercentage == 75)
                PrintStatusLbl.Text = "Checking print status";
            //if (e.ProgressPercentage == 100)
                //PrintStatusLbl.Text = "Finished";

        }

        private void PicsbgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintIO.Generate_receipt(SelectedImages, taxinvoicenumber+".pdf",taxinvoicenumber);
            PrintReceipt();
        }

        private void PrintWatch_Tick(object sender, EventArgs e)
        {
            PrintState = PrintIO.CheckPrinterQueue(ConfigurationManager.AppSettings["PhotoPrinterName"]);

            if (PrintJobStatus.None != PrintState.status)
            {
                PrintStatusLbl.Visible = true;
            PrintStatusLbl.Text = PrintState.status.ToString();
            }
            else
            {
                //status is None
                PrintStatusLbl.Visible = false;
            }
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            FlexMessageBox fmsg = new FlexMessageBox();
            fmsg.TopMost = true;
            fmsg.ShowIcon = false;
            fmsg.ShowDialog();

            if (fmsg.Result == DialogResult.Yes)
            {
                if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                    logger.Log(NLog.LogLevel.Info, "Inside Finish function. Closing forms");
                List<Form> openForms = new List<Form>();

                foreach (Form f in Application.OpenForms)
                    openForms.Add(f);

                foreach (Form f in openForms)
                {
                    if (f.Name != "Main")
                        f.Close();
                }

            }
            // Application.Exit();
            //if (galleryFormObject != null && wifiHelpFormObject != null && AnimationFormObject != null)
            //{
            //    galleryFormObject.Close();
            //    galleryFormObject.Dispose();
            //    galleryFormObject = null;
            //    wifiHelpFormObject.Close();
            //    wifiHelpFormObject.Dispose();
            //    wifiHelpFormObject = null;
            //    waiterObject.Close();
            //    waiterObject.Dispose();
            //    waiterObject = null;
            //    this.Close();
            //    this.Dispose();
            //    Application.Exit();
            //}
        }

        private void Tb_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
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

            //if (!btn_print.Enabled)
            //{
            //    btn_print.Enabled = true;
            //}
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
