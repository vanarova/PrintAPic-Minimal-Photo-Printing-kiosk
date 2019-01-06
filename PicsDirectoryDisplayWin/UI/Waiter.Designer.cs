namespace PicsDirectoryDisplayWin
{
    partial class Waiter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Waiter));
            this.waitbar = new System.Windows.Forms.ProgressBar();
            this.FileFoundLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // waitbar
            // 
            this.waitbar.Location = new System.Drawing.Point(161, 145);
            this.waitbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.waitbar.MarqueeAnimationSpeed = 25;
            this.waitbar.Name = "waitbar";
            this.waitbar.Size = new System.Drawing.Size(316, 28);
            this.waitbar.Step = 5;
            this.waitbar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.waitbar.TabIndex = 0;
            // 
            // FileFoundLabel
            // 
            this.FileFoundLabel.AutoSize = true;
            this.FileFoundLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileFoundLabel.Location = new System.Drawing.Point(157, 177);
            this.FileFoundLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FileFoundLabel.Name = "FileFoundLabel";
            this.FileFoundLabel.Size = new System.Drawing.Size(122, 20);
            this.FileFoundLabel.TabIndex = 1;
            this.FileFoundLabel.Text = "Please wait ..";
            // 
            // Waiter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(661, 348);
            this.Controls.Add(this.FileFoundLabel);
            this.Controls.Add(this.waitbar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Waiter";
            this.Text = "Please wait..";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar waitbar;
        private System.Windows.Forms.Label FileFoundLabel;
    }
}