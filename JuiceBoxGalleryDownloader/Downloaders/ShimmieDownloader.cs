using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace JuiceBoxGalleryDownloader.Downloaders {
    public class ShimmieDownloader: ADownloader {
        public ShimmieDownloader(List<string> urls, string download_dir): base(urls, download_dir) {

        }

        private Regex address_regex = new Regex(@"(.+)/post/list/([^/]+/)?(\d+)");
        private Regex page_nav_regex = new Regex(@"/post/list/[^/]+/(\d+)");

        #region IImageListSource Members


        override protected List<string> GetImages(string url) {
            List<string> output = new List<string>();

            if (!address_regex.IsMatch(url)) {
                throw new Exception("Shimmie URL not understood");
            }
            MatchCollection address_matches = address_regex.Matches(url);
            string address_root = address_matches[0].Groups[1].Value;

            long max_page_found = 0;
            Dictionary<long, string> pages = new Dictionary<long,string>();

            bool new_max_found = true;

            while (new_max_found) {
                new_max_found = false;
                string primer_page = GetWebPageContents(url);
                MatchCollection matches = page_nav_regex.Matches(primer_page);
                foreach (Match match in matches) {
                    long test = 0;
                    
                    string value = match.Groups[1].Value;
                    if (Int64.TryParse(value, out test)) {
                        if (test > max_page_found) {
                            max_page_found = test;
                            new_max_found = true;
                            url = address_root + match.Value;
                        }
                    }
                }
            }

            worker.ReportProgress(current_progress, "Found " + max_page_found.ToString() + " pages");

            return output;
        }

        #endregion



    }
}
