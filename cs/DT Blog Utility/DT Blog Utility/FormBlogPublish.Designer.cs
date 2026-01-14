namespace DT_Blog_Utility
{
    partial class FormBlogPublish
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxPagesToPublish = new System.Windows.Forms.ListBox();
            this.buttonBrowsePublishPageLink = new System.Windows.Forms.Button();
            this.textBoxAddPublishPageLink = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonPublish = new System.Windows.Forms.Button();
            this.textBoxPublishDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBrowseForPublishDirectory = new System.Windows.Forms.Button();
            this.listBoxMutations = new System.Windows.Forms.ListBox();
            this.textBoxMutation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonMutationGo = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonAddUrlMutationGo = new System.Windows.Forms.Button();
            this.listBoxUrlMutations = new System.Windows.Forms.ListBox();
            this.textBoxUrlMutation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCopyImages = new System.Windows.Forms.Button();
            this.buttonOpenSiteBaseFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Files to Publish to HTML";
            // 
            // listBoxPagesToPublish
            // 
            this.listBoxPagesToPublish.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPagesToPublish.FormattingEnabled = true;
            this.listBoxPagesToPublish.Location = new System.Drawing.Point(13, 50);
            this.listBoxPagesToPublish.Name = "listBoxPagesToPublish";
            this.listBoxPagesToPublish.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxPagesToPublish.Size = new System.Drawing.Size(693, 238);
            this.listBoxPagesToPublish.TabIndex = 11;
            this.listBoxPagesToPublish.SelectedIndexChanged += new System.EventHandler(this.listBoxFilesToPublish_SelectedIndexChanged);
            // 
            // buttonBrowsePublishPageLink
            // 
            this.buttonBrowsePublishPageLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowsePublishPageLink.Location = new System.Drawing.Point(641, 4);
            this.buttonBrowsePublishPageLink.Name = "buttonBrowsePublishPageLink";
            this.buttonBrowsePublishPageLink.Size = new System.Drawing.Size(34, 23);
            this.buttonBrowsePublishPageLink.TabIndex = 10;
            this.buttonBrowsePublishPageLink.Text = "...";
            this.buttonBrowsePublishPageLink.UseVisualStyleBackColor = true;
            this.buttonBrowsePublishPageLink.Click += new System.EventHandler(this.buttonBrowsePublishPageLink_Click);
            // 
            // textBoxAddPublishPageLink
            // 
            this.textBoxAddPublishPageLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAddPublishPageLink.Location = new System.Drawing.Point(134, 6);
            this.textBoxAddPublishPageLink.Name = "textBoxAddPublishPageLink";
            this.textBoxAddPublishPageLink.Size = new System.Drawing.Size(506, 20);
            this.textBoxAddPublishPageLink.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Add Publish Page/Link";
            // 
            // buttonPublish
            // 
            this.buttonPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPublish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(220)))), ((int)(((byte)(0)))));
            this.buttonPublish.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPublish.ForeColor = System.Drawing.Color.White;
            this.buttonPublish.Location = new System.Drawing.Point(613, 623);
            this.buttonPublish.Name = "buttonPublish";
            this.buttonPublish.Size = new System.Drawing.Size(93, 23);
            this.buttonPublish.TabIndex = 13;
            this.buttonPublish.Text = "Publish!";
            this.buttonPublish.UseVisualStyleBackColor = false;
            this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
            // 
            // textBoxPublishDirectory
            // 
            this.textBoxPublishDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPublishDirectory.Location = new System.Drawing.Point(134, 299);
            this.textBoxPublishDirectory.Name = "textBoxPublishDirectory";
            this.textBoxPublishDirectory.Size = new System.Drawing.Size(541, 20);
            this.textBoxPublishDirectory.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Publish Directory";
            // 
            // buttonBrowseForPublishDirectory
            // 
            this.buttonBrowseForPublishDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseForPublishDirectory.Location = new System.Drawing.Point(604, 297);
            this.buttonBrowseForPublishDirectory.Name = "buttonBrowseForPublishDirectory";
            this.buttonBrowseForPublishDirectory.Size = new System.Drawing.Size(102, 23);
            this.buttonBrowseForPublishDirectory.TabIndex = 16;
            this.buttonBrowseForPublishDirectory.Text = "...";
            this.buttonBrowseForPublishDirectory.UseVisualStyleBackColor = true;
            this.buttonBrowseForPublishDirectory.Click += new System.EventHandler(this.buttonBrowseForPublishDirectory_Click);
            // 
            // listBoxMutations
            // 
            this.listBoxMutations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMutations.FormattingEnabled = true;
            this.listBoxMutations.Location = new System.Drawing.Point(13, 382);
            this.listBoxMutations.Name = "listBoxMutations";
            this.listBoxMutations.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxMutations.Size = new System.Drawing.Size(693, 82);
            this.listBoxMutations.TabIndex = 19;
            this.listBoxMutations.SelectedIndexChanged += new System.EventHandler(this.listBoxMutations_SelectedIndexChanged);
            this.listBoxMutations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListBoxMutations_KeyDown);
            // 
            // textBoxMutation
            // 
            this.textBoxMutation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMutation.Location = new System.Drawing.Point(181, 338);
            this.textBoxMutation.Name = "textBoxMutation";
            this.textBoxMutation.Size = new System.Drawing.Size(494, 20);
            this.textBoxMutation.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 341);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Add Mutation (Find and Replace)";
            // 
            // buttonMutationGo
            // 
            this.buttonMutationGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMutationGo.Location = new System.Drawing.Point(604, 336);
            this.buttonMutationGo.Name = "buttonMutationGo";
            this.buttonMutationGo.Size = new System.Drawing.Size(102, 23);
            this.buttonMutationGo.TabIndex = 20;
            this.buttonMutationGo.Text = "GO!";
            this.buttonMutationGo.UseVisualStyleBackColor = true;
            this.buttonMutationGo.Click += new System.EventHandler(this.buttonMutationGo_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(672, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonAddUrlMutationGo
            // 
            this.buttonAddUrlMutationGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddUrlMutationGo.Location = new System.Drawing.Point(606, 478);
            this.buttonAddUrlMutationGo.Name = "buttonAddUrlMutationGo";
            this.buttonAddUrlMutationGo.Size = new System.Drawing.Size(102, 23);
            this.buttonAddUrlMutationGo.TabIndex = 25;
            this.buttonAddUrlMutationGo.Text = "GO!";
            this.buttonAddUrlMutationGo.UseVisualStyleBackColor = true;
            this.buttonAddUrlMutationGo.Click += new System.EventHandler(this.buttonAddUrlMutationGo_Click);
            // 
            // listBoxUrlMutations
            // 
            this.listBoxUrlMutations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxUrlMutations.FormattingEnabled = true;
            this.listBoxUrlMutations.Location = new System.Drawing.Point(15, 524);
            this.listBoxUrlMutations.Name = "listBoxUrlMutations";
            this.listBoxUrlMutations.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxUrlMutations.Size = new System.Drawing.Size(693, 82);
            this.listBoxUrlMutations.TabIndex = 24;
            this.listBoxUrlMutations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListBoxUrlMutations_KeyDown);
            // 
            // textBoxUrlMutation
            // 
            this.textBoxUrlMutation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrlMutation.Location = new System.Drawing.Point(205, 480);
            this.textBoxUrlMutation.Name = "textBoxUrlMutation";
            this.textBoxUrlMutation.Size = new System.Drawing.Size(472, 20);
            this.textBoxUrlMutation.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 483);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Add URL Mutation ( url \" to \" newUrl )";
            // 
            // buttonCopyImages
            // 
            this.buttonCopyImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonCopyImages.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCopyImages.ForeColor = System.Drawing.Color.White;
            this.buttonCopyImages.Location = new System.Drawing.Point(497, 623);
            this.buttonCopyImages.Name = "buttonCopyImages";
            this.buttonCopyImages.Size = new System.Drawing.Size(110, 23);
            this.buttonCopyImages.TabIndex = 26;
            this.buttonCopyImages.Text = "Copy Images";
            this.buttonCopyImages.UseVisualStyleBackColor = false;
            this.buttonCopyImages.Click += new System.EventHandler(this.buttonCopyImages_Click);
            // 
            // buttonOpenSiteBaseFolder
            // 
            this.buttonOpenSiteBaseFolder.BackColor = System.Drawing.Color.Pink;
            this.buttonOpenSiteBaseFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenSiteBaseFolder.Location = new System.Drawing.Point(334, 623);
            this.buttonOpenSiteBaseFolder.Name = "buttonOpenSiteBaseFolder";
            this.buttonOpenSiteBaseFolder.Size = new System.Drawing.Size(157, 23);
            this.buttonOpenSiteBaseFolder.TabIndex = 63;
            this.buttonOpenSiteBaseFolder.Text = "Open Folder";
            this.buttonOpenSiteBaseFolder.UseVisualStyleBackColor = false;
            this.buttonOpenSiteBaseFolder.Click += new System.EventHandler(this.buttonOpenSiteBaseFolder_Click);
            // 
            // FormBlogPublish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 658);
            this.Controls.Add(this.buttonOpenSiteBaseFolder);
            this.Controls.Add(this.buttonCopyImages);
            this.Controls.Add(this.buttonAddUrlMutationGo);
            this.Controls.Add(this.listBoxUrlMutations);
            this.Controls.Add(this.textBoxUrlMutation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonMutationGo);
            this.Controls.Add(this.listBoxMutations);
            this.Controls.Add(this.textBoxMutation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonBrowseForPublishDirectory);
            this.Controls.Add(this.textBoxPublishDirectory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPublish);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxPagesToPublish);
            this.Controls.Add(this.buttonBrowsePublishPageLink);
            this.Controls.Add(this.textBoxAddPublishPageLink);
            this.Controls.Add(this.label2);
            this.Name = "FormBlogPublish";
            this.Text = "[ m o t h ] Blog Publish";
            this.Load += new System.EventHandler(this.FormBlogPublish_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxPagesToPublish;
        private System.Windows.Forms.Button buttonBrowsePublishPageLink;
        private System.Windows.Forms.TextBox textBoxAddPublishPageLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonPublish;
        private System.Windows.Forms.TextBox textBoxPublishDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBrowseForPublishDirectory;
        private System.Windows.Forms.ListBox listBoxMutations;
        private System.Windows.Forms.TextBox textBoxMutation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonMutationGo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonAddUrlMutationGo;
        private System.Windows.Forms.ListBox listBoxUrlMutations;
        private System.Windows.Forms.TextBox textBoxUrlMutation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCopyImages;
        private System.Windows.Forms.Button buttonOpenSiteBaseFolder;
    }
}