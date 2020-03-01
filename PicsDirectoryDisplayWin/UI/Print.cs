using PicsDirectoryDisplayWin.lib_Print;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Printing;
using System.Windows.Forms;
using static PicsDirectoryDisplayWin.Globals;

namespace PicsDirectoryDisplayWin.UI
{
    public partial class Print : Form
    {
        public int selectedImagesCount;

        private Form AnimationFormObject = null;

        private Form galleryFormObject = null;

        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //string receiptDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +"\\Receipt\\";
        //string PrintDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Prints\\";
        private PrinterState PrintState = new PrinterState();

        private string taxinvoicenumber =
       DateTime.Now.Day.ToString()
       + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.Hours.ToString()
       + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString();

        private Form waiterObject = null;
        private Form wifiHelpFormObject = null;
        public Print(Form gallery, Form wifiHelp, Form animation, Form waiter, int SelectedImagesCount)
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
            label8.Text = ConfigurationManager.AppSettings["CostValue" + Globals.PrintSelection.ToString()];
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

        public List<string> SelectedImages { get; set; }
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
            PicsbgWorker.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintSettings pst = new PrintSettings(SelectedImages);
            pst.PrintDir = Globals.PrintDir;
            pst.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintIO.Generate_receipt(SelectedImages, taxinvoicenumber + ".pdf", taxinvoicenumber);
            PrintReceipt();
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
        }

        private bool CheckPhotoPrinterStatus()
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (ConfigurationManager.AppSettings["PhotoPrinterName"].Equals(printer))
                    return true;
            }
            return false;
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

        private void GeneratePicsPDF()
        {
            //Start();
            PdfImage image1 = null;
            PdfImage image2 = null;
            AspectRatio aspectRatio1 = AspectRatio.S4x3;
            AspectRatio aspectRatio2 = AspectRatio.S4x3;


            if (Globals.PrintSelection == Globals.PrintSize.A5)
            {
                for (int i = 0; i < SelectedImages.Count; i = i + 2)
                {
                    image1 = null; image2 = null;
                    //first image on page
                    image1 = PdfImage.FromFile(SelectedImages[i].Split('|')[0]); //Take Image name from image key
                    aspectRatio1 = PrintIO.DetectImageAspectRatio_A5(image1);
                    //second image on page
                    if (!(i + 1 >= SelectedImages.Count))
                    {
                        image2 = PdfImage.FromFile(SelectedImages[i + 1].Split('|')[0]);
                        aspectRatio2 = PrintIO.DetectImageAspectRatio_A5(image2);
                    }
                    PrintIO.SetPageMarginGeneratePDF_ImageRatio4x3_A5(Globals.PrintDir + "Print" + i + ".pdf", image1, image2, aspectRatio1, aspectRatio2);
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
                    aspectRatio1 = PrintIO.DetectImageAspectRatio_A4(image1);
                    PrintIO.SetPageMarginGeneratePDF_ImageRatio4x3_A4(Globals.PrintDir + "Print" + i + ".pdf", image1, aspectRatio1);
                }
            }
            else if (Globals.PrintSelection == Globals.PrintSize.Passport)
            {
                image1 = PdfImage.FromFile(SelectedImages[0].Split('|')[0]); //Take Image name from image key
                PrintIO.SetPageMarginGeneratePDF_ImageRatio4x3_Passport(Globals.PrintDir + "Print" + 0 + ".pdf", image1);
            }
            else if (Globals.PrintSelection == Globals.PrintSize.Postcard)
            {
                for (int i = 0; i < SelectedImages.Count; i = i + 2)
                {
                    image1 = null; image2 = null;
                    //first image on page
                    image1 = PdfImage.FromFile(SelectedImages[i].Split('|')[0]); //Take Image name from image key
                    aspectRatio1 = PrintIO.DetectImageAspectRatio_Postcard(image1);
                    //second image on page
                    if (!(i + 1 >= SelectedImages.Count))
                    {
                        image2 = PdfImage.FromFile(SelectedImages[i + 1].Split('|')[0]);
                        aspectRatio2 = PrintIO.DetectImageAspectRatio_Postcard(image2);
                    }
                    PrintIO.SetPageMarginGeneratePDF_ImageRatio6x4_Postcard(Globals.PrintDir + "Print" + i + ".pdf", image1, image2, aspectRatio1, aspectRatio2);
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

        

        private void PicsbgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            PicsbgWorker.ReportProgress(25);

            PrintIO.Generate_receipt(SelectedImages, taxinvoicenumber + ".pdf", taxinvoicenumber);
            PrintReceipt();
            PicsbgWorker.ReportProgress(50);

            GeneratePicsPDF();

            PicsbgWorker.ReportProgress(75);
            PrintPicsPDF();
            //CheckPrinterQueue();
            PicsbgWorker.ReportProgress(100);
        }

        private void PicsbgWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (PrintState.alert && MessageBox.Show("Printing issue: " + ((PrinterState)e.UserState).status, "Print Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
            {
                PrintIO.AbortPrinting();
            }

            if (e.ProgressPercentage == 25)
            {
                PrintStatusLbl.Text = "Printing receipt";
            }

            if (e.ProgressPercentage == 50)
                PrintStatusLbl.Text = "Printing pics";
            if (e.ProgressPercentage == 75)
                PrintStatusLbl.Text = "Checking print status";
        }

        private void PicsbgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
        }

        private void PrintPicsPDF()
        {
            if (CheckPhotoPrinterStatus())
                PrintIO.PrintPDFs(Globals.PrintDir);
        }

        private void PrintReceipt()
        {
            if (CheckReceiptPrinterStatus())
                PrintIO.PrintReceipt(Globals.receiptDir, taxinvoicenumber);
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

        private void Tb_Paint(object sender, PaintEventArgs e)
        {
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
    }
}