//using AForge.Imaging.Filters;
using MothPictureViewer.controls;
using MothPictureViewer.models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MothPictureViewer.controls;
using System.Net.WebSockets;

namespace MothPictureViewer
{
    public partial class FormMothPictureViewer : Form
    {
        private MothCanvas Canvas { get; set; }
        public FormMothPictureViewer ThisForm { get; set; } // am I insane?  Y do I need this?  Probably don't but scope problm?
        public static decimal MouseSensitivityPercentage { get; set; }
        private static FormMothPictureViewer mInstance { get; set; }
        //private static List<FormMothPictureViewer> AllInstances = new List<FormMothPictureViewer>();

        Timer EnterFrameTimer { get; set; }
        public Image CurrentImage { get; set; }

        private List<PictureBox> PbList { get; set; } = new List<PictureBox>();
        private PictureBox CurrentPictureBox { get; set; }
        private decimal ImageCurrentScale { get; set; } = 1;
        public bool Dragging { get; set; }
        private Point StartDragPoint { get; set; }
        private Point EndDragPoint { get; set; }
        private Point StartDragPbStartingPoint { get; set; }
        private PictureBox SelectionPictureBoxTop { get; set; }
        private PictureBox SelectionPictureBoxBottom { get; set; }
        private PictureBox SelectionPictureBoxLeft { get; set; }
        private PictureBox SelectionPictureBoxRight { get; set; }
        public int SelectionDashFlipAlt { get; set; } = -1;
        public bool SelectionDragging { get; set; }
        public Label DebuggingLabel { get; set; }
        private int XMN { get; set; }
        private int YMN { get; set; }
        private bool SaveSelectionToClipboardNextFrame { get; set; }
        private PictureBox TopLayerContainer { get; set; }
        public MothGraphicsOverlay TopLayerForEventsAndDrawing { get; set; }
        private string BasePath { get; set; }
        public string ImgDirectory { get; set; }
        public int ImgDirectoryIndex { get; set; }
        public string CurrentImageFullName { get; set; }
        private bool BestFitMenuItemStickieOn { get; set; }
        public List<Point> PolygonPoints { get; set; } = new List<Point>();
        public List<FilledPolygon> ActivePolygons { get; set; } = new List<FilledPolygon>();
        public int PolygonPointRadius { get; set; } = 4;
        //private MothGraphicsOverlay PolygonPictureBox { get; set; }

        private GraphicalOverlay graphicalOverlay { get; set; }


        public static FormMothPictureViewer Instance
        {
            get
            {
                return mInstance;
            }
        }


        public FormMothPictureViewer(string[] files, Bitmap bmp = null)
        {
            InitializeComponent();

            if (mInstance == null)
            {
                // this is the first instance
                mInstance = this;               
            }

            DoubleBuffered = true;

            ThisForm = this;
            ThisForm.BackColor = Color.AntiqueWhite;
            ThisForm.KeyPreview = true;
            ThisForm.KeyDown += new KeyEventHandler(DoKeyDown);

            // disabling this causes bad overlay behavior
            // this handler calls ReorderControlHeiarchy()
            // which is that method I gleaned off the internet
            // and it puts the overlay layer on the BOTTOM of the stack of Controls!
            // like it is going through the controls in reverse order when rendering!
            // let's try disabling
            ThisForm.Paint += new PaintEventHandler(OnFormPaint);

            //if (AllInstances == null) AllInstances = new List<FormMothPictureViewer>();
            //AllInstances.Add(this);

            MouseSensitivityPercentage = (decimal).1;
            SaveSelectionToClipboardNextFrame = false;

            // try putting all controls in a MothCanvas (having problems getting top overlay to be transparent
            Canvas = new MothCanvas(Color.LightBlue);
            Canvas.Name = "MothCanvasForMothPictureViewer";
            Canvas.Dock = DockStyle.Fill;
            //Controls.Add(Canvas);

            // this is a debugging textbox
            DebuggingLabel = new Label();
            DebuggingLabel.Name = "DebuggingLabel";
            DebuggingLabel.Width = 100;
            DebuggingLabel.AutoSize = false;
            DebuggingLabel.Top = 50;
            DebuggingLabel.Left = 10;
            DebuggingLabel.Text = "1234567890";
            this.Controls.Add(DebuggingLabel);

            Canvas.Dock = DockStyle.Fill;
            Pb = new PictureBox();
            Pb.Name = "Pb";
            Pb.Enabled = false;
            //Pb.Visible = false;
            //Pb.MouseWheel += new MouseEventHandler(DoZoom);
            //Pb.MouseDown += new MouseEventHandler(DoMouseDown);
            //Pb.MouseMove += new MouseEventHandler(DoMouseMoveGetCoordinates);
            //Pb.MouseUp += new MouseEventHandler(DoMouseUp);

            
            Init(files, bmp);

            SelectionPictureBoxTop = new PictureBox();
            SelectionPictureBoxTop.Name = "SelectionPictureBox";
            SelectionPictureBoxTop.Enabled = false;
            SelectionPictureBoxTop.BackColor = Color.Transparent;
            SelectionPictureBoxTop.Visible = false;
            this.Controls.Add(SelectionPictureBoxTop);
            SelectionPictureBoxBottom = new PictureBox();
            SelectionPictureBoxBottom.Name = "SelectionPictureBox";
            SelectionPictureBoxBottom.Enabled = false;
            SelectionPictureBoxBottom.BackColor = Color.Transparent;
            SelectionPictureBoxBottom.Visible = false;
            this.Controls.Add(SelectionPictureBoxBottom);
            SelectionPictureBoxLeft = new PictureBox();
            SelectionPictureBoxLeft.Name = "SelectionPictureBox";
            SelectionPictureBoxLeft.Enabled = false;
            SelectionPictureBoxLeft.BackColor = Color.Transparent;
            SelectionPictureBoxLeft.Visible = false;
            this.Controls.Add(SelectionPictureBoxLeft);
            SelectionPictureBoxRight = new PictureBox();
            SelectionPictureBoxRight.Name = "SelectionPictureBox";
            SelectionPictureBoxRight.Enabled = false;
            SelectionPictureBoxRight.BackColor = Color.Transparent;
            SelectionPictureBoxRight.Visible = false;
            this.Controls.Add(SelectionPictureBoxRight);
            //PolygonPictureBox = new MothGraphicsOverlay();
            //PolygonPictureBox.Name = "PolygonPictureBox";
            //PolygonPictureBox.Enabled = false;
            //PolygonPictureBox.BackColor = Color.Transparent;
            //PolygonPictureBox.Visible = false;
            //PolygonPictureBox.Dock = DockStyle.Fill;
            //// calling Invalidate() on PolygonPictureBox will clear the graphics and fire the Paint event
            //PolygonPictureBox.Paint += new PaintEventHandler(OnPaintPolygonOverlay);
            //Controls.Add(PolygonPictureBox);


            // trying to do all our mouse event listeners on one object.
            // tried the stage, but didn't work well.  Q: Can we put a label on top of everything to capture events AND draw?
            //TopLayerContainer = new PictureBox();
            //TopLayerContainer.BackColor = Color.Transparent;
            //TopLayerContainer.Dock = DockStyle.Fill;
            //this.Controls.Add(TopLayerContainer);
            TopLayerForEventsAndDrawing = new MothGraphicsOverlay(this);//new Panel();// 
            //TopLayerLabelForEventsAndDrawing.Visible = true;
            //TopLayerLabelForEventsAndDrawing.BackColor = Color.Transparent;// Color.Green;
            //TopLayerLabelForEventsAndDrawing.Enabled = true;
            TopLayerForEventsAndDrawing.Dock = DockStyle.Fill;

            // try to achieve "true transparency" background
            // override the OnPaintBackground providing an empty implemention
            // the other method is to use the "WS-EX_LAYERED extended style, which is part of the 
            // layered window functionality introduced around Windows 2000
            // attempting #1
            // with an extension method?  I'll put that method
            // NO, not attempted
            // going back to using MothGraphicsOverlay but using Panel instead of Label

            // making progress now, it is complicated
            // wrote notes on the MothGraphicOverlay.cs in the DoPaintBackground method
            // YES! this is working as expected!
            TopLayerForEventsAndDrawing.Visible = false;

            // the next step is to fix the polygon drawing methods to have a Graphics.Clear each time
            // as it's drawing over itself each iteration!

            
            this.Controls.Add(TopLayerForEventsAndDrawing);

            // listeners on form now?
            //TopLayerLabelForEventsAndDrawing.Name = "TopLayerLabelForEventsAndDrawing";
            //TopLayerLabelForEventsAndDrawing.MouseWheel += new MouseEventHandler(DoZoom);
            //TopLayerLabelForEventsAndDrawing.MouseDown += new MouseEventHandler(DoMouseDown);
            //TopLayerLabelForEventsAndDrawing.MouseUp += new MouseEventHandler(DoMouseUp);
            //TopLayerLabelForEventsAndDrawing.MouseMove += new MouseEventHandler(DoMouseMoveGetCoordinates);
            //TopLayerLabelForEventsAndDrawing.Paint += new PaintEventHandler(OnPaintPolygonOverlay);

            //graphicalOverlay = new GraphicalOverlay();
            //graphicalOverlay.Owner = this;
            //this.Container.Add(graphicalOverlay);


            TopLayerForEventsAndDrawing.Paint += new PaintEventHandler(OnPaintPolygonOverlay);
            this.MouseWheel += new MouseEventHandler(DoZoom);
            this.MouseDown += new MouseEventHandler(DoMouseDown);
            this.MouseUp += new MouseEventHandler(DoMouseUp);
            this.MouseMove += new MouseEventHandler(DoMouseMoveGetCoordinates);
            TopLayerForEventsAndDrawing.MouseMove += new MouseEventHandler(DoMouseMoveGetCoordinates);

            this.Resize += new EventHandler(DoResize);
            this.FormClosing += new FormClosingEventHandler(Destroy);
            this.ControlAdded += new ControlEventHandler(OnControlAdded);

            DoResize(null, null);

            //TopLayerLabelForEventsAndDrawing.Focus();
        }

        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            // bubble the TopLayerLabelForEventsAndDrawing to the top of the stack so that it can draw on top of all controls
            //this.Controls.Remove(TopLayerLabelForEventsAndDrawing);
            //this.Controls.Add(TopLayerLabelForEventsAndDrawing);
        }

