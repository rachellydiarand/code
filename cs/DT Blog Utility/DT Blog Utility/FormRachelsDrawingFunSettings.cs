using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DT_Blog_Utility
{
    public partial class FormRachelsDrawingFunSettings : Form
    {
        public FormRachelsDrawingFunSettings()
        {
            InitializeComponent();
        }

        private void textBoxRedrawSkipFrames_TextChanged(object sender, EventArgs e)
        {
            int i = RachelsDrawingFun.Instance.ResizeSkipOverflowStep;
            bool didChange = int.TryParse(textBoxRedrawSkipFrames.Text, out i);
            if(!didChange)
            {
                return;
            }
            RachelsDrawingFun.Instance.ResizeSkipOverflowStep = i;
        }

        private void FormRachelsDrawingFunSettings_Load(object sender, EventArgs e)
        {
            textBoxRedrawSkipFrames.Text = RachelsDrawingFun.Instance.ResizeSkipOverflowStep.ToString();
        }
    }
}
