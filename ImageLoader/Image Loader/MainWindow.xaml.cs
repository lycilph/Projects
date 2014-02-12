using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Image_Loader
{
    public partial class MainWindow
    {
        // Start with finished task to simplify handling later
        //private CancellationTokenSource cancellation_token_source = new CancellationTokenSource();
        //private Task load_task = Task.Delay(0);

        public bool LoadingAlbums
        {
            get { return (bool)GetValue(LoadingAlbumsProperty); }
            set { SetValue(LoadingAlbumsProperty, value); }
        }
        public static readonly DependencyProperty LoadingAlbumsProperty =
            DependencyProperty.Register("LoadingAlbums", typeof(bool), typeof(MainWindow), new PropertyMetadata(false, new PropertyChangedCallback(LoadingAlbumChanged)));
        private static void LoadingAlbumChanged(Object obj, DependencyPropertyChangedEventArgs e)
        {
            var win = obj as MainWindow;
            if (win != null)
            {
                win.Status = (win.LoadingAlbums ? "Loading albums" : "Loading albums done");
            }
        }

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        //public ObservableCollection<ImageViewModel> Images
        //{
        //    get { return (ObservableCollection<ImageViewModel>)GetValue(ImagesProperty); }
        //    set { SetValue(ImagesProperty, value); }
        //}
        //public static readonly DependencyProperty ImagesProperty =
        //    DependencyProperty.Register("Images", typeof(ObservableCollection<ImageViewModel>), typeof(MainWindow), new PropertyMetadata(null));
        
        public ObservableCollection<Album> Albums
        {
            get { return (ObservableCollection<Album>)GetValue(AlbumsProperty); }
            set { SetValue(AlbumsProperty, value); }
        }
        public static readonly DependencyProperty AlbumsProperty =
            DependencyProperty.Register("Albums", typeof(ObservableCollection<Album>), typeof(MainWindow), new PropertyMetadata(null));

        //public Album SelectedAlbum
        //{
        //    get { return (Album)GetValue(SelectedAlbumProperty); }
        //    set { SetValue(SelectedAlbumProperty, value); }
        //}
        //public static readonly DependencyProperty SelectedAlbumProperty =
        //    DependencyProperty.Register("SelectedAlbum", typeof(Album), typeof(MainWindow), new PropertyMetadata(null, new PropertyChangedCallback(SelectedAlbumChanged)));

        //private static void SelectedAlbumChanged(Object obj, DependencyPropertyChangedEventArgs e)
        //{
        //    var win = obj as MainWindow;
        //    if (win != null)
        //        win.LoadSelectedAlbum();
        //}

        public MainWindow()
        {
            InitializeComponent();
            Albums = new ObservableCollection<Album>();
            //Images = new ObservableCollection<ImageViewModel>();

            DataContext = this;
        }

        private async void WindowContentRendered(object sender, EventArgs e)
        {
            LoadingAlbums = true;

            Albums.Clear();
            
            string str = await Cache.GetString(Cache.ALBUM_LIST_URL);
            System.Diagnostics.Debug.Print(str);

            await Task.Delay(1000);

            LoadingAlbums = false;
        }

        //private async void LoadAlbums()
        //{
        //    LoadingAlbums = true;
        //    Albums.Clear();

        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    using (WebClient client = new WebClient())
        //    {
        //        string str = await client.DownloadStringTaskAsync(ALBUMS_URL);
        //        //string str = File.ReadAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\album_list.txt");

        //        HtmlDocument doc = new HtmlDocument();
        //        doc.LoadHtml(str);
        //        foreach (var link in doc.DocumentNode.SelectNodes("//a[@href]"))
        //        {
        //            // Replace runs of whitespace with single space
        //            string link_text = Regex.Replace(link.InnerHtml, @"\s+", " ").Trim();
        //            string link_ref = link.Attributes["href"].Value;

        //            if (link_ref.Contains("Album"))
        //                Albums.Add(new Album(link_text, MAIN_URL + link_ref));
        //        }
        //    }
        //    sw.Stop();
        //    System.Diagnostics.Debug.Print("Loading albums took {0} ms", sw.ElapsedMilliseconds);

        //    await Task.Delay(1000);

        //    if (Albums.Count > 0)
        //        SelectedAlbum = Albums.First();

        //    LoadingAlbums = false;
        //}

    //    private async void LoadSelectedAlbum()
    //    {
    //        System.Diagnostics.Debug.Print("Loading album " + SelectedAlbum.Name);

    //        // Cancel and wait for previous task
    //        if (!load_task.IsCompleted)
    //        {
    //            System.Diagnostics.Debug.Print("Cancelling load task");
    //            cancellation_token_source.Cancel();
    //            await load_task;
    //            System.Diagnostics.Debug.Print("Load task cancelled");
    //        }

    //        ClearImages();

    //        Album current_album = SelectedAlbum;
    //        TaskScheduler ui_context = TaskScheduler.FromCurrentSynchronizationContext();
    //        cancellation_token_source = new CancellationTokenSource();
    //        CancellationToken token = cancellation_token_source.Token;
    //        load_task = Task.Factory.StartNew(() =>
    //        {
    //            HashSet<String> image_names = new HashSet<string>();

    //            // Get images for selected album
    //            string page = GetFirstAlbumPage(current_album);
    //            while (page != string.Empty)
    //            {
    //                if (token.IsCancellationRequested)
    //                    break;

    //                System.Diagnostics.Debug.Print("Parsing page " + page);
    //                page = ParsePage(page, image_names);

    //                if (page != string.Empty)
    //                    page = current_album.SlidesLink + page;
    //            }

    //            // Download images from web or load from file
    //            foreach (var image in image_names)
    //            {
    //                Task.Factory.StartNew(() =>
    //                {
    //                    using (WebClient client = new WebClient())
    //                    {
    //                        string fileurl = current_album.ThumbsLink + image;
    //                        string filename = @"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\thumbs\" + image;
    //                        Console.WriteLine("Downloading " + fileurl);
    //                        client.DownloadFile(fileurl, filename);
    //                        return filename;
    //                    }

    //                    //string filename = @"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\images\" + image;
    //                    //for (int i = 0; i < 10; i++)
    //                    //{
    //                    //    if (token.IsCancellationRequested)
    //                    //        break;
    //                    //    Thread.Sleep(100);
    //                    //}
    //                    //return filename;
    //                }, token, TaskCreationOptions.AttachedToParent, TaskScheduler.Current)
    //                .ContinueWith((parent) =>
    //                {
    //                    if (!token.IsCancellationRequested)
    //                    {
    //                        var vm = new ImageViewModel(parent.Result);
    //                        vm.PropertyChanged += ImageViewModelPropertyChanged;
    //                        Images.Add(vm);
    //                    }
    //                }, token, TaskContinuationOptions.AttachedToParent, ui_context);
    //            }
    //        }, token)
    //        .ContinueWith((parent) =>
    //        {
    //            Status = Images.Count + " images loaded";
    //        }, ui_context);

    //        //IsBusy = true;
    //        //// Cancel and wait for previous task
    //        //if (!load_task.IsCompleted)
    //        //{
    //        //    System.Diagnostics.Debug.Print("Cancelling task");
    //        //    token_source.Cancel();
    //        //    await load_task;
    //        //    System.Diagnostics.Debug.Print("Task cancelled");
    //        //}

    //        //token_source = new CancellationTokenSource();
    //        //CancellationToken token = token_source.Token;
    //        //load_task = Task.Run(() =>
    //        //{
    //        //    for (int i = 0; i < 20; i++)
    //        //    {
    //        //        Thread.Sleep(100);
    //        //        if (token.IsCancellationRequested)
    //        //            return;
    //        //    }
    //        //}, token)
    //        //.ContinueWith((parent) =>
    //        //{
    //        //    if (token.IsCancellationRequested)
    //        //        System.Diagnostics.Debug.Print("Task was cancelled");
    //        //    else
    //        //    {
    //        //        System.Diagnostics.Debug.Print("Task was NOT cancelled");
    //        //        IsBusy = false;
    //        //    }
    //        //}, TaskScheduler.FromCurrentSynchronizationContext());




    //        //System.Diagnostics.Debug.Print("Parsing album " + SelectedAlbum.Link);
    //        //System.Diagnostics.Debug.Print(" - slides " + SelectedAlbum.SlidesLink);
    //        //System.Diagnostics.Debug.Print(" - thumbs " + SelectedAlbum.ThumbsLink);

    //        //// Find all image names
    //        //SelectedAlbum.ImageNames.Clear();
    //        //string page = await GetFirstAlbumPage(SelectedAlbum);
    //        //while (page != string.Empty)
    //        //{
    //        //    System.Diagnostics.Debug.Print("Parsing page " + page);
    //        //    page = await ParsePage(page, SelectedAlbum);
    //        //}

    //        //// Download images
    //        //ClearImages();
    //        //foreach (var image in SelectedAlbum.ImageNames)
    //        //    DownloadImage(SelectedAlbum, image);
    //    }

    //    private static string GetFirstAlbumPage(Album album)
    //    {
    //        String page = string.Empty;

    //        using (WebClient client = new WebClient())
    //        {
    //            string str = client.DownloadString(album.Link);
    //            //string str = File.ReadAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\20131119Jager.txt");

    //            HtmlDocument doc = new HtmlDocument();
    //            doc.LoadHtml(str);
    //            var first_page = doc.DocumentNode.SelectNodes("//a[@href]")
    //                                                .Select(l => l.Attributes["href"].Value)
    //                                                .Where(a => a.Contains("slides"))
    //                                                .First();

    //            page = album.AlbumLink + first_page;
    //        }

    //        return page;
    //    }

    //    private static string ParsePage(string page, HashSet<string> image_names)
    //    {
    //        string next_page = string.Empty;

    //        using (WebClient client = new WebClient())
    //        {
    //            string str = client.DownloadString(page);
    //            //string file = Path.GetFileNameWithoutExtension(page);
    //            //string str = File.ReadAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\" + file + ".txt");

    //            HtmlDocument doc = new HtmlDocument();
    //            doc.LoadHtml(str);
    //            foreach (var link in doc.DocumentNode.SelectNodes("//img[@src]"))
    //            {
    //                string link_ref = link.Attributes["src"].Value;
    //                string filename = Path.GetFileName(link_ref);
    //                string extension = Path.GetExtension(link_ref);

    //                if (extension.ToLower() == ".jpg" && image_names.Add(filename))
    //                    next_page = Path.ChangeExtension(filename, ".html");
    //            }
    //        }

    //        return next_page;
    //    }

    //    //private string DownloadImage(string image)
    //    //{
    //    //    Task<string>.Run(() =>
    //    //    {
    //    //        using (WebClient client = new WebClient())
    //    //        {
    //    //            string filename = @"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\images\" + image;
    //    //            Thread.Sleep(1000);
    //    //            return filename;
    //    //        }
    //    //    })
    //    //    .ContinueWith(parent =>
    //    //    {
    //    //        var vm = new ImageViewModel(parent.Result);
    //    //        vm.PropertyChanged += ImageViewModelPropertyChanged;
    //    //        Images.Add(vm);
    //    //    }, TaskScheduler.FromCurrentSynchronizationContext());
    //    //}

    //    private void ClearImages()
    //    {
    //        foreach (var vm in Images)
    //            vm.PropertyChanged -= ImageViewModelPropertyChanged;
    //        Images.Clear();
    //    }

    //    private void ImageViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        if (e.PropertyName == "Selected")
    //        {
    //            int selected_items = Images.Sum(vm => vm.Selected ? 1 : 0);
    //            if (selected_items == 0)
    //                Status = string.Empty;
    //            else if (selected_items == 1)
    //                Status = string.Format("1 item selected");
    //            else
    //                Status = string.Format("{0} items selected", selected_items);
    //        }
    //    }
    }
}
