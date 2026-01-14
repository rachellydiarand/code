namespace MothPictureViewer
{
    partial class FormPictureViewerSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxImageSaveDirectory = new System.Windows.Forms.TextBox();
            this.buttonImageSaveDirectoryBrowse = new System.Windows.Forms.Button();
            this.textBoxImageCopyOffsetX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxImageCopyOffsetY = new System.Windows.Forms.TextBox();
            this.textBoxBackgroundColor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSlideshowTimingSeconds = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image Save Directory";
            // 
            // textBoxImageSaveDirectory
            // 
            this.textBoxImageSaveDirectory.Location = new System.Drawing.Point(128, 10);
            this.textBoxImageSaveDirectory.Name = "textBoxImageSaveDirectory";
            this.textBoxImageSaveDirectory.Size = new System.Drawing.Size(374, 20);
            this.textBoxImageSaveDirectory.TabIndex = 1;
            this.textBoxImageSaveDirectory.TextChanged += new System.EventHandler(this.textBoxImageSaveDirectory_TextChanged);
            // 
            // buttonImageSaveDirectoryBrowse
            // 
            this.buttonImageSaveDirectoryBrowse.Location = new System.Drawing.Point(498, 9);
            this.buttonImageSaveDirectoryBrowse.Name = "buttonImageSaveDirectoryBrowse";
            this.buttonImageSaveDirectoryBrowse.Size = new System.Drawing.Size(26, 23);
            this.buttonImageSaveDirectoryBrowse.TabIndex = 2;
            this.buttonImageSaveDirectoryBrowse.Text = "...";
            this.buttonImageSaveDirectoryBrowse.UseVisualStyleBackColor = true;
            this.buttonImageSaveDirectoryBrowse.Click += new System.EventHandler(this.buttonImageSaveDirectoryBrowse_Click);
            // 
            // textBoxImageCopyOffsetX
            // 
            this.textBoxImageCopyOffsetX.Location = new System.Drawing.Point(128, 40);
            this.textBoxImageCopyOffsetX.Name = "textBoxImageCopyOffsetX";
            this.textBoxImageCopyOffsetX.Size = new System.Drawing.Size(44, 20);
            this.textBoxImageCopyOffsetX.TabIndex = 4;
            this.textBoxImageCopyOffsetX.TextChanged += new System.EventHandler(this.textBoxImageCopyOffsetX_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Image Copy Offset x,y";
            // 
            // textBoxImageCopyOffsetY
            // 
            this.textBoxImageCopyOffsetY.Location = new System.Drawing.Point(178, 40);
            this.textBoxImageCopyOffsetY.Name = "textBoxImageCopyOffsetY";
            this.textBoxImageCopyOffsetY.Size = new System.Drawing.Size(44, 20);
            this.textBoxImageCopyOffsetY.TabIndex = 5;
            this.textBoxImageCopyOffsetY.TextChanged += new System.EventHandler(this.textBoxImageCopyOffsetY_TextChanged);
            // 
            // textBoxBackgroundColor
            // 
            this.textBoxBackgroundColor.Location = new System.Drawing.Point(128, 71);
            this.textBoxBackgroundColor.Name = "textBoxBackgroundColor";
            this.textBoxBackgroundColor.Size = new System.Drawing.Size(44, 20);
            this.textBoxBackgroundColor.TabIndex = 7;
            this.textBoxBackgroundColor.TextChanged += new System.EventHandler(this.textBoxBackgroundColor_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Background Color";
            // 
            // textBoxSlideshowTimingSeconds
            // 
            this.textBoxSlideshowTimingSeconds.Location = new System.Drawing.Point(157, 99);
            this.textBoxSlideshowTimingSeconds.Name = "textBoxSlideshowTimingSeconds";
            this.textBoxSlideshowTimingSeconds.Size = new System.Drawing.Size(44, 20);
            this.textBoxSlideshowTimingSeconds.TabIndex = 9;
            this.textBoxSlideshowTimingSeconds.TextChanged += new System.EventHandler(this.textBoxSlideshowTimingSeconds_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Slideshow Timing (seconds)";
            // 
            // FormPictureViewerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 173);
            this.Controls.Add(this.textBoxSlideshowTimingSeconds);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxBackgroundColor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxImageCopyOffsetY);
            this.Controls.Add(this.textBoxImageCopyOffsetX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonImageSaveDirectoryBrowse);
            this.Controls.Add(this.textBoxImageSaveDirectory);
            this.Controls.Add(this.label1);
            this.Name = "FormPictureViewerSettings";
            this.Text = "FormPictureViewerSettings";
            this.Load += new System.EventHandler(this.FormPictureViewerSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxImageSaveDirectory;
        private System.Windows.Forms.Button buttonImageSaveDirectoryBrowse;
        private System.Windows.Forms.TextBox textBoxImageCopyOffsetX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxImageCopyOffsetY;
        private System.Windows.Forms.TextBox textBoxBackgroundColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSlideshowTimingSeconds;
        private System.Windows.Forms.Label label4;
    }
}