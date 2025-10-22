using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MothPictureViewer.controls
{
    public class MothCanvas : Control
    {
        Color CanvasColor = Color.White;

        public MothCanvas(Color c)
        {
            CanvasColor = c;
        }
        protected override void InitLayout()
        {
            base.InitLayout();
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.LightBlue;// Color.Transparent; // Set the actual background color to Transparent
            //this.Paint += new PaintEventHandler(DoPaintEvent);
            DoubleBuffered = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // overrides this method to do nothing which will make it transparent
            Rectangle r = e.ClipRectangle;
            FillTransparent(e.Graphics, r);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Rectangle r = e.ClipRectangle;
            FillTransparent(e.Graphics, r);
        }

        //private void DoPaintEvent(object sender, PaintEventArgs e)
        //{
        //    // calling Invalidate() will trigger the Paint method and clear the graphics (apparently)
        //    base.OnPaint(e);
        //}

        private void FillTransparent(Graphics g, Rectangle r)
        {
            Color c = Color.FromArgb(255, CanvasColor);
            g.FillRectangle(new SolidBrush(c), r);
        }
    }
}
