using PicsDirectoryDisplayWin.lib_ImgIO;
using PicsDirectoryDisplayWin.lib_Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin
{
    public partial class PrintSettings : Form
    {
        //private List<string> selectedImages = new List<string>();
        //public List<string> SelectedImages { set { selectedImages = value; } }
        private readonly Font SelectedFont = new Font(new Font("Arial", 10.0f), FontStyle.Bold);
        private readonly Font UnSelectedFont = new Font(new Font("Arial", 8.0f), FontStyle.Regular);
        private string checkUnicode = "2714"; // ballot box -1F5F9
        int value; private string CheckSymbol;
        private readonly Color SelectedColor = Color.Silver;
        public string PrintDir;
        int index = 0;
        System.Windows.Forms.ListViewItemSelectionChangedEventHandler chandler;
        public PrintSettings(List<string> selectedImages)
        {
            InitializeComponent();
            if(selectedImages.Count > 0)
            ShowSelectedImages(selectedImages);
            chandler = new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.Imglist_ItemSelectionChanged);
            this.imglist.ItemSelectionChanged += chandler;
            //lbl_PrintStatus.Text = "Print command sent";
            value = int.Parse(checkUnicode, System.Globalization.NumberStyles.HexNumber);
            CheckSymbol = char.ConvertFromUtf32(value).ToString();
    }

        private void ShowSelectedImages(List<string> imageKeys)
        {
            //ImageIO imageIO = new ImageIO();
            imgs.Images.Clear();
            if (imglist.LargeImageList != null && imglist.LargeImageList.Images.Count > 0)
                imglist.LargeImageList.Images.Clear();
            imglist.Clear(); //galleryPreview.LargeImageList.Images.Clear();
            foreach (var item in imageKeys)
            {

                string[] imgDetails = item.Split('|');
                string tempImg = imgDetails[0].Replace(imgDetails[1], "thumbs/") + imgDetails[1];

                //string imgName = item.Split('|')[1];
                imgs.Images.Add(tempImg, new ImageIO().GetImage(tempImg));
                //previewImages.Images.Add(tempImg, Image.FromFile(tempImg));
                //logger.Log(NLog.LogLevel.Info, "thumbnail size 80,80 should be in a config file.");
                imgs.ImageSize = new Size(80, 80);
                imglist.LargeImageList = imgs;
                // image key is the image sleected from imagelist collection, key must present in imagelist above\
                imglist.Items.Add(imgDetails[1], tempImg);
                imglist.Show();
            }

        }

        private void Btn_Print_Click(object sender, EventArgs e)
        {
            //lib_Print.PrintIO.PrintPic();
            if (imglist.SelectedItems.Count > 0) {
                //lib_Print.PrintIO.PrintPic(imglist.SelectedItems[0].ImageKey);
                if (lib_Print.PrintIO.PrintPDF(PrintDir, index) ==1)
                {
                    lbl_PrintStatus.Text = "Prints not generated, Press print button from Print screen first.";
                }
                //lbl_PrintStatus.Text = "Print Sent for: " + PrintDir + "//Print" + index.ToString() + ".pdf";
             }
        }

        private void PrintSettings_Load(object sender, EventArgs e)
        {
            lbl_PrintStatus.Text = "";
        }


        private void UnSelectImage(ListViewItem item)
        {
            if (item.Checked)
            {
                //SelectedImageKeys.Remove(item.ImageKey);

                item.Checked = false;
                item.BackColor = Color.White;
                item.Focused = false;
                //string copyrightUnicode = "2714"; // ballot box -1F5F9
                //int value = int.Parse(copyrightUnicode, System.Globalization.NumberStyles.HexNumber);
                //string symbol = char.ConvertFromUtf32(value).ToString();
                item.Font = UnSelectedFont;
                item.Text = item.Text.Replace("[" + CheckSymbol + "] ", "");
                //UpdateBillDetails(SelectedImageKeys.Count);
            }


        }

        private void SelectImage(ListViewItem item)
        {
            if (item.Checked == false)
            {
                //SelectedImageKeys.Add(item.ImageKey);
                item.Checked = true;
                item.BackColor = SelectedColor;
                item.Focused = true;
                //string copyrightUnicode = "2714"; // ballot box -1F5F9
                //int value = int.Parse(copyrightUnicode, System.Globalization.NumberStyles.HexNumber);
                //string symbol = char.ConvertFromUtf32(value).ToString();
                item.Font = SelectedFont;
                item.Text = "[" + CheckSymbol + "] " + item.Text;
                //UpdateBillDetails(SelectedImageKeys.Count);
            }
            //item.Bounds.Inflate(60, 60);
            //item.ForeColor = Color.White;

        }

        private void Imglist_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
           
                if (imglist.SelectedItems.Count > 0)
                {

                    imglist.ItemSelectionChanged -= chandler;

                    if (imglist.SelectedIndices[0] == 0 || imglist.SelectedIndices[0] == 1)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[0]);
                        if(imglist.Items.Count>1) //this condition, occurs when one image is present, selecting second image will fail.
                        SelectImage(imglist.Items[1]);
                        index = 0;
                    }
                    if (imglist.SelectedIndices[0] == 2 || imglist.SelectedIndices[0] == 3)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[2]);
                        if (imglist.Items.Count > 3) 
                        SelectImage(imglist.Items[3]);
                        index = 2;
                }

                    if (imglist.SelectedIndices[0] == 4 || imglist.SelectedIndices[0] == 5)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[4]);
                    if (imglist.Items.Count > 5)
                        SelectImage(imglist.Items[5]);
                        index = 4;
                }

                    if (imglist.SelectedIndices[0] == 6 || imglist.SelectedIndices[0] == 7)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[6]);
                    if (imglist.Items.Count > 7)
                        SelectImage(imglist.Items[7]);
                        index = 6;
                }

                    if (imglist.SelectedIndices[0] == 8 || imglist.SelectedIndices[0] == 9)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[8]);
                    if (imglist.Items.Count > 9)
                        SelectImage(imglist.Items[9]);
                        index = 8;
                    }

                    if (imglist.SelectedIndices[0] == 10 || imglist.SelectedIndices[0] == 11)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[10]);
                    if (imglist.Items.Count > 11)
                        SelectImage(imglist.Items[11]);
                        index = 10;
                }

                    if (imglist.SelectedIndices[0] == 12 || imglist.SelectedIndices[0] == 13)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[12]);
                    if (imglist.Items.Count > 13)
                        SelectImage(imglist.Items[13]);
                         index = 12;
                    }

                    if (imglist.SelectedIndices[0] == 14 || imglist.SelectedIndices[0] == 15)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[14]);
                    if (imglist.Items.Count > 15)
                        SelectImage(imglist.Items[15]);
                        index = 14;
                }

                 

                    if (imglist.SelectedIndices[0] == 16 || imglist.SelectedIndices[0] == 17)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[16]);
                    if (imglist.Items.Count > 17)
                        SelectImage(imglist.Items[17]);
                        index = 16;
                }

                    if (imglist.SelectedIndices[0] == 18 || imglist.SelectedIndices[0] == 19)
                    {
                        for (int i = 0; i < imglist.Items.Count; i++)
                            UnSelectImage(imglist.Items[i]);
                        SelectImage(imglist.Items[18]);
                    if (imglist.Items.Count > 19)
                        SelectImage(imglist.Items[19]);
                        index = 18;
                }
                    imglist.ItemSelectionChanged += chandler;
                }
            
        }

        private void Imglist_SelectedIndexChanged(object sender, EventArgs e)
        {
            //imglist.SelectedItems.Clear();
        }

        private void Imglist_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //imglist.SelectedItems.Clear();
        }

        private void Imglist_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //Cancel Print
            PrintIO.AbortPrinting();
        }
    }
}
