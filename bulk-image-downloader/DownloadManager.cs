using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
namespace bulk_image_downloader {

    class DownloadManager : ObservableCollection<Downloadable> {

        private Thread supervisor_thread = new Thread(Supervise);

        public static string DownloadDir {
            get {
                return Properties.Settings.Default.DownloadDir;
            }
            set {
                Properties.Settings.Default.DownloadDir = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool Overwrite = false;

        private static bool StopTheMadness = false;

        private static List<string> locker = new List<string>();

        private static DownloadManager manager;

        public int DownloadProgress {
            get {
                return 50;
            }
        }

        public static int MaxConcurrentDownloads = 3;

        public DownloadManager() {
            manager = this;
            if (Properties.Settings.Default.Downloadables == null) {
                Properties.Settings.Default.Downloadables = new System.Collections.Specialized.StringCollection();
            } else {
                Downloadable down = null;
                foreach (string line in Properties.Settings.Default.Downloadables) {
                    down = new Downloadable(line);
                    this.Add(down);
                }
            }

        }

        public void Start() {
            StopTheMadness = false;
            supervisor_thread.Start();
        }

        public void Stop() {
            StopTheMadness = true;
        }

        private static void Supervise() {
            while (!StopTheMadness) {
                lock (manager) {
                    int downloading_count = 0;
                    for (int i = 0; i < manager.Count; i++) {
                        if (manager[i].State == DownloadState.Downloading) {
                            downloading_count++;
                        }
                    }
                    for (int i = 0; i < manager.Count; i++) {
                        if ((downloading_count < MaxConcurrentDownloads || manager[i].Type == DownloadType.Text) && manager[i].State == DownloadState.Pending) {
                            manager[i].Download();
                            downloading_count++;
                        }

                    }
                }
                Thread.Sleep(500);
            }
        }

        public static void DownloadImage(Uri url) {
            AddDownloadable(url, DownloadAction.SaveToFile, DownloadType.Binary);
        }

        public static Downloadable AddDownloadable(Uri url, DownloadAction action, DownloadType type) {
            Downloadable down = new Downloadable(url);
            down.Type = type;
            down.Action = action;
            App.Current.Dispatcher.Invoke((Action)(() => {
                lock (manager) {
                    manager.Add(down);
                }
            }));

            if (action == DownloadAction.SaveToFile) {
                Properties.Settings.Default.Downloadables.Add(down.ToString());
                Properties.Settings.Default.Save();
            }

            return down;
        }

        public static String GetWebPageContents(Uri url) {
            Downloadable down = AddDownloadable(url, DownloadAction.SaveToMemory, DownloadType.Text);

            while (down.State < DownloadState.Complete) {
                Thread.Sleep(500);
            }

            App.Current.Dispatcher.Invoke((Action)(() => {
                lock (manager) {
                    manager.Remove(down);
                }
            }));


            if (down.State == DownloadState.Complete) {
                return down.Data.ToString();
            } else {
                return "";
            }

        }

        public void ClearAllDownloads() {
            this.Clear();
            Properties.Settings.Default.Downloadables.Clear();
            Properties.Settings.Default.Save();

        }

        public void ClearCompleted() {
            lock (manager) {
                for (int i = 0; i < manager.Count; i++) {
                    if (manager[i].State == DownloadState.Complete || manager[i].State == DownloadState.Skipped) {
                        manager.RemoveAt(i);
                        i--;
                    }
                }
            }
            SaveAll();
        }

        public static void SaveAll() {
            lock(manager) {
                Properties.Settings.Default.Downloadables.Clear();
                for (int i = 0; i < manager.Count; i++) {
                    Properties.Settings.Default.Downloadables.Add(manager[i].ToString());
                }
                Properties.Settings.Default.Save();
            }
        }

        public void RestartFailed() {
            lock (manager) {
                for (int i = 0; i < manager.Count; i++) {
                    if (manager[i].State == DownloadState.Error) {
                        manager[i].Reset();
                    }
                }
            }
            SaveAll();
        }

    }
}
