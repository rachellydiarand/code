using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DT_Blog_Utility
{
    public partial class FormImageCopy : Form
    {

        public Thread FileTransferThread { get; set; }
        public CancellationTokenSource FileTransferThreadCancelatonTokenSource { get; set; }
        public List<string> FilesToCopy { get; set; }
        public string SiteBasePath { get; set; }

        public bool FileTransferComplete { get; set; } = false;

        public FormImageCopy(List<string> pFiles, string pSiteBasePath, string pDestinationPath)
        {
            InitializeComponent();

            SiteBasePath = pSiteBasePath;

            foreach (string file in pFiles)
            {
                listBoxImagesToCopy.Items.Add(file);
            }

            FilesToCopy = pFiles;
            textBoxDestinationPath.Text = pDestinationPath;
        }

        private void FormImageCopy_Load(object sender, EventArgs e)
        {
            
        }

        public void FileTransferThreadWorker(CancellationTokenSource pCancelationToken)
        {
            string destination = textBoxDestinationPath.Text;
            if(!Directory.Exists(destination))
            {
                try
                {
                    Directory.CreateDirectory(destination);
                }
                catch(Exception ex){
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            // grab the files from the list box
            // we added a delete feature on them
            List<string> files = new List<string>();
            foreach(var item in listBoxImagesToCopy.Items)
            {
                files.Add(item.ToString());
            }

            foreach (string file in files)
            {
                if (pCancelationToken.Token.IsCancellationRequested)
                {
                    break;
                }
                TransferFile(SiteBasePath, file, destination);
            }

            FileTransferComplete = true;
            MessageBox.Show("File Transfer Complete");
        }

        private void Log(string str)
        {
            if(textBoxProgress.InvokeRequired)
            {
                textBoxProgress.Invoke(new Action<string>(Log), str);
            }
            else
            {
                textBoxProgress.Text = textBoxProgress.Text + System.Environment.NewLine + str;
            }
        }

        public void TransferFile(string basePath, string fileLocalName, string destination)
        {
            string source = basePath;// fileLocalName;// Path.Combine(basePath, fileLocalName);
            
            if(System.IO.File.Exists(source))
            {
                                
            }
            else if(File.Exists(fileLocalName))
            {
                source = fileLocalName;
            }
            else
            {
                if(fileLocalName.StartsWith("\\"))
                {
                    fileLocalName = fileLocalName.Substring(1);
                }
                source = Path.Combine(basePath, fileLocalName);                
            }

            Log(source + " to " + destination);

            FileInfo fileInfo = new FileInfo(source);
            string destinationPathAndFileName = Path.Combine(destination, fileInfo.Name);
            if (!File.Exists(destinationPathAndFileName))
            {
                File.Copy(source, destinationPathAndFileName);
            }
        }

        private void buttonTransferNow_Click(object sender, EventArgs e)
        {
            FileTransferComplete = false;
            FileTransferThreadCancelatonTokenSource = new CancellationTokenSource();
            //FileTransferThread = new Thread(FileTransferThreadWorker);
            FileTransferThread = new Thread(() => FileTransferThreadWorker(FileTransferThreadCancelatonTokenSource));
            FileTransferThread.Start();
        }

        private void listBoxImagesToCopy_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (listBoxImagesToCopy.SelectedIndex > -1)
                {
                    int indexToDelete = listBoxImagesToCopy.SelectedIndex;
                    listBoxImagesToCopy.Items.RemoveAt(indexToDelete);
                    listBoxImagesToCopy.Refresh();
                }
            }
        }
    }
}
