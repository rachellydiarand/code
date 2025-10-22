using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MothBLogCamera
{
    public partial class FormCameraSettings : Form
    {
        public FormCameraSettings()
        {
            InitializeComponent();

            textBoxSavePath.Text = Properties.Settings.Default.ImageSavePath;
        }

        private void textBoxSavePath_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ImageSavePath = textBoxSavePath.Text;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            string currentPath = Properties.Settings.Default.ImageSavePath;
            if (!string.IsNullOrEmpty(currentPath))
            {
                dialog.SelectedPath = currentPath;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.ImageSavePath = dialog.SelectedPath;
                Properties.Settings.Default.Save();
                textBoxSavePath.Text = dialog.SelectedPath;
            }
        }
    }
}
