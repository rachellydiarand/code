using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MothPictureViewer
{
    public partial class FormPictureViewerSettings : Form
    {
        public FormPictureViewerSettings()
        {
            InitializeComponent();

            string currentPath = Properties.Settings.Default.ImageSavePath;
            if (!string.IsNullOrEmpty(currentPath))
            {
                textBoxImageSaveDirectory.Text = currentPath;
            }

            textBoxImageCopyOffsetX.Text = Properties.Settings.Default.ImageCopyOffsetX.ToString();
            textBoxImageCopyOffsetY.Text = Properties.Settings.Default.ImageCopyOffsetY.ToString();

            string colorStr = Properties.Settings.Default.BackgroundColor;
            if(!string.IsNullOrEmpty(colorStr))
            {
                textBoxBackgroundColor.Text = colorStr;
            }
        }

        private void buttonImageSaveDirectoryBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            
            string currentPath = Properties.Settings.Default.ImageSavePath;
            if(!string.IsNullOrEmpty(currentPath))
            {
                dialog.SelectedPath = currentPath;
            }

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.ImageSavePath = dialog.SelectedPath;
                Properties.Settings.Default.Save();
                textBoxImageSaveDirectory.Text = dialog.SelectedPath;
            }
        }

        private void textBoxImageSaveDirectory_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ImageSavePath = textBoxImageSaveDirectory.Text;
            Properties.Settings.Default.Save();
        }

        private void FormPictureViewerSettings_Load(object sender, EventArgs e)
        {

        }

        private void textBoxImageCopyOffsetX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int x = int.Parse(textBoxImageCopyOffsetX.Text);
                Properties.Settings.Default.ImageCopyOffsetX = x;
                Properties.Settings.Default.Save();
            }
            catch(Exception ex)
            {
                
            }
        }

        private void textBoxImageCopyOffsetY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int y = int.Parse(textBoxImageCopyOffsetY.Text);
                Properties.Settings.Default.ImageCopyOffsetY = y;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public static Color GetColorFromHexString(string hexColor)
        {
            // Ensure the string starts with '#' for proper parsing
            if (!hexColor.StartsWith("#"))
            {
                hexColor = "#" + hexColor;
            }

            // Convert the hex string to a Color object
            Color color = ColorTranslator.FromHtml(hexColor);
            return color;
        }

        private void textBoxBackgroundColor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Color c = GetColorFromHexString(textBoxBackgroundColor.Text);
                Properties.Settings.Default.BackgroundColor = textBoxBackgroundColor.Text;
                Properties.Settings.Default.Save();
                FormMothPictureViewer.Instance.BackColor = c;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Settings: Color string was invalid.");
            }
        }
    }
}
