namespace MothBLogCamera
{
    partial class FormCamera
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
            // 2024-10-21 Rachel Lydia Rand
            // Camera Form closing causes exception that prevents Application.Exit and keeps the camera active
            // disposing is true

            // commenting this out
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            // in favor of this -- didn't work
            // this gets rid of the error that was thrown, but the problem still exists
            //base.Dispose(false);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCamera));
            this.pictureBoxCamera = new System.Windows.Forms.PictureBox();
            this.comboBoxCameraDevices = new System.Windows.Forms.ComboBox();
            this.buttonFullscreen = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSnapPictureDelay = new System.Windows.Forms.TextBox();
            this.buttonSnap = new System.Windows.Forms.Button();
            this.labelSnapCountdown = new System.Windows.Forms.Label();
            this.comboBoxResolutions = new System.Windows.Forms.ComboBox();
            this.panelSplashGraphics = new System.Windows.Forms.Panel();
            this.buttonSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCamera
            // 
            this.pictureBoxCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCamera.Location = new System.Drawing.Point(-1, 26);
            this.pictureBoxCamera.Name = "pictureBoxCamera";
            this.pictureBoxCamera.Size = new System.Drawing.Size(987, 561);
            this.pictureBoxCamera.TabIndex = 0;
            this.pictureBoxCamera.TabStop = false;
            // 
            // comboBoxCameraDevices
            // 
            this.comboBoxCameraDevices.FormattingEnabled = true;
            this.comboBoxCameraDevices.Location = new System.Drawing.Point(-1, 2);
            this.comboBoxCameraDevices.Name = "comboBoxCameraDevices";
            this.comboBoxCameraDevices.Size = new System.Drawing.Size(197, 21);
            this.comboBoxCameraDevices.TabIndex = 1;
            this.comboBoxCameraDevices.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameraDevices_SelectedIndexChanged);
            // 
            // buttonFullscreen
            // 
            this.buttonFullscreen.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonFullscreen.ForeColor = System.Drawing.Color.Black;
            this.buttonFullscreen.Location = new System.Drawing.Point(426, 2);
            this.buttonFullscreen.Name = "buttonFullscreen";
            this.buttonFullscreen.Size = new System.Drawing.Size(124, 23);
            this.buttonFullscreen.TabIndex = 2;
            this.buttonFullscreen.Text = "Fullscreen (Exit=ESC)";
            this.buttonFullscreen.UseVisualStyleBackColor = false;
            this.buttonFullscreen.Click += new System.EventHandler(this.buttonFullscreen_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(557, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Snap Picture Delay";
            // 
            // textBoxSnapPictureDelay
            // 
            this.textBoxSnapPictureDelay.Location = new System.Drawing.Point(661, 4);
            this.textBoxSnapPictureDelay.Name = "textBoxSnapPictureDelay";
            this.textBoxSnapPictureDelay.Size = new System.Drawing.Size(67, 20);
            this.textBoxSnapPictureDelay.TabIndex = 4;
            // 
            // buttonSnap
            // 
            this.buttonSnap.Location = new System.Drawing.Point(734, 2);
            this.buttonSnap.Name = "buttonSnap";
            this.buttonSnap.Size = new System.Drawing.Size(51, 23);
            this.buttonSnap.TabIndex = 5;
            this.buttonSnap.Text = "Snap";
            this.buttonSnap.UseVisualStyleBackColor = true;
            this.buttonSnap.Click += new System.EventHandler(this.buttonSnap_Click);
            // 
            // labelSnapCountdown
            // 
            this.labelSnapCountdown.AutoSize = true;
            this.labelSnapCountdown.ForeColor = System.Drawing.Color.Red;
            this.labelSnapCountdown.Location = new System.Drawing.Point(787, 7);
            this.labelSnapCountdown.Name = "labelSnapCountdown";
            this.labelSnapCountdown.Size = new System.Drawing.Size(89, 13);
            this.labelSnapCountdown.TabIndex = 6;
            this.labelSnapCountdown.Text = "Snap Countdown";
            // 
            // comboBoxResolutions
            // 
            this.comboBoxResolutions.FormattingEnabled = true;
            this.comboBoxResolutions.Location = new System.Drawing.Point(202, 2);
            this.comboBoxResolutions.Name = "comboBoxResolutions";
            this.comboBoxResolutions.Size = new System.Drawing.Size(197, 21);
            this.comboBoxResolutions.TabIndex = 7;
            this.comboBoxResolutions.SelectedIndexChanged += new System.EventHandler(this.comboBoxResolutions_SelectedIndexChanged);
            // 
            // panelSplashGraphics
            // 
            this.panelSplashGraphics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSplashGraphics.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSplashGraphics.BackColor = System.Drawing.Color.Transparent;
            this.panelSplashGraphics.ForeColor = System.Drawing.Color.Transparent;
            this.panelSplashGraphics.Location = new System.Drawing.Point(260, 134);
            this.panelSplashGraphics.MaximumSize = new System.Drawing.Size(5000, 5000);
            this.panelSplashGraphics.Name = "panelSplashGraphics";
            this.panelSplashGraphics.Size = new System.Drawing.Size(510, 561);
            this.panelSplashGraphics.TabIndex = 8;
            this.panelSplashGraphics.Visible = false;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.Location = new System.Drawing.Point(902, 2);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSettings.TabIndex = 9;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // FormCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.panelSplashGraphics);
            this.Controls.Add(this.comboBoxResolutions);
            this.Controls.Add(this.labelSnapCountdown);
            this.Controls.Add(this.buttonSnap);
            this.Controls.Add(this.textBoxSnapPictureDelay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonFullscreen);
            this.Controls.Add(this.comboBoxCameraDevices);
            this.Controls.Add(this.pictureBoxCamera);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(200, 160);
            this.Name = "FormCamera";
            this.Text = "DT Blog Utility Camera++";
            this.Load += new System.EventHandler(this.FormCamera_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCamera;
        private System.Windows.Forms.ComboBox comboBoxCameraDevices;
        private System.Windows.Forms.Button buttonFullscreen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSnapPictureDelay;
        private System.Windows.Forms.Button buttonSnap;
        private System.Windows.Forms.Label labelSnapCountdown;
        private System.Windows.Forms.ComboBox comboBoxResolutions;
        private System.Windows.Forms.Panel panelSplashGraphics;
        private System.Windows.Forms.Button buttonSettings;
    }
}