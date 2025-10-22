using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace DT_Blog_Utility
{
    public partial class BlogUtilitySettings : Form
    {
        public BlogUtilitySettings()
        {
            InitializeComponent();
            textBoxCameraExecutable.Text = Properties.Settings.Default.CameraExectuable;
        }

        private void buttonCameraExecutablePathBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();

            string currentPath = Properties.Settings.Default.CameraExectuable;
            if (!string.IsNullOrEmpty(currentPath))
            {
                dialog.FileName = currentPath;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.CameraExectuable = dialog.FileName;
                Properties.Settings.Default.Save();
                textBoxCameraExecutable.Text = dialog.FileName;
            }
        }

        

        private void textBoxCameraExecutable_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CameraExectuable = textBoxCameraExecutable.Text;
            Properties.Settings.Default.Save();
        }

        private void BlogUtilitySettings_Load(object sender, EventArgs e)
        {

        }
    }
}