        private void OnFormPaint(object sender, PaintEventArgs e)
        {
            // repositions control order to put the Top drawing layer on top of everything
            ReorderControlHeiarchy();
            //TopLayerLabelForEventsAndDrawing.BringToFront();
        }

        private PictureBox Pb 
        {
            get
            {
                if(CurrentPictureBox != null)
                {
                    return CurrentPictureBox;
                }
                else
                {
                    CurrentPictureBox = new PictureBox();
                    return CurrentPictureBox;
                }
            }
            set
            {
                PbList.Add(value);
                CurrentPictureBox = value;// PbList[PbList.Count - 1];
            }
        }

        private Image Img 
        {
            get
            {
                if(CurrentImage != null)
                {
                    return CurrentImage;
                }
                return Pb.Image;
            }
            set
            {
                CurrentImage = value;
                Pb.Image = value;
            }
        }

        private void FormMothPictureViewer_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FormMothPictureViewer_Load(object sender, EventArgs e)
        {
            EnterFrameTimer = new System.Windows.Forms.Timer();
            EnterFrameTimer.Interval = 100;
            EnterFrameTimer.Tick += new EventHandler(DoEnterFrame);
            EnterFrameTimer.Start();
        }

        public void DoEnterFrame(object sender = null, EventArgs args = null)
        {
            // this is called by the [ MASTER FORM ]]] on this mInstance (Instance)
            // OnMouseMove was drawing too slowly, so we'll fire that on this event for all instances
            //foreach(var viewer in AllInstances)
            //{
            //    viewer.DoMouseMove();
            //}

            DoMouseMove();
        }

        private void DoMouseMoveGetCoordinates(object sender, MouseEventArgs e)
        {
            // This is fun code, but not using it anymore!  (da parent loop)
            //int xmn = e.X;
            //int ymn = e.Y;
            //Control c = (sender as Control);

            //if (c.Parent != null)
            //{
            //    do
            //    {
            //        xmn += c.Left;
            //        ymn += c.Top;
            //        c = c.Parent as Control;
            //    } while (c.Parent != null && c.GetType() != typeof(FormMothPictureViewer));
            //}
            //XMN = xmn;
            //YMN = ymn;

            XMN = e.X;
            YMN = e.Y;

            DebuggingLabel.Text = "X: " + XMN + ", Y: " + YMN;
            Debug.WriteLine(DebuggingLabel.Text);
        }

        public void Init(string[] files, Bitmap bmp = null)
        {
            ImgDirectoryIndex = -1;
            string file = string.Empty;
            if(files.Count() > 0)
            {
                file = files[0];
                
            }

            // for testing....
            //file = "C:\\_rachel\\Pics\\Classic 2\\moth_picture_viewer_image_paste_2025-09-11_11_11_54.png";

            if (string.IsNullOrEmpty(file))
            {
                Img = bmp;
            }
            else
            {
                Img = Image.FromFile(file);
                if(Img != null)
                {
                    FileInfo fi = new FileInfo(file);
                    ImgDirectory = fi.DirectoryName;
                    CurrentImageFullName = fi.FullName;
                }
            }
            Pb.Image = Img;
            this.Controls.Add(Pb);
            BestFit();
        }

