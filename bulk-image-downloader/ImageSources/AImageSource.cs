using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace bulk_image_downloader.ImageSources {
    public abstract class AImageSource: INotifyPropertyChanged {
        protected Uri url;

        protected string download_dir;

        public BackgroundWorker worker;

        protected bool pause_work = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public AImageSource(Uri url, string download_dir) {
            this.url = url;
            this.download_dir = download_dir;
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e) {
            ProcessImages();
        }


        abstract protected void ProcessImages();

        public void Start() {
            if (worker.IsBusy) {
                pause_work = false;
            } else {
                worker.RunWorkerAsync();
            }
        }

        public void Pause() {
            pause_work = true;
        }

        public void Cancel() {
            worker.CancelAsync();
        }

        protected void NotifyPropertyChanged(String propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        protected void IfPausedWaitUntilUnPaused() {
            while (pause_work) {
                Thread.Sleep(500);
            }
        }

        protected static string GetPageContents(Uri url) {
            using(WebClient wc = new WebClient()) {
                for (int i = 0; i < 5; i++) {
                    try {
                        return wc.DownloadString(url);
                    } catch {
                        if (i == 4) {
                            throw;
                        }
                    }
                }
            }
            return "";
        }
    }

}
