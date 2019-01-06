namespace PicsDirectoryDisplayWin
{
    partial class DirectoryGallery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tb = new System.Windows.Forms.TableLayoutPanel();
            this.imglist = new System.Windows.Forms.ListView();
            this.folder_list = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_Back = new System.Windows.Forms.Button();
            this.btn_Next = new System.Windows.Forms.Button();
            this.imgs = new System.Windows.Forms.ImageList(this.components);
            this.previewImages = new System.Windows.Forms.ImageList(this.components);
            this.tb.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb
            // 
            this.tb.ColumnCount = 2;
            this.tb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.1492F));
            this.tb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.8508F));
            this.tb.Controls.Add(this.imglist, 1, 0);
            this.tb.Controls.Add(this.folder_list, 0, 0);
            this.tb.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb.Location = new System.Drawing.Point(0, 0);
            this.tb.Margin = new System.Windows.Forms.Padding(2);
            this.tb.Name = "tb";
            this.tb.RowCount = 2;
            this.tb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.89071F));
            this.tb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.10929F));
            this.tb.Size = new System.Drawing.Size(980, 366);
            this.tb.TabIndex = 1;
            // 
            // imglist
            // 
            this.imglist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imglist.Location = new System.Drawing.Point(150, 2);
            this.imglist.Margin = new System.Windows.Forms.Padding(2);
            this.imglist.Name = "imglist";
            this.imglist.Size = new System.Drawing.Size(828, 324);
            this.imglist.TabIndex = 2;
            this.imglist.UseCompatibleStateImageBehavior = false;
            // 
            // folder_list
            // 
            this.folder_list.AutoScroll = true;
            this.folder_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folder_list.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.folder_list.Location = new System.Drawing.Point(2, 2);
            this.folder_list.Margin = new System.Windows.Forms.Padding(2);
            this.folder_list.Name = "folder_list";
            this.folder_list.Size = new System.Drawing.Size(144, 324);
            this.folder_list.TabIndex = 1;
            this.folder_list.WrapContents = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btn_Back);
            this.flowLayoutPanel1.Controls.Add(this.btn_Next);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(807, 331);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(170, 32);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btn_Back
            // 
            this.btn_Back.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btn_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Back.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Back.Location = new System.Drawing.Point(3, 3);
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(78, 23);
            this.btn_Back.TabIndex = 3;
            this.btn_Back.Text = "Back";
            this.btn_Back.UseVisualStyleBackColor = true;
            this.btn_Back.Click += new System.EventHandler(this.btn_Back_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Next.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Next.Location = new System.Drawing.Point(87, 3);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(75, 23);
            this.btn_Next.TabIndex = 4;
            this.btn_Next.Text = "Next";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // imgs
            // 
            this.imgs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgs.ImageSize = new System.Drawing.Size(16, 16);
            this.imgs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // previewImages
            // 
            this.previewImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.previewImages.ImageSize = new System.Drawing.Size(16, 16);
            this.previewImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Gallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 366);
            this.Controls.Add(this.tb);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Gallery";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tb.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tb;
        private System.Windows.Forms.FlowLayoutPanel folder_list;
        private System.Windows.Forms.ImageList imgs;
        private System.Windows.Forms.ListView imglist;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn_Back;
        private System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.ImageList previewImages;
    }
}

