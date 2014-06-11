using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace bulk_image_downloader {
    enum DownloadState {
        Pending,
        Paused,
        Downloading,
        Complete,
        Skipped,
        Error
    }
    enum DownloadType {
        Binary,
        Text
    }
    enum DownloadAction {
        SaveToFile,
        SaveToMemory
    }

    class Downloadable : INotifyPropertyChanged {
        private Thread download_thread;

        private const char seperator = '«';

        public DownloadAction Action { get; set; }

        private DownloadState _State = DownloadState.Pending;
        public DownloadState State {
            get {
                return _State;
            }

            protected set {
                _State = value;
                NotifyPropertyChanged("State");
                NotifyPropertyChanged("StateText");
            }
        }
        public string StateText {
            get {
                return State.ToString();
            }
        }

        private Exception _except = null;
        public Exception Exception {
            get {
                return _except;
            }
            protected set {
                _except = value;
                NotifyPropertyChanged("Exception");
                NotifyPropertyChanged("Error");
            }

        }
        public String Error {
            get {
                if (_except != null) {
                    return _except.Message;
                } else {
                    return "";
                }
            }
        }

        private long _length = -1;
        public long Length {
            get {
                return _length;
            }

            protected set {
                _length = value;
                NotifyPropertyChanged("Length");
                NotifyPropertyChanged("Progress");
            }
        }


        private long _downloaded_length = 0;
        public long DownloadedLength {
            get {
                return _downloaded_length;
            }
            protected set {
                _downloaded_length = value;
                NotifyPropertyChanged("DownloadedLength");
                NotifyPropertyChanged("Progress");
            }
        }

        public int Progress {
            get {
                if (Length <= 0) {
                    return 0;
                }
                double output = 0;
                output += DownloadedLength;
                output /= Length;
                output *= 100;
                return Convert.ToInt32(output);
            }
        }

        private int _attempts = 0;
        public int Attempts {
            get {
                return _attempts;
            }
            protected set {
                _attempts = value;
                NotifyPropertyChanged("Attempts");
            }
        }

        public int MaxAttempts { get; set; }

        public DownloadType Type = DownloadType.Binary;

        public string FileName { get; protected set; }
        public Uri URL { get; protected set; }

        public Object Data { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public Downloadable(Uri url)
{
            this.URL = url;
            FileName = Path.GetFileName(url.ToString());
            download_thread = new Thread(DownloadThread);
            MaxAttempts = 5;
        }

        public Downloadable(String saved_downloadable)  {
            String[] values= saved_downloadable.Split(seperator);
            this.URL = new Uri(values[0]);

            Enum.TryParse<DownloadState>(values[1], out this._State);
            Enum.TryParse<DownloadType>(values[2], out this.Type);
            download_thread = new Thread(DownloadThread);
            MaxAttempts = 5;
        }

        private void NotifyPropertyChanged(String propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Download() {
            this.State = DownloadState.Downloading;
            this.download_thread.Start();
        }

        private WebClient client;

        private void DownloadThread() {
            try {
                if (client != null) {
                    if (client.IsBusy) {
                        throw new Exception("File is already downloading");
                    }
                }

                if(!DownloadManager.Overwrite&& File.Exists(this.GetDownloadPath())) {
                    this.State = DownloadState.Skipped;
                    return;
                }

                client = new WebClient();
                client.DownloadProgressChanged += wc_DownloadProgressChanged;
                client.DownloadDataCompleted += client_DownloadDataCompleted;
                client.DownloadStringCompleted += client_DownloadStringCompleted;

                switch (this.Type) {
                    case DownloadType.Binary:
                        client.DownloadDataAsync(this.URL);
                        break;
                    case DownloadType.Text:
                        client.DownloadStringAsync(this.URL);
                        break;
                }
                this.State = DownloadState.Downloading;
                Attempts = 1;
                return;

            } catch (Exception e) {
                this.Exception = e;
                this.State = DownloadState.Error;
            }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
            if (e.Error != null) {
                if (Attempts < MaxAttempts) {
                    Thread.Sleep(5000);
                    client.DownloadStringAsync(this.URL);
                    Attempts = Attempts + 1;
                } else {
                    this.Exception = e.Error;
                    this.State = DownloadState.Error;
                    try {
                        client.Dispose();
                    } catch (Exception) { }
                    client = null;
                }
                return;
            } 
            try {
                if (e.Cancelled) {
                    this.State = DownloadState.Paused;
                    return;
                }
                try {
                    this.Data = e.Result;

                    if (this.Action == DownloadAction.SaveToFile) {
                        if (!DownloadManager.Overwrite && File.Exists(this.GetDownloadPath())) {
                            this.State = DownloadState.Skipped;
                            return;
                        } else {
                            File.WriteAllText(this.GetDownloadPath(), e.Result);
                        }
                    }


                    this.State = DownloadState.Complete;
                } catch (Exception ex) {
                    this.Exception = ex;
                    this.State = DownloadState.Error;
                }
            } finally {
                try {
                    client.Dispose();
                } catch (Exception) { }
                client = null;
            }

        }

        void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
            if (e.Error != null) {
                if (Attempts < MaxAttempts) {
                    Thread.Sleep(5000);
                    client.DownloadDataAsync(this.URL);
                    Attempts = Attempts + 1;
                } else {
                    this.Exception = e.Error;
                    this.State = DownloadState.Error;
                    try {
                        client.Dispose();
                    } catch (Exception) { }
                    client = null;
                }
                return;
            }

            try {
                if (e.Cancelled) {
                    this.State = DownloadState.Paused;
                    return;
                }
                try {
                    this.Data = e.Result;

                    if (this.Action == DownloadAction.SaveToFile) {
                        if (!DownloadManager.Overwrite && File.Exists(this.GetDownloadPath())) {
                            this.State = DownloadState.Skipped;
                            return;
                        } else {
                            File.WriteAllBytes(this.GetDownloadPath(), e.Result);
                        }
                    }

                    this.State = DownloadState.Complete;

                } catch (Exception ex) {
                    this.Exception = ex;
                    this.State = DownloadState.Error;
                }
            } finally {
                try {
                    client.Dispose();
                } catch (Exception) { }
                client = null;
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            this.Length = e.TotalBytesToReceive;
            this.DownloadedLength = e.BytesReceived;
        }

        private string GetDownloadPath() {
            string filename = this.FileName;
            if (filename.Length > 248) {
                string ext = Path.GetExtension(filename);
                filename = filename.Substring(0, 248 - ext.Length - 1) + "." + ext;
            }

            return Path.Combine(DownloadManager.DownloadDir, filename);
        }

        private void FetchHeaderInfo() {
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create(this.URL);
            req.Method = "HEAD";
            using (System.Net.WebResponse resp = req.GetResponse()) {
                int ContentLength;
                if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength)) {
                    //Do something useful with ContentLength here 
                }
            }
        }

        protected static WebClient PrepareClient() {
            WebClient wc = new WebClient();

            //if (!string.IsNullOrEmpty(Properties.Settings.Default.SOCKS_Proxy_Host))
            //{
            //    wc.Proxy = new WebProxy(Properties.Settings.Default.SOCKS_Proxy_Host, Properties.Settings.Default.SOCKS_Proxy_Port);
            //}

            return wc;

        }

        public override string ToString() {
            StringBuilder output = new StringBuilder();
            output.Append(URL.ToString());
            output.Append(seperator);
            output.Append(this.StateText);
            output.Append(seperator);
            output.Append(this.Type.ToString());
            return output.ToString();
        }

        public void Reset() {
            this.State = DownloadState.Pending;
        }

    }
}
