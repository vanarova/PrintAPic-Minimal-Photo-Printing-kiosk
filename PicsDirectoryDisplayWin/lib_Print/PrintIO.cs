
using System;
using System.Collections.Generic;
using System.Configuration;
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

    class PrintIO
    {

        internal static void PrintPDFs(string PrintDir)
        {
            using (PdfDocument pdf = new PdfDocument())
            {
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
        }

        internal static int PrintPDF(string PrintDir, int index)
        {
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

        internal static void PrintReceipt(string receiptDir,string taxinvoicenumber)
        {

            using (PdfDocument pdf = new PdfDocument())
            {
                pdf.LoadFromFile(receiptDir + taxinvoicenumber + ".pdf");
                pdf.PrintSettings.PrinterName = ConfigurationManager.AppSettings["ReceiptPrinterName"];
                pdf.Print();
            }

        }
        internal static PrinterState CheckPrinterQueue(string printerName)
        {
            //List<PrintJobStatus> prints = new List<PrintJobStatus>();
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
            //throw new NotImplementedException();
        }
    }
}
