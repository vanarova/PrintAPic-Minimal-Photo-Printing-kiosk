using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicsDirectoryDisplayWin.lib
{
    public class ChitraKhoj
    {
        private String SearchDirectory;
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        private int NoOfTotalDirsFound = 0;
        private readonly int IncludeDirectoryContainingMinImages = 1;
        private readonly int IncludeMaxImages = 20;
        private readonly int MaxDirectoryToSearchLimit = 50;

        public ChitraKhoj(string searchDirectory)
        {
            SearchDirectory = searchDirectory;
        }

        void WalkDirectoryTree(System.IO.DirectoryInfo root, IProgress<ChitraKiAlbumAurVivaran> progress)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;
            if (NoOfTotalDirsFound > MaxDirectoryToSearchLimit)
                return;
            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.jpg");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                log.Add(e.Message);
            }

            if (files != null)
            {
                int count = 0; List<ChitraKiAlbumAurVivaran> peerImages = new List<ChitraKiAlbumAurVivaran>();
                int ImageLimit;
                // if image count is lower than min images, leave this directory
                if (files.Length < IncludeDirectoryContainingMinImages)
                    return;

                if (files.Length > IncludeMaxImages)
                    ImageLimit = IncludeMaxImages;
                else
                    ImageLimit = files.Length;
                foreach (System.IO.FileInfo fi in files)
                    {
                        peerImages.Add(new ChitraKiAlbumAurVivaran()
                        {
                            ImageName = fi.Name,
                            ImageFullName = fi.FullName,
                            ImageDirName = root.Name,
                            ImageDirFullName = root.FullName,
                            ImageDirTotalImages = files.Length
                        });

                        if (count >= ImageLimit-1)
                        {
                            count = 0;
                            NoOfTotalDirsFound++;
                            progress.Report(new ChitraKiAlbumAurVivaran()
                            {
                                ImageName = "(" + (files.Length - ImageLimit) + ") More Images",
                                ImageFullName = "..\\..\\..\\pics\\vst.png",
                                ImageDirName = root.Name,
                                ImageDirFullName = root.FullName,
                                ImageDirTotalImages = files.Length,
                                PeerImages = peerImages
                            });
                            //progress.Report(root.Name +" | " + root.FullName + " | "+fi.FullName + " | " + fi.Name + " | " + files.Length);
                            break;

                        }
                        count++;
                    }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, progress);
                }
            }
        }

       public async Task Search(IProgress<ChitraKiAlbumAurVivaran> progress)
        {
            await Task.Run(() =>
            {
                WalkDirectoryTree(new System.IO.DirectoryInfo(SearchDirectory), progress);
            });

            //foreach (string s in log)
            //{
            //    Console.WriteLine(s);
            //}
        }

    }
}
