using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PicsDirectoryDisplayWin
{

    public static class Globals
    {
        public enum PrintSize
        {
            A4,A5,Passport,Postcard,pdf
        }

        private static PrintSize _PrintSelection= PrintSize.A5;
        private static int _IncludeMaxImages = 6;
        public static PrintSize PrintSelection { get { return _PrintSelection; } }
        public static void SetPrintSelection(PrintSize printSize) { _PrintSelection = printSize; }

        //public static int NoOfTotalDirsFound = 0;
        public static readonly int IncludeDirectoryContainingMinImages = 1;
        public static int IncludeMaxImages {
            get {
                if (PrintSelection == PrintSize.Passport)
                    _IncludeMaxImages = 8;
                //if (PrintSelection == PrintSize.Postcard)
                //    _IncludeMaxImages = 6;
                return _IncludeMaxImages;
            }
        }
        public static readonly int MaxDirectoryToSearchLimit = 50;
        //public static string USBSearchPath = "";

        public static readonly int PassportImageCountInAPage = 8;
        public static readonly int PostcardImageCountInAPage = 2;


        public static readonly string logDirPath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\PrintAPic";
        public static readonly string logDir = logDirPath + "\\log.txt";
        public static readonly string receiptDir = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\PrintAPic" + "\\Receipt\\";
        public static readonly string PrintDir = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\PrintAPic" + "\\Prints\\";
        public static readonly string ProcessedImagesDir = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\PrintAPic" + "\\Processed\\";
        // @"C:\Users\Arunav\Pictures\Camera Roll";
        //public static string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        //private static int _filesInWebSearchDir;
        //private static int _filesInThumbsDir;


      
    }

        public static class GlobalImageCache
    {
       
        private static string TableBackground = @"..\..\..\pics\Alien_Ink_2560X1600_Abstract_Background_1.jpg";
        private static string Arrow = @"..\..\..\pics\Blue_Left_Arrow_PNG_Clip_Art_Image.png";
        private static string wifiStep = @"..\..\..\pics\3718444199423493837.png";
        private static string BrowserStep = @"..\..\..\pics\sshot1.png";
        private static string WifiIcon = @"..\..\..\pics\3.png";
        private static string HorseAnim = @"..\..\..\pics\Horse_gallop.gif";
        private static string Logo = @"..\..\..\pics\HE logo.png";
        private static string TransferPics = @"..\..\..\pics\printapicsite.png";


        private static Image _TransferPics;
        public static Image TransferPic
        {
            get
            {
                if (_TransferPics == null)
                    _TransferPics = GetImage(TransferPics);

                return _TransferPics;
            }
        }

        private static Image _LogoImg;
        public static Image LogoImg
        {
            get
            {
                if (_LogoImg == null)
                    _LogoImg = GetImage(Logo);

                return _LogoImg;
            }
        }


        private static Image _HorseAnimImg;
        public static Image HorseAnimImg
        {
            get
            {
                if (_HorseAnimImg == null)
                    _HorseAnimImg = GetImage(HorseAnim);

                return _HorseAnimImg;
            }
        }


        private static Image _BrowserStepImg;
        public static Image BrowserStepImg
        {
            get
            {
                if (_BrowserStepImg == null)
                    _BrowserStepImg = GetImage(BrowserStep);

                return _BrowserStepImg;
            }
        }


        private static Image _WifiIconImg;
        public static Image WifiIconImg
        {
            get
            {
                if (_WifiIconImg == null)
                    _WifiIconImg = GetImage(WifiIcon);

                return _WifiIconImg;
            }
        }

        private static Image _wifiStepImg;
        public static Image wifiStepImg
        {
            get
            {
                if (_wifiStepImg == null)
                    _wifiStepImg = GetImage(wifiStep);

                return _wifiStepImg;
            }
        }

        private static Image _ArrowImg;
        public static Image ArrowImg { get{
                if (_ArrowImg == null)
                    _ArrowImg = GetImage(Arrow);

                    return _ArrowImg;
            } }

        private static Image _TableBgImg;
        public static Image TableBgImg
        {
            get
            {
                if (_TableBgImg == null)
                    _TableBgImg = GetImage(TableBackground);

                return _TableBgImg;
            }
        }

        private static Image GetImage(string path)
        {
            FileStream fread = null;
            try
            {
                using (fread = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.None))
                    return Image.FromStream(fread);
            }
            finally
            {
                if (fread!=null)
                {
                    fread.Close(); fread.Dispose();
                }
               
            }
         
        }
  

    }
}
