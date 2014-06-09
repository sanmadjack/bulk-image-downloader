using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using JuiceBoxGalleryDownloader.Downloaders;

namespace JuiceBoxGalleryDownloader {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            if(!string.IsNullOrWhiteSpace(Properties.Settings.Default.DownloadFolder)) {
                if (!Directory.Exists(Properties.Settings.Default.DownloadFolder)) {
                    Properties.Settings.Default.DownloadFolder = "";
                    Properties.Settings.Default.Save();
                } else {
                    this.dirTxt.Text = Properties.Settings.Default.DownloadFolder;
                }
            }


        }

        private void checkEnableds() {
            goBtn.Enabled = !string.IsNullOrWhiteSpace(dirTxt.Text) && ! string.IsNullOrWhiteSpace(urlsTxt.Text);
        }

        private void dirTxt_TextChanged(object sender, EventArgs e) {
            checkEnableds();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.folderBrowserDialog1.SelectedPath = dirTxt.Text;
            if (this.folderBrowserDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                dirTxt.Text = this.folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.DownloadFolder = dirTxt.Text;
                Properties.Settings.Default.Save();
            } else {

            }
        }

        private void urlsTxt_TextChanged(object sender, EventArgs e) {
            checkEnableds();
        }



        private void goBtn_Click(object sender, EventArgs e) {
            try {
                goBtn.Enabled = false;
                button2.Enabled = false;
                urlsTxt.Enabled = false;
                outputTxt.Text = "";
                List<string> urls = new List<string>();
                urls.AddRange(urlsTxt.Text.Split('\n'));

                ADownloader downloader = null;

                if (rdoShimmie.Checked) {
                    downloader = new ShimmieDownloader(urls, dirTxt.Text);
                } else if (rdoJuiceBox.Checked) {
                    downloader = new JuiceBoxDownloader(urls, dirTxt.Text);
                } else {
                    throw new NotSupportedException("No supported gallery type selected.");
                }

                downloader.worker.ProgressChanged += backgroundWorker1_ProgressChanged;
                downloader.worker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

                downloader.worker.RunWorkerAsync();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            string text = e.UserState.ToString();
            if (text.StartsWith("urls: ")) {
                filesProg.Value = e.ProgressPercentage;
                text = text.Substring(6);
            } else if (text.StartsWith("images: ")) {
                urlsProg.Value = e.ProgressPercentage;
                text = text.Substring(8);
            }
            
                outputTxt.Text += e.UserState.ToString() + Environment.NewLine;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null) {
                MessageBox.Show(this, e.Error.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                outputTxt.Text += e.Error.Message + Environment.NewLine;
            }
            MessageBox.Show(this, "Done!");
            goBtn.Enabled = true;
            button2.Enabled = true;
            urlsTxt.Enabled = true;
        }
    }
}
