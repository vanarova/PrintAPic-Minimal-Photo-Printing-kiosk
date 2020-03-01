using System;

namespace PrintAPicStart
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Start_Button = new System.Windows.Forms.Button();
            this.main_grpbox = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Register_btn = new System.Windows.Forms.Button();
            this.IdleTimelbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Stop_Button = new System.Windows.Forms.Button();
            this.hide_button = new System.Windows.Forms.Button();
            this.CloseExit_button = new System.Windows.Forms.Button();
            this.main_notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.userSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.main_grpbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Start_Button
            // 
            this.Start_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Start_Button.Location = new System.Drawing.Point(13, 165);
            this.Start_Button.Margin = new System.Windows.Forms.Padding(4);
            this.Start_Button.Name = "Start_Button";
            this.Start_Button.Size = new System.Drawing.Size(228, 95);
            this.Start_Button.TabIndex = 0;
            this.Start_Button.Text = "Start";
            this.Start_Button.UseVisualStyleBackColor = true;
            this.Start_Button.Click += new System.EventHandler(this.StartStop_Button_Click);
            // 
            // main_grpbox
            // 
            this.main_grpbox.Controls.Add(this.checkBox1);
            this.main_grpbox.Controls.Add(this.button4);
            this.main_grpbox.Controls.Add(this.button3);
            this.main_grpbox.Controls.Add(this.button2);
            this.main_grpbox.Controls.Add(this.pictureBox1);
            this.main_grpbox.Controls.Add(this.Register_btn);
            this.main_grpbox.Controls.Add(this.IdleTimelbl);
            this.main_grpbox.Controls.Add(this.label1);
            this.main_grpbox.Controls.Add(this.Stop_Button);
            this.main_grpbox.Controls.Add(this.hide_button);
            this.main_grpbox.Controls.Add(this.CloseExit_button);
            this.main_grpbox.Controls.Add(this.Start_Button);
            this.main_grpbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_grpbox.Location = new System.Drawing.Point(0, 0);
            this.main_grpbox.Margin = new System.Windows.Forms.Padding(4);
            this.main_grpbox.Name = "main_grpbox";
            this.main_grpbox.Padding = new System.Windows.Forms.Padding(4);
            this.main_grpbox.Size = new System.Drawing.Size(467, 457);
            this.main_grpbox.TabIndex = 2;
            this.main_grpbox.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(249, 137);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(144, 21);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "Autorun at startup";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(15, 22);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(226, 37);
            this.button4.TabIndex = 20;
            this.button4.Text = "Router Test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(151, 396);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(129, 50);
            this.button3.TabIndex = 19;
            this.button3.Text = "Settings";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.BSettings_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(286, 395);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 50);
            this.button2.TabIndex = 18;
            this.button2.Text = "Diagnostics";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Diagnostics_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PrintAPicStart.Properties.Resources._3;
            this.pictureBox1.Location = new System.Drawing.Point(254, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(201, 99);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // Register_btn
            // 
            this.Register_btn.BackColor = System.Drawing.Color.LightSalmon;
            this.Register_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Register_btn.Location = new System.Drawing.Point(13, 395);
            this.Register_btn.Margin = new System.Windows.Forms.Padding(4);
            this.Register_btn.Name = "Register_btn";
            this.Register_btn.Size = new System.Drawing.Size(131, 51);
            this.Register_btn.TabIndex = 16;
            this.Register_btn.Text = "Register";
            this.Register_btn.UseVisualStyleBackColor = false;
            this.Register_btn.Click += new System.EventHandler(this.Register_Click);
            // 
            // IdleTimelbl
            // 
            this.IdleTimelbl.AutoSize = true;
            this.IdleTimelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IdleTimelbl.Location = new System.Drawing.Point(124, 92);
            this.IdleTimelbl.Name = "IdleTimelbl";
            this.IdleTimelbl.Size = new System.Drawing.Size(20, 29);
            this.IdleTimelbl.TabIndex = 15;
            this.IdleTimelbl.Text = ".";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 29);
            this.label1.TabIndex = 14;
            this.label1.Text = "Idle time:";
            // 
            // Stop_Button
            // 
            this.Stop_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Stop_Button.Location = new System.Drawing.Point(249, 165);
            this.Stop_Button.Margin = new System.Windows.Forms.Padding(4);
            this.Stop_Button.Name = "Stop_Button";
            this.Stop_Button.Size = new System.Drawing.Size(201, 95);
            this.Stop_Button.TabIndex = 13;
            this.Stop_Button.Text = "Stop";
            this.Stop_Button.UseVisualStyleBackColor = true;
            this.Stop_Button.Click += new System.EventHandler(this.Stop_Button_Click);
            // 
            // hide_button
            // 
            this.hide_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hide_button.Location = new System.Drawing.Point(13, 268);
            this.hide_button.Margin = new System.Windows.Forms.Padding(4);
            this.hide_button.Name = "hide_button";
            this.hide_button.Size = new System.Drawing.Size(228, 101);
            this.hide_button.TabIndex = 8;
            this.hide_button.Text = "Hide to taskbar";
            this.hide_button.UseVisualStyleBackColor = true;
            this.hide_button.Click += new System.EventHandler(this.hide_button_Click);
            // 
            // CloseExit_button
            // 
            this.CloseExit_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseExit_button.Location = new System.Drawing.Point(249, 268);
            this.CloseExit_button.Margin = new System.Windows.Forms.Padding(4);
            this.CloseExit_button.Name = "CloseExit_button";
            this.CloseExit_button.Size = new System.Drawing.Size(201, 101);
            this.CloseExit_button.TabIndex = 7;
            this.CloseExit_button.Text = "Close and exit";
            this.CloseExit_button.UseVisualStyleBackColor = true;
            this.CloseExit_button.Click += new System.EventHandler(this.CloseExit_button_Click);
            // 
            // main_notifyIcon
            // 
            this.main_notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.main_notifyIcon.BalloonTipText = "Matlab script scheduler";
            this.main_notifyIcon.BalloonTipTitle = "Matlab script scheduler";
            this.main_notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("main_notifyIcon.Icon")));
            this.main_notifyIcon.Text = "Matlab script scheduler";
            this.main_notifyIcon.Visible = true;
            this.main_notifyIcon.Click += new System.EventHandler(this.main_notifyIcon_Click);
            // 
            // userSettingsBindingSource
            // 
            
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 457);
            this.Controls.Add(this.main_grpbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "PrintAPicStart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.main_Load);
            this.main_grpbox.ResumeLayout(false);
            this.main_grpbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.Button Start_Button;
        private System.Windows.Forms.GroupBox main_grpbox;
        private System.Windows.Forms.NotifyIcon main_notifyIcon;
        private System.Windows.Forms.Button CloseExit_button;
        private System.Windows.Forms.Button hide_button;
        private System.Windows.Forms.BindingSource userSettingsBindingSource;
        private System.Windows.Forms.Button Stop_Button;
        private System.Windows.Forms.Label IdleTimelbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Register_btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

