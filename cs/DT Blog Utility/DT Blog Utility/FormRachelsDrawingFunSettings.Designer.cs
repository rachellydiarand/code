namespace DT_Blog_Utility
{
    partial class FormRachelsDrawingFunSettings
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
            this.textBoxRedrawSkipFrames = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Redraw Skip Rate";
            // 
            // textBoxRedrawSkipFrames
            // 
            this.textBoxRedrawSkipFrames.Location = new System.Drawing.Point(114, 10);
            this.textBoxRedrawSkipFrames.Name = "textBoxRedrawSkipFrames";
            this.textBoxRedrawSkipFrames.Size = new System.Drawing.Size(100, 20);
            this.textBoxRedrawSkipFrames.TabIndex = 1;
            this.textBoxRedrawSkipFrames.TextChanged += new System.EventHandler(this.textBoxRedrawSkipFrames_TextChanged);
            // 
            // FormRachelsDrawingFunSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 48);
            this.Controls.Add(this.textBoxRedrawSkipFrames);
            this.Controls.Add(this.label1);
            this.Name = "FormRachelsDrawingFunSettings";
            this.Text = "FormRachelsDrawingFunSettings";
            this.Load += new System.EventHandler(this.FormRachelsDrawingFunSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRedrawSkipFrames;
    }
}