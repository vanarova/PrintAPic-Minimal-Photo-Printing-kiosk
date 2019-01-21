using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicsDirectoryDisplayWin.lib
{
    public class ChitraKiAlbumAurVivaran
    {
        public string ImageName { get; set; }
        public string ImageFullName { get; set; }
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
