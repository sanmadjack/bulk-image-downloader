using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace JuiceBoxGalleryDownloader.Downloaders {
    public abstract class ADownloader {
        private List<string> urls;
        protected string download_dir;
        public BackgroundWorker worker = new BackgroundWorker();

        abstract protected List<String> GetImages(string url);

        
        public ADownloader(List<string> urls, string download_dir) {
            this.urls = urls;
            worker.WorkerReportsProgress = true;
            worker.DoWork += backgroundWorker1_DoWork;
            this.download_dir = download_dir;
        }

        protected int current_progress;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
                       
            for (int i = 0; i < urls.Count; i++) {
                string url = urls[i];
                current_progress = (i / urls.Count) * 100;
                worker.ReportProgress(current_progress, "urls: Processing URL " + url);

                List<String> images = GetImages(url);
                using (WebClient wc = PrepareClient()) {
                    for (int j = 0; j < images.Count; j++) {
                        string image = images[j];
                        Uri uri = new Uri(image);
                        //if (uri.IsFile) {
                        string filename = System.IO.Path.GetFileName(uri.LocalPath);
                        wc.DownloadFile(image, Path.Combine(download_dir, filename));
                        current_progress = (j / images.Count) * 100;
                        worker.ReportProgress(current_progress, "images: Downloading Image" + image);
                        //}
                    }
                }


            }
        }



        protected string GetWebPageContents(string url) {
            using (WebClient wc = PrepareClient()) {
                using (MemoryStream stream = new MemoryStream(wc.DownloadData(url))) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        protected static WebClient PrepareClient() {
            WebClient wc = new WebClient();

            if(!string.IsNullOrEmpty(Properties.Settings.Default.SOCKS_Proxy_Host)) {
                wc.Proxy = new WebProxy(Properties.Settings.Default.SOCKS_Proxy_Host,Properties.Settings.Default.SOCKS_Proxy_Port);
            }

            return wc;

        }
    }

}
