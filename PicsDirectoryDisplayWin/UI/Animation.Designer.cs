namespace PicsDirectoryDisplayWin
{
    partial class Animation
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
            this.DirectConnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DirectConnectButton
            // 
            this.DirectConnectButton.Location = new System.Drawing.Point(520, 326);
            this.DirectConnectButton.Name = "DirectConnectButton";
            this.DirectConnectButton.Size = new System.Drawing.Size(75, 23);
            this.DirectConnectButton.TabIndex = 0;
            this.DirectConnectButton.Text = "Start";
            this.DirectConnectButton.UseVisualStyleBackColor = true;
            this.DirectConnectButton.Click += new System.EventHandler(this.DirectConnectButton_Click);
            // 
            // Animation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 361);
            this.Controls.Add(this.DirectConnectButton);
            this.Name = "Animation";
            this.Text = "Animation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DirectConnectButton;
    }
}