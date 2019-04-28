using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicsDirectoryDisplayWin
{

    public static class Globals
    {
        //public static int NoOfTotalDirsFound = 0;
        public static readonly int IncludeDirectoryContainingMinImages = 1;
        public static readonly int IncludeMaxImages = 20;
        public static readonly int MaxDirectoryToSearchLimit = 50;
        public static string USBSearchPath = "";// @"C:\Users\Arunav\Pictures\Camera Roll";
        //public static string WebSiteSearchDir = @"C:\inetpub\wwwroot\ps\Uploads\030357B624D9";
        //private static int _filesInWebSearchDir;
        //private static int _filesInThumbsDir;


        //public static int FilesInWebSearchDir
        //{
        //    get
        //    {
        //        if (_filesInWebSearchDir == 0)
        //            _filesInWebSearchDir = new DirectoryInfo(WebSiteSearchDir).GetFiles().Length;

        //        return _filesInWebSearchDir;
        //    }
        //}
        //public static int FilesInThumbsDir
        //{
        //    get
        //    {
        //        if (_filesInThumbsDir == 0)
        //            _filesInThumbsDir = new DirectoryInfo(Globals.WebSiteSearchDir + "\\thumbs").GetFiles().Length;

        //        return _filesInThumbsDir;
        //    }
        //}
    }

        public static class GlobalImageCache
    {
       
        private static string TableBackground = @"..\..\pics\Alien_Ink_2560X1600_Abstract_Background_1.jpg";
        private static string Arrow = @"..\..\pics\Blue_Left_Arrow_PNG_Clip_Art_Image.png";
        private static string wifiStep = @"..\..\pics\3718444199423493837.png";
        private static string BrowserStep = @"..\..\pics\sshot1.png";
        private static string WifiIcon = @"..\..\pics\Wifi.png";
        private static string HorseAnim = @"..\..\pics\Horse_gallop.gif";
        private static string Logo = @"..\..\pics\HE logo.png";

       


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
