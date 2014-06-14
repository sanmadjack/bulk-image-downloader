using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bulk_image_downloader.ImageSources {
    public class NextGENImageSource: AImageSource {
        public NextGENImageSource(Uri url, string download_dir)
            : base(url, download_dir) {

        }

        private static Regex page_nav_regex = new Regex(@"http.+?/gallery/page/(\d+)");

        private static Regex images_regex = new Regex(@"['""](http.+?/wp-content/gallery/[^/]+/[^'""/]+)['""]");

        override protected void ProcessImages() {



        }

        private void DetectImages(Uri page_url) {
            string page_contents = GetPageContents(page_url);
            MatchCollection image_matches = images_regex.Matches(page_contents);
            foreach (Match image_match in image_matches) {
                IfPausedWaitUntilUnPaused();

                GroupCollection groups = image_match.Groups;
                Group group = groups[0];

                DownloadManager.DownloadImage(new Uri(image_match.Groups[1].Value), this.download_dir, page_url.ToString());
            }
        }


        private static int GetHighestPageNumber(string page_contents) {
            int total_pages = -1;
            MatchCollection matches = page_nav_regex.Matches(page_contents);
            foreach (Match match in matches) {
                int test = -1;

                string value = match.Groups[1].Value;
                if (Int32.TryParse(value, out test)) {
                    if (test > total_pages) {
                        total_pages = test;
                    }
                }
            }
            return total_pages;
        }
    }
}
