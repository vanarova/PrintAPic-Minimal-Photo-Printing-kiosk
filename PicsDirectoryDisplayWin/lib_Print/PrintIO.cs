using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Printing;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace PicsDirectoryDisplayWin.lib_Print
{
    public class PrinterState
    {
        public bool alert { get; set; }
        public string message { get; set; }
        public PrintJobStatus status { get; set; }
    }

    public class PrintIO
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void SetPageMarginGeneratePDF_ImageRatio6x4_Postcard(string ImageFileName, PdfImage img1, PdfImage img2,
           Globals.AspectRatio aspectRatioImage1 = Globals.AspectRatio.S6x4, Globals.AspectRatio aspectRatioImage2 = Globals.AspectRatio.S6x4)
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

            //Adjust width, height with 4:3 ratio in PDF points :
            float A4PostMarginWidthPts4x3 = PdfPageSize.A4.Width - (margin.Left * 2); // left,right margins
            float A4PostMarginHeightPts4x3 = A4PostMarginWidthPts4x3 * 0.67f;  // height = 3/4 * width

            //TransformText(page);
            DrawImagesInPage_Postcard(img1, img2, page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3,
                DISTANCE_BTW_IMAGES, aspectRatioImage1, aspectRatioImage2);
            //TransformImage(page);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                PrintIO.PDFDocumentViewer(ImageFileName);

        }

        internal static void AbortPrinting()
        {
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside Abort printing function");
            //List<PrintJobStatus> prints = new List<PrintJobStatus>();
            PrinterState pstate = new PrinterState();
            var printServer = new PrintServer();
            var myPrintQueues = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });

            foreach (PrintQueue pq in myPrintQueues)
            {
                pq.Refresh();
                //if (!pq.Name.ToLower().Contains(printerName.ToLower())) continue;
                PrintJobInfoCollection jobs = pq.GetPrintJobInfoCollection();
                foreach (PrintSystemJobInfo job in jobs)
                {
                    job.Cancel();
                }// end for each print job
            }// end for each print queue
        }

        internal static PrinterState CheckPrinterQueue(string printerName)
        {
            //List<PrintJobStatus> prints = new List<PrintJobStatus>();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside CheckPrinterQueue function");
            PrinterState pstate = new PrinterState();
            var printServer = new PrintServer();
            var myPrintQueues = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });

            foreach (PrintQueue pq in myPrintQueues)
            {
                pq.Refresh();
                if (!pq.Name.ToLower().Contains(printerName.ToLower())) continue;
                PrintJobInfoCollection jobs = pq.GetPrintJobInfoCollection();
                foreach (PrintSystemJobInfo job in jobs)
                {
                    if (job.JobStatus == PrintJobStatus.PaperOut ||
                        job.JobStatus == PrintJobStatus.UserIntervention ||
                        job.JobStatus == PrintJobStatus.Offline ||
                        job.JobStatus == PrintJobStatus.Error ||
                        job.JobStatus == PrintJobStatus.Blocked)
                    {
                        pstate.alert = true;
                        pstate.status = job.JobStatus;
                    }
                    else
                    {
                        pstate.alert = false;
                        pstate.status = job.JobStatus;
                    }

                    //var aux = job;
                    //Check job status here
                    //job.JobStatus
                }// end for each print job
            }// end for each print queue

            return pstate;
        }

        internal static Globals.AspectRatio DetectImageAspectRatio_A4(PdfImage image)
        {
            logger.Info("Printing image");
            float height = image.Width;// Swap width and height in case of A4
            float width = image.Height;
            //if (width < height)
            //    logger.Error("Image rotation is needed");

            float ap = width / height;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) < 0.1) //compare with 4/3 aspect ratio, if equal
                return Globals.AspectRatio.S4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1
                && (Math.Round(ap, 2) - 1.33) < 0) // -ve value means aspect ratio is lower than 4x3
                return Globals.AspectRatio.LessThanS4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1
                && (Math.Round(ap, 2) - 1.33) > 0) // +ve value means aspect ratio is higher than 4x3, could be 3x2 = 1.5
                return Globals.AspectRatio.GreaterThanS4x3;

            return Globals.AspectRatio.S4x3;
        }

        internal static Globals.AspectRatio DetectImageAspectRatio_A5(PdfImage image)
        {
            logger.Info("Printing image");
            float width = image.Width;
            float height = image.Height;
            if (width < height)
                logger.Error("Image rotation is needed");

            float ap = width / height;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) < 0.1) //compare with 4/3 aspect ratio, if equal
                return Globals.AspectRatio.S4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1
                && (Math.Round(ap, 2) - 1.33) < 0) // -ve value means aspect ratio is lower than 4x3
                return Globals.AspectRatio.LessThanS4x3;
            if (Math.Abs(Math.Round(ap, 2) - 1.33) > 0.1
                && (Math.Round(ap, 2) - 1.33) > 0) // +ve value means aspect ratio is higher than 4x3, could be 3x2 = 1.5
                return Globals.AspectRatio.GreaterThanS4x3;
            return Globals.AspectRatio.S4x3;
        }

        internal static Globals.AspectRatio DetectImageAspectRatio_Postcard(PdfImage image)
        {
            logger.Info("Printing image");
            float height = image.Width;// Swap width and height in case of A4
            float width = image.Height;
            //if (width < height)
            //    logger.Error("Image rotation is needed");

            float ap = width / height;
            if (Math.Abs(Math.Round(ap, 2) - 1.5) < 0.1) //compare with 4/3 aspect ratio, if equal
                return Globals.AspectRatio.S6x4;
            if (Math.Abs(Math.Round(ap, 2) - 1.5) > 0.1
                && (Math.Round(ap, 2) - 1.5) < 0) // -ve value means aspect ratio is lower than 4x3
                return Globals.AspectRatio.LessThanS6x4;
            if (Math.Abs(Math.Round(ap, 2) - 1.5) > 0.1
                && (Math.Round(ap, 2) - 1.5) > 0) // +ve value means aspect ratio is higher than 4x3, could be 3x2 = 1.5
                return Globals.AspectRatio.GreaterThanS6x4;

            return Globals.AspectRatio.S6x4;
        }

        internal static void Generate_receipt(List<string> SelectedImages, string ImageFileName, string taxinvoicenumber)
        {
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

            PdfMargins margin = new PdfMargins();
            margin.Top = InPoints(0.5f);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
           
            //Create one page
            PdfPageBase page = doc.Pages.Add(new SizeF(InPoints(float.Parse(ConfigurationManager.AppSettings["ReceiptWidth"])),
                InPoints(float.Parse(ConfigurationManager.AppSettings["ReceiptHeight"]))), margin);
            //PdfFont font12 = new PdfFont(PdfFontFamily.Helvetica, 12f);
            PdfFont font10 = new PdfFont(PdfFontFamily.Helvetica, 10f);
            //PdfFont font8 = new PdfFont(PdfFontFamily.Helvetica, 8f);
            PdfSolidBrush brush1 = new PdfSolidBrush(Color.Black);
            PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            PdfStringFormat rightAlignment = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);

            string total = Math.Round(
                (((Convert.ToDouble(ConfigurationManager.AppSettings["CostValue" + Globals.PrintSelection.ToString()]) *
                SelectedImages.Count) * Convert.ToDouble(ConfigurationManager.AppSettings["GSTValue"]) /
                100) +
                (Convert.ToDouble(ConfigurationManager.AppSettings["CostValue" + Globals.PrintSelection.ToString()]) * SelectedImages.Count)
                ), 2
                ).ToString();

            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line1"], font10, brush1, page.Canvas.ClientSize.Width, 10, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line2"] + taxinvoicenumber, font10, brush1, page.Canvas.ClientSize.Width, 25, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line3"] + SelectedImages.Count + "/-", font10, brush1, page.Canvas.ClientSize.Width, 40, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line4"] + ConfigurationManager.AppSettings["CostValue"] + "/-"
                , font10, brush1, page.Canvas.ClientSize.Width, 55, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line5"] +
                ConfigurationManager.AppSettings["GSTValue"] + "%", font10, brush1, page.Canvas.ClientSize.Width, 70, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line6"], font10, brush1, page.Canvas.ClientSize.Width, 85, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line7"] + total + "/-", font10, brush1, page.Canvas.ClientSize.Width, 100, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line8"], font10, brush1, page.Canvas.ClientSize.Width, 120, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line9"], font10, brush1, page.Canvas.ClientSize.Width, 130, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line10"], font10, brush1, page.Canvas.ClientSize.Width, 140, rightAlignment);
            page.Canvas.DrawString(ConfigurationManager.AppSettings["Line11"], font10, brush1, page.Canvas.ClientSize.Width, 155, rightAlignment);

            
            //Save the document
            doc.SaveToFile(Globals.receiptDir + ImageFileName);
            doc.SaveToFile(ConfigurationManager.AppSettings["ReceiptBackupDir"] + "\\" + ImageFileName);
            doc.Close();

            //Launch the Pdf file
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
            {
                logger.Log(NLog.LogLevel.Info, "Inside Generate_receipt function.");
                PDFDocumentViewer(Globals.receiptDir + ImageFileName);
            }
        }

        internal static float InPoints(float value)
        {
            return new PdfUnitConvertor().ConvertUnits(value, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        }

        internal static void PDFDocumentViewer(string fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch { }
        }

        internal static int PrintPDF(string PrintDir, int index)
        {
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside PrintPDFs function. print dir:" + PrintDir);
            using (PdfDocument pdf = new PdfDocument())
            {
                pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["PhotoPrinterName"];
                //search all files prresent in "printdir"
                //load and print them one by one.
                FileInfo file = new FileInfo(PrintDir + "//Print" + index.ToString() + ".pdf");
                if (file.Exists)
                {
                    pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["PhotoPrinterName"];
                    pdf.LoadFromFile(file.FullName);
                    pdf.Print();
                }
                else
                {
                    return 1;
                }
                return 0;
            }
        }

        internal static void PrintPDFs(string PrintDir)
        {
            using (PdfDocument pdf = new PdfDocument())
            {
                if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                    logger.Log(NLog.LogLevel.Info, "Inside PrintPDFs function. print dir:" + PrintDir);

                pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["PhotoPrinterName"];
                //search all files prresent in "printdir"
                //load and print them one by one.
                IEnumerable<FileInfo> files = new DirectoryInfo(PrintDir).EnumerateFiles();
                foreach (var item in files)
                {
                    // for pdf, show page selection dialog
                    //pdf.PrintSettings.SelectSomePages()
                    pdf.LoadFromFile(item.FullName);
                    pdf.Print();
                    if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                        logger.Log(NLog.LogLevel.Info, "Printing image file :" + item.FullName);
                }
            }
        }
        internal static void PrintPic(string pic)
        {
            using (PdfDocument pdf = new PdfDocument())
            {
                pic = pic.Replace("thumbs/", "");
                FileInfo file = new FileInfo(pic);
                if (file.Exists)
                {
                    pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["PhotoPrinterName"];
                    pdf.LoadFromFile(file.FullName);
                    pdf.Print();
                }
            }
        }

        internal static void PrintReceipt(string receiptDir, string taxinvoicenumber)
        {
            using (PdfDocument pdf = new PdfDocument())
            {
                pdf.LoadFromFile(receiptDir + taxinvoicenumber + ".pdf");
                pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["ReceiptPrinterName"];
                pdf.Print();
                if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                    logger.Log(NLog.LogLevel.Info, "Printing receipt file :" + taxinvoicenumber + ".pdf");
            }
        }
        internal static void SetPageMarginGeneratePDF_ImageRatio4x3_A4(string ImageFileName, PdfImage img1,
     Globals.AspectRatio aspectRatioImage1 = Globals.AspectRatio.S4x3)
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

            DrawImagesInPage_A4(img1, page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3, DISTANCE_BTW_IMAGES, aspectRatioImage1);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                PrintIO.PDFDocumentViewer(ImageFileName);

        }

        internal static void SetPageMarginGeneratePDF_ImageRatio4x3_A5(string ImageFileName, PdfImage img1, PdfImage img2,
           Globals.AspectRatio aspectRatioImage1 = Globals.AspectRatio.S4x3, Globals.AspectRatio aspectRatioImage2 = Globals.AspectRatio.S4x3)
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

            //Adjust width, height with 4:3 ratio in PDF points :
            float A4PostMarginWidthPts4x3 = PdfPageSize.A4.Width - (margin.Left * 2); // left,right margins
            float A4PostMarginHeightPts4x3 = A4PostMarginWidthPts4x3 * 0.75f;  // height = 3/4 * width


            //TransformText(page);
            DrawImagesInPage_A5(img1, img2, page, A4PostMarginWidthPts4x3, A4PostMarginHeightPts4x3,
                DISTANCE_BTW_IMAGES, aspectRatioImage1, aspectRatioImage2);
            //TransformImage(page);

            //Save the document
            doc.SaveToFile(ImageFileName);
            doc.Close();

            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                PrintIO.PDFDocumentViewer(ImageFileName);

   
        }

        internal static void SetPageMarginGeneratePDF_ImageRatio4x3_Passport(string ImageFileName, PdfImage img1)
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


            //divde A4 210mm page to 6 parts to get 35mm image width
            float passport_35x45_width = PdfPageSize.A4.Width / 6; // left,right margins
            float passport_35x45_height = passport_35x45_width * 1.28f;  // calc height image size = 4.5 x 3.5 cm, width is known

            //Passport size image
            float postcard_101_1x152_4_width = PdfPageSize.A4.Width / 2.08f; //101.1
            float postcard_101_1x152_4_height = postcard_101_1x152_4_width * 1.5f; //151.6


            //Firt row
            float DistanceBtwSecondImg = passport_35x45_width + PrintIO.InPoints(1f);
            DrawImageInFrame_wxh_Passport(img1, page, 0, 0, passport_35x45_width, passport_35x45_height);
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
#region Private members

        private static void DrawImageInFrame_wxh_A4(PdfImage img, PdfPageBase page, float y, float w, float h, Globals.AspectRatio aspectRatioImage1)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

            //Load an image
            if (img != null)
            {
                if (aspectRatioImage1 == Globals.AspectRatio.S4x3)
                {
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y, w, h));
                }
                else
                {
                    page.Canvas.DrawRectangle(brush, new Rectangle(new Point(0, (int)y), new Size(Convert.ToInt32(w), Convert.ToInt32(h))));
                }
                if (aspectRatioImage1 == Globals.AspectRatio.GreaterThanS4x3)
                {
                    float new_w = h * img.Width / img.Height;
                    float new_MarginLeftRight = (w - new_w) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(new_MarginLeftRight, y, new_w, h)); //use width of frame, adjust height.
                    //Graphics.FromImage(img.);
                }
                if (aspectRatioImage1 == Globals.AspectRatio.LessThanS4x3)
                {
                    float new_h = w * img.Height / img.Width;
                    float new_MarginTopBottom = (h - new_h) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y + new_MarginTopBottom, w, new_h)); //use width of frame, adjust height.
                }
            }
        }

        private static void DrawImageInFrame_wxh_A5(PdfImage img, PdfPageBase page, float y, float w, float h, Globals.AspectRatio aspectRatioImage1)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

            //Load an image
            if (img != null)
            {
                if (aspectRatioImage1 == Globals.AspectRatio.S4x3)
                {
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y, w, h));
                }
                else
                {
                    page.Canvas.DrawRectangle(brush, new Rectangle(new Point(0, (int)y), new Size(Convert.ToInt32(w), Convert.ToInt32(h))));
                }
                if (aspectRatioImage1 == Globals.AspectRatio.GreaterThanS4x3)
                {
                    float new_h = w * img.Height / img.Width;
                    float new_MarginTopBottom = (h - new_h) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y + new_MarginTopBottom, w, new_h)); //use width of frame, adjust height.
                }
                if (aspectRatioImage1 == Globals.AspectRatio.LessThanS4x3)
                {
                    float new_w = h * img.Width / img.Height;
                    float new_MarginLeftRight = (w - new_w) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(new_MarginLeftRight, y, new_w, h)); //use width of frame, adjust height.
                    //Graphics.FromImage(img.);
                }
            }
        }

        private static void DrawImageInFrame_wxh_Passport(PdfImage img, PdfPageBase page, float x, float y, float w, float h)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            if (img != null)
            {
                page.Canvas.DrawImage(img, new System.Drawing.RectangleF(x, y, w, h));
            }
        }

        private static void DrawImageInFrame_wxh_Postcard(PdfImage img, PdfPageBase page, float y, float w, float h, Globals.AspectRatio aspectRatioImage1)
        {
            PdfBrush brush = new PdfSolidBrush(Color.Silver);
            // float heightBtwImages = unitCvtr.ConvertUnits(0.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

            //Load an image

            if (img != null)
            {
                if (aspectRatioImage1 == Globals.AspectRatio.S6x4)
                {
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y, w, h));
                }
                else
                {
                    page.Canvas.DrawRectangle(brush, new Rectangle(new Point(0, (int)y), new Size(Convert.ToInt32(w), Convert.ToInt32(h))));
                }
                if (aspectRatioImage1 == Globals.AspectRatio.GreaterThanS6x4)
                {
                    float new_h = w * img.Height / img.Width;
                    float new_MarginTopBottom = (h - new_h) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(0, y + new_MarginTopBottom, w, new_h)); //use width of frame, adjust height.
                }
                if (aspectRatioImage1 == Globals.AspectRatio.LessThanS6x4)
                {
                    float new_w = h * img.Width / img.Height;
                    float new_MarginLeftRight = (w - new_w) / 2;
                    page.Canvas.DrawImage(img, new System.Drawing.RectangleF(new_MarginLeftRight, y, new_w, h)); //use width of frame, adjust height.
                    //Graphics.FromImage(img.);
                }
            }
        }

        private static void DrawImagesInPage_A4(PdfImage img1, PdfPageBase page, float w, float h,
           float distance_btw_images, Globals.AspectRatio aspectRatioImage1)
        {
            DrawImageInFrame_wxh_A4(img1, page, 0, w, h, aspectRatioImage1);
        }

        private static void DrawImagesInPage_A5(PdfImage img1, PdfImage img2, PdfPageBase page, float w, float h,
            float distance_btw_images, Globals.AspectRatio aspectRatioImage1, Globals.AspectRatio aspectRatioImage2)
        {
            float heightBtwImages = PrintIO.InPoints(distance_btw_images);
            //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            DrawImageInFrame_wxh_A5(img1, page, 0, w, h, aspectRatioImage1);
            DrawImageInFrame_wxh_A5(img2, page, h + heightBtwImages, w, h, aspectRatioImage2);
            //if (img2 != null)
            //{
            //    if (aspectRatioImage2 == AspectRatio.S4x3)
            //    {
        }

        private static void DrawImagesInPage_Postcard(PdfImage img1, PdfImage img2, PdfPageBase page, float w, float h,
                          float distance_btw_images, Globals.AspectRatio aspectRatioImage1, Globals.AspectRatio aspectRatioImage2)
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
#endregion
    }
}