using DT_Blog_Utility.models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DT_Blog_Utility
{
    public partial class FormBlogPublish : Form
    {
        public string ServerUrl { get; set; }
        public string SiteBasePath { get; set; }
        public FormBlogPublish(List<string> pFilesToPublishToHtml, string pSiteBasePath, string pServerUrl)
        {
            InitializeComponent();

            ServerUrl = pServerUrl;
            SiteBasePath = pSiteBasePath;
            if(pFilesToPublishToHtml.Count > 0)
            {
                listBoxPagesToPublish.Items.AddRange(pFilesToPublishToHtml.ToArray());
            }

            // new select items if they have been saved as selected to be published last time
            ModelPagesToPublish pagesToPublish = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelPagesToPublish>(Properties.Settings.Default.FilesToPublishJson);
            if(pagesToPublish != null && pagesToPublish.PagesToPublish != null && pagesToPublish.PagesToPublish.Count > 0)
            {
                foreach(var modelPageToPublish  in pagesToPublish.PagesToPublish)
                {                    
                    if(modelPageToPublish.SiteUrl != null && modelPageToPublish.SiteUrl == ServerUrl)
                    {
                        for(var a = 0; a < listBoxPagesToPublish.Items.Count; a++)
                        {
                            if (modelPageToPublish.Pages.Contains(listBoxPagesToPublish.Items[a]))
                            {
                                listBoxPagesToPublish.SetSelected(a, true);
                            }
                        }

                        if(modelPageToPublish.PublishDirectory != null)
                        {
                            textBoxPublishDirectory.Text = modelPageToPublish.PublishDirectory;
                        }

                        if(modelPageToPublish.Mutations != null && modelPageToPublish.Mutations.Count > 0)
                        {
                            foreach(var record in modelPageToPublish.Mutations)
                            {
                                listBoxMutations.Items.Add(record);
                            }
                        }

                        if (modelPageToPublish.UrlMutations != null && modelPageToPublish.UrlMutations.Count > 0)
                        {
                            foreach (var record in modelPageToPublish.UrlMutations)
                            {
                                listBoxUrlMutations.Items.Add(record);
                            }
                        }
                    }
                }
            }

            if(string.IsNullOrEmpty(textBoxPublishDirectory.Text)) 
            {
                // manually add this, getting tired of debugging this now, will fix later
                textBoxPublishDirectory.Text = "C:\\_rachel\\websites\\moth\\git\\moth";
            }

            if(true || listBoxMutations.Items.Count == 0)
            {
                // just clear it out for now
                // you can always add at run time
                // will likely fix soon, it's not in the repo yet anyway
                listBoxMutations.Items.Clear();

                // add these manually
                listBoxMutations.Items.Add(".php\" to .html\"");
                listBoxMutations.Items.Add("/for/innerLog.php?i=1' to /for/innerLogImages.html'");
                listBoxMutations.Items.Add(".php' to .html'");
                listBoxMutations.Items.Add("/moth/ to /for/");

            }

            if(listBoxUrlMutations.Items.Count == 0)
            {
                // add these manually, can always press delete key on them
                listBoxUrlMutations.Items.Add("innerLog.php?i=1 to innerLogImages.html");
            }

            listBoxPagesToPublish.Items.Add("innerLog.php?i=1");

            SaveListBoxContents();
        }

        private void buttonBrowsePublishPageLink_Click(object sender, EventArgs e)
        {
            // this is browsing for a file to add to the list of files to publish from .php to .html

            var dialog = new OpenFileDialog();

            string currentPath = Properties.Settings.Default.PublishFileDirectory;
            if (!string.IsNullOrEmpty(currentPath))
            {
                dialog.FileName = currentPath;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(dialog.FileName);
                Properties.Settings.Default.PublishFileDirectory = fileInfo.DirectoryName;
                Properties.Settings.Default.Save();

                textBoxAddPublishPageLink.Text = fileInfo.Name;
                //listBoxFilesToPublish.Items.Add(fileInfo.Name);

                SaveListBoxContents();
            }
        }

        private void buttonBrowseForPublishDirectory_Click(object sender, EventArgs e)
        {
            // this is browsing for the directory to publish to
            // both this and the browse for file to add to the publish list save the directory that was browsed to
            var dialog = new FolderBrowserDialog();

            string currentPath = Properties.Settings.Default.PublishFileDirectory;
            if (!string.IsNullOrEmpty(currentPath))
            {
                dialog.SelectedPath = currentPath;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {                
                Properties.Settings.Default.PublishFileDirectory = dialog.SelectedPath;
                Properties.Settings.Default.Save();

                textBoxPublishDirectory.Text = dialog.SelectedPath;
            }
        }

        private async void buttonPublish_Click(object sender, EventArgs e)
        {
            if(listBoxPagesToPublish.Items == null || listBoxPagesToPublish.Items.Count == 0)
            {
                MessageBox.Show("No files to publish! Please try again....");
                return;
            }

            if (listBoxPagesToPublish.SelectedItems.Count == 0)
            {
                MessageBox.Show("No files to publish! Please multi-select the items to publish....");
                return;
            }

            if (string.IsNullOrEmpty(textBoxPublishDirectory.Text) || !Directory.Exists(textBoxPublishDirectory.Text))
            {
                MessageBox.Show("The directory/folder you are trying to publish to does not exist!");
                return;
            }

            //StringContent contentType = new StringContent(postStr, System.Text.Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient()) // WebClient class inherits IDisposable
            {
                List<string> files = listBoxPagesToPublish.SelectedItems.Cast<string>().ToList();
                foreach (string file in files)
                {
                    string fileTemp = file;
                    string url = ServerUrl + "/" + fileTemp;
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string htmlCode = await response.Content.ReadAsStringAsync();

                        // do the mutation substitutions
                        var mutations = GetMutationsList("findAndReplace");
                        if(mutations != null && mutations.Count > 0)
                        {
                            foreach(var mutation in mutations)
                            {
                                string find = mutation.Substring(0, mutation.IndexOf(" to "));
                                string replace = mutation.Substring(mutation.IndexOf(" to ") + 4);
                                htmlCode = htmlCode.Replace(find, replace);
                            }
                        }


                        mutations = GetMutationsList("urlMutations");
                        if (mutations != null && mutations.Count > 0)
                        {
                            foreach (var mutation in mutations)
                            {
                                string find = mutation.Substring(0, mutation.IndexOf(" to "));
                                string replace = mutation.Substring(mutation.IndexOf(" to ") + 4);

                                if (!fileTemp.Contains(find)) continue;
                                fileTemp = fileTemp.Replace(find, replace);
                            }
                        }

                        // could be .css .js or something else
                        string newfilePathAndName = Path.Combine(textBoxPublishDirectory.Text, fileTemp);

                        if(fileTemp.IndexOf(".php") > -1)
                        {
                            // change .php to .html
                            newfilePathAndName = Path.Combine(textBoxPublishDirectory.Text, fileTemp.Replace(".php", ".html"));
                        }

                        File.WriteAllText(newfilePathAndName, htmlCode);
                    }
                }
            }

            // save the publish directory so that it will populate next time this window opens
            ModelPagesToPublish json = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelPagesToPublish>(Properties.Settings.Default.FilesToPublishJson);
            if(json != null && json.PagesToPublish != null && json.PagesToPublish.Count > 0)
            {
                for(var a = 0; a < json.PagesToPublish.Count; a++)
                {
                    if (json.PagesToPublish[a].SiteUrl != null && json.PagesToPublish[a].SiteUrl == ServerUrl)
                    {
                        json.PagesToPublish[a].PublishDirectory = textBoxPublishDirectory.Text;
                        // now save the mutations to this site record
                        json.PagesToPublish[a].Mutations = GetMutationsList("findAndReplace");
                        json.PagesToPublish[a].UrlMutations = GetMutationsList("urlMutations");
                        break;
                    }
                }                
            }

            Properties.Settings.Default.FilesToPublishJson = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            Properties.Settings.Default.Save();

            MessageBox.Show("Publish Complete!");
        }

        private void listBoxFilesToPublish_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveListBoxContents();
        }

        private void SaveListBoxContents()
        {
            ModelPagesToPublish json = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelPagesToPublish>(Properties.Settings.Default.FilesToPublishJson);

            ModelFilesToPublish newFilesToPublish = new ModelFilesToPublish();
            newFilesToPublish.SiteUrl = ServerUrl;
            newFilesToPublish.PublishDirectory = textBoxPublishDirectory.Text;
            newFilesToPublish.Pages = new List<string>();
            foreach (string item in listBoxPagesToPublish.SelectedItems)
            {
                newFilesToPublish.Pages.Add(item);
            }

            if (json == null)
            {
                // create json
                json = new ModelPagesToPublish();
            }

            if (json.PagesToPublish == null || json.PagesToPublish.Count == 0)
            {
                // create new
                json.PagesToPublish = new List<ModelFilesToPublish>();
                json.PagesToPublish.Add(newFilesToPublish);
            }
            else
            {
                bool hit = false;
                for (int a = 0; a < json.PagesToPublish.Count; a++)
                {
                    if (json.PagesToPublish[a].SiteUrl == newFilesToPublish.SiteUrl)
                    {
                        json.PagesToPublish[a] = newFilesToPublish;
                        hit = true;
                        break;
                    }
                }

                if (!hit)
                {
                    // the list of sites/files had sites in it but not this one
                    json.PagesToPublish.Add(newFilesToPublish);
                }
            }

            Properties.Settings.Default.FilesToPublishJson = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            Properties.Settings.Default.Save();

            
        }

        private List<string> GetMutationsList(string pType)
        {
            List<string> mutations = new List<string>();
            if (pType == "findAndReplace")
            {                
                if (listBoxMutations.Items.Count == 0) return mutations;
                foreach (var record in listBoxMutations.Items)
                {
                    mutations.Add(record.ToString());
                }
            }
            else if (pType == "urlMutations")
            {
                if (listBoxUrlMutations.Items.Count == 0) return mutations;
                foreach (var record in listBoxUrlMutations.Items)
                {
                    mutations.Add(record.ToString());
                }
            }

            return mutations;
        }

        private void buttonMutationGo_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBoxMutation.Text))
            {
                if(textBoxMutation.Text.IndexOf(" to ") == -1)
                {
                    MessageBox.Show("The mutation field must have \" to \" in it.  The left side is the find and the right side is replace with. Please try again!");
                    return;
                }
                //string find = textBoxMutation.Text.Substring(0, textBoxMutation.Text.IndexOf(" to "));
                //string replace = textBoxMutation.Text.Substring(textBoxMutation.Text.IndexOf(" to "));
                listBoxMutations.Items.Add(textBoxMutation.Text);

            }
        }

        private void listBoxMutations_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ListBoxMutations_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.KeyCode.ToString());
            if(e.KeyCode == Keys.Delete)
            {
                if(listBoxMutations.SelectedIndex > -1)
                {
                    int indexToDelete = listBoxMutations.SelectedIndex;
                    listBoxMutations.Items.RemoveAt(indexToDelete);
                    listBoxMutations.Refresh();
                }
            }
        }

        private void ListBoxUrlMutations_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.KeyCode == Keys.Delete)
            {
                if (listBoxUrlMutations.SelectedIndex > -1)
                {
                    int indexToDelete = listBoxUrlMutations.SelectedIndex;
                    listBoxUrlMutations.Items.RemoveAt(indexToDelete);
                    listBoxUrlMutations.Refresh();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty (textBoxAddPublishPageLink.Text))
            {
                listBoxPagesToPublish.Items.Add(textBoxAddPublishPageLink.Text);
                //textBoxAddPublishPageLink.Text = "";// do this manually in case you make a mistake
            }
        }

        private void buttonAddUrlMutationGo_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBoxUrlMutation.Text))
            {
                listBoxUrlMutations.Items.Add(textBoxUrlMutation.Text);
            }
        }

        private async void buttonCopyImages_Click(object sender, EventArgs e)
        {
            // parse all .php files in the list
            // make a list of all image references
            // be careful about the base url
            // after the list is done, open a new FormImageCopy
            // and send the image list and desitination directory suggestion
            // to the constructor function
            // show the form and bringToFront()
            // the form allows the user to double check
            // the form uses a thread system to not block the ui

            List<string> imagesToCopy = new List<string>();

            if (listBoxPagesToPublish.Items == null || listBoxPagesToPublish.Items.Count == 0)
            {
                MessageBox.Show("No files to publish! Please try again....");
                return;
            }

            if (listBoxPagesToPublish.SelectedItems.Count == 0)
            {
                MessageBox.Show("No files to publish! Please multi-select the items to publish....");
                return;
            }

            if (string.IsNullOrEmpty(textBoxPublishDirectory.Text) || !Directory.Exists(textBoxPublishDirectory.Text))
            {
                MessageBox.Show("The directory/folder you are trying to publish to does not exist!");
                return;
            }

            //StringContent contentType = new StringContent(postStr, System.Text.Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient()) // WebClient class inherits IDisposable
            {
                List<string> pages = listBoxPagesToPublish.SelectedItems.Cast<string>().ToList();
                foreach (string page in pages)
                {
                    string pageTemp = page;
                    string url = ServerUrl + "/" + pageTemp;
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string htmlCode = await response.Content.ReadAsStringAsync();

                        // ok, let's start basic.
                        // we are grabbing images from the <img src="
                        // so, search for space src="
                        // we'll keep a running index or substring
                        // when we find one, we'll look for the next instance of the double quote
                        // I almost always use double quotes in my html for element parameter assignments

                        // we can just alter the htmlCode string
                        do
                        {
                            DataImageSearchResult dataImageSearchResult = GetNextImageFromHtml(htmlCode);
                            if (string.IsNullOrEmpty(dataImageSearchResult.ImagePath)) break;
                            imagesToCopy.Add(dataImageSearchResult.ImagePath);
                            htmlCode = dataImageSearchResult.RemainingHtml;
                        } while (true);

                    }
                }

                if(imagesToCopy.Count == 0)
                {
                    MessageBox.Show("Could not find any images in any files.");
                }
                else
                {
                    string proposedNewImageDirectory = Path.Combine(textBoxPublishDirectory.Text, "images");
                    FormImageCopy f = new FormImageCopy(imagesToCopy, SiteBasePath, proposedNewImageDirectory);
                    f.Show();
                    f.BringToFront();
                }
            }
        }

        private DataImageSearchResult GetNextImageFromHtml(string htmlStr)
        {
            DataImageSearchResult returnResult = new DataImageSearchResult();
            if (string.IsNullOrEmpty(htmlStr)) return returnResult;

            do
            {
                // some images are <img tags and some are <a href tags
                // find the next one 'cause it's all linear
                int nextTagIndex = htmlStr.IndexOf(" src=\"") + 6;
                int nextTagIndex2 = htmlStr.IndexOf(" href=\"") + 7;
                string tagType = "src";
                if (nextTagIndex == -1)
                {
                    nextTagIndex = nextTagIndex2;
                    tagType = "href";
                }
                else
                {
                    if (nextTagIndex2 != -1)
                    {
                        if (nextTagIndex > nextTagIndex2)
                        {
                            nextTagIndex = nextTagIndex2;
                            tagType = "href";
                        }
                    }
                }

                if (nextTagIndex > -1)
                {
                    int startingIndex = nextTagIndex;
                    string shortenedStr = htmlStr.Substring(startingIndex);
                    int endingIndex = shortenedStr.IndexOf("\"");

                    if (endingIndex > -1)
                    {
                        string imageUrl = shortenedStr.Substring(0, endingIndex);
                        htmlStr = shortenedStr.Substring(endingIndex + 1);

                        if (imageUrl.Contains(".jpg") || imageUrl.Contains(".png"))
                        {                            
                            returnResult.ImagePath = imageUrl.Replace("/", "\\");
                            returnResult.RemainingHtml = htmlStr;
                            return returnResult;
                        }
                        else
                        {
                            // need to continue the do loop here
                            // with the shortened htmlStr
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            } while (true);
            

            return returnResult;
        }

        private void FormBlogPublish_Load(object sender, EventArgs e)
        {

        }
    }
}
