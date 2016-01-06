﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;

namespace bulk_image_downloader {

    class DownloadManager : ObservableCollection<Downloadable> {

        private Thread supervisor_thread = new Thread(Supervise);

        //public static string DownloadDir {
        //    get {
        //        return Properties.Settings.Default.DownloadDir;
        //    }
        //    set {
        //        Properties.Settings.Default.DownloadDir = value;
        //        Properties.Settings.Default.Save();
        //    }
        //}
        public static bool Overwrite = false;

        private static bool StopTheMadness = false;

        private static List<string> locker = new List<string>();

        private static DownloadManager manager;

        public int DownloadProgress {
            get {
                return 50;
            }
        }

        public static int MaxConcurrentDownloads {
            get {
                return Properties.Settings.Default.MaxConcurrentDownloads;
            }
            set {
                Properties.Settings.Default.MaxConcurrentDownloads = value;
                Properties.Settings.Default.Save();
            }
        }

        public DownloadManager() {
            manager = this;
                if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.Downloadables)) {
                    try {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Properties.Settings.Default.Downloadables);

                        if (doc.GetElementsByTagName("downloadables").Count > 0) {
                            XmlElement ele = (XmlElement)doc.GetElementsByTagName("downloadables")[0];
                            lock (manager) {
                                Downloadable down = null;
                                foreach (XmlElement dele in ele.GetElementsByTagName("downloadable")) {
                                    down = new Downloadable(dele);
                                    this.Add(down);
                                }
                            }
                        }
                    } catch { }
                }
            SaveAll();

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
                            manager[i].Start();
                            downloading_count++;
                        }

                    }
                }
                Thread.Sleep(50);
            }
        }

        public static void DownloadImage(Uri url, string download_dir, string source) {
            AddDownloadable(url, download_dir, source, DownloadType.Binary);
        }

        public static Downloadable AddDownloadable(Uri url, string download_dir, string source, DownloadType type) {
            Downloadable down = new Downloadable(url, download_dir);
            down.Type = type;
            down.Source = source;
            App.Current.Dispatcher.Invoke((Action)(() => {
                lock (manager) {
                    manager.Add(down);
                }
            }));

            return down;
        }

        //public static String GetWebPageContents(Uri url) {
        //    Downloadable down = AddDownloadable(url, "", DownloadType.Text);

        //    while (down.State < DownloadState.Complete) {
        //        Thread.Sleep(500);
        //    }

        //    App.Current.Dispatcher.Invoke((Action)(() => {
        //        lock (manager) {
        //            manager.Remove(down);
        //        }
        //    }));


        //    if (down.State == DownloadState.Complete) {
        //        return down.Data.ToString();
        //    } else {
        //        return "";
        //    }

        //}

        public void ClearAllDownloads() {
            lock (manager) {
                for (int i = 0; i < manager.Count; i++) {
                    try {
                        manager[i].Pause();
                    } catch { }
                }
                this.Clear();
            }
            SaveAll();
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
            lock (manager) {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement downloadables = doc.CreateElement("downloadables");

                doc.AppendChild(downloadables);

                for (int i = 0; i < manager.Count; i++) {
                    downloadables.AppendChild(manager[i].CreateElement(doc));
                }

                using (var stringWriter = new StringWriter()) {
                    using (var xmlTextWriter = XmlWriter.Create(stringWriter)) {
                        doc.WriteTo(xmlTextWriter);
                        xmlTextWriter.Flush();
                        Properties.Settings.Default.Downloadables = stringWriter.GetStringBuilder().ToString();
                    }
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
