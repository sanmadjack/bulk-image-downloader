using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using bulk_image_downloader.ImageSources;

namespace bulk_image_downloader {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        DownloadManager manager;

        private String download_dir;

        public MainWindow() {
            InitializeComponent();
        }

        private void DaWindow_Loaded(object sender, RoutedEventArgs e) {
            try {
                manager = new DownloadManager();
                lstDownloadables.DataContext = manager;
                lstDownloadables.ItemsSource = manager;
                inputMaxDownloads.DataContext = manager;
                manager.Start();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error!");
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            AImageSource source = null;
            try {
                Uri url = new Uri(txtURL.Text);

                ComboBoxItem item = (ComboBoxItem)cboUrlType.SelectedItem;

                CommonOpenFileDialog dlg = new CommonOpenFileDialog();
                dlg.Title = "Select download folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = Properties.Settings.Default.LastDownloadDir;
                dlg.AddToMostRecentlyUsedList = false;
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = Properties.Settings.Default.LastDownloadDir;
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if(dlg.ShowDialog(this) == CommonFileDialogResult.Cancel) {
                    return;
                }

                download_dir = dlg.FileName;
                Properties.Settings.Default.LastDownloadDir = download_dir;
                Properties.Settings.Default.Save();

                switch (item.Tag.ToString()) {
                    case "shimmie":
                        source = new ShimmieImageSource(url);
                        break;
                    case "flickr":
                        source = new FlickrImageSource(url);
                        break;
                    case "juicebox":
                        //source = new JuiceBoxImageSource(url);
                        break;
                    case "nextgen":
                        source = new NextGENImageSource(url);
                        break;
                    case "deviantart":
                        source = new DeviantArtImageSource(url);
                        break;
                    default:
                        throw new Exception("URL Type not supported");
                }

                source.worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                source.Start();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message);
            }


        }

        private void DisableInterface() {
            SetInterface(false);
        }

        private void EnableInterface() {
            SetInterface(true);
        }

        private void SetInterface(bool enabled) {

            
        }

        void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            if(e.Error!= null)
            {
                MessageBox.Show(e.Error.Message);
                EnableInterface();
                return;
            }

            Dictionary<Uri, List<Uri>> images = (Dictionary<Uri, List<Uri>>)e.Result;
            foreach (Uri page in images.Keys)
            {
                foreach (Uri image in images[page])
                {
                    DownloadManager.DownloadImage(image, download_dir, page.ToString());
                }
            }
            DownloadManager.SaveAll();
            EnableInterface();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (manager != null) {
                manager.Stop();
            }
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e) {
            this.manager.ClearAllDownloads();
        }

        private void btnRetryFailed_Click(object sender, RoutedEventArgs e) {
            this.manager.RestartFailed();
        }

        private void btnClearCompleted_Click(object sender, RoutedEventArgs e) {
            manager.ClearCompleted();
        }

        private void btnPauseSelected_Click(object sender, RoutedEventArgs e) {
            foreach (Downloadable down in this.lstDownloadables.SelectedItems) {
                down.Pause();
            }
            DownloadManager.SaveAll();
        }

        private void chkAllPages_Checked(object sender, RoutedEventArgs e) {
            if (chkAllPages.IsChecked==true) {
                Properties.Settings.Default.DetectAdditionalPages = true;
                Properties.Settings.Default.Save();
            } else {
                Properties.Settings.Default.DetectAdditionalPages = false;
                Properties.Settings.Default.Save();
            }
        }

    }
}
