using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace JuiceBoxGalleryDownloader.Downloaders {
    public class JuiceBoxDownloader: ADownloader {


        public JuiceBoxDownloader(List<string> urls, string download_dir)
            : base(urls, download_dir) {
        }

        #region IImageListSource Members

        override protected List<string> GetImages(string url) {
            using (WebClient wc = PrepareClient()) {
                JuiceBoxConfigFile config_file;
                using (MemoryStream stream = new MemoryStream(wc.DownloadData(url))) {
                    config_file = new JuiceBoxConfigFile(stream);
                }
                return config_file.GetImageURLS();
            }
        }

        #endregion
    }
}