        private void DoMouseDown(object sender, MouseEventArgs e)
        {
            // YES!! this mouse down on the top label thingy I made works!
            // So, we'll now go back to an old design pattern of mine in Flash 5 of doing my own hittest against object z-order to see if we clicked something!
            //MessageBox.Show("DoMouseDown()");

            string objectThatWasClicked = "";
            Control clickedControl = null;

            int highestZorder = -1;
            foreach(Control c in this.Controls)
            {                
                if(c.Name != "TopLayerLabelForEventsAndDrawing")
                {
                    if (c.Location.X <= e.X &&
                    c.Location.Y <= e.Y &&
                    c.Location.X + c.Width >= e.X &&
                    c.Location.Y + c.Height >= e.Y)
                    {
                        // is this GetChildIndex the z-order?  Makes sense, it is likely.
                        if (this.Controls.GetChildIndex(c) >= highestZorder)
                        {
                            clickedControl = c;
                        }
                    }
                }                
            }

            // stop dragging
            if(clickedControl != null && e.Button == MouseButtons.Left)
            {
                // continue with the old code to start drag etc...
                // may want the if condition here if we want to do something else on a click!

                // couldn't make center of single SlectionPictureBox transparent, so using 4 picture boxes now
               
                Dragging = false;
                SelectionDragging = false;

                StartDragPoint = new Point(XMN, YMN);

                if (clickedControl.Name == "Pb")
                {
                    // we are dragging the picture itself
                    StartDragPbStartingPoint = new Point(Pb.Left, Pb.Top);
                    Dragging = true;
                    Cursor = Cursors.Hand;
                }
                //else if (clickedControl.Name == "SelectionPictureBox")
                //{
                //    // this is the selection picture box!
                //    if (e.Button == MouseButtons.Right)
                //    {
                //        // start a selection
                //        StartDragPoint = new Point(XMN, YMN);
                //        SelectionDragging = true;
                //        SelectionPictureBox.Visible = true;
                //    }
                
                
            }

            // do this every click
            if (e.Button == MouseButtons.Right)
            {
                // start a selection
                StartDragPoint = new Point(XMN, YMN);
                SelectionDragging = true;
            }

            // control the selection picture box visibility (SelectionDragging set just above this line ^ up there ^^
            if (SelectionDragging)
            {
                // start a selection
                SelectionPictureBoxTop.Visible = true;
                SelectionPictureBoxBottom.Visible = true;
                SelectionPictureBoxLeft.Visible = true;
                SelectionPictureBoxRight.Visible = true;
                PutSelectionPictureBoxesOnTop();
            }
            else
            {
                SelectionPictureBoxTop.Visible = false;
                SelectionPictureBoxBottom.Visible = false;
                SelectionPictureBoxLeft.Visible = false;
                SelectionPictureBoxRight.Visible = false;
            }


        }

        private void DoMouseUp(object sender, MouseEventArgs e)
        {
            if (SelectionDragging)
            {
                SaveSelectionToClipboardNextFrame = true;

                // hide it so that we don't take a screenshot of it! (for pasteing using the button in the menu :-)  -- no double chin )
                //SelectionPictureBoxTop.Visible = false;
                //SelectionPictureBoxBottom.Visible = false;
                //SelectionPictureBoxLeft.Visible = false;
                //SelectionPictureBoxRight.Visible = false;
            }

            EndDragPoint = new Point(XMN, YMN);

            SelectionDragging = false;
            Dragging = false;
            Cursor = Cursors.Default;
        }

        private int Abs(int i)
        {
            if (i < 0) return i * -1;
            return i;
        }

        private void UpdateSelectionBox(Rectangle r)
        {
            int dashLength = 6;

            using (Graphics g = SelectionPictureBoxTop.CreateGraphics())
            {
                g.Clear(Color.Transparent);
                SelectionPictureBoxTop.Width = r.Width;
                SelectionPictureBoxTop.Height = 1;
                SelectionPictureBoxTop.Left = r.Left;
                SelectionPictureBoxTop.Top = r.Top;
                for (int i = 0; i < r.Width; i += dashLength)
                {
                    // top
                    g.DrawLine(new Pen(Color.Black, 1), i, 0, i + 3, 0);
                    g.DrawLine(new Pen(Color.White, 1), i + 3, 0, i + 6, 0);
                }
            }

            using (Graphics g = SelectionPictureBoxBottom.CreateGraphics())
            {
                SelectionPictureBoxBottom.Width = r.Width;
                SelectionPictureBoxBottom.Height = 1;
                SelectionPictureBoxBottom.Left = r.Left;
                SelectionPictureBoxBottom.Top = r.Top + r.Height - 1;
                for (int i = 0; i < r.Width; i += dashLength)
                {
                    g.DrawLine(new Pen(Color.White, 1), i, 0, i + 3, 0);
                    g.DrawLine(new Pen(Color.Black, 1), i + 3, 0, i + 6, 0);
                }
            }

            using (Graphics g = SelectionPictureBoxLeft.CreateGraphics())
            {
                SelectionPictureBoxLeft.Width = 2;
                SelectionPictureBoxLeft.Height = r.Height;
                SelectionPictureBoxLeft.Left = r.Left;
                SelectionPictureBoxLeft.Top = r.Top;

                for (int i = 0; i < r.Height; i += dashLength)
                {
                    g.DrawLine(new Pen(Color.White, 1), 0, i, 0, i + 3);
                    g.DrawLine(new Pen(Color.Black, 1), 0, i + 3, 0, i + 6);
                }
            }

            using (Graphics g = SelectionPictureBoxRight.CreateGraphics())
            {
                SelectionPictureBoxRight.Width = 1;
                SelectionPictureBoxRight.Height = r.Height;
                SelectionPictureBoxRight.Left = r.Left + r.Width - 1;
                SelectionPictureBoxRight.Top = r.Top;

                for (int i = 0; i < r.Height; i += dashLength)
                {
                    g.DrawLine(new Pen(Color.Black, 1), 0, i, 0, i + 3);
                    g.DrawLine(new Pen(Color.White, 1), 0, i + 3, 0, i + 6);
                }
            }
        }

