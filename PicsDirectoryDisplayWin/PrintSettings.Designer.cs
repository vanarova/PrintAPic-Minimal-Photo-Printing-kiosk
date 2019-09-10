namespace PicsDirectoryDisplayWin
{
    partial class PrintSettings
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
            this.imglist = new System.Windows.Forms.ListView();
            this.imgs = new System.Windows.Forms.ImageList(this.components);
            this.btn_Print = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_PrintStatus = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_TestRcpt = new System.Windows.Forms.Button();
            this.btn_PrntTest = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imglist
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.imglist, 4);
            this.imglist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imglist.HideSelection = false;
            this.imglist.Location = new System.Drawing.Point(3, 65);
            this.imglist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imglist.Name = "imglist";
            this.imglist.Size = new System.Drawing.Size(737, 351);
            this.imglist.TabIndex = 3;
            this.imglist.UseCompatibleStateImageBehavior = false;
            this.imglist.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Imglist_ItemCheck);
            this.imglist.SelectedIndexChanged += new System.EventHandler(this.Imglist_SelectedIndexChanged);
            this.imglist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Imglist_MouseDown);
            // 
            // imgs
            // 
            this.imgs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgs.ImageSize = new System.Drawing.Size(16, 16);
            this.imgs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btn_Print
            // 
            this.btn_Print.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Print.Location = new System.Drawing.Point(301, 421);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(141, 63);
            this.btn_Print.TabIndex = 4;
            this.btn_Print.Text = "Print Selected Pics";
            this.btn_Print.UseVisualStyleBackColor = true;
            this.btn_Print.Click += new System.EventHandler(this.Btn_Print_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.01031F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.98969F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel1.Controls.Add(this.imglist, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_PrintStatus, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.button1, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn_Print, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.button2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn_TestRcpt, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_PrntTest, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.11475F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.95082F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.13934F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(743, 488);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // lbl_PrintStatus
            // 
            this.lbl_PrintStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_PrintStatus.AutoSize = true;
            this.lbl_PrintStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PrintStatus.ForeColor = System.Drawing.Color.Red;
            this.lbl_PrintStatus.Location = new System.Drawing.Point(282, 461);
            this.lbl_PrintStatus.Name = "lbl_PrintStatus";
            this.lbl_PrintStatus.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lbl_PrintStatus.Size = new System.Drawing.Size(13, 27);
            this.lbl_PrintStatus.TabIndex = 5;
            this.lbl_PrintStatus.Text = ".";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(597, 421);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 64);
            this.button1.TabIndex = 6;
            this.button1.Text = "Restart Program";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(448, 421);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(143, 64);
            this.button2.TabIndex = 7;
            this.button2.Text = "Clean Printer Queue";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btn_TestRcpt
            // 
            this.btn_TestRcpt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_TestRcpt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_TestRcpt.Location = new System.Drawing.Point(448, 3);
            this.btn_TestRcpt.Name = "btn_TestRcpt";
            this.btn_TestRcpt.Size = new System.Drawing.Size(143, 57);
            this.btn_TestRcpt.TabIndex = 8;
            this.btn_TestRcpt.Text = "Test Receipt";
            this.btn_TestRcpt.UseVisualStyleBackColor = true;
            this.btn_TestRcpt.Click += new System.EventHandler(this.Btn_TestRcpt_Click);
            // 
            // btn_PrntTest
            // 
            this.btn_PrntTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_PrntTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PrntTest.Location = new System.Drawing.Point(301, 3);
            this.btn_PrntTest.Name = "btn_PrntTest";
            this.btn_PrntTest.Size = new System.Drawing.Size(141, 57);
            this.btn_PrntTest.TabIndex = 9;
            this.btn_PrntTest.Text = "Test Print";
            this.btn_PrntTest.UseVisualStyleBackColor = true;
            this.btn_PrntTest.Click += new System.EventHandler(this.Btn_PrntTest_Click);
            // 
            // PrintSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 488);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PrintSettings";
            this.Text = "PrintSettings";
            this.Load += new System.EventHandler(this.PrintSettings_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView imglist;
        private System.Windows.Forms.ImageList imgs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Print;
        private System.Windows.Forms.Label lbl_PrintStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_TestRcpt;
        private System.Windows.Forms.Button btn_PrntTest;
    }
}