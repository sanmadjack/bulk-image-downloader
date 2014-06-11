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
    public abstract class AImageSource {
        protected Uri url;
        protected string download_dir;

        private Thread thread;

        public AImageSource(Uri url) {
            this.url = url;
            thread = new Thread(ProcessImages);
        }

        abstract protected void ProcessImages();

        public void Start() {
            thread.Start();
        }




    }

}
