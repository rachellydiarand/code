namespace DT_Blog_Utility
{
    partial class FormImageCopy
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
            this.listBoxImagesToCopy = new System.Windows.Forms.ListBox();
            this.textBoxDestinationPath = new System.Windows.Forms.TextBox();
            this.textBoxProgress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonTransferNow = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxImagesToCopy
            // 
            this.listBoxImagesToCopy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxImagesToCopy.FormattingEnabled = true;
            this.listBoxImagesToCopy.Location = new System.Drawing.Point(13, 13);
            this.listBoxImagesToCopy.Name = "listBoxImagesToCopy";
            this.listBoxImagesToCopy.ScrollAlwaysVisible = true;
            this.listBoxImagesToCopy.Size = new System.Drawing.Size(775, 381);
            this.listBoxImagesToCopy.TabIndex = 0;
            this.listBoxImagesToCopy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxImagesToCopy_KeyDown);
            // 
            // textBoxDestinationPath
            // 
            this.textBoxDestinationPath.Location = new System.Drawing.Point(13, 438);
            this.textBoxDestinationPath.Name = "textBoxDestinationPath";
            this.textBoxDestinationPath.Size = new System.Drawing.Size(609, 20);
            this.textBoxDestinationPath.TabIndex = 1;
            // 
            // textBoxProgress
            // 
            this.textBoxProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProgress.Location = new System.Drawing.Point(13, 510);
            this.textBoxProgress.Multiline = true;
            this.textBoxProgress.Name = "textBoxProgress";
            this.textBoxProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxProgress.Size = new System.Drawing.Size(779, 96);
            this.textBoxProgress.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 480);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Progress....";
            // 
            // buttonTransferNow
            // 
            this.buttonTransferNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonTransferNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTransferNow.ForeColor = System.Drawing.Color.White;
            this.buttonTransferNow.Location = new System.Drawing.Point(628, 435);
            this.buttonTransferNow.Name = "buttonTransferNow";
            this.buttonTransferNow.Size = new System.Drawing.Size(139, 23);
            this.buttonTransferNow.TabIndex = 4;
            this.buttonTransferNow.Text = "Transfer Now";
            this.buttonTransferNow.UseVisualStyleBackColor = false;
            this.buttonTransferNow.Click += new System.EventHandler(this.buttonTransferNow_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(628, 464);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // FormImageCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 618);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonTransferNow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxProgress);
            this.Controls.Add(this.textBoxDestinationPath);
            this.Controls.Add(this.listBoxImagesToCopy);
            this.Name = "FormImageCopy";
            this.Text = "FormImageCopy";
            this.Load += new System.EventHandler(this.FormImageCopy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxImagesToCopy;
        private System.Windows.Forms.TextBox textBoxDestinationPath;
        private System.Windows.Forms.TextBox textBoxProgress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonTransferNow;
        private System.Windows.Forms.Button button1;
    }
}