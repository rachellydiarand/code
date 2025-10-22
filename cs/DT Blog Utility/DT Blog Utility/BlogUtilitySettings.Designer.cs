namespace DT_Blog_Utility
{
    partial class BlogUtilitySettings
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
            this.textBoxCameraExecutable = new System.Windows.Forms.TextBox();
            this.buttonCameraExecutablePathBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Camera Executable";
            // 
            // textBoxCameraExecutable
            // 
            this.textBoxCameraExecutable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraExecutable.Location = new System.Drawing.Point(117, 13);
            this.textBoxCameraExecutable.Name = "textBoxCameraExecutable";
            this.textBoxCameraExecutable.Size = new System.Drawing.Size(621, 20);
            this.textBoxCameraExecutable.TabIndex = 1;
            this.textBoxCameraExecutable.TextChanged += new System.EventHandler(this.textBoxCameraExecutable_TextChanged);
            // 
            // buttonCameraExecutablePathBrowse
            // 
            this.buttonCameraExecutablePathBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCameraExecutablePathBrowse.Location = new System.Drawing.Point(735, 11);
            this.buttonCameraExecutablePathBrowse.Name = "buttonCameraExecutablePathBrowse";
            this.buttonCameraExecutablePathBrowse.Size = new System.Drawing.Size(34, 23);
            this.buttonCameraExecutablePathBrowse.TabIndex = 2;
            this.buttonCameraExecutablePathBrowse.Text = "...";
            this.buttonCameraExecutablePathBrowse.UseVisualStyleBackColor = true;
            this.buttonCameraExecutablePathBrowse.Click += new System.EventHandler(this.buttonCameraExecutablePathBrowse_Click);
            // 
            // BlogUtilitySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 450);
            this.Controls.Add(this.buttonCameraExecutablePathBrowse);
            this.Controls.Add(this.textBoxCameraExecutable);
            this.Controls.Add(this.label1);
            this.Name = "BlogUtilitySettings";
            this.Text = "BlogUtilitySettings";
            this.Load += new System.EventHandler(this.BlogUtilitySettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCameraExecutable;
        private System.Windows.Forms.Button buttonCameraExecutablePathBrowse;
    }
}