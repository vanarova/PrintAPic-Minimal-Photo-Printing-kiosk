using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace PicsDirectoryDisplayWin.UI
{
    public partial class Print : Form
    {
        public List<string> SelectedImages { get; set; }
        public Print()
        {
            InitializeComponent();
            SelectedImages = new List<string>();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            //Start();
            PrintUsingSpirePDF();
        }

        private void PrintUsingSpirePDF()
        {
            ///Create a pdf document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //Set the margin
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = unitCvtr.ConvertUnits(1f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = margin.Top;
            //TODO: Add margins in global settings xml file.
            margin.Left = unitCvtr.ConvertUnits(0.75f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Right = margin.Left;

            //Create one page
            PdfPageBase page = doc.Pages.Add(PdfPageSize.A4, margin);
            
           

            //TransformText(page);
            DrawImage(page);
            //TransformImage(page);

            //Save the document
            doc.SaveToFile("Image.pdf");
            doc.Close();

            //Launch the Pdf file
            PDFDocumentViewer("Image.pdf");
        }

        private void DrawImage(PdfPageBase page)
        {
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            
            float heightBtwImages = unitCvtr.ConvertUnits(0.75f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            //Load an image
            PdfImage image = PdfImage.FromFile(@"C:\inetpub\wwwroot\ps\Uploads\nasa-89125-unsplash - Copy.jpg");
            PdfImage image2 = PdfImage.FromFile(@"C:\inetpub\wwwroot\ps\Uploads\20160530_200327.jpg");
            

            //float width = image.Width * 0.75f;
            //float height = image.Height * 0.75f;
            //float x = (page.Canvas.ClientSize.Width - width) / 2;

            //Draw the image
            //page.Canvas.DrawImage(image, x, 60, width, height);
            float rectangleHeight = page.Canvas.ClientSize.Height/2- heightBtwImages;
            page.Canvas.DrawImage(image, new System.Drawing.RectangleF(0, 0, page.Canvas.ClientSize.Width, rectangleHeight));
            page.Canvas.DrawImage(image2, new System.Drawing.RectangleF(0, rectangleHeight + heightBtwImages, page.Canvas.ClientSize.Width, rectangleHeight));

            //if (image2.Height > image2.Width)
            //{
            //    //Rotate image
            //    PdfTemplate template = new PdfTemplate(page.Canvas.ClientSize.Width, rectangleHeight);
            //    template.Graphics.RotateTransform(90);
            //    template.Graphics.DrawImage(image2, new System.Drawing.RectangleF(0, rectangleHeight + heightBtwImages, page.Canvas.ClientSize.Width, rectangleHeight));

            //}

           
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
