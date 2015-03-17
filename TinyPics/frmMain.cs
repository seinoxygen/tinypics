using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TinyPics.Properties;
using System.Threading;

namespace TinyPic
{
    public partial class TinyPics : Form
    {
        private string _apikey;
        private string _savepath;

        private Thread uploadThread;

        public TinyPics()
        {
            InitializeComponent();

            this._apikey = Settings.Default.ApiKey;
            this._savepath = Settings.Default.SavePath;

            this.Size = new System.Drawing.Size(292, 227);
            
            pnlUpload.DragEnter += new DragEventHandler(pnlUpload_DragEnter);
            pnlUpload.DragDrop += new DragEventHandler(pnlUpload_DragDrop);

            pnlSettings.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void pnlUpload_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void pnlUpload_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                uploadThread = new Thread(() => UploadThread(file));
                uploadThread.Start();
            } 
        }

        public void UploadThread(string file)
        {
            TinyPic.TinyPicApi TinyPic = new TinyPic.TinyPicApi(this._apikey, this._savepath);
            TinyPic.Upload(file);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtApiKey.Text = Settings.Default.ApiKey;
            txtSavePath.Text = Settings.Default.SavePath;

            pnlUpload.Visible = false;
            pnlAbout.Visible = false;
            pnlSettings.Location = new System.Drawing.Point(12, 27);
            pnlSettings.Visible = true;
        }

        private void compressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlSettings.Visible = false;
            pnlAbout.Visible = false;
            pnlUpload.Visible = true;            
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            this._apikey = txtApiKey.Text;
            this._savepath = txtSavePath.Text;

            Settings.Default.ApiKey = txtApiKey.Text;
            Settings.Default.SavePath = txtSavePath.Text;
            Settings.Default.Save();

            pnlSettings.Visible = false;
            pnlAbout.Visible = false;
            pnlUpload.Visible = true;        
        }

        private void lnkObtainApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://tinypng.com/developers");
        }

        private void btnBrowseSavePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowDialog();
            txtSavePath.Text = browse.SelectedPath;
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=35QQNQ6WWRR8C");
        }

        private void checkNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/seinoxygen/tinypics");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picIcon.Image = Bitmap.FromHicon(new Icon(this.Icon, new Size(48, 48)).Handle);

            pnlUpload.Visible = false;
            pnlSettings.Visible = false;
            pnlAbout.Location = new System.Drawing.Point(12, 27);
            pnlAbout.Visible = true;
        }
    }
}