        private void DoMouseMove(object sender = null, EventArgs args = null)
        {
            // still experimenting with the best way to capture mouse movement in this Environment/Language
            //DoMouseMoveGetCoordinates(null, null);

            if (SaveSelectionToClipboardNextFrame)
            {
                SaveSelectionToClipboardNextFrame = false;
                // subtracting 2 from these because we are using a screenshot and want to eliminate the bounding box we drew without a system redraw()

                int w = Abs(EndDragPoint.X - StartDragPoint.X);
                int h = Abs(EndDragPoint.Y - StartDragPoint.Y);

                if(w == 0 || h == 0) return;

                Bitmap bmp = new Bitmap(w, h);

                int x = StartDragPoint.X + Properties.Settings.Default.ImageCopyOffsetX;
                int y = StartDragPoint.Y + Properties.Settings.Default.ImageCopyOffsetY;

                if(EndDragPoint.X < x) x = EndDragPoint.X;
                if(EndDragPoint.Y < y) y = EndDragPoint.Y;

                // apply the offset
                x += Properties.Settings.Default.ImageCopyOffsetX;
                y += Properties.Settings.Default.ImageCopyOffsetY;

                // Draw the screenshot into our bitmap.

                // Determine the size of the "virtual screen", which includes all monitors.
                int screenLeft = SystemInformation.VirtualScreen.Left;//SystemInformation.VirtualScreen.Left;
                int screenTop = SystemInformation.VirtualScreen.Top;
                int screenWidth = SystemInformation.VirtualScreen.Width;
                int screenHeight = SystemInformation.VirtualScreen.Height;

                // this.ClientRectangle.Left
                int xBound = this.Location.X + this.ClientRectangle.Left + x;// + Pb.Left + (SelectionPictureBox.Left - Pb.Left) + 1;
                int yBound = this.Location.Y + ClientRectangle.Top + y;// + Pb.Top + (SelectionPictureBox.Top - Pb.Top) + 1;

                Rectangle r = new Rectangle(
                xBound,
                yBound,
                w,
                h);

                if (r.X + r.Width <= screenWidth &&
                    r.Y + r.Height <= screenHeight)
                {
                    // we need to hide the selection picture box
                    // we are copying graphics from the multi-monitor display to this bitmap to go on the clipboard
                    //SelectionPictureBox.Visible = false;
                    //SelectionPictureBox.Refresh();
                    
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(r.Left, r.Top, 0, 0, new Size(r.Width, r.Height));
                        Clipboard.SetImage(bmp);
                    }

                    // set it back to visible
                    SelectionPictureBoxTop.Visible = true;
                    SelectionPictureBoxBottom.Visible = true;
                    SelectionPictureBoxLeft.Visible = true;
                    SelectionPictureBoxRight.Visible = true;
                    PutSelectionPictureBoxesOnTop();
                }
            }
            
            // this selection drawing isn't working well, but taking the image is, let's leave it invisible!
            if (SelectionDragging)
            {
                int ymn = YMN;
                int xmn = XMN;

                if (xmn == StartDragPoint.X || ymn == StartDragPoint.Y) return;

                //using (Graphics g = TopLayerLabelForEventsAndDrawing.CreateGraphics())
                //{
                //    g.Clear(Color.Transparent);
                //    //g.Flush();
                //}

                // used before we switched the OnEnterFrame to the master form
                //if((sender as PictureBox).Image == null) return;// just a test, we'll want to drag if the selection box too

                // tried Math.Abs here but a negative input vector returns a zero :-(
                int widthProp = XMN - StartDragPoint.X;
                int heightProp = YMN - StartDragPoint.Y;

                // There is a solution! Math.Abs() failed and returned a zero for negative values!
                if(widthProp < 0) widthProp *= -1;
                if(heightProp < 0) heightProp *= -1;

                // more sanity!
                if (widthProp < 4) widthProp = 4;
                if (heightProp < 4) heightProp = 4;

                //SelectionPictureBox.Left = StartDragPoint.X;
                //SelectionPictureBox.Top = StartDragPoint.Y;
                //SelectionPictureBox.Width = widthProp;
                //SelectionPictureBox.Height = heightProp;

                //if(xmn - StartDragPoint.X < 0)
                //{
                //    SelectionPictureBox.Left = StartDragPoint.X - widthProp;
                //}
                //if(ymn  - StartDragPoint.Y < 0)
                //{
                //    SelectionPictureBox.Top = StartDragPoint.Y - heightProp;
                //}

                int w = widthProp;// SelectionPictureBox.Width;
                int h = heightProp;// SelectionPictureBox.Height;

                // now (next version of this code), we'll create a rectangle object here so we can just draw that directely to the top label control where we capture mouse events
                
                int xr = XMN;
                int yr= YMN;
                if(xr > StartDragPoint.X) {
                    // we use StartDrawPoint.X as the left anchor
                    xr = StartDragPoint.X;
                }
                if(yr > StartDragPoint.Y)
                {
                    yr = StartDragPoint.Y;
                }

                Rectangle r = new Rectangle(xr, yr, w, h);
                UpdateSelectionBox(r);

                // update the boxes in the lower right
                textBoxSelectionWidth.Text = r.Width.ToString();
                textBoxSelectionHeight.Text = r.Height.ToString();


                //using (Graphics g = TopLayerLabelForEventsAndDrawing.CreateGraphics())
                //{
                //    g.Clear(Color.Transparent);
                //    var whitePen = new Pen(Color.White, 1);
                //    var blackPen = new Pen(Color.Black, 1);
                //    dashLength = 6;

                //    for (int x = 0; x < widthProp + (dashLength * 2); x += dashLength * 2)
                //    {
                //        int x1 = x;
                //        int x2 = x + dashLength;
                //        int x3 = x + dashLength + dashLength;

                //        if (x1 > r.Width - 1) x1 = r.Width - 1;
                //        if (x2 > r.Width - 1) x2 = r.Width - 1;
                //        if (x3 > r.Width - 1) x3 = r.Width - 1;

                //        // top
                //        g.DrawLine(whitePen, new Point(r.X + x1, r.Y), new Point(r.X + x2, r.Y));
                //        g.DrawLine(blackPen, new Point(r.X + x2, r.Y), new Point(r.X + x3, r.Y));
                //        // bottom
                //        g.DrawLine(blackPen, new Point(r.X + x1, r.Y + r.Height - 1), new Point(r.X + x2, r.Y + r.Height - 1));
                //        g.DrawLine(whitePen, new Point(r.X + x2, r.Y + r.Height - 1), new Point(r.X + x3, r.Y + r.Height - 1));
                //    }

                //    for (int y = 0; y < heightProp + (dashLength * 2); y += dashLength * 2)
                //    {
                //        int y1 = y;
                //        int y2 = y + dashLength;
                //        int y3 = y + dashLength + dashLength;

                //        // left
                //        g.DrawLine(blackPen, new Point(r.X, r.Y + y1), new Point(r.X, r.Y + y2));
                //        g.DrawLine(whitePen, new Point(r.X, r.Y + y2), new Point(r.X, r.Y + y3));
                //        // right
                //        g.DrawLine(whitePen, new Point(r.X + r.Width - 1, r.Y + y1), new Point(r.Width - 1, r.Y + y2));
                //        g.DrawLine(blackPen, new Point(r.X + r.Width - 1, r.Y + y2), new Point(r.Width - 1, r.Y + y3));
                //    }

                //Color c = Color.FromArgb(0x000000ff);
                //g.DrawRectangle(new Pen(c), new Rectangle(2, 2, widthProp - 4, heightProp - 4));

                //SelectionPictureBox.BringToFront();

            }
            
