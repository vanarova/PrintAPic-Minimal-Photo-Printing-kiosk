using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin.lib
{
    public class ChitraKhoj
    {
        private String SearchDirectory;
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        private int NoOfTotalDirsFound = 0;
        //private readonly int IncludeDirectoryContainingMinImages = 1;
        //private readonly int IncludeMaxImages = 20;
        //private readonly int MaxDirectoryToSearchLimit = 50;

        public ChitraKhoj(string searchDirectory)
        {
            SearchDirectory = searchDirectory;
        }

        void WalkDirectoryTree(System.IO.DirectoryInfo root, IProgress<ChitraKiAlbumAurVivaran> progress,
            Form form = null, bool InvokeRequired = false, int searchDepth =1)
        {
            IEnumerable<FileInfo> allimgfiles = null;
            IEnumerable<FileInfo> allJpegfiles = null;
            IEnumerable<FileInfo> HEICfiles = null;
            System.IO.DirectoryInfo[] subDirs = null;
            if (NoOfTotalDirsFound > Globals.MaxDirectoryToSearchLimit)
                return;
            // First, process all the files directly under this folder
            try
            {
                allimgfiles = root.EnumerateFiles("*.jpg");
                allJpegfiles = root.EnumerateFiles("*.jpeg");
                HEICfiles = root.EnumerateFiles("*.heic");
                allimgfiles = allimgfiles.Concat<FileInfo>(HEICfiles);
                allimgfiles = allimgfiles.Concat<FileInfo>(allJpegfiles);

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

            if (allimgfiles != null)
            {
                int count = 0; List<ChitraKiAlbumAurVivaran> peerImages = new List<ChitraKiAlbumAurVivaran>();
                int ImageLimit;
                // if image count is lower than min images, leave this directory
                if (allimgfiles.Count() < Globals.IncludeDirectoryContainingMinImages)
                {

                    return;
                 }

                if (allimgfiles.Count() > Globals.IncludeMaxImages)
                    ImageLimit = Globals.IncludeMaxImages;
                else
                    ImageLimit = allimgfiles.Count();
                foreach (System.IO.FileInfo fi in allimgfiles)
                    {
                        peerImages.Add(new ChitraKiAlbumAurVivaran()
                        {
                            ImageName = fi.Name,
                            ImageFullName = fi.FullName,
                            ImageDirName = root.Name,
                            ImageDirFullName = root.FullName,
                            ImageDirTotalImages = allimgfiles.Count()
                        });

                        if (count >= ImageLimit-1)
                        {
                            count = 0;
                            NoOfTotalDirsFound++;
                        if (InvokeRequired)
                        {
                            //Parentform.Invoke((Action<bool>)Done, true);
                            form.Invoke(
                                new Action<IProgress<ChitraKiAlbumAurVivaran>,
                                System.IO.DirectoryInfo, IEnumerable<FileInfo>
                                , List<ChitraKiAlbumAurVivaran>, int>
                                (
                                    Report
                                    //(prog, rt, fls, prImages, ImgLimit) =>  Report(prog, rt, fls, prImages, ImgLimit)
                                ), progress, root, allimgfiles, peerImages, ImageLimit);
                        }
                        //form.Invoke(
                        else
                        {
                            Report(progress,root,allimgfiles,peerImages,ImageLimit);
                            //progress.Report(new ChitraKiAlbumAurVivaran()
                            //{
                            //    ImageName = "(" + (files.Length - ImageLimit) + ") More Images",
                            //    ImageFullName = "..\\..\\..\\pics\\vst.png",
                            //    ImageDirName = root.Name,
                            //    ImageDirFullName = root.FullName,
                            //    ImageDirTotalImages = files.Length,
                            //    PeerImages = peerImages
                            //});
                        }
                            //progress.Report(root.Name +" | " + root.FullName + " | "+fi.FullName + " | " + fi.Name + " | " + files.Length);
                            break;

                        }
                        count++;
                    }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                //if (searchDepth >0)
                //{
                //    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                //    {
                //        // Resursive call for each subdirectory.
                //        WalkDirectoryTree(dirInfo, progress);
                //    }
                //}
               
            }
        }

       private void Report(IProgress<ChitraKiAlbumAurVivaran> progress,
           System.IO.DirectoryInfo directoryinfo, IEnumerable<FileInfo> files,
           List<ChitraKiAlbumAurVivaran> peerImages, int ImageLimit)
        {
            progress.Report(new ChitraKiAlbumAurVivaran()
            {
                ImageName = "(" + (files.Count() - ImageLimit) + ") More Images",
                ImageFullName = "..\\..\\..\\pics\\vst.png",
                ImageDirName = directoryinfo.Name,
                ImageDirFullName = directoryinfo.FullName,
                ImageDirTotalImages = files.Count(),
                PeerImages = peerImages
            });

        }

       public async Task Search(IProgress<ChitraKiAlbumAurVivaran> progress,Form form = null, bool InvokeRequired = false,int searchDepth =1)
        {
            await Task.Run(() =>
            {
                WalkDirectoryTree(new System.IO.DirectoryInfo(SearchDirectory), progress,form, InvokeRequired, searchDepth);
            });

            //foreach (string s in log)
            //{
            //    Console.WriteLine(s);
            //}
        }

    }
}
