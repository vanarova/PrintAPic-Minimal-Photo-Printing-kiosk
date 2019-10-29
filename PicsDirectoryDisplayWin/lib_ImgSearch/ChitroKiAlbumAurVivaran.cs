using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicsDirectoryDisplayWin.lib
{
    /// <summary>
    /// This object stores current image directory metadata and peerimages store actual images meta data
    /// </summary>
    public class ChitraKiAlbumAurVivaran
    {
        public string ImageName { get; set; }
        public string ImageFullName { get; set; }

        public string ImageThumbnailFullName
        {
            get
            {
                return ImageDirFullName + "\\thumbs\\" + ImageName.Replace(Path.GetExtension(ImageName),".jpg");
            }
        }

        public string ImagePostProcessedFullName
        {
            get
            {
                return Globals.ProcessedImagesDir + "\\" + ImageName.Replace(Path.GetExtension(ImageName), ".jpg");
            }
        }

        public string ImageDirName { get; set; }
        public string ImageDirFullName { get; set; }
        public int ImageDirTotalImages { get; set; }

        public Image ThumbnailImage { get; set; }

        /// <summary>
        /// List of files in a this folder. Max limit set. Only a certain number of max images are counted to increase performance.
        /// </summary>
        public List<ChitraKiAlbumAurVivaran> PeerImages { get; set; }

        public string ImageKey { get { return ImageFullName + "|"+ImageName ; } }
    }

}
