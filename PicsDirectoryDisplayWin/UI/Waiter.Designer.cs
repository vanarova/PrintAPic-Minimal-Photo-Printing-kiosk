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
            this.waitbar = new System.Windows.Forms.ProgressBar();
            this.FileFoundLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // waitbar
            // 
            this.waitbar.Location = new System.Drawing.Point(149, 118);
            this.waitbar.MarqueeAnimationSpeed = 25;
            this.waitbar.Name = "waitbar";
            this.waitbar.Size = new System.Drawing.Size(210, 23);
            this.waitbar.Step = 5;
            this.waitbar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.waitbar.TabIndex = 0;
            // 
            // FileFoundLabel
            // 
            this.FileFoundLabel.AutoSize = true;
            this.FileFoundLabel.Location = new System.Drawing.Point(146, 146);
            this.FileFoundLabel.Name = "FileFoundLabel";
            this.FileFoundLabel.Size = new System.Drawing.Size(70, 13);
            this.FileFoundLabel.TabIndex = 1;
            this.FileFoundLabel.Text = "Please wait ..";
            // 
            // Waiter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 283);
            this.Controls.Add(this.FileFoundLabel);
            this.Controls.Add(this.waitbar);
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