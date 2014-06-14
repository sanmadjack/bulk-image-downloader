using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace bulk_image_downloader.ImageSources {
    public class ShimmieImageSource : AImageSource {

        private static Regex address_regex = new Regex(@"((.+)/post/list/[^/]+/?)(\d+)");
        private static Regex page_nav_regex = new Regex(@"/post/list/[^/]+/(\d+)");
        private static Regex images_regex = new Regex("class='[^'\"]+' href='(/post/view/[\\d]+)'");
        private static Regex image_regex = new Regex("http://.+/_images/[^'\"]+");

        private string address_root;
        private string query_root;

        public ShimmieImageSource(Uri url, string download_dir)
            : base(url, download_dir) {

            if (!address_regex.IsMatch(url.ToString())) {
                throw new Exception("Shimmie URL not understood");
            }
            MatchCollection address_matches = address_regex.Matches(url.ToString());
            address_root = address_matches[0].Groups[2].Value;
            query_root = address_matches[0].Groups[1].Value;

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

        


        override protected void ProcessImages() {


            if (!Properties.Settings.Default.DetectAdditionalPages) {
                DetectImages(new Uri(this.url.ToString()));
            } else {


                bool new_max_found = true;
                int total_pages = 0;

                string test_url = url.ToString();

                while (new_max_found) {
                    IfPausedWaitUntilUnPaused();
                    string primer_page = GetPageContents(new Uri(test_url));
                    new_max_found = false;

                    int test = GetHighestPageNumber(primer_page);
                    if (test > total_pages) {
                        total_pages = test;
                        new_max_found = true;
                        test_url = query_root + total_pages;
                    }
                }


                //(.+)/post/list/([^/]+/)?(\d+)
                for (int i = 1; i <= total_pages; i++) {
                    IfPausedWaitUntilUnPaused();

                    test_url = query_root + i.ToString();

                    DetectImages(new Uri(test_url));
                }
            }
        }




        private void DetectImages(Uri page_url) {
            string page_contents = GetPageContents(page_url);
            MatchCollection image_matches = images_regex.Matches(page_contents);
            foreach (Match image_match in image_matches) {
                IfPausedWaitUntilUnPaused();

                GroupCollection groups = image_match.Groups;
                Group group = groups[0];

                string page_content = GetPageContents(new Uri(address_root + image_match.Groups[1].Value));

                if (image_regex.IsMatch(page_content)) {
                    DownloadManager.DownloadImage(new Uri(image_regex.Match(page_content).Value),this.download_dir, page_url.ToString());
                }
            }
        }



    }
}
