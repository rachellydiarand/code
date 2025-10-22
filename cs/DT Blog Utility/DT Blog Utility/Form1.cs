//using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DT_Blog_Utility
{
    public partial class Form1 : Form
    {

        public string AppVersionMoth { get; set; } = "1.84";
        public static Form1 Instance { get; set; }

        public System.Windows.Forms.Timer CameraTimer { get; set; }
        private int CameraTimerFlip = 1;

        public System.Windows.Forms.Timer EnterFrameTimer { get; set; }
        private Thread CameraFormThread { get; set; }
        public bool RemoveCameraNextFrame = false;
        private bool ApplicationWasClosed { get; set; }

        //public FormCamera CameraForm { get; set; }
        public bool CameraOn { get; set; }

        public int ScanTolerance { get; set; }
        public int ReadTolerance { get; set; }
        public Rectangle? ScanRectangle { get; set; }
        public int ToleranceCalculated { get; set; }

        public RachelsDrawingFun DrawingForm { get; set; }

        // third attempt at webcam
        // Create class-level accesible variables
        /*
        OpenCvSharp.VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;
        bool isCameraRunning = false;
        */

        public Form1(string[] args)
        {
            InitializeComponent();
            Instance = this;

            labelAppVersion.Text = "v" + AppVersionMoth; 

            pictureBoxSource.BackgroundImageLayout = ImageLayout.Center;

            FormClosing += new FormClosingEventHandler(Form1_Closing);

            ToleranceCalculated = 0;

            EnterFrameTimer = new System.Windows.Forms.Timer();
            EnterFrameTimer.Interval = 80;
            EnterFrameTimer.Tick += new EventHandler(EnterFrame);
            EnterFrameTimer.Start();

            if(args.Length > 0)
            {
                // an image(s) was opened with this program
                // open a picture viewer!
                foreach(var  arg in args)
                {
                    OpenImage(arg);
                }

                //if(FormMothPictureViewer.Instance != null)
                //{
                //    FormMothPictureViewer.Instance.Focus();
                //}
            }
        }

        private void EnterFrame(object sender, EventArgs e)
        {
            labelDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd     HH:mm:ss");

            // if placing in the lower right, moving to upper left for now, no positioning need there
            //labelAppVersion.Location = new Point(this.Width - 20 - labelAppVersion.Width, labelAppVersion.Location.Y);

            //if(FormMothPictureViewer.Instance != null)
            //{
            //    FormMothPictureViewer.Instance.DoEnterFrame();
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // this can be dangerous!!!  disabling for now, settings don't seem to be saving well
            /*
            string windowLocationStr = Properties.Settings.Default.Location;
            if(!string.IsNullOrEmpty(windowLocationStr))
            {
                this.Location = Newtonsoft.Json.JsonConvert.DeserializeObject<Point>(windowLocationStr);
            }
            */

            textBoxScanTolerancePercentage.Text = Properties.Settings.Default.ScanTolerancePercentage;
            int val = 0;
            if (!string.IsNullOrEmpty(textBoxScanTolerancePercentage.Text))
            {
                int.TryParse(textBoxScanTolerancePercentage.Text, out val);
            }
            ScanTolerance = val;

            textBoxReadTolerancePercentage.Text = Properties.Settings.Default.ReadTolerancePercentage;
            int val2 = 0;
            if (!string.IsNullOrEmpty(textBoxReadTolerancePercentage.Text))
            {
                int.TryParse(textBoxReadTolerancePercentage.Text, out val2);
            }
            ReadTolerance = val2;

            textBoxImageSourceFolder.Text = Properties.Settings.Default.SourceImageFolder;
            textBoxImageDestinationFolder.Text = Properties.Settings.Default.DestinationImageFolder;
            textBoxPictureClonerSourceFolder.Text = Properties.Settings.Default.PictureClonerSourceFolder;
            textBoxPictureClonerDestinationFolder.Text = Properties.Settings.Default.PictureClonerDestinationFolder;
            textBoxSiteBaseFolder.Text = Properties.Settings.Default.SiteBaseFolder;
            textBoxMasterHtmlFile.Text = Properties.Settings.Default.MasterHtmlFile;
            textBoxPreviewFileName.Text = Properties.Settings.Default.PreviewHtmlFile;
            textBoxOtherProductionURL.Text = Properties.Settings.Default.OtherProductionURL;
            checkBoxUseOtherProductionURL.Checked = Properties.Settings.Default.UseOtherProductonURL;
            textBoxNewWidth.Text = Properties.Settings.Default.ImageResizeWidth;
            textBoxNewHeight.Text = Properties.Settings.Default.ImageResizeHeight;
            checkBoxUseShowImagesWrapper.Checked = Properties.Settings.Default.UseShowImagesWrapper;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.BlogSlug))
            {
                textBoxInsertSlug.Text = Properties.Settings.Default.BlogSlug;
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.BlogDeleteSlug))
            {
                textBoxDeleteSlug.Text = Properties.Settings.Default.BlogDeleteSlug;
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.FileSizeConcatenator))
            {
                textBoxFileNameSizeConcatenator.Text = Properties.Settings.Default.FileSizeConcatenator;
            }

            if(!string.IsNullOrEmpty(Properties.Settings.Default.RecipeImages))
            {
                List<string> recipeImages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>> (Properties.Settings.Default.RecipeImages);
                if(recipeImages.Count > 0)
                {
                    foreach(var item in recipeImages)
                    {
                        listBoxAudio.Items.Add(item);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.BlogImages))
            {
                List<string> blogImages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Properties.Settings.Default.BlogImages);
                if (blogImages.Count > 0)
                {
                    foreach (var item in blogImages)
                    {
                        listBoxBlogPictures.Items.Add(item);
                    }
                }
            }

            FetchSourceImages();
        }

        private void FetchSourceImages()
        {
            if(!string.IsNullOrEmpty(textBoxImageSourceFolder.Text) && Directory.Exists(textBoxImageSourceFolder.Text))
            {
                SourceImages = Directory.EnumerateFiles(textBoxImageSourceFolder.Text).ToList();
                List<string> imgFiles = new List<string>();
                foreach (var file in SourceImages)
                {
                    if (file.ToLower().IndexOf(".jpg") != -1 || file.ToLower().IndexOf(".png") != -1)
                    {
                        imgFiles.Add(file);
                    }
                }
                SourceImages = imgFiles;
                SourceImagesCurrentImageIndex = 0;
                string lastBrowsedImage = Properties.Settings.Default.ImageBrowseLastImage;

                if (!string.IsNullOrEmpty(lastBrowsedImage))
                {
                    for(int a = 0; a< SourceImages.Count; a++)
                    {
                        if (SourceImages[a] == lastBrowsedImage)
                        {
                            SourceImagesCurrentImageIndex = a;
                        }
                    }
                }

                RefreshSourceImage();
            }            
        }

        private void buttonBrowseSourceFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBoxImageSourceFolder.Text;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImageSourceFolder.Text = dialog.SelectedPath;
                Properties.Settings.Default.SourceImageFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
                FetchSourceImages();
            }
        }

        private void buttonBrowseDestinationFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBoxImageDestinationFolder.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImageDestinationFolder.Text = dialog.SelectedPath;
                Properties.Settings.Default.DestinationImageFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private List<string> SourceImages = new List<string>();
        private int SourceImagesCurrentImageIndex = 0;

        private System.Drawing.Image ScaleToWidth (System.Drawing.Image pImage, int pWidth)
        {
            decimal nW = (decimal)pWidth;// new width
            decimal nH = 0;
            decimal wN = (decimal)pImage.Size.Width;// width now
            decimal hN = (decimal)pImage.Size.Height;

            nH = (hN * nW) / wN;

            return ResizeImage(pImage, (int)nW, (int)nH);
        }

        private System.Drawing.Image ScaleToHeight(System.Drawing.Image pImage, int pHeight)
        {
            decimal nW = 0;// new width
            decimal nH = (decimal)pHeight;
            decimal wN = (decimal)pImage.Size.Width;// width now
            decimal hN = (decimal)pImage.Size.Height;

            nW = (wN * nH) / hN;

            return ResizeImage(pImage, (int)nW, (int)nH);
        }

        private void RefreshSourceImage()
        {
            if(SourceImages.Count > 0 && SourceImagesCurrentImageIndex < SourceImages.Count)
            {
                pictureBoxSource.Image = null;
                this.Refresh();
                var img = System.Drawing.Image.FromFile(SourceImages[SourceImagesCurrentImageIndex]);

                // save image in settings so that image opens/sticks when app opens
                Properties.Settings.Default.ImageBrowseLastImage = SourceImages[SourceImagesCurrentImageIndex];
                Properties.Settings.Default.Save();

                decimal wN = (decimal)pictureBoxSource.Width;
                decimal hN = (decimal)pictureBoxSource.Height;
                decimal wProp = (((decimal)img.Width) * hN) / ((decimal)img.Height);
                decimal hProp = (((decimal)img.Height) * wProp) / ((decimal)img.Width);
                if(wProp > wN || hProp > hN)
                {
                    // flip the ratio (black bars on horizontal instead of vertical ish)
                    if (wProp > wN)
                    {
                        wProp = wN;
                        hProp = (((decimal)img.Height) * wN) / ((decimal)img.Width);
                    }
                }
                //var img2 = Bitmap.FromFile(SourceImages[SourceImagesCurrentImageIndex]);
                //img = new Size(wProp, hProp);
                img = ResizeImage(img, (int)wProp, (int)hProp);

                pictureBoxSource.Image = img;
                textBoxSourceImageName.Text = new FileInfo(SourceImages[SourceImagesCurrentImageIndex]).Name;
                textBoxDestinationImageName.Text = textBoxSourceImageName.Text;

                ScanRectangle = null;

                SetImageScrubbingSliderBounds(SourceImages.Count, SourceImagesCurrentImageIndex);
            }
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

        public System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        {     
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return (System.Drawing.Image)bmp;
        }

        private void buttonNavigateSourceFolderBack_Click(object sender, EventArgs e)
        {
            if(SourceImages.Count > -1)
            {
                SourceImagesCurrentImageIndex--;
                if(SourceImagesCurrentImageIndex < 0)
                {
                    SourceImagesCurrentImageIndex = SourceImages.Count - 1;
                }
                RefreshSourceImage();
            }
        }

        private void buttonNavigateSourceFolderForward_Click(object sender, EventArgs e)
        {
            if (SourceImages.Count > -1)
            {
                SourceImagesCurrentImageIndex++;
                if (SourceImagesCurrentImageIndex > SourceImages.Count - 1)
                {
                    SourceImagesCurrentImageIndex = 0;
                }
                RefreshSourceImage();
            }
        }

        private string DESTINATION_FILENAME_ACCESSOR
        {
            get {
                if (string.IsNullOrEmpty(textBoxImageDestinationFolder.Text)) return string.Empty;
                string destFolder = textBoxImageDestinationFolder.Text;
                string sourceFolder = textBoxImageSourceFolder.Text;
                string destImageName = textBoxDestinationImageName.Text;
                string sourceImageName = textBoxSourceImageName.Text;
                if (string.IsNullOrEmpty(destFolder))
                {
                    return Path.Combine(sourceFolder, sourceImageName);
                }
                return Path.Combine(destFolder, destImageName);
            }
            set
            {
                return;
            }
        }

        private void buttonResizeImage_Click(object sender, EventArgs e)
        {
            if(SourceImages.Count < 1 ||  SourceImagesCurrentImageIndex > SourceImages.Count - 1)
            {
                MessageBox.Show("No source image selected.");
                return;
            }
            var img = System.Drawing.Image.FromFile(SourceImages[SourceImagesCurrentImageIndex]);
            System.Drawing.Image imageResized = null;
            string destFilename = DESTINATION_FILENAME_ACCESSOR;
            string ext = Path.GetExtension(destFilename);
            if (string.IsNullOrEmpty(DESTINATION_FILENAME_ACCESSOR))
            {
                MessageBox.Show("Destination FileName or Location is incomplete.");
                return;
            }

            string strResizeWidth = textBoxNewWidth.Text;
            string strResizeHeight = textBoxNewHeight.Text;
            if (!string.IsNullOrEmpty(strResizeWidth) && !string.IsNullOrEmpty(strResizeHeight))
            {
                MessageBox.Show("Only define a new width or a new height.  This is a scaling function.");
                return;
            }
            if (string.IsNullOrEmpty(strResizeWidth) && string.IsNullOrEmpty(strResizeHeight))
            {
                if(MessageBox.Show("No resize defined.  This will just copy image to destination folder (and rename if defined). Proceed?", "Proceed with Image Copy", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    img.Save(DESTINATION_FILENAME_ACCESSOR);
                    MessageBox.Show("File moved from source to destination.");
                    return;
                }
                else
                {
                    return;
                }
            }

            if(DESTINATION_FILENAME_ACCESSOR.ToLower().Substring(DESTINATION_FILENAME_ACCESSOR.Length - ext.Length) != ext.ToLower())
            {
                destFilename = DESTINATION_FILENAME_ACCESSOR + ext;
            }

            if(!string.IsNullOrEmpty(strResizeWidth))
            {
                imageResized = ScaleToWidth(img, int.Parse(strResizeWidth));
            }
            else if(!string.IsNullOrEmpty(strResizeHeight))
            {
                imageResized = ScaleToHeight(img, int.Parse(strResizeHeight));
            }

            if(checkBoxCopySourceImageToDestination.Checked)
            {
                img.Save(destFilename);
            }

            int w = imageResized.Width;
            int h = imageResized.Height;
            string imageSizeExtension = textBoxFileNameSizeConcatenator.Text + w.ToString() + "x" + h.ToString() + ext;

            destFilename = destFilename.Substring(0, destFilename.IndexOf(ext)) + imageSizeExtension;
            imageResized.Save(destFilename);

            MessageBox.Show("Resize Comlete!");
        }

        private void buttonClearResizeFields_Click(object sender, EventArgs e)
        {
            textBoxDestinationImageName.Text = textBoxSourceImageName.Text;
            textBoxNewWidth.Text = "";
            textBoxNewHeight.Text = "";
        }

        private void buttonOpenSourceImage_Click(object sender, EventArgs e)
        {
            string file = Path.Combine(textBoxImageSourceFolder.Text, textBoxSourceImageName.Text);
            Process.Start(file);

            //OpenImage(file);
        }

        public void OpenImage(string file)
        {
            if(!File.Exists(file))
            {
                MessageBox.Show("The app tried to open a file that does not exist!");
                return;
            }

            // for the singlton way
            if (false)
            {
                //if (FormMothPictureViewer.Instance == null)
                //{
                //    new FormMothPictureViewer(file);
                //}
                //else
                //{
                //    FormMothPictureViewer.Instance.Init(file);
                //}

                //FormMothPictureViewer.Instance.Show();
            }
            else
            {
                // multiple instances of picture viewer
                List<string> files = new List<string>();
                files.Add(file);

                // removed when pictureviewer was taken out of this namespace as a new project in the solution
                //var pv = new FormMothPictureViewer(files.ToArray());
                //pv.Show();
                //pv.Focus();
            }
        }

        private void buttonRefreshSourceFolder_Click(object sender, EventArgs e)
        {
            string currentImageFileName = textBoxSourceImageName.Text;
            textBoxSourceImageName.Text = "";
            FetchSourceImages();
            if(SourceImages.Count > 0)
            {
                for(int a = 0; a < SourceImages.Count; a++)
                {
                    if (new FileInfo(SourceImages[a]).Name == currentImageFileName)
                    {
                        SourceImagesCurrentImageIndex = a;
                        break;
                    }
                }
                RefreshSourceImage();
                //textBoxSourceImageName.Text = new FileInfo(SourceImages[SourceImagesCurrentImageIndex]).Name;
            }            
        }

        private void textBoxSourceImageName_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonPictureClonerSourceBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBoxPictureClonerSourceFolder.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPictureClonerSourceFolder.Text = dialog.SelectedPath;
                Properties.Settings.Default.PictureClonerSourceFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonPictureClonerDestinationBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBoxPictureClonerDestinationFolder.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPictureClonerDestinationFolder.Text = dialog.SelectedPath;
                Properties.Settings.Default.PictureClonerDestinationFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonPictureClonerGo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPictureClonerSourceFolder.Text) || string.IsNullOrEmpty(textBoxPictureClonerDestinationFolder.Text))
            {
                MessageBox.Show("Please select source and destination folders.");
            }

            if(!Directory.Exists(textBoxPictureClonerDestinationFolder.Text) || !Directory.Exists(textBoxPictureClonerSourceFolder.Text))
            {
                MessageBox.Show("Source or destination folder does not exist!");
                return;
            }

            List<string> srcImages = Directory.EnumerateFiles(textBoxPictureClonerSourceFolder.Text).ToList();
            if(srcImages.Count < 1)
            {
                MessageBox.Show("Source folder is empty!");
                return;
            }
            int fileCopyCount = 0;
            foreach (var file in srcImages)
            {
                string newPath = Path.Combine(textBoxPictureClonerDestinationFolder.Text, new FileInfo(file).Name);
                if (File.Exists(newPath)) continue;
                byte[] fileBytes = File.ReadAllBytes(file);
                File.WriteAllBytes(newPath, fileBytes);
                fileCopyCount++;
            }

            
            if(checkBoxPictureClonerDeleteAllFromSource.Checked)
            {
                if (MessageBox.Show("Are you sure you want to delete files from source?", "Delete Files from Source?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (var file in srcImages)
                    {
                        File.Delete(file);
                    }
                }                
            }

            MessageBox.Show("File copy complete!  " + fileCopyCount.ToString() + " files moved.");
        }

        private void buttonAddToRecipesFromResizer_Click(object sender, EventArgs e)
        {
            if(!File.Exists(Path.Combine(textBoxPictureClonerDestinationFolder.Text, DESTINATION_FILENAME_ACCESSOR)))
            {
                MessageBox.Show("Could not find resized image in destination folder.  Did not add to recipes.");
                return;
            }

            AddImageToBlogOrAudio(DESTINATION_FILENAME_ACCESSOR, "recipes");
        }

        private void buttonAddToBlogFromResizer_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(textBoxPictureClonerDestinationFolder.Text, DESTINATION_FILENAME_ACCESSOR)))
            {
                MessageBox.Show("Could not find resized image in destination folder.  Did not add to blog.");
                return;
            }

            AddImageToBlogOrAudio(DESTINATION_FILENAME_ACCESSOR, "blog");
        }

        private void buttonAddToRecipesFromSource_Click(object sender, EventArgs e)
        {
            if(SourceImages.Count < 1 || SourceImagesCurrentImageIndex > SourceImages.Count - 1
                || !File.Exists(SourceImages[SourceImagesCurrentImageIndex]))
            {
                MessageBox.Show("Could not find resized image in source folder.  Did not add to recipes.");
                return;
            }

            AddImageToBlogOrAudio(SourceImages[SourceImagesCurrentImageIndex], "recipes");
        }

        private void buttonAddToBlogFromSource_Click(object sender, EventArgs e)
        {
            if (SourceImages.Count < 1 || SourceImagesCurrentImageIndex > SourceImages.Count - 1
                || !File.Exists(SourceImages[SourceImagesCurrentImageIndex]))
            {
                MessageBox.Show("Could not find resized image in source folder.  Did not add to blog.");
                return;
            }

            AddImageToBlogOrAudio(SourceImages[SourceImagesCurrentImageIndex], "blog");
        }

        private void AddImageToBlogOrAudio(string pFile, string pRecipesOPrAudio)
        {
            if(pRecipesOPrAudio == "audio")
            {
                if(!listBoxAudio.Items.Contains(pFile))
                {
                    listBoxAudio.Items.Add(pFile);
                }
            }
            else
            {
                if(!listBoxBlogPictures.Items.Contains(pFile))
                {
                    listBoxBlogPictures.Items.Add(pFile);
                }
            }
        }

        private void buttonBrowseAudio_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                AddImageToBlogOrAudio(dialog.FileName, "audio");
            }
        }

        private void buttonRemoveBlogPicture_Click(object sender, EventArgs e)
        {
            if (listBoxBlogPictures.SelectedItems.Count > 0)
            {
                listBoxBlogPictures.Items.Remove(listBoxBlogPictures.SelectedItems[0]);
                listBoxBlogPictures.Refresh();
            }
        }

        private void textBoxInsertSlug_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RecipeSlug = textBoxInsertSlug.Text;
            Properties.Settings.Default.Save();
        }
        private void textBoxBlogSlug_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.BlogSlug = textBoxInsertSlug.Text;
            Properties.Settings.Default.Save();
        }

        private string FindImageLowRes(string pImg)
        {
            if (!File.Exists(pImg)) return string.Empty;
            FileInfo fi = new FileInfo(pImg);
            List<string> files = Directory.EnumerateFiles(fi.DirectoryName).ToList();
            foreach(string file in files)
            {
                if (file == pImg) continue;
                FileInfo fi2 = new FileInfo(pImg);
                string fileNameProp = fi2.Name.Substring(0, fi2.Name.Length - fi2.Extension.Length) + textBoxFileNameSizeConcatenator.Text;
                if(file.IndexOf(fileNameProp) > -1)
                {
                    // this is the resized image for the img parameter
                    return file;
                }
            }
            return string.Empty;
        }

        string ConvertWindowsPathToUrl(string path)
        {
            if(checkBoxUseOtherProductionURL.Checked)
            {
                // need to compare the "other" Production URL path with the SiteBaseFolder
                // trying to fix problems with a url like https://rachellydiarand.github.io/moth/images/myImage.png
                // when the site base path is C:\_rachel\sites\moth\git\moth
                if (textBoxOtherProductionURL.Text.Contains(":"))
                {
                    // this is the instance that I need on my virtual host
                    // look for the slash after the colon and use that as the site base path /moth
                    string str = textBoxOtherProductionURL.Text.Substring(textBoxOtherProductionURL.Text.IndexOf(":") + 1);
                    if(str.Length > 0 && str.Contains(":"))
                    {
                        // this is likely a path with a port
                        str = str.Substring(str.IndexOf(":") + 1);
                        if(str.Length > 0)
                        {
                            if (str.Contains("/"))
                            {
                                // there is a slash after the port most likely
                                str = str.Substring(str.IndexOf("/") + 1);
                                if(str.Length > 0)
                                {
                                    // ok, now str should be "moth"
                                    // if the base folder has this on the end,
                                    // then it solves my case
                                    if(textBoxSiteBaseFolder.Text.Length > str.Length)
                                    {
                                        if(textBoxSiteBaseFolder.Text.Substring(textBoxSiteBaseFolder.Text.Length - str.Length) == str)
                                        { 
                                            // verified
                                            if(path.Substring(0, 1) == "\\")
                                            {
                                                path = path.Substring(1);
                                            }
                                            path = str + "/" + path;
                                            if(path.Substring(0, 1) != "/")
                                            {
                                                path = "/" + path;
                                            }

                                            // sanitize
                                            path = path.Replace("\\//", "/");
                                            path = path.Replace("//", "/");
                                            path = path.Replace("\\\\", "\\");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(textBoxSiteBaseFolder.Text) && path.Contains(textBoxSiteBaseFolder.Text))
                {
                    path = path.Replace(textBoxSiteBaseFolder.Text, "");
                }
            }
            return path.Replace("\\", "/");
        }

        private string ComposeImageLink(string pImg)
        {
            string imgUrl = pImg.Substring(textBoxSiteBaseFolder.Text.Length);

            string thumbnail = FindImageLowRes(pImg);
            if(!string.IsNullOrEmpty(thumbnail))
            {
                thumbnail = thumbnail.Substring(textBoxSiteBaseFolder.Text.Length);
            }
            if (!string.IsNullOrEmpty(thumbnail))
            {
                if (checkBoxUseShowImagesWrapper.Checked)
                {
                    return "<?php echo renderImageIfOn('<a target=\"_rlr\" href=\"" + ConvertWindowsPathToUrl(imgUrl) + "\"><img src=\"" + ConvertWindowsPathToUrl(thumbnail) + "\" class=\"imageBorder\" /></a>'); ?>";
                }
                else
                {
                    return "<a target=\"_rlr\" href=\"" + ConvertWindowsPathToUrl(imgUrl) + "\"><img src=\"." + ConvertWindowsPathToUrl(thumbnail) + "\" class=\"imageBorder\" /></a>";
                }
            }
            else
            {
                return "<img src=\"." + ConvertWindowsPathToUrl(imgUrl) + "\" class=\"imageBorder\" />";
            }
        }

        private string ComposeAudioLink(string pAudioFile)
        {
            FileInfo fi = new FileInfo(pAudioFile);
            if (fi.Exists)
            {
                string songName = fi.Name.Substring(0, fi.Name.IndexOf(fi.Extension));
                string relativeUrl = ConvertWindowsPathToUrl(pAudioFile);
                return "<b>" + songName +"</b><br /><audio controls loop class=\"audio\" title=\"" + songName + "\"><source src=\"" + relativeUrl + "\" /></audio>";
            }

            MessageBox.Show("Could not complete audio insert for: " + pAudioFile);
            return string.Empty;
        }

        private string MyReplace(string pStr, string pDelimiter, string pConcatenator)
        {
            if (string.IsNullOrEmpty(pStr)) return string.Empty;
            List<string> arr = pStr.Split(pDelimiter.ToCharArray()).ToList();
            StringBuilder sb = new StringBuilder();
            foreach(string item in arr)
            {
                sb.Append(item);
                sb.Append(pConcatenator);
            }
            return sb.ToString();
        }

        private string BuildHtml()
        {
            if(!File.Exists(textBoxMasterHtmlFile.Text))
            {
                MessageBox.Show("Could not find Master HTML file.");
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            string html = File.ReadAllText(textBoxMasterHtmlFile.Text);
            int blogInsertIndexOf = html.IndexOf(textBoxInsertSlug.Text);

            if(blogInsertIndexOf == -1)
            {
                return html;
            }

            // grab the two log text box content and add line breaks on system new line
            // we'll leave old content as is, however it got there, we don't want to mess with it, it'll never get back into these boxes, right?
            string logStr = MyReplace(textBoxBlogText.Text, "\r", "\r<br>"); 

            string date = DateTime.Now.ToString("yyyy-MM-dd ") + "&nbsp;&nbsp;&nbsp;" + DateTime.Now.ToString("HH:mm:ss tt");

            string tag = textBoxBlogTag.Text;
            string tagReadable = "";
            if (!string.IsNullOrEmpty(tag))
            {
                // using a comma now to split between code name and readable name
                List<string> tagParts = tag.Split(',').ToList();
                
                if(tagParts.Count > 1)
                {
                    for(int a = 1; a < tagParts.Count; a++)
                    {
                        if (!string.IsNullOrEmpty(tagReadable)) tagReadable += ", ";// account for commas in the readable name split
                        tagReadable += tagParts[a].Trim();
                    }
                    tag = tagParts[0];
                }

                // search html for other tags with the same name
                // we are trying to deep link with the #tag to scroll to the post div with that id
                // it's like an index that we will have in a drop down box populated with jabascript on load
                int oldTagPosition = html.IndexOf("id=\"" + tag);
                List<string> otherMatchingTags = new List<string>();

                if (oldTagPosition > -1)
                {
                    string substringPiece = html.Substring(oldTagPosition + 4);

                    do
                    {                        
                        int substringPieceEnd = substringPiece.IndexOf("\"");// should be the ending character id="tag"<--
                        if (substringPieceEnd < 0) break;
                        string oldTag = substringPiece.Substring(0, substringPieceEnd);
                        if(oldTag.IndexOf(tag) > -1)
                        {
                            // this is a matching tag
                            // we're going to ignore what number or whatever is past it
                            // yeah, this kind of thing gets messy and complicated
                            // just another day at work, but I haven't programmed in a few months now because of lack of electricity
                            otherMatchingTags.Add(oldTag);
                        }
                        oldTagPosition = substringPiece.IndexOf("id=\"" + tag);
                        if (oldTagPosition < 0) break;
                    } while(oldTagPosition < substringPiece.Length);
                }

                if(otherMatchingTags.Count > 0)
                {
                    int multipleTagIncrementalSuffix = 0;
                    string proposedTag = tag + "-" + multipleTagIncrementalSuffix.ToString();
                    bool didIncrementTagSuffix = false;
                    do
                    {
                        proposedTag = tag + "-" + multipleTagIncrementalSuffix.ToString();
                        foreach (string otherTag in otherMatchingTags)
                        {
                            if (proposedTag == otherTag)
                            {
                                multipleTagIncrementalSuffix++;
                                didIncrementTagSuffix = true;
                            }
                        }
                    } while (didIncrementTagSuffix);

                    tag = proposedTag;
                }
            }

            sb.AppendLine("");
            sb.AppendLine("");
            if (!string.IsNullOrEmpty(tag))
            {
                sb.AppendLine("<div id=\"" + tag + "\" tagdescription=\"" + tagReadable + "\" class='post'>");
            }
            else
            {
                sb.AppendLine("<div class='post'>");
            }
            
            sb.AppendLine("<h4>" + date + "</h4>");

            if(listBoxAudio.Items.Count > 0)
            {
                foreach (string audioFile in listBoxAudio.Items)
                {
                    sb.AppendLine(ComposeAudioLink(audioFile));
                    sb.AppendLine("<br /><br />");
                }
                sb.AppendLine("<br />");
                sb.AppendLine();
                sb.AppendLine("");
            }
            
            if (listBoxBlogPictures.Items.Count > 0)
            {
                foreach (string img in listBoxBlogPictures.Items)
                {
                    sb.AppendLine(ComposeImageLink(img));
                    sb.AppendLine("<br /><br />");
                }
                sb.AppendLine("<br />");
                sb.AppendLine();
                sb.AppendLine("");
            }

            if (!string.IsNullOrEmpty(logStr))
            {
                sb.Append(logStr);
                //sb.AppendLine("<br /><br />");
                //sb.AppendLine();
                //sb.AppendLine("");
            }



            sb.AppendLine("</div>");

            if (checkBoxDeletePreviousPosts.Checked)
            {
                if (html.IndexOf(textBoxDeleteSlug.Text) > -1)
                {
                    string htmlStart = html.Substring(0, html.IndexOf(textBoxInsertSlug.Text) + textBoxInsertSlug.Text.Length);
                    string htmlEnd = html.Substring(html.IndexOf(textBoxDeleteSlug.Text));
                    html = htmlStart +
                        Environment.NewLine +
                        Environment.NewLine +
                        sb.ToString() +
                        htmlEnd;
                }
            }
            else
            {
                html = html.Substring(0, blogInsertIndexOf + textBoxInsertSlug.Text.Length)
                + sb.ToString()
                + html.Substring(blogInsertIndexOf + textBoxInsertSlug.Text.Length + 2);
            }

            
            return html;
        }

        private void buttonInsertIntoProduction_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(textBoxSiteBaseFolder.Text))
            {
                if(MessageBox.Show("Could not create backup in _backup folder.  Do you wish to proceed without a backup?", "! Proceed without Backup?", MessageBoxButtons.YesNo) == DialogResult.No) 
                {
                    return;
                }
            }
            else
            {
                FileInfo fi = new FileInfo(textBoxMasterHtmlFile.Text);
                string dateTime = DateTime.Now.ToString("_yyyy_MM_dd-hh_mm_ss");
                string backupFileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + dateTime + fi.Extension;
                File.Copy(textBoxMasterHtmlFile.Text, Path.Combine(textBoxSiteBaseFolder.Text, "_backup", backupFileName));
            }
            string html = BuildHtml();

            File.WriteAllText(textBoxMasterHtmlFile.Text, html);

            
            if (textBoxMasterHtmlFile.Text.IndexOf(".php") != -1)
            {
                // this is a .php file

                // turning this into a pretty url (removing .php file extension)
                // may be bad for other users (unintended/undocumented function)
                // may want to make a checkbox for this behavior
                string url = textBoxMasterHtmlFile.Text;
                if (checkBoxUseOtherProductionURL.Checked)
                {
                    url = textBoxMasterHtmlFile.Text.Replace(".php", "");
                    url = url.Replace(textBoxSiteBaseFolder.Text, textBoxOtherProductionURL.Text);
                }
                Process.Start(url);

                // the old function
                // we've already written to the master html file
                // it got complicated on my system
                // assumming that if this is a .php file extension, we'll be using some kind of server

                //var tempFileName = textBoxMasterHtmlFile.Text.Substring(0, textBoxMasterHtmlFile.Text.IndexOf(".php")) + "_master_temp.php";
                //var tempFile = Path.Combine(textBoxSiteBaseFolder.Text, tempFileName);
                //File.WriteAllText(tempFile,
                //    File.ReadAllText(Path.Combine(textBoxSiteBaseFolder.Text, textBoxMasterHtmlFile.Text)));
                //Process.Start(tempFileName);
            }
            else
            {
                // this is not a .php file
                string url = textBoxMasterHtmlFile.Text;
                if (checkBoxUseOtherProductionURL.Checked)
                {
                    url = url.Replace(textBoxSiteBaseFolder.Text, textBoxOtherProductionURL.Text);
                }
                Process.Start(url);
            }

            MessageBox.Show("File saved to production.");
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            string html = BuildHtml();
            FileInfo fi = new FileInfo(textBoxMasterHtmlFile.Text);
            string url = Path.Combine(fi.DirectoryName, textBoxPreviewFileName.Text);
            

            File.WriteAllText(url, html);

            if (checkBoxUseOtherProductionURL.Checked)
            {
                url = textBoxOtherProductionURL.Text + "/" + textBoxPreviewFileName.Text;
            }

            // I'm adding my ?i=1 to the utrl so I can see the images!
            if(url.IndexOf('?') == -1)
            {
                url = url + "?i=1";
            }

            Process.Start(url);
        }

        private void textBoxFileNameSizeConcatenator_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FileSizeConcatenator = textBoxFileNameSizeConcatenator.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxPreviewFileName_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PreviewHtmlFile = textBoxPreviewFileName.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxMasterHtmlFile_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MasterHtmlFile = textBoxMasterHtmlFile.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxPictureClonerSourceFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PictureClonerSourceFolder = textBoxPictureClonerSourceFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxPictureClonerDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PictureClonerDestinationFolder = textBoxPictureClonerDestinationFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxImageDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DestinationImageFolder = textBoxImageDestinationFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxImageSourceFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SourceImageFolder = textBoxImageSourceFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void buttonMasterHtmlFileBrowse_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            if(!string.IsNullOrEmpty(textBoxSiteBaseFolder.Text))
            {
                if(Directory.Exists(textBoxSiteBaseFolder.Text))
                {
                    dialog.InitialDirectory = textBoxSiteBaseFolder.Text;
                }
            }
            if(dialog.ShowDialog()  == DialogResult.OK)
            {
                textBoxMasterHtmlFile.Text = dialog.FileName;
                Properties.Settings.Default.MasterHtmlFile = dialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonSiteBaseFolder_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBoxSiteBaseFolder.Text;
            if (dialog.ShowDialog () == DialogResult.OK)
            {
                textBoxSiteBaseFolder.Text = dialog.SelectedPath;
                Properties.Settings.Default.SiteBaseFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void CameraTimerTick(object sender, EventArgs e)
        {
            //if (FormCamera._instance == null) return;
            //if (FormCamera._instance.SizeIsChanging) return;
            //if (!CameraOn) return;
            
            //CameraTimerFlip *= -1;
            //if(CameraTimerFlip == 1)
            //{
            //    buttonCamera.BackColor = buttonPreview.BackColor;
            //    buttonCamera.ForeColor = buttonPreview.ForeColor;
            //}
            //else
            //{
            //    buttonCamera.BackColor = Color.Red;
            //    buttonCamera.ForeColor = Color.White;
            //}

            //if(RemoveCameraNextFrame)
            //{
            //    buttonCamera.BackColor = buttonPreview.BackColor;
            //    buttonCamera.ForeColor = buttonPreview.ForeColor;
            //    //if (!CameraFormThread.IsAlive)
            //    //{
            //    //    RemoveCameraNextFrame = false;
            //    //    DestroyCameraAction();
            //    //}
            //    if(CameraForm != null)
            //    {
            //        CameraForm.Visible = false;
            //        //CameraForm.Close();
            //        //DestroyCameraAction();
            //        //CameraForm = null;
            //    }
                
            //}
        }

        
        //c# webcam capture picture
        public void FinalFrame_NewFrame(System.Drawing.Image pImg)
        {
            if(!CameraOn) return;
            if(pictureBoxSource.InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                Action safeWrite = delegate { FinalFrame_NewFrame(pImg); };
                pictureBoxSource.Invoke(safeWrite);
            }
            else
            {
                System.Drawing.Image bmp = pImg;
                Size sourceSize = new Size(bmp.Width, bmp.Height);
                Size destinationSize = new Size(pictureBoxSource.Width, pictureBoxSource.Height);
                Size newSize = BestFit(sourceSize, destinationSize);
                pictureBoxSource.Image = ResizeImage(bmp, newSize.Width, newSize.Height);
            }            
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            // Replace with the path to the executable of the app you want to launch
            string executablePath = Properties.Settings.Default.CameraExectuable;

            try
            {
                // Create a new process object
                Process process = new Process();

                // Set the startup info
                process.StartInfo.FileName = executablePath;
                // Optionally add command line arguments
                // process.StartInfo.Arguments = "-arg1 -arg2";
                process.StartInfo.UseShellExecute = false; // True if no arguments and no redirect
                                                           // process.StartInfo.WorkingDirectory = "C:\\Path\\To\\WorkingDirectory"; //Optional

                // Start the process
                process.Start();

                // Optionally wait for the app to finish
                // process.WaitForExit();
                //if(FormCamera._instance == null)
                //{
                //    CameraForm = new FormCamera(this);
                //    CameraForm.Show();
                //    CameraForm.Init();

                //    CameraTimer = new System.Windows.Forms.Timer();
                //    CameraTimer.Interval = 500;
                //    CameraTimer.Tick += new EventHandler(CameraTimerTick);
                //}


                //if(CameraOn)
                //{
                //    // turn camera off
                //    CameraOn = false;
                //    buttonCamera.BackColor = buttonPreview.BackColor;
                //    buttonCamera.ForeColor = buttonPreview.ForeColor;
                //    CameraTimer.Stop();
                //    FormCamera._instance.Visible = false;
                //}
                //else
                //{
                //    CameraOn = true;
                //    CameraTimer.Start();
                //    FormCamera._instance.Visible = true;
                //    FormCamera._instance.TopMost = true;
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void CameraSaveSnappedImage(Bitmap img)
        {
            string filename = Path.Combine(textBoxImageSourceFolder.Text, DateTime.Now.ToString("yyyy-MM-dd_hh.mm.ss") + "_dt_blog_image.jpg");
            img.Save(filename);
        }

        public void DestroyCamera()
        {
            CameraOn = false;
            RefreshSourceImage();
            RemoveCameraNextFrame = true;            
        }

        private void Form1_Closing(Object sender, FormClosingEventArgs e)
        {
            if(!ApplicationWasClosed)
            {
                Point windowLocation = new Point(this.Top, this.Left);
                string jsonRecipeImages = Newtonsoft.Json.JsonConvert.SerializeObject(listBoxAudio.Items);
                string jsonBlogImages = Newtonsoft.Json.JsonConvert.SerializeObject(listBoxBlogPictures.Items);
                string jsonWindowLocation = Newtonsoft.Json.JsonConvert.SerializeObject(windowLocation);

                Properties.Settings.Default.RecipeImages = jsonRecipeImages;
                Properties.Settings.Default.BlogImages = jsonBlogImages;
                Properties.Settings.Default.Location = jsonWindowLocation;
                Properties.Settings.Default.Save();

                ApplicationWasClosed = true;
                Application.Exit();
            }
            
        }

        private void buttonViewProduction_Click(object sender, EventArgs e)
        {
            if (checkBoxUseOtherProductionURL.Checked)
            {
                string url = textBoxOtherProductionURL.Text + (textBoxMasterHtmlFile.Text.Replace(textBoxSiteBaseFolder.Text, ""));
                Process.Start(url);
                return;
            }

            if (textBoxMasterHtmlFile.Text.IndexOf(".php") != -1)
            {
                // using all php files now in my implentation
                // should change this to switch on file types I suppose (too tired now to really make that decision)
                var tempFileName = textBoxMasterHtmlFile.Text.Substring(0, textBoxMasterHtmlFile.Text.IndexOf(".php")) + "_temp.php";
                var tempFile = Path.Combine(textBoxSiteBaseFolder.Text, tempFileName);
                File.WriteAllText(tempFile,
                    File.ReadAllText(Path.Combine(textBoxSiteBaseFolder.Text, textBoxMasterHtmlFile.Text)));
                Process.Start(tempFileName);
                return;
            }

            Process.Start(textBoxMasterHtmlFile.Text);
        }

        private void buttonInsertLineBreakBlog_Click(object sender, EventArgs e)
        {
            // deleted listener
            //InsertTextIntoBlogTextBox("<br><br>");
        }

        private void InsertTextIntoBlogTextBox(string txt, int pNewCaretPosition = -1, int pSelectionLength = 0)
        {
            string str = textBoxBlogText.Text.Substring(0, textBoxBlogText.SelectionStart);
            string str2 = textBoxBlogText.Text.Substring(textBoxBlogText.SelectionStart);
            textBoxBlogText.Text = str + txt + str2 + Environment.NewLine;
            textBoxBlogText.Focus();
            if(pNewCaretPosition > -1)
            {
                textBoxBlogText.SelectionStart = pNewCaretPosition;
                textBoxBlogText.SelectionLength = 0;
            }
            else
            {
                textBoxBlogText.SelectionStart = str.Length;
                textBoxBlogText.SelectionLength = 0;
            }
            
            textBoxBlogText.ScrollToCaret();
        }

        private void buttonClearTextBlog_Click(object sender, EventArgs e)
        {
            textBoxBlogText.Text = "";
        }

        private void buttonRemoveAllPicturesBlog_Click(object sender, EventArgs e)
        {
            listBoxBlogPictures.Items.Clear();
        }

        private void textBoxDeleteSlug_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.BlogDeleteSlug = textBoxDeleteSlug.Text;
            Properties.Settings.Default.Save();
        }

        private void Trace(string msg)
        {
            //Debug.Write(msg);
        }

        private int PixelBrightness(Color c)
        {
            return (c.R + c.G + c.B) / 3;
        }

        private bool DetectPixelOn(Color c, int tolerance)
        {
            // add them all up, average and compare to tolerance (0-255)
            //int average = byte.Parse(c.R.ToString() + c.G.ToString() + c.B.ToString()) / 3;
            //int average = (c.R + c.G + c.B) / 3;

            if (c.R < tolerance && c.G < tolerance && c.B < tolerance) return true;
           

            //if (c.R < tolerance && c.G < tolerance && c.R < tolerance) return true;

            //if (average < tolerance) return true;

            //if (c.GetBrightness() < (tolerance / 255) * .5) return true;

            //int hitCount = 0;
            //if (c.R < tolerance) hitCount++;
            //if (c.G < tolerance) hitCount++;
            //if (c.B < tolerance) hitCount++;

            //if (hitCount > 1) return true;
            return false;
        }

        private void buttonScanCode_Click(object sender, EventArgs e)
        {
            Bitmap bmp = Bitmap.FromFile(Path.Combine(textBoxImageSourceFolder.Text, textBoxSourceImageName.Text)) as Bitmap;

            int darkCount = 0;
            int lightCount = 0;
            Color c;

            int randomRow = (new Random()).Next(0, bmp.Height);
            int randomCol = (new Random()).Next(0, bmp.Width);

            int minX = -1;
            int maxX = -1;
            int minY = -1;
            int maxY = -1;

            int lightestPixel = 0;
            int darkestPixel = 255;

            // first find lightest light and darkest dark
            for (int row = 0; row < bmp.Height; row++)
            {
                for (int col = 0; col < bmp.Width; col++)
                {
                    c = bmp.GetPixel(col, row);
                    int bright = PixelBrightness(c);

                    if (bright > lightestPixel) lightestPixel = bright;
                    if (bright < darkestPixel) darkestPixel = bright;
                }
            }

            // now reset tolerance
            ToleranceCalculated = darkestPixel + ((int)((decimal)(((lightestPixel - darkestPixel)) / 2) * ((decimal)(((decimal)ScanTolerance) / ((decimal)(100))))));
            Trace("lightestPixel: " + lightestPixel.ToString());
            Trace("darkestPixel: " + darkestPixel.ToString());
            Trace("tolerance: " + ToleranceCalculated.ToString());

            int hitCount = 0;

            for (int row = 0; row < bmp.Height; row++)
            {
                for(int col = 0; col < bmp.Width; col++)
                {
                    c = bmp.GetPixel(col, row);

                    //if(row == randomRow && col == randomCol)
                    //{
                    //    MessageBox.Show(c.ToString());
                    //    MessageBox.Show((c.R).ToString());
                    //}

                    if(DetectPixelOn(c, ToleranceCalculated))
                    {
                        hitCount++;
                        if(hitCount < 1000)
                        {
                            Trace("pixel color: " + c.ToString());
                        }

                        darkCount++;
                        if (maxX == -1) maxX = col;
                        if(col > maxX) maxX = col;

                        if (minX == -1) minX = col;
                        if (col < minX) minX = col;

                        maxY = row;
                        if (minY == -1) minY = row;
                    }
                    else
                    {
                        lightCount++;
                    }
                }
            }

            bool isLandscape = false;
            if(maxX - minX > maxY - minY)
            {
                isLandscape = true;
            }

            Trace("lightCount: " + lightCount.ToString() + Environment.NewLine + "darkCount: " + darkCount.ToString());

           Trace(
                "minX: " + minX.ToString() + Environment.NewLine +
                "maxX: " + maxX.ToString() + Environment.NewLine +
                "minY: " + minY.ToString() + Environment.NewLine +
                "maxY: " + maxY.ToString() + Environment.NewLine + Environment.NewLine +
                "Image Width: " + bmp.Width + Environment.NewLine +
                "Image Height: " + bmp.Height);

            MessageBox.Show(
                "minX: " + minX.ToString() + Environment.NewLine +
                "maxX: " + maxX.ToString() + Environment.NewLine +
                "minY: " + minY.ToString() + Environment.NewLine +
                "maxY: " + maxY.ToString() + Environment.NewLine + Environment.NewLine +
                "Image Width: " + bmp.Width + Environment.NewLine +
                "Image Height: " + bmp.Height);

            Trace("isLandscape: " + isLandscape.ToString());
            
            ScanRectangle = new Rectangle(minX, minY, maxX - minX, maxY - minY);
            ShowScanDocument(bmp, ScanRectangle.Value);

        }

        private void textBoxScanTolerance_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if(!string.IsNullOrEmpty(textBoxScanTolerancePercentage.Text))
            {
               int.TryParse(textBoxScanTolerancePercentage.Text, out val);
            }

            ScanTolerance = val;

            Properties.Settings.Default.ScanTolerancePercentage = val.ToString();
            Properties.Settings.Default.Save();
        }

        private void textBoxReadTolerancePercentage_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (!string.IsNullOrEmpty(textBoxReadTolerancePercentage.Text))
            {
                int.TryParse(textBoxReadTolerancePercentage.Text, out val);
            }

            ReadTolerance = val;

            Properties.Settings.Default.ReadTolerancePercentage = val.ToString();
            Properties.Settings.Default.Save();
        }

        private void DrawRectangleOnBitmap(Bitmap bmp, Color c, int x, int y, int width, int height)
        {
            for (int row = y; row < y + height; row++)
            {
                for (int col = x; col < x + width; col++)
                {
                    if (row == y || row == y + height - 1)
                    {
                        bmp.SetPixel(col, row, c);
                    }
                    else if (col == x || col == x + width - 1)
                    {
                        bmp.SetPixel(col, row, c);
                    }
                }
            }
        }

        private void ShowScanDocument(Bitmap bmp, Rectangle rect)
        {
            int bmpOriginalWidth = bmp.Width;
            int bmpOriginalHeight = bmp.Height;

            if (bmp.Width > this.Width - 50 && bmp.Height > this.Height - 50)
            {
                bmp = (Bitmap)ScaleToHeight(bmp as System.Drawing.Image, 920);
                if (bmp.Width > 1860) bmp = (Bitmap)ScaleToWidth(bmp as System.Drawing.Image, 1860);
            }

            decimal scaleFactor = 1;

            if(bmp.Height != bmpOriginalHeight)
            {
                scaleFactor = (decimal)bmp.Height / (decimal)bmpOriginalHeight;
            }

            int formPadding = 10;

            Form f = new Form();
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = bmp;



            f.Controls.Add(pictureBox);
            pictureBox.Left = formPadding;
            pictureBox.Top = formPadding;
            pictureBox.Width = bmp.Width + formPadding;
            pictureBox.Height = bmp.Height + formPadding;

            f.Show();

            // mutate rect by scale factor
            Rectangle scaledRect = new Rectangle();
            scaledRect.Width = (int)( ((decimal)rect.Width) * scaleFactor);
            scaledRect.Height = (int)( ((decimal)rect.Height) * scaleFactor);
            scaledRect.X = (int)(((decimal)rect.X) * scaleFactor);
            scaledRect.Y = (int)(((decimal)rect.Y) * scaleFactor);

            DrawRectangleOnBitmap(bmp, Color.Red, scaledRect.X, scaledRect.Y, scaledRect.Width, scaledRect.Height);
            
            f.Width = bmp.Width + (2 * formPadding);
            f.Height = bmp.Height + (2 * formPadding);
        }

        private void buttonReadCode_Click(object sender, EventArgs e)
        {
            if(ScanRectangle == null)
            {
                MessageBox.Show("You must first click Scan Code to define the bounding rectangle.");
                return;
            }

            Bitmap bmp = Bitmap.FromFile(Path.Combine(textBoxImageSourceFolder.Text, textBoxSourceImageName.Text)) as Bitmap;
            var rect = ScanRectangle.Value;

            bool cellState = false;

            int analyzingRow = rect.Y + (rect.Height / 2);
            StringBuilder sb = new StringBuilder();

            // find row min/max color
            int lightestPixel = 0;
            int darkestPixel = 255;
            Color c;

            // first find lightest light and darkest dark
            for (int row = 0; row < bmp.Height; row++)
            {
                for (int col = 0; col < bmp.Width; col++)
                {
                    c = bmp.GetPixel(col, row);
                    int bright = PixelBrightness(c);

                    if (bright > lightestPixel) lightestPixel = bright;
                    if (bright < darkestPixel) darkestPixel = bright;
                }
            }

            int rowToleranceBreakPoint = darkestPixel + ((int)(((decimal)lightestPixel - (decimal)darkestPixel) * ((decimal) ((decimal)ReadTolerance / (decimal)100))));
            int pixelValue = 0;

            int subWidth = rect.Width;
            //int subHeight = 250;// testing....
            bool lastState = false;
            string cellStateStr;

            // analyze a row
            for (int col =  0; col < subWidth; col++)
            {
                pixelValue = PixelBrightness(bmp.GetPixel(rect.X + col, analyzingRow));
                cellState = false;
                if (pixelValue < rowToleranceBreakPoint) cellState = true;
                //cellState = DetectPixelOn(bmp.GetPixel(col, analyzingRow), ToleranceCalculated);

                cellStateStr = (cellState ? "1" : "0");

                if (cellState != lastState)
                {
                    sb.Append(Environment.NewLine + cellStateStr);
                }
                else
                {
                    sb.Append(cellStateStr);
                }

                lastState = cellState;
            }

            Rectangle analyzedRectangle = new Rectangle(rect.X, analyzingRow, subWidth, 3);

            ShowScanDocument(bmp, analyzedRectangle);

            MessageBox.Show("Binary Row Data" + Environment.NewLine + sb.ToString());
        }

        private void buttonMath_Click(object sender, EventArgs e)
        {
            var f = new RachelsDrawingFun();
            f.Show();
        }

        private void buttonBookSlug_Click(object sender, EventArgs e)
        {
            InsertTextIntoBlogTextBox("<b><strike>Book</strike> Movie Report:</b> <i></i>", 46);
        }

        private void buttonOpenMasterHtml_Click(object sender, EventArgs e)
        {
            //if (textBoxMasterHtmlFile.Text.IndexOf(".php") != -1)
            //{
            //    var tempFileName = textBoxMasterHtmlFile.Text.Substring(0, textBoxMasterHtmlFile.Text.IndexOf(".php")) + "_temp.html";
            //    var tempFile = Path.Combine(textBoxSiteBaseFolder.Text, tempFileName);
            //    File.WriteAllText(tempFile,
            //        File.ReadAllText(Path.Combine(textBoxSiteBaseFolder.Text, textBoxMasterHtmlFile.Text)));
            //    Process.Start(tempFileName);
            //    return;
            //}

            Process.Start(textBoxMasterHtmlFile.Text);
        }

        private void SetImageScrubbingSliderBounds(int pCount, int pCurrent)
        {
            hScrollBarImageScrubbingSlider.Maximum = pCount;
            hScrollBarImageScrubbingSlider.Minimum = 1;
            hScrollBarImageScrubbingSlider.Value = pCurrent + 1;
            hScrollBarImageScrubbingSlider.Refresh();
        }

        private void hScrollBarImageScrubbingSlider_Scroll(object sender, ScrollEventArgs e)
        {
            SourceImagesCurrentImageIndex += e.NewValue - e.OldValue;
            RefreshSourceImage();
        }

        private void textBoxOtherProductionURL_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OtherProductionURL = textBoxOtherProductionURL.Text;
            Properties.Settings.Default.Save();
        }

        private void checkBoxUseOtherProductionURL_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseOtherProductonURL = checkBoxUseOtherProductionURL.Checked;
            Properties.Settings.Default.Save();
        }

        private void textBoxNewWidth_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ImageResizeWidth = textBoxNewWidth.Text;
            Properties.Settings.Default.Save();
        }

        private void textBoxNewHeight_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ImageResizeHeight = textBoxNewHeight.Text;
            Properties.Settings.Default.Save();
        }

        private void buttonBudget_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxUseShowImagesWrapper_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseShowImagesWrapper = checkBoxUseShowImagesWrapper.Checked;
            Properties.Settings.Default.Save();
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            new BlogUtilitySettings().Show();
        }

        private void buttonRemoveAudio_Click(object sender, EventArgs e)
        {
            if (listBoxAudio.SelectedItems.Count > 0)
            {
                listBoxAudio.Items.Remove(listBoxAudio.SelectedItems[0]);
                listBoxAudio.Refresh();
            }
        }

        private void buttonRemoveAllAudio_Click(object sender, EventArgs e)
        {
            if(listBoxAudio.Items.Count > 0)
            {
                do
                {
                    listBoxAudio.Items.RemoveAt(0);
                } while (listBoxAudio.Items.Count > 0);
            }
            
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            // if textBoxOtherProductionURL exists (it's a local apache virtual host)
            // then enumerate all the .php files in textBoxSiteBaseFolder (should put a button there to enumerate files in subdirectories)

            if(string.IsNullOrEmpty(textBoxSiteBaseFolder.Text))
            {
                MessageBox.Show("Site Base Folder is not defined!  Publish will enumerate .php files in that directory and prompt to publish those files to .html");
                return;
            }

            if(!Directory.Exists(textBoxSiteBaseFolder.Text))
            {
                MessageBox.Show("Site Base Folder does not exist!");
                return;
            }

            DirectoryInfo di = new DirectoryInfo(textBoxSiteBaseFolder.Text);
            var files = di.GetFiles("*.php");
            if(files.Length > 0)
            {
                List<string> filesToPublish = new List<string>();
                foreach( var file in files)
                {
                    filesToPublish.Add(file.Name);
                }

                FormBlogPublish form = new FormBlogPublish(filesToPublish, textBoxSiteBaseFolder.Text, textBoxOtherProductionURL.Text);
                form.Show();
                form.BringToFront();
                return;
            }

            MessageBox.Show("Site Base Path did not contain .php files to publish to .html files.");

            
        }
    }
}
