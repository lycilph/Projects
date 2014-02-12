using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LoadImageAsync
{
    public class AsyncImage : ObservableObject
    {
        private static Random rnd = new Random();

        private Uri placeholder;
        private Uri final;

        public AsyncImage(Uri final, Uri placeholder)
        {
            this.final = final;
            this.placeholder = placeholder;

            Source = new BitmapImage(placeholder);

            Task.Run(() =>
            {
                BitmapImage img = new BitmapImage();
                using (WebClient client = new WebClient())
                {
                    var data = client.DownloadData(final);
                    using(var stream = new MemoryStream(data))
                    {
                        img.BeginInit();
                        img.StreamSource = stream;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();

                        img.Freeze();
                    }
                }

                return img;
            })
            .ContinueWith(parent =>
            {
                Source = parent.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private ImageSource source = null;
        public ImageSource Source
        {
            get { return source; }
            set
            {
                if (source != value)
                {
                    source = value;
                    NotifyPropertyChanged("Source");
                }
            }
        }
    }
}
