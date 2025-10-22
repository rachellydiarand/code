//using AForge.Video;
//using AForge.Video.DirectShow;
using AForge.Video;
using AForge.Video.DirectShow;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MothBLogCamera
{
    public partial class FormCamera : Form
    {
        public static FormCamera _instance { get; set; }
        public double rd = (double)180 / Math.PI;
        public double dg = (double)Math.PI / 180;

        
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        private int GridSpacing { get; set; }
        private int NumOfGridlines { get; set; }
        private string GridCellStates { get; set; }
        private bool GridCellStatesInitialized = false;
        public Point LeftPoint { get; set; }

        private bool FormWasClosed = false;
        private DateTime FormInitTime;
        public Form ParentForm { get; set; }
        static public Size PictureBoxSize = new Size(2000, 2000);
        private bool SnapClicked = false;
        private bool CameraRunning = false;

        public bool SnapImage { get; set; }
        private DateTime SnappedImageTime = DateTime.Now;
        private int SnappedImageDelaySeconds = 0;
        public Image SnappedImage { get; set; }
        private List<VideoCapabilities> videoCapabilities = new List<VideoCapabilities>();
        public bool SizeIsChanging = false;

        public FormCamera(string[] args)
        {
            _instance = this;
            ParentForm = null;// populate from args possibly?
            InitializeComponent();

            GridCellStates = null;

            Init();

            FormClosing += new FormClosingEventHandler(FormCamera_Closing);
        }

        public bool Init()
        {
            FormInitTime = DateTime.Now;
            PictureBoxSize.Width = pictureBoxCamera.Width;
            PictureBoxSize.Height = pictureBoxCamera.Height;
            labelSnapCountdown.Text = "";
            
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in filterInfoCollection)
                comboBoxCameraDevices.Items.Add(Device.Name);

            if (comboBoxCameraDevices.Items.Count == 0)
            {
                MessageBox.Show("No cameras attached to system.");
                this.Close();
                return false;
            }

            comboBoxCameraDevices.SelectedIndex = 0;
            InitCamera();


            this.ResizeBegin += new EventHandler(PictureBoxResizeBegin);
            this.Resize += new EventHandler(PictureBoxSizeChangeHandler);
            this.ResizeEnd += new EventHandler(PictureBoxSizeChangeHandlerEnd);    
            panelSplashGraphics.MouseDown += new MouseEventHandler(DoSplashPanelMouseDown);
            

            return true;
        }

        private void PictureBoxResizeBegin(object sender, EventArgs e)
        {
            SizeIsChanging = true;
        }

        private void PictureBoxSizeChangeHandler(object sender, EventArgs e)
        {
            SizeIsChanging = true;
            PictureBoxSize.Width = pictureBoxCamera.Width;
            PictureBoxSize.Height = pictureBoxCamera.Height;
        }

        private void PictureBoxSizeChangeHandlerEnd(object sender, EventArgs e)
        {
            SizeIsChanging = false;
        }

        private void InitCamera()
        {            
            //StopCamera();
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBoxCameraDevices.SelectedIndex].MonikerString);
            int maxWidth = 400;
            int maxHeight = 300;
            VideoCapabilities maxVideoCapabilities = null;
            videoCapabilities = new List<VideoCapabilities>();
            comboBoxResolutions.Items.Clear();
            foreach (var item in videoCaptureDevice.VideoCapabilities)
            {
                bool addItem = false;
                if (item.FrameSize.Width > maxWidth)
                {
                    maxWidth = item.FrameSize.Width;
                    maxVideoCapabilities = item;
                    addItem = true;
                }
                if (item.FrameSize.Height > maxHeight)
                {
                    maxHeight = item.FrameSize.Height;
                    maxVideoCapabilities = item;
                    addItem = true;
                }
                if(addItem)
                {
                    videoCapabilities.Add(item);
                    comboBoxResolutions.Items.Add(item.FrameSize.Width.ToString() + " x " + item.FrameSize.Height.ToString());
                }
            }
            comboBoxResolutions.SelectedIndex = 0;
            //videoCaptureDevice.NewFrame += FinalFrame_NewFrame;
            //videoCaptureDevice.Start();
            //CameraRunning = true;            
        }

        private void SaveSnappedImage(Bitmap img)
        {
            string path = Properties.Settings.Default.ImageSavePath;
            if(!Directory.Exists(path))
            {
                MessageBox.Show("No save path was defined.  Please select one in Settings.");
                return;
            }
            string filename = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd_hh.mm.ss") + "_dt_blog_image.jpg");
            img.Save(filename);
        }

        public void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if(FormWasClosed) return;
            if (!CameraRunning) return;
            if(SizeIsChanging) return;
            try
            {
                if (pictureBoxCamera.InvokeRequired)
                {
                    // Call this same method but append THREAD2 to the text
                    Action safeWrite = delegate { FinalFrame_NewFrame(sender, eventArgs); };
                    pictureBoxCamera.Invoke(safeWrite);
                }
                else
                {
                    bool processSnappedImage = false;
                    if (SnapImage)
                    {
                        int secondsPassed = (int)(DateTime.Now - SnappedImageTime).TotalSeconds;
                        if(secondsPassed > SnappedImageDelaySeconds)
                        {
                            processSnappedImage = true;
                        }
                        else
                        {
                            labelSnapCountdown.Text = (SnappedImageDelaySeconds - secondsPassed).ToString();
                        }
                    }
                    if (processSnappedImage)
                    {
                        if(SnappedImage == null)
                        {
                           
                            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
                            pictureBoxCamera.Image = ResizeImage(bmp);
                            SnappedImage = bmp;
                            //ParentForm.CameraSaveSnappedImage(bmp);
                            CameraSaveSnappedImage(bmp);
                            labelSnapCountdown.Text = "Picture Saved";

                            Bitmap bmp2 = (Bitmap)eventArgs.Frame.Clone();
                            //ParentForm.FinalFrame_NewFrame(bmp2);
                            bmp2.Dispose();
                        }
                    }
                    else
                    {
                        Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
                        pictureBoxCamera.Image = ResizeImage(bmp);
                        bmp.Dispose();

                        bmp = (Bitmap)eventArgs.Frame.Clone();
                        //ParentForm.FinalFrame_NewFrame(bmp);
                        bmp.Dispose();
                    }
                    
                }
            }
            catch(Exception ex)
            {

            }

        }

        public void CameraSaveSnappedImage(Bitmap img)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.ImageSavePath))
            {
                MessageBox.Show("Save path not defined.  Go to settings to choose one.");
                return;
            }
            string filename = Path.Combine(Properties.Settings.Default.ImageSavePath, DateTime.Now.ToString("yyyy-MM-dd_hh.mm.ss") + "_dt_blog_image.jpg");
            img.Save(filename);
        }

        public System.Drawing.Image ResizeImageWorker(System.Drawing.Image image, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return (System.Drawing.Image)bmp;
        }

        private Image ResizeImage(Bitmap bmp)
        {
            Size sourceSize = new Size(bmp.Width, bmp.Height);
            Size destinationSize = new Size(PictureBoxSize.Width, PictureBoxSize.Height);
            Size newSize = BestFit(sourceSize, destinationSize);
            return ResizeImageWorker(bmp, newSize.Width, newSize.Height);
        }
                
        private void comboBoxCameraDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxResolutions.SelectedIndex < 0) return;
            RestartCamera();
        }

        private void pictureBoxCamera_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Size = " + pictureBoxCamera.Width.ToString() + ", " + pictureBoxCamera.Height.ToString());
        }
        
        private void buttonSnap_Click(object sender, EventArgs e)
        {
            if(SnapImage)
            {
                SnappedImage.Dispose();
                SnappedImage = null;
                SnapImage = false;
                SnappedImageDelaySeconds = 0;
                labelSnapCountdown.Text = "";
            }
            else
            {
                SnappedImageTime = DateTime.Now;
                if(!string.IsNullOrEmpty(textBoxSnapPictureDelay.Text))
                {
                    SnappedImageDelaySeconds = int.Parse(textBoxSnapPictureDelay.Text);
                }
                else 
                {
                    SnappedImageDelaySeconds = 0;
                }
                SnapImage = true;
            }
            /*
            // let's just use this button to stop the damn camera!
            //StopCamera();
            if(SnapClicked)
            {
                //SnapClicked = false;
                //if (videoCaptureDevice.IsRunning == true)
                //    videoCaptureDevice.Stop();

                //videoCaptureDevice = null;
                videoCaptureDevice.WaitForStop();
            }
            else
            {
                StopCamera();
                var vcd = videoCaptureDevice;
                videoCaptureDevice.SignalToStop();
                //videoCaptureDevice.Stop();
                SnapClicked = true;
            }
            */
        }

        private void RestartCamera()
        {
            if(FormWasClosed) return;
            if (FormInitTime == null) return;
            if (((int)(DateTime.Now - FormInitTime).TotalSeconds) < 2) return;

            CameraRunning = false;
            if(videoCaptureDevice != null)
            {
                videoCaptureDevice.NewFrame -= FinalFrame_NewFrame;
                videoCaptureDevice = null;
                
            }
            
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBoxCameraDevices.SelectedIndex].MonikerString);
            videoCaptureDevice.VideoResolution = videoCapabilities[comboBoxResolutions.SelectedIndex];
            videoCaptureDevice.Start();
            videoCaptureDevice.NewFrame += FinalFrame_NewFrame;
            CameraRunning = true;
        }

        private void comboBoxResolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCameraDevices.SelectedIndex < 0) return;
            RestartCamera();
        }

        public void DestroyCamera()
        {
            // called from main form and the close button
            CameraRunning = false;
            this.Visible = false;
        }

        private void FormCamera_Closing(Object sender, FormClosingEventArgs e)
        {
            // the close button
            // we'll use the _instance singleton to bring the form back
            DestroyCamera();
            //ParentForm.DestroyCamera();
            e.Cancel = true;
        }

        protected override void OnResize(EventArgs e)
        {
            DrawSplashScreen();
            base.OnResize(e);
        }

        private void DoSplashPanelMouseDown(object sender, MouseEventArgs e)
        {            
            int xmn = e.X;
            int ymn = e.Y;

            int row = (int) Math.Floor((decimal) (xmn / GridSpacing));
            int col = (int)Math.Floor((decimal)(ymn / GridSpacing));

            int i = (row * NumOfGridlines) + col;
            string val = GridCellStates.Substring(i, 1);
            if(val == "1")
            {
                string str = GridCellStates;
                GridCellStates = str.Substring(0, i) + "0" + str.Substring(i + 2);
            }
            else
            {
                string str = GridCellStates;
                GridCellStates = str.Substring(0, i) + "1" + str.Substring(i + 2);
            }

            DrawSplashScreen();
        }

        private void DrawSplashScreen()
        {
            //MessageBox.Show("Hit");

            int padding = 30;
            Size formSize = new Size();
            formSize.Width = this.Size.Width - (2 * padding);
            formSize.Height = this.Size.Height - (2 * padding);

            panelSplashGraphics.Size = BestFit(new Size(100, 100), formSize);
            //panelSplashGraphics.Size = new Size(panelSplashGraphics.Size.Width - padding, panelSplashGraphics.Size.Height - padding);
            panelSplashGraphics.Left = ((int)((double) ((double)formSize.Width / (double)2) - (((double)panelSplashGraphics.Width / (double)2))));
            panelSplashGraphics.Top = padding + ((int)((double)(formSize.Height / (double)2) - ((panelSplashGraphics.Height / (double)2))));
            //this.Refresh();

            var graphics = panelSplashGraphics.CreateGraphics();
            //graphics.Clear(Color.Transparent);

            double radius = Math.Min(panelSplashGraphics.Size.Width - (2 * padding), panelSplashGraphics.Size.Height - (2 * padding)) / 2;
            double diameter = radius * 2;
            double xO = ((double)panelSplashGraphics.Width / (double)2);
            double yO = ((double)panelSplashGraphics.Height / (double)2);
            int numOfCircumferenceDots = 360;

            Rectangle rect = new Rectangle();
            rect.X = (int)xO - (int)radius;
            rect.Y = (int)yO - (int)radius;
            rect.Width = ((int)radius * 2) - 1;
            rect.Height = ((int)radius * 2) - 1;

            //graphics.DrawRectangle(new Pen(Color.Black), rect);

            //DrawRectangle(graphics, (int)xO, (int)yO, (int)(radius * 2), (int)(radius * 2), Color.Black);

            // draw the circle
            for (int i = 1; i <= numOfCircumferenceDots; i++) 
            {
                var dot = new Rectangle();
                double d = 360 * ((double)i / (double)numOfCircumferenceDots);
                dot.X = (int)(xO + (Math.Cos(d * dg) * radius));
                dot.Y = (int)(yO + (Math.Sin(d * dg) * radius));
                dot.Width = 2;
                dot.Height = 2;

                graphics.DrawRectangle(new Pen(Color.Red, 2), dot);
            }

            // draw the grid
            GridSpacing = 10;
            NumOfGridlines = (int)(radius * ((double)2));
            LeftPoint = new Point();
            for(int i = 0; i < NumOfGridlines; i += GridSpacing)
            {
                // horizontal
                Point p1 = new Point((int)(xO - radius), (int)((yO - radius) + i));
                Point p2 = new Point((int)(xO - radius) + ((int)diameter), (int)((yO - radius) + i));
                graphics.DrawLine(new Pen(Color.DarkGreen), p1, p2);

                // vertical
                Point p3 = new Point((int)(xO - radius + i), (int)(yO - radius));
                Point p4 = new Point((int)(xO - radius + i), (int)(yO + radius));
                graphics.DrawLine(new Pen(Color.DarkGreen), p3, p4);

                if(i == 0)
                {
                    LeftPoint = p3;
                }
            }

            // set cell states
            if(GridCellStatesInitialized)
            {
                for (int row = 0; row < NumOfGridlines; row++)
                {
                    for (int col = 0; col < NumOfGridlines; col++)
                    {
                        bool state = GridCellStates.Substring((row * NumOfGridlines) + col, 1) == "1" ? true : false;
                        if (state)
                        {
                            graphics.FillRectangle(new SolidBrush(Color.Red), LeftPoint.X + (col * GridSpacing), LeftPoint.Y + (row * GridSpacing), GridSpacing, GridSpacing);
                        }
                    }
                }
            }
            else
            {
                var sb = new StringBuilder();
                for (int i = 0; i < NumOfGridlines * NumOfGridlines; i++)
                {
                    sb.Append("0");
                }

                GridCellStates = sb.ToString();
                MessageBox.Show(GridCellStates);
            }

            panelSplashGraphics.BackColor = Color.Transparent;
        }

        private void DrawRectangle(Graphics g, int x, int y, int width, int height, Color c)
        {
            Pen p = new Pen(c);
            for(int x1 = x; x1 < x + width; x1++)
            {
                for(int y1 = y; y1 < y + height; y1++)
                {
                    Rectangle rect = new Rectangle();
                    rect.X = x1;
                    rect.Y = y1;
                    rect.Width = 1;
                    rect.Height = 1;
                    g.DrawRectangle(p, rect);
                }
            }
        }

        private void buttonFullscreen_Click(object sender, EventArgs e)
        {
            panelSplashGraphics.Visible = !panelSplashGraphics.Visible;
            DrawSplashScreen();
        }

        private void FormCamera_Load(object sender, EventArgs e)
        {
            this.ResizeRedraw = false;
        }

        public Size BestFit(Size pSizeSource, Size pSizeDestination)
        {
            decimal wN = (decimal)pSizeDestination.Width;
            decimal hN = (decimal)pSizeDestination.Height;
            decimal wProp = (((decimal)pSizeSource.Width) * hN) / ((decimal)pSizeSource.Height);
            decimal hProp = (((decimal)pSizeSource.Height) * wProp) / ((decimal)pSizeSource.Width);
            if (wProp > wN || hProp > hN)
            {
                // flip the ratio (black bars on horizontal instead of vertical ish)
                if (wProp > wN)
                {
                    wProp = wN;
                    hProp = (((decimal)pSizeSource.Height) * wN) / ((decimal)pSizeSource.Width);
                }
            }

            return new Size((int)wProp, (int)hProp);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            new FormCameraSettings().Show();
        }
    }
}
