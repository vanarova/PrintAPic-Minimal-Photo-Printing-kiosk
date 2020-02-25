
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace PicsDirectoryDisplayWin.lib_Print
{

    public class PrinterState
    {
        public string message { get; set; }
        public bool alert { get; set; }
        public PrintJobStatus status { get; set; }

    }

    public class PrintIO
    {
       static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
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

        public static int PrintPDF(string PrintDir, int index)
        {
            if (ConfigurationManager.AppSettings["Mode"] == "Diagnostic")
                logger.Log(NLog.LogLevel.Info, "Inside PrintPDFs function. print dir:" + PrintDir);
            using (PdfDocument pdf = new PdfDocument())
            {
                pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["PhotoPrinterName"];
                //search all files prresent in "printdir"
                //load and print them one by one.
                FileInfo file = new FileInfo(PrintDir +"//Print" +index.ToString()+".pdf");
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



        internal static void PrintPic(string pic)
        {
            using (PdfDocument pdf = new PdfDocument())
            {
                pic = pic.Replace("thumbs/","");
                FileInfo file = new FileInfo(pic);
                if (file.Exists)
                {
                    pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["PhotoPrinterName"];
                    pdf.LoadFromFile(file.FullName);
                    pdf.Print();
                }
            }
        }

        public static void PrintReceipt(string receiptDir,string taxinvoicenumber)
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

        internal static float InPoints(float value)
        {
            return new PdfUnitConvertor().ConvertUnits(value, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
        }

        public static void Generate_receipt(List<string> SelectedImages , string ImageFileName, string taxinvoicenumber)
        {
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();

            PdfMargins margin = new PdfMargins();
            margin.Top = InPoints(0.5f);//unitCvtr.ConvertUnits(MARGINTOP_BOTTOM, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            //margin.Left = InPoints(0.5f); //unitCvtr.ConvertUnits(MARGINLEFT_RIGHT, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            //margin.Right = margin.Left;

            //PdfPageSettings settings = new PdfPageSettings();
            //settings.Size = new SizeF(InPoints(7.2f), InPoints(7.2f)); //here should 72mmx height
            //PaperSize psizee = new PaperSize();
            //psizee.Height = (int)settings.Size.Height;
            //psizee.Width = (int)settings.Size.Width;
            //doc.PrintSettings.PaperSize = psizee;
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
                (((Convert.ToDouble(ConfigurationManager.AppSettings["CostValue"+ Globals.PrintSelection.ToString()]) *
                SelectedImages.Count) * Convert.ToDouble(ConfigurationManager.AppSettings["GSTValue"]) /
                100) +
                (Convert.ToDouble(ConfigurationManager.AppSettings["CostValue"+ Globals.PrintSelection.ToString()]) * SelectedImages.Count)
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



            //page.Canvas.DrawString(ConfigurationManager.AppSettings["Line1"], font10, brush1, page.Canvas.ClientSize.Width, 185, rightAlignment);
            //page.Canvas.DrawString(ConfigurationManager.AppSettings["Line2"] + taxinvoicenumber, font10, brush1, page.Canvas.ClientSize.Width, 200, rightAlignment);
            //page.Canvas.DrawString(ConfigurationManager.AppSettings["Line3"] + SelectedImages.Count + "/-", font10, brush1, page.Canvas.ClientSize.Width, 215, rightAlignment);
            //page.Canvas.DrawString(ConfigurationManager.AppSettings["Line4"] + ConfigurationManager.AppSettings["CostValue"] + "/-"
            //    , font10, brush1, page.Canvas.ClientSize.Width, 230, rightAlignment);
            //page.Canvas.DrawString(ConfigurationManager.AppSettings["Line5"] +
            //    ConfigurationManager.AppSettings["GSTValue"] + "%", font10, brush1, page.Canvas.ClientSize.Width, 245, rightAlignment);
            //page.Canvas.DrawString(ConfigurationManager.AppSettings["Line6"], font10, brush1, page.Canvas.ClientSize.Width, 250, rightAlignment);
            //page.Canvas.DrawString(ConfigurationManager.AppSettings["Line7"] + total + "/-", font10, brush1, page.Canvas.ClientSize.Width, 265, rightAlignment);


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

        internal static void PDFDocumentViewer(string fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch { }
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
    }
}