            if(Dragging)
            {
                Pb.Left = XMN - (StartDragPoint.X - StartDragPbStartingPoint.X);
                Pb.Top = YMN - (StartDragPoint.Y - StartDragPbStartingPoint.Y);
            }

        }

        private void DoKeyDown(object sender, KeyEventArgs args)
        {
            //MessageBox.Show(args.KeyValue.ToString());
            // polygon fill tools
            if(args.KeyCode == Keys.F1)
            {
                // place point
                int xOffset = Properties.Settings.Default.ImageCopyOffsetX;
                int yOffset = Properties.Settings.Default.ImageCopyOffsetY;
                int w2 = PolygonPointRadius / 2;

                Point p = new Point(XMN + xOffset- w2, YMN - yOffset - w2);
                PolygonPoints.Add(p);
            }
            else if(args.KeyCode == Keys.F4)
            {
                // fill polygon
                if(PolygonPoints.Count > 0)
                {
                    FilledPolygon newPolygon = new FilledPolygon();
                    newPolygon.Points = PolygonPoints;
                    ActivePolygons.Add(newPolygon);
                    //PolygonPoints = new List<Point>();

                    // clear points
                    PolygonPoints = new List<Point>();
                }
            }
            else if (args.KeyCode == Keys.F5)
            {                
                
                // for testing
                // this does toggle the TopLayerForEventsAndDrawing Visibility
                // I had put the z-order of this child at zero and the picture Pb was hidden
                // so this will show it
                TopLayerForEventsAndDrawing.Visible = !TopLayerForEventsAndDrawing.Visible;

                if(TopLayerForEventsAndDrawing.Visible)
                {
                    MessageBox.Show("Drawing Layer Activated");
                }
                else
                {
                    MessageBox.Show("Drawing Layer Disabled");
                }

                // it's still a transparent background problem
                // of a panel?
                // fixed, there is a reorder function now
            }
            else if (args.KeyCode == Keys.F8)
            {
                // remove last polygon
                if(ActivePolygons.Count > 0)
                {
                    ActivePolygons.RemoveAt(ActivePolygons.Count - 1);
                }
            }
            else if (args.KeyCode == Keys.Escape)
            {
                // clear points
                PolygonPoints = new List<Point>();
            }

            // trying something different
            //RefreshPolygonOverlay();

            // like this
            // it should cause the paint listeners to fire
            // and hopefully invalidate the context
            // ok, this is working well enough to get my current political/personal
            // business project completed, thankfully!
            // =^.^=
            // it's actually the F3 key that does .Visible toggle on the TopLayerFroEventsAndDrawing
            // that redraws the context when shown after hidding
            // and that causes all the red dots points to be cleared after the ESC key
            // and the F4 clearning of the last ploygon works as well
            // .Visible toggle and back on redraws the overlay with the code in the above method
            // and allows me to complete my business project today.
            // I could put it on a frame event
            // or there is something else with a Paint event that is overriding it
            // I guess I'll look to see if I still have the ThisForm.Paint += listener still enabled first.
            TopLayerForEventsAndDrawing.Refresh();
        }

        private void ReorderControlHeiarchy()
        {
            int i = this.Controls.GetChildIndex(TopLayerForEventsAndDrawing);
            if (i > 0)
            {
                // move the top drawing layer to the top of the control list
                // the master OnPaint method likely goes through this list in order
                // and the Pb is under the drawing layer.
                this.Controls.SetChildIndex(TopLayerForEventsAndDrawing, 0);
            }
        }

        private void RefreshPolygonOverlay()
        {
            //this.Invalidate();
            //TopLayerLabelForEventsAndDrawing.Invalidate();// this will raise the Paint event and clear the graphics
            using (var g = TopLayerForEventsAndDrawing.CreateGraphics())
            {
                //TopLayerLabelForEventsAndDrawing.Invalidate(true);
                DoPaintPolygonOverlay(g);
            }
                
        }

        private void OnPaintPolygonOverlay(object sender, PaintEventArgs e)
        {
            // can also use args.graphics
            // this is not clearing the context either
            // g.Clear(Color.Transparent) not working too
            //TopLayerForEventsAndDrawing.Invalidate(true);

            // try calling the DoPaintBackground
            // it doesn't clear the background either, but it is slightly better I think
            //TopLayerForEventsAndDrawing.DoPaintBackground(e);

            // this likely won't work, but let's try
            // hiding the control and then showing it
            // this produces flickering elements, but doesn't clear anything
            //TopLayerForEventsAndDrawing.Visible = false;
            //TopLayerForEventsAndDrawing.Visible = true;

            // maybe a simple refresh?
            // will this cause this method to fire twice?  or more?
            // yes, it causes an infinite loop!
            // let's try putting this call outside of the event listener!
            //TopLayerForEventsAndDrawing.Refresh();

            DoPaintPolygonOverlay(e.Graphics);          
        }

        private int DoPaintPolygonOverlyCount = 0;
        public void DoPaintPolygonOverlay(Graphics g)
        {
            //Debug.WriteLine("OnPointPolygon Overlay " + (++DoPaintPolygonOverlyCount).ToString());
            //return;
            // polygon drawing for blacking out unwanted items on a document (like a medical record)
            //TopLayerLabelForEventsAndDrawing.BringToFront();
            //using (Graphics g = TopLayerLabelForEventsAndDrawing.CreateGraphics())
            //{
            //    g.Clear(TopLayerLabelForEventsAndDrawing.BackColor);
            //}

            // this is not working well
            //g.Clear(Color.Transparent);
            //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.

            if (PolygonPoints != null && PolygonPoints.Count > 0)
            {
                foreach (var point in PolygonPoints)
                {
                    Pen pen = new Pen(new SolidBrush(Color.Red), PolygonPointRadius);
                    g.DrawEllipse(pen, point.X, point.Y, PolygonPointRadius, PolygonPointRadius);
                }
            }

            if (ActivePolygons != null && ActivePolygons.Count > 0)
            {
                foreach (var polygon in ActivePolygons)
                {
                    Brush b = new SolidBrush(Color.Black);
                    g.FillPolygon(b, polygon.Points.ToArray());
                }
            }

            //TopLayerForEventsAndDrawing.Invalidate();
            //Pb.Invalidate();

            //e.ClipRectangle
        }

        public void BestFit(bool make100percent = false)
        {
            if (Img == null || Pb == null) return;

            Pb.Width = this.ClientRectangle.Width;
            Pb.Height = this.ClientRectangle.Height;

            decimal imgW = (decimal)Img.Width;
            decimal imgH = (decimal)Img.Height;

            decimal wProp = (decimal)Pb.Width;
            decimal hProp = (decimal)Pb.Height;

            if (!make100percent)
            {
                // (imgW / imgH) = (this.Width / this.Hieght)

                wProp = (decimal)this.ClientRectangle.Width;
                hProp = (decimal)(((decimal)((decimal)wProp * imgH)) / imgW);

                if (hProp > this.ClientRectangle.Height)
                {
                    // reverse which will put the black bars on top and bottom
                    hProp = this.ClientRectangle.Height;
                    wProp = ((decimal)(((decimal)hProp * imgW)) / imgH);
                }

                Bitmap imgProp = new Bitmap(Img, (int)wProp, (int)hProp);

                Pb.Controls.Clear();
                Pb.Image = imgProp;

                Pb.Width = (int)wProp;
                Pb.Height = (int)hProp;

                Pb.Top = this.ClientRectangle.Top + menuStrip1.Top + menuStrip1.Height + ((this.ClientRectangle.Height / 2) - ((int)(hProp / 2)));
                Pb.Left = this.ClientRectangle.Left + ((this.ClientRectangle.Width / 2) - ((int)(wProp / 2)));

                // if the image is bigger than the screen, lets center it again

            }
            else
            {
                Pb.Image = Img;
                Pb.Width = Img.Width;
                Pb.Height = Img.Height;
                Pb.Top = (this.Height / 2) - (Img.Height / 2) + menuStrip1.Height;
                Pb.Left = (this.Width / 2) - (Img.Width / 2);
            }

        }

        private void ScaleImage(decimal pNewScale)
        {
            // sanity check
            if (pNewScale <= 0) return;

            // doing this fancy stuff to try to eliminate loss of precision
            // we'll always do the final calculation on the master image itself

            Image pbImage = Pb.Image;
            decimal imgWidth = (decimal)pbImage.Width * pNewScale;
            decimal imgHeight = (decimal)pbImage.Height * pNewScale;

            //// compute the image in the picture box's current scale
            //decimal xScale = imgWidth / (decimal)Img.Width;
            //decimal yScale = imgHeight / (decimal)Img.Height;

            //// compute the image in the picture box scale to the paremeter scale "suggestion"
            //decimal xScaleSuggestedPercentage = w / imgWidth;
            //decimal yScaleSuggestedPercentage = h / imgHeight;

            //Pb.Image.Dispose();
            Bitmap imgProp = new Bitmap(Img, (int)imgWidth, (int)imgHeight);
            Pb.Image = imgProp;
        }

        private void DoResize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized) return;
            if (BestFitMenuItemStickieOn)
            {
                BestFit();
            }
            
            //TopLayerLabelForEventsAndDrawing.Top = this.ClientRectangle.Top;
            //TopLayerLabelForEventsAndDrawing.Left = this.ClientRectangle.Left;
            //TopLayerLabelForEventsAndDrawing.Width = this.ClientRectangle.Width;
            //TopLayerLabelForEventsAndDrawing.Height = this.ClientRectangle.Height;
        }

        private bool HitTest(Control c, int x, int y)
        {
            Control clickedControl = null;
            int highestZorder = -1;
            // this is a BASIC (rectangular hitTest) with a mouse/point
            if (c.Name != "TopLayerLabelForEventsAndDrawing")
            {
                if (c.Location.X <= x &&
                c.Location.Y <= y &&
                c.Location.X + c.Width >= x &&
                c.Location.Y + c.Height >= y)
                {
                    // is this GetChildIndex the z-order?  Makes sense, it is likely.
                    if (Controls.GetChildIndex(c) >= highestZorder)
                    {
                        clickedControl = c;
                    }
                }
            }

            if(clickedControl == c) return true;
            return false;
        }

        private void DoZoom(object sender, MouseEventArgs e)
        {
            // I like this line of code, so leaving for reference, maybe for future projects?  Or just a friendly reminder to myself or others?
            //if (sender.GetType() == typeof(FormMothPictureViewer) || Pb == null || Pb.Image == null) return;

            if(!HitTest(Pb, e.X, e.Y)) return;

            // stage coordinates
            int xmn = e.X;
            int ymn = e.Y;

            int xmnInsidePictureBox = e.X - Pb.Location.X;
            int ymnInsidePictureBox = e.Y - Pb.Location.Y;

            decimal xMousePercentage = (decimal)xmnInsidePictureBox / (decimal)(Pb.Image.Width);
            decimal yMousePercentage = (decimal)ymnInsidePictureBox / (decimal)(Pb.Image.Height);


            decimal movementAmount = ((decimal)((decimal)e.Delta / (decimal)SystemInformation.MouseWheelScrollDelta));
            movementAmount = (decimal)1 + (movementAmount * MouseSensitivityPercentage);

            if (true)
            {
                int newPbW = Math.Max(this.Width, Pb.Image.Width);
                int newPbH = Math.Max(this.Height, Pb.Image.Height);
                decimal imgWidth = (decimal)Pb.Image.Width * movementAmount;
                decimal imgHeight = (decimal)Pb.Image.Height * movementAmount;

                ScaleImage(movementAmount);


                decimal newImageW = Pb.Image.Width;
                decimal newImageH = Pb.Image.Height;
                int leftSubtractionAmount = (int)(imgWidth * xMousePercentage);
                int topSubtractionAmount = (int)(imgHeight * yMousePercentage);
                // need to reposition (center to test, then reposition on mouse position
                Pb.Width = (int)newImageW;// used to be newPbW
                Pb.Height = (int)newImageH;
                Pb.Left = xmn - leftSubtractionAmount;
                Pb.Top = ymn - topSubtractionAmount;
            }
            else
            {
                int xn = Pb.Left;
                int yn = Pb.Top;
                int wn = Pb.Width;
                int hn = Pb.Height;

                decimal wProp = (decimal)wn * movementAmount;
                decimal hProp = (decimal)hn * movementAmount;

                Pb.Width = (int)wProp;
                Pb.Height = (int)hProp;
                //ScaleImage(wProp, hProp);

                // need to compute the percentage of the width that the mouse was pointed at so that we can move the Left that percentage times the width
                decimal xPercentagePosition = ((decimal)xmnInsidePictureBox / (decimal)wn);
                decimal yPercentagePosition = ((decimal)ymnInsidePictureBox / (decimal)yn);

                Pb.Left = xn - ((int)((decimal)movementAmount * xPercentagePosition));
                Pb.Top = yn - ((int)((decimal)movementAmount * yPercentagePosition));
            }


        }

        private void oneHundredPercentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // this is the 100% menu item (group);
            BestFit(true);
        }

        private void bestFitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // this is the best fit menu item (group)
            BestFit(false);
        }

        private void bestFitToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {
            if(BestFitMenuItemStickieOn)
            {
                // turn the sticky off on bestFit!
                BestFitMenuItemStickieOn = false;
                (sender as ToolStripMenuItem).ForeColor = Color.Black;
            }
            else
            {
                BestFitMenuItemStickieOn = true;
                (sender as ToolStripMenuItem).ForeColor = Color.LightBlue;
                BestFit();
            }
        }

        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Form1.Instance == null) return;

            // Determine the size of the "virtual screen", which includes all monitors.
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            // Create a bitmap of the appropriate size to receive the screenshot.
            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                }

                // Do something with the Bitmap here, like save it to a file:
                //Form1.Instance.CameraSaveSnappedImage(bmp);

                // or save it to clipboard
                Clipboard.SetImage(bmp);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        
        private void DoControlsAdded(object sender, EventArgs e)
        {
            // we want the Label control to be on top of everything to capture mouse events (events) and for drawing
            //TopLayerLabelForEventsAndDrawing.BringToFront();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // well, we are going to flesh this out later
            // the naming convention is pulled from the blogutility
            // of course there are the best fit and grab from other sources type stuff there too
            // but this is a stand alone app, so we are not going to inherit those dependencies, we could, but likely not ever going to
            string filename = DateTime.Now.ToString("yyyy-MM-dd_hh.mm.ss" + "_moth_picture_viewer.jpg");
           
        }

        private void PutSelectionPictureBoxesOnTop()
        {
            SelectionPictureBoxTop.BringToFront();
            SelectionPictureBoxBottom.BringToFront();
            SelectionPictureBoxLeft.BringToFront();
            SelectionPictureBoxRight.BringToFront();
            TopLayerForEventsAndDrawing.BringToFront();
        }

        private void PutPolygonPictureBoxOnTop()
        {
            TopLayerForEventsAndDrawing.BringToFront();
        }

        public void Destroy(object sender = null, EventArgs e = null)
        {
            if (Instance == null) return;
            if (Pb != null)
            {
                Pb.Controls.Clear();
                this.Controls.Remove(Pb);
                Pb = null;
            }

            //if (AllInstances.Count != 0)
            //{
            //    AllInstances.Remove(this);
            //    if (AllInstances.Count > 0)
            //    {
            //        mInstance = AllInstances[0];
            //    }
            //}

            //if (AllInstances.Count == 0)
            //{
            //    // all instances have been removed, destoy everything
            //    mInstance = null;
            //    AllInstances = null;
            //}

            EnterFrameTimer.Stop();
            EnterFrameTimer = null;
            mInstance = null;
        }

        private List<string> EnumerateFileDirectory()
        {
            List<string> returnFiles = new List<string>();
            if(ImgDirectory != null)
            {
                List<string> allFiles = Directory.EnumerateFiles(ImgDirectory).ToList();
                if(allFiles != null && allFiles.Count > 0)
                {
                    for(int i = 0; i < allFiles.Count; i++)
                    {
                        string file = allFiles[i];
                        FileInfo fi = new FileInfo(file);
                        if (fi.Exists && (fi.Extension.ToLower() == ".jpg" || fi.Extension.ToLower() == ".png"))
                        {
                            returnFiles.Add(fi.FullName);
                        }
                    }
                }
            }
            return returnFiles;
        }

        private void previousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = EnumerateFileDirectory();
            if (files.Count == 0) return;

            int currentIndex = -1;
            FileInfo fi;
            for(int i = 0; i < files.Count; i++)
            {
                fi = new FileInfo(files[i]);
                if(fi.FullName == CurrentImageFullName)
                {
                    currentIndex = i;
                    break;
                }
            }

            currentIndex--;
            if(currentIndex < 0)
            {
                currentIndex = files.Count - 1;
            }

            Img = Image.FromFile(files[currentIndex]);
            if(Img != null)
            {
                Pb.Image = Img;
                BestFit();

                ImgDirectoryIndex = currentIndex;
                CurrentImageFullName = files[currentIndex];
            }
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = EnumerateFileDirectory();
            if (files.Count == 0) return;

            int currentIndex = -1;
            FileInfo fi;
            for (int i = 0; i < files.Count; i++)
            {
                fi = new FileInfo(files[i]);
                if (fi.FullName == CurrentImageFullName)
                {
                    currentIndex = i;
                    break;
                }
            }

            currentIndex++;
            if (currentIndex > files.Count - 1)
            {
                currentIndex = 0;
            }

            Img = Image.FromFile(files[currentIndex]);
            if (Img != null)
            {
                Pb.Image = Img;
                BestFit();

                ImgDirectoryIndex = currentIndex;
                CurrentImageFullName = files[currentIndex];
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormPictureViewerSettings().Show();
        }

        private void RotateFlipImage(RotateFlipType rotateFlipType)
        {
            if ((Pb != null && Pb.Image != null) == false) return;

            var img = Pb.Image;
            img.RotateFlip(rotateFlipType);

            // need to scale to Pb dimensions before
            // because image is likely the full size image?
            if(img.Width != Pb.Width)
            {
                Pb.Width = img.Width;
            }
            if (img.Height != Pb.Height)
            {
                Pb.Height = img.Height;
            }

            Pb.Image = img;
        }

        private void flipHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateFlipImage(RotateFlipType.RotateNoneFlipX);
        }

        private void flipVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateFlipImage(RotateFlipType.RotateNoneFlipY);
        }

        private void rotate90DegreesClockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateFlipImage(RotateFlipType.Rotate90FlipNone);
        }

        private void rotate90DegreesCounterClockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateFlipImage(RotateFlipType.Rotate270FlipNone);
        }

        private void pasteInPlaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                string currentSavePath = Properties.Settings.Default.ImageSavePath;
                if (string.IsNullOrEmpty(currentSavePath) || !Directory.Exists(currentSavePath))
                {
                    MessageBox.Show("There is no save path defined.  Settings (on the File->Settings) will now open so that you can select one.  You'll have to select paste again to save this image.");
                    new FormPictureViewerSettings().Show();
                    return;
                }

                // need to create new instance of app instead of simple new form so that closing can be independent

                //var newPictureView = new FormMothPictureViewer(new string[0], Clipboard.GetImage() as Bitmap);
                //newPictureView.Show();
                //newPictureView.SetTopLevel(true);
                string fileName = Path.Combine(currentSavePath, "moth_picture_viewer_image_paste_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".png");
                var img = (Image)Clipboard.GetImage();
                img.Save(fileName, ImageFormat.Png);

                // this is where the process changes to paste in place
                // with the new picturebox list
                // we add to the list
                // then we'll have to handle the z-order and dragging of multiple picture boxes
                // the new setter/mutator on the Pb variable is handy and was quickly coded!
                // a nice foundation for this new feature!
                //Process.Start(fileName);

                Pb = new PictureBox();
                //Pb.Image = img;
                Img = img;
                this.Controls.Add(Pb);
                Pb.BringToFront();
                //TopLayerLabelForEventsAndDrawing.BringToFront();
                menuStrip1.BringToFront();
                this.BringToFront();
                TopLayerForEventsAndDrawing.Focus();
                BestFit();
            }
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                string currentSavePath = Properties.Settings.Default.ImageSavePath;
                if (string.IsNullOrEmpty(currentSavePath) || !Directory.Exists(currentSavePath))
                {
                    MessageBox.Show("There is no save path defined.  Settings (on the File->Settings) will now open so that you can select one.  You'll have to select paste again to save this image.");
                    new FormPictureViewerSettings().Show();
                    return;
                }

                // need to create new instance of app instead of simple new form so that closing can be independent

                //var newPictureView = new FormMothPictureViewer(new string[0], Clipboard.GetImage() as Bitmap);
                //newPictureView.Show();
                //newPictureView.SetTopLevel(true);
                string fileName = Path.Combine(currentSavePath, "moth_picture_viewer_image_paste_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".png");
                var img = (Image)Clipboard.GetImage();
                img.Save(fileName, ImageFormat.Png);

                Process.Start(fileName);
            }
        }

        private void WriteSelectionRectangleToClipboard(Size widthAndHeightSize)
        {
            // used before we switched the OnEnterFrame to the master form
            //if((sender as PictureBox).Image == null) return;// just a test, we'll want to drag if the selection box too

            // tried Math.Abs here but a negative input vector returns a zero :-(
            int widthProp = widthAndHeightSize.Width;
            int heightProp = widthAndHeightSize.Height;

            // There is a solution! Math.Abs() failed and returned a zero for negative values!
            if (widthProp < 0) widthProp *= -1;
            if (heightProp < 0) heightProp *= -1;

            // more sanity!
            if (widthProp < 4) widthProp = 4;
            if (heightProp < 4) heightProp = 4;

            //SelectionPictureBox.Left = StartDragPoint.X;
            //SelectionPictureBox.Top = StartDragPoint.Y;
            //SelectionPictureBox.Width = widthProp;
            //SelectionPictureBox.Height = heightProp;

            //if(xmn - StartDragPoint.X < 0)
            //{
            //    SelectionPictureBox.Left = StartDragPoint.X - widthProp;
            //}
            //if(ymn  - StartDragPoint.Y < 0)
            //{
            //    SelectionPictureBox.Top = StartDragPoint.Y - heightProp;
            //}

            int w = widthProp;// SelectionPictureBox.Width;
            int h = heightProp;// SelectionPictureBox.Height;

            // now (next version of this code), we'll create a rectangle object here so we can just draw that directely to the top label control where we capture mouse events

            int xr = StartDragPoint.X;
            int yr = StartDragPoint.Y;
            if (xr > StartDragPoint.X)
            {
                // we use StartDrawPoint.X as the left anchor
                xr = StartDragPoint.X;
            }
            if (yr > StartDragPoint.Y)
            {
                yr = StartDragPoint.Y;
            }

            Rectangle r = new Rectangle(xr, yr, w, h);
            UpdateSelectionBox(r);

            // copy to clipboard

            // we copied this from another method
            // and now disabling this because we just
            // defined the new width and height
            // with the textboxes, right?
            //int w = Abs(EndDragPoint.X - StartDragPoint.X);
            //int h = Abs(EndDragPoint.Y - StartDragPoint.Y);

            if (w == 0 || h == 0) return;

            Bitmap bmp = new Bitmap(w, h);

            int x = StartDragPoint.X + Properties.Settings.Default.ImageCopyOffsetX;
            int y = StartDragPoint.Y + Properties.Settings.Default.ImageCopyOffsetY;

            if (EndDragPoint.X < x) x = EndDragPoint.X;
            if (EndDragPoint.Y < y) y = EndDragPoint.Y;

            // apply the offset
            x += Properties.Settings.Default.ImageCopyOffsetX;
            y += Properties.Settings.Default.ImageCopyOffsetY;

            // Draw the screenshot into our bitmap.

            // Determine the size of the "virtual screen", which includes all monitors.
            int screenLeft = SystemInformation.VirtualScreen.Left;//SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            // this.ClientRectangle.Left
            int xBound = this.Location.X + this.ClientRectangle.Left + x;// + Pb.Left + (SelectionPictureBox.Left - Pb.Left) + 1;
            int yBound = this.Location.Y + ClientRectangle.Top + y;// + Pb.Top + (SelectionPictureBox.Top - Pb.Top) + 1;

            r = new Rectangle(
            xBound,
            yBound,
            w,
            h);

            if (r.X + r.Width <= screenWidth &&
                r.Y + r.Height <= screenHeight)
            {
                // we need to hide the selection picture box
                // we are copying graphics from the multi-monitor display to this bitmap to go on the clipboard
                //SelectionPictureBox.Visible = false;
                //SelectionPictureBox.Refresh();

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(r.Left, r.Top, 0, 0, new Size(r.Width, r.Height));
                    Clipboard.SetImage(bmp);
                }

                // set it back to visible
                SelectionPictureBoxTop.Visible = true;
                SelectionPictureBoxBottom.Visible = true;
                SelectionPictureBoxLeft.Visible = true;
                SelectionPictureBoxRight.Visible = true;
                PutSelectionPictureBoxesOnTop();
            }

        }

        private void textBoxSelectionHeight_TextChanged(object sender, EventArgs e)
        {
            if(!SelectionDragging)
            {
                Size s = new Size();
                try
                {
                    s.Width = int.Parse(textBoxSelectionWidth.Text);
                    s.Height = int.Parse(textBoxSelectionHeight.Text);
                    WriteSelectionRectangleToClipboard(s);
                } catch(Exception ex)
                {
                    Debug.WriteLine("Selection Size Manual Resize failed....");
                }
                
            }
        }

        private void textBoxSelectionWidth_TextChanged(object sender, EventArgs e)
        {
            if (!SelectionDragging)
            {
                Size s = new Size();
                try
                {
                    s.Width = int.Parse(textBoxSelectionWidth.Text);
                    s.Height = int.Parse(textBoxSelectionHeight.Text);
                    WriteSelectionRectangleToClipboard(s);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Selection Size Manual Resize failed....");
                }

            }
        }
    }
}
