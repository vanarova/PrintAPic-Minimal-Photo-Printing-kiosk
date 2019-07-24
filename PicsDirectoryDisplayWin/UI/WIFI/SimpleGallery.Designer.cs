namespace PicsDirectoryDisplayWin
{
    partial class SimpleGallery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleGallery));
            this.tb = new System.Windows.Forms.TableLayoutPanel();
            this.imglist = new System.Windows.Forms.ListView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.warningTxt = new System.Windows.Forms.Label();
            this.UploadButton = new System.Windows.Forms.Button();
            this.btn_Back = new System.Windows.Forms.Button();
            this.btn_Next = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_PicsCount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.loadingImageslabel = new System.Windows.Forms.Label();
            this.LoadingImagesPBar = new System.Windows.Forms.ProgressBar();
            this.imgs = new System.Windows.Forms.ImageList(this.components);
            this.previewImages = new System.Windows.Forms.ImageList(this.components);
            this.LoadingImagesProgBar = new System.Windows.Forms.ProgressBar();
            this.UploadUSBFilesDialog = new System.Windows.Forms.OpenFileDialog();
            this.tb.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb
            // 
            this.tb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tb.ColumnCount = 2;
            this.tb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tb.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tb.Controls.Add(this.imglist, 1, 0);
            this.tb.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tb.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tb.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb.Location = new System.Drawing.Point(0, 0);
            this.tb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb.Name = "tb";
            this.tb.RowCount = 2;
            this.tb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tb.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tb.Size = new System.Drawing.Size(1307, 450);
            this.tb.TabIndex = 1;
            // 
            // imglist
            // 
            this.imglist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imglist.Location = new System.Drawing.Point(264, 2);
            this.imglist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imglist.Name = "imglist";
            this.imglist.Size = new System.Drawing.Size(1040, 378);
            this.imglist.TabIndex = 2;
            this.imglist.UseCompatibleStateImageBehavior = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.pictureBox4);
            this.flowLayoutPanel1.Controls.Add(this.warningTxt);
            this.flowLayoutPanel1.Controls.Add(this.UploadButton);
            this.flowLayoutPanel1.Controls.Add(this.btn_Back);
            this.flowLayoutPanel1.Controls.Add(this.btn_Next);
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(718, 382);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(589, 68);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox4.BackgroundImage")));
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(3, 20);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(88, 31);
            this.pictureBox4.TabIndex = 7;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Visible = false;
            // 
            // warningTxt
            // 
            this.warningTxt.AutoSize = true;
            this.warningTxt.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningTxt.Location = new System.Drawing.Point(97, 18);
            this.warningTxt.Margin = new System.Windows.Forms.Padding(3, 18, 3, 0);
            this.warningTxt.Name = "warningTxt";
            this.warningTxt.Size = new System.Drawing.Size(0, 29);
            this.warningTxt.TabIndex = 6;
            // 
            // UploadButton
            // 
            this.UploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadButton.AutoSize = true;
            this.UploadButton.Enabled = false;
            this.UploadButton.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UploadButton.Location = new System.Drawing.Point(100, 0);
            this.UploadButton.Margin = new System.Windows.Forms.Padding(0);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(159, 75);
            this.UploadButton.TabIndex = 5;
            this.UploadButton.Text = "Upload";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // btn_Back
            // 
            this.btn_Back.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btn_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Back.AutoSize = true;
            this.btn_Back.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Back.Location = new System.Drawing.Point(259, 0);
            this.btn_Back.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(169, 75);
            this.btn_Back.TabIndex = 3;
            this.btn_Back.Text = "Back";
            this.btn_Back.UseVisualStyleBackColor = true;
            this.btn_Back.Click += new System.EventHandler(this.btn_Back_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Next.AutoSize = true;
            this.btn_Next.Enabled = false;
            this.btn_Next.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Next.Location = new System.Drawing.Point(428, 0);
            this.btn_Next.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(161, 75);
            this.btn_Next.TabIndex = 4;
            this.btn_Next.Text = "Next";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.78431F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.21569F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label_PicsCount, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label9, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label11, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label13, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox3, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.label15, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.label16, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.label17, 2, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(255, 376);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "No. of pics :";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(50, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Cost/pic:";
            // 
            // label_PicsCount
            // 
            this.label_PicsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_PicsCount.AutoSize = true;
            this.label_PicsCount.Location = new System.Drawing.Point(179, 255);
            this.label_PicsCount.Name = "label_PicsCount";
            this.label_PicsCount.Size = new System.Drawing.Size(31, 18);
            this.label_PicsCount.TabIndex = 6;
            this.label_PicsCount.Text = "x 0";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(192, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 18);
            this.label8.TabIndex = 8;
            this.label8.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(192, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 305);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "GST 18% :";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(54, 280);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 18);
            this.label10.TabIndex = 10;
            this.label10.Text = "Amount :";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(192, 280);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 18);
            this.label9.TabIndex = 9;
            this.label9.Text = "0";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(39, 205);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 18);
            this.label12.TabIndex = 12;
            this.label12.Text = "Print size :";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(182, 205);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 18);
            this.label11.TabIndex = 11;
            this.label11.Text = "A5";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(187, 332);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 24);
            this.label13.TabIndex = 13;
            this.label13.Text = "0";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(0, 330);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(130, 2);
            this.pictureBox2.TabIndex = 16;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox3, 2);
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Location = new System.Drawing.Point(130, 330);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(125, 2);
            this.pictureBox3.TabIndex = 17;
            this.pictureBox3.TabStop = false;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(54, 332);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 24);
            this.label14.TabIndex = 14;
            this.label14.Text = "Total :";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 40);
            this.label6.TabIndex = 18;
            this.label6.Text = "Bill info..";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 3);
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(3, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(249, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 18);
            this.label3.TabIndex = 19;
            this.label3.Text = "/-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(216, 255);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 18);
            this.label7.TabIndex = 20;
            this.label7.Text = "x";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(216, 280);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 18);
            this.label15.TabIndex = 21;
            this.label15.Text = "/-";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(216, 305);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(26, 18);
            this.label16.TabIndex = 22;
            this.label16.Text = "%";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(216, 332);
            this.label17.Name = "label17";
            this.label17.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label17.Size = new System.Drawing.Size(23, 21);
            this.label17.TabIndex = 23;
            this.label17.Text = "/-";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.Controls.Add(this.loadingImageslabel);
            this.flowLayoutPanel2.Controls.Add(this.LoadingImagesPBar);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 385);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(255, 62);
            this.flowLayoutPanel2.TabIndex = 6;
            // 
            // loadingImageslabel
            // 
            this.loadingImageslabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadingImageslabel.AutoSize = true;
            this.loadingImageslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadingImageslabel.Location = new System.Drawing.Point(3, 0);
            this.loadingImageslabel.Name = "loadingImageslabel";
            this.loadingImageslabel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 5);
            this.loadingImageslabel.Size = new System.Drawing.Size(142, 28);
            this.loadingImageslabel.TabIndex = 1;
            this.loadingImageslabel.Text = "Please Wait  ....";
            // 
            // LoadingImagesPBar
            // 
            this.LoadingImagesPBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadingImagesPBar.Location = new System.Drawing.Point(3, 31);
            this.LoadingImagesPBar.MarqueeAnimationSpeed = 40;
            this.LoadingImagesPBar.Name = "LoadingImagesPBar";
            this.LoadingImagesPBar.Size = new System.Drawing.Size(249, 29);
            this.LoadingImagesPBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.LoadingImagesPBar.TabIndex = 0;
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
            // LoadingImagesProgBar
            // 
            this.LoadingImagesProgBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadingImagesProgBar.Location = new System.Drawing.Point(3, 40);
            this.LoadingImagesProgBar.MarqueeAnimationSpeed = 40;
            this.LoadingImagesProgBar.Name = "LoadingImagesProgBar";
            this.LoadingImagesProgBar.Size = new System.Drawing.Size(249, 16);
            this.LoadingImagesProgBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.LoadingImagesProgBar.TabIndex = 0;
            // 
            // UploadUSBFilesDialog
            // 
            this.UploadUSBFilesDialog.Multiselect = true;
            // 
            // SimpleGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1307, 450);
            this.ControlBox = false;
            this.Controls.Add(this.tb);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SimpleGallery";
            this.Text = "SimpleGallery";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SimpleGallery_Load);
            this.tb.ResumeLayout(false);
            this.tb.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tb;
        private System.Windows.Forms.ImageList imgs;
        private System.Windows.Forms.ListView imglist;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn_Back;
        private System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.ImageList previewImages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label_PicsCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label warningTxt;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label loadingImageslabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar LoadingImagesPBar;
        private System.Windows.Forms.ProgressBar LoadingImagesProgBar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.OpenFileDialog UploadUSBFilesDialog;
    }
}

