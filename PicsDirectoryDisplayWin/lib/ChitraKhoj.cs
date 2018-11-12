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
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        int NoOfTotalDirsFound = 0;
        int IncludeDirectoryContainingMinImages = 2; int IncludeMaxImages = 20;
        int MaxDirectoryToSearchLimit = 100;


        public ChitraKhoj()
        {
            
        }

        //private void ReportProgress(TheImage obj)
        //{

        //    foreach (var item in obj.PeerImages)
        //    {
        //        imgs.Images.Add(item.ImageKey, Image.FromFile(item.ImageFullName));
        //        imgs.ImageSize = new Size(70, 70);
        //        imglist.LargeImageList = imgs;
        //        imglist.Items.Add(item.ImageName, item.ImageKey);
        //        imglist.Show();
        //    }
        //    imgs.Images.Add(obj.ImageKey, Image.FromFile(obj.ImageFullName));
        //    imgs.ImageSize = new Size(70, 70);
        //    imglist.LargeImageList = imgs;
        //    imglist.Items.Add(obj.ImageName, obj.ImageKey);
        //    imglist.Show();

        //    fl.Controls.Add(new Button()
        //    {
        //        Text = obj.ImageDirName,
        //        TextImageRelation = TextImageRelation.ImageBeforeText,
        //        Size = new Size(120, 52),
        //        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        //        TextAlign = ContentAlignment.MiddleLeft,
        //        FlatStyle = FlatStyle.Popup,
        //        BackColor = Color.White,
        //        Image = new Bitmap("..\\..\\pics\\vst.png"),
        //        ImageAlign = ContentAlignment.TopLeft
        //    });
        //}


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
                if (files.Length > IncludeMaxImages)
                    ImageLimit = IncludeMaxImages;
                else
                    ImageLimit = IncludeDirectoryContainingMinImages;
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

                        if (count >= ImageLimit)
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

                        // In this example, we only access the existing FileInfo object. If we
                        // want to open, delete or modify the file, then
                        // a try-catch block is required here to handle the case
                        // where the file has been deleted since the call to TraverseTree().
                        //RaiseUpdateEvent("Dir: " + root.FullName + "File: " + fi.FullName);
                        //Dirs.Items.Add("Dir: " + root.FullName + "File: " + fi.FullName);

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
            // Start with drives if you have to search the entire computer.
            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                if (!di.IsReady)
                {
                    //Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }
                System.IO.DirectoryInfo rootDir = di.RootDirectory;
                await Task.Run(() =>
                {
                    WalkDirectoryTree(rootDir, progress);
                });
            }

            // Write out all the files that could not be processed.
            //Console.WriteLine("Files with restricted access:");
            foreach (string s in log)
            {
                Console.WriteLine(s);
            }
            // Keep the console window open in debug mode.
            //Console.WriteLine("Press any key");
            //Console.ReadKey();
        }

    }
}
