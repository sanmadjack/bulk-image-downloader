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
using bulk_image_downloader.ImageSources;

namespace bulk_image_downloader {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        DownloadManager manager;
        public MainWindow() {
            InitializeComponent();
            manager = new DownloadManager();
            lstDownloadables.DataContext = manager;
            lstDownloadables.ItemsSource = manager;
            manager.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            AImageSource source = null;
            try {
                Uri url = new Uri(txtURL.Text);


                switch (cboUrlType.SelectedIndex) {
                    case 0:
                        source = new ShimmieImageSource(url);
                        break;
                    default:
                        throw new Exception("URL Type not supported");
                }


                source.Start();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message);
            }


        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            manager.Stop();
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e) {
            this.manager.ClearAllDownloads();
        }

        private void btnRetryFailed_Click(object sender, RoutedEventArgs e) {

        }

        private void btnClearCompleted_Click(object sender, RoutedEventArgs e) {
            manager.ClearCompleted();
        }
    }
}
