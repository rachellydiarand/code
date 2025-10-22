using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MothPictureViewer.controls
{
    public class MothGraphicsOverlay : Panel
    {
        private FormMothPictureViewer DaParent;


        public MothGraphicsOverlay(FormMothPictureViewer pictureViewerParent)
        {            
            DaParent = pictureViewerParent;

            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.Opaque, true); // Important for transparent background
            //this.BackColor = Color.Red; // Make the background transparent
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    Graphics g = e.Graphics;

        //    // Example: Draw a semi-transparent rectangle
        //    using (SolidBrush semiTransparentBrush = new SolidBrush(Color.FromArgb(100, Color.LightYellow)))
        //    {
        //        g.FillRectangle(semiTransparentBrush, this.ClientRectangle);
        //    }

        //    // Example: Draw text
        //    using (Font font = new Font("Arial", 12))
        //    using (SolidBrush textBrush = new SolidBrush(Color.Black))
        //    {
        //        g.DrawString("Overlay Text", font, textBrush, 210, 210);
        //    }
        //}



        protected override void InitLayout()
        {
            base.InitLayout();
            Enabled = false;
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.BackColor = Color.Transparent; // Set the actual background color to Transparent
            //this.Paint += new PaintEventHandler(DoPaintEvent);
            //DoubleBuffered = true;
            // removing DoubleBuffered changed the background from black to 
            // well, somthing more transparent and a little broken
        }

        public void DoPaintBackground(PaintEventArgs e)
        {
            // this is almost working now!
            // the problem is that using the FillTransparent() method I coded
            // causes the whole form to be invisible!
            // until I swap layers with the PictureBox and then it's all perfect
            // so maybe wait to fire this until the first/second execution loop?

            // calling this from the OnPaintBackground caused
            // the form to be invisible
            // then I resized the window and the bigger area
            // was drawn with the new background color
            // it essentially didn't work as intended
            //using(var g = this.CreateGraphics())
            //{
            //    Rectangle r = this.Bounds;
            //    FillTransparent(g, r);
            //}

            // try to just pass to this function from the OnPaintBackground
            // then try to remove that and fire it from the main form OnPaint or TopLayer OnPaint
            Rectangle r = e.ClipRectangle;
            FillTransparent(e.Graphics, r);

            // almost there!
            // I have TopLayerLabelForEventsAndDrawing.Visible = !TopLayerLabelForEventsAndDrawing.Visible;
            // on the F3 key in the main app
            // it starts off showing an invisible form
            // then on first press, it shows the PictureBox Image, but it doesn't show the overlay drawings
            // then on second press, the overlay drawings are showing and I think it's perfect
            // double checking these results now....
            // well, that's basically it
            // on that second press, the mouse event listeners for the PictureBox stop working
            // and the overlay drawing funciton is a little broken because of 
            // graphics.clear() type issues.
            // It may be fixable, which is why I'm writing these notes
            // so that I may go back to this state.
            // I could add this project to git now,
            // but I'm kind of old and remember getting started with SVN (before GIT) in 2008.
            // I'll attempt to clean it up as I will be committing to GIT soon.
            // I'll start by making .Enabled = false; in this constructor function
            // that didn't work, I observed the coordinate box on screen
            // not updating, but placing new points and polygons
            // with the F1 and F2 keys was working
            // so it's just a display redraw issue.
            // the background of the parent must be drawn into the background
            // hmm, options
            // I have all the information to draw this layer (Panel) programmatically every time
            // so I don't care about the background on that level
            // but the problem of not being able to drag elements on the background exists
            // the long way to handle that is to handle the background elements
            // and then redraw this element
            // not really needed for the REASON I'm coding this feature right now
            // could even be a positive keeping this behavior as an option of "frozen background"
            // so that the background stays put while you draw over it
            // both ways are nice actually
            // but....  cutting to the chase, if I can get it to work this way
            // I can do my political work for the rest of the week and come back to this
            // right?
            // so, to finish up....
            // the clear dots ESC key and clear last polygon F4 key
            // are not functioning properly because of graphics.Clear()
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // empty implementation
            // can this work on a Control.Panel?
            //return;
            //// overrides this method to do nothing which will make it transparent
            DoPaintBackground(e);


            //Color c = BackColor;
            ////Parent.BackColor = Color.Transparent;
            //Color pc = Parent.BackColor;
            //string pcName = Parent.Name;
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{            
        //    e.Graphics.Clear(BackColor);
        //    Rectangle r = e.ClipRectangle;
        //    FillTransparent(e.Graphics, r);

        //    //DaParent.OnPaintPolygonOverlay(this, e);
        //    //base.OnPaint(e);
        //    //Rectangle r = e.ClipRectangle;
        //    //FillTransparent(e.Graphics, r);
        //}

        //private void DoPaintEvent(object sender, PaintEventArgs e)
        //{
        //    // calling Invalidate() will trigger the Paint method and clear the graphics (apparently)
        //    base.OnPaint(e);
        //}

        private void FillTransparent(Graphics g, Rectangle r)
        {
            Color c = Color.FromArgb(0, Color.Yellow);
            g.FillRectangle(new SolidBrush(c), r);
        }
    }

    
}
