using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Image_Loader_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadImages();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static void LoadImages()
        {
            HashSet<string> images = new HashSet<string>();

            using (WebClient client = new WebClient())
            {
                //string str = client.DownloadString(@"http://www.skovboernehave.dk/Album/20131119Jager/index.html");
                //File.WriteAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\20131119Jager.txt", str);
                string str = File.ReadAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\20131119Jager.txt");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(str);

                var first_page = doc.DocumentNode.SelectNodes("//a[@href]")
                                                 .Select(l => l.Attributes["href"].Value)
                                                 .Where(a => a.Contains("slides"))
                                                 .First();
                var page = Path.GetFileName(first_page);
                Console.WriteLine("Parsing page " + page);

                while (ParsePage(page, images))
                {
                    page = Path.ChangeExtension(images.Last(), ".html");
                    Console.WriteLine("Parsing page " + page);
                }
            }

            Console.WriteLine("Downloading images");
            foreach (var image in images)
                DownloadImage(image);
            Console.WriteLine("Images found " + images.Count);
        }

        private static void DownloadImage(string image)
        {
            using (WebClient client = new WebClient())
            {
                string fileurl = @"http://www.skovboernehave.dk/Album/20131119Jager/thumbs/" + image;
                string filename = @"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\thumbs\" + image;
                Console.WriteLine("Downloading " + image);
                client.DownloadFile(fileurl, filename);
            }
        }

        private static bool ParsePage(string page, HashSet<string> images)
        {
            bool found_new_images = false;

            using (WebClient client = new WebClient())
            {
                string file = Path.GetFileNameWithoutExtension(page);
                //string str = client.DownloadString(@"http://www.skovboernehave.dk/Album/20131119Jager/slides/" + page);
                //File.WriteAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\" + file + ".txt", str);
                string str = File.ReadAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\" + file + ".txt");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(str);
                foreach (var link in doc.DocumentNode.SelectNodes("//img[@src]"))
                {
                    string link_ref = link.Attributes["src"].Value;
                    string filename = Path.GetFileName(link_ref);
                    string extension = Path.GetExtension(link_ref);

                    if (extension.ToLower() == ".jpg")
                    {
                        if (images.Add(filename))
                        {
                            found_new_images = true;
                            Console.WriteLine(filename);
                        }
                    }
                }
            }

            return found_new_images;
        }

        private static void LoadAlbumList()
        {
            List<string> albums = new List<string>();

            using (WebClient client = new WebClient())
            {
                //string str = client.DownloadString(@"http://www.skovboernehave.dk/Foto.htm");
                //File.WriteAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\album_list.txt", str);
                string str = File.ReadAllText(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\album_list.txt");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(str);
                foreach (var link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    // Replace runs of whitespace with single space
                    string link_text = Regex.Replace(link.InnerHtml, @"\s+", " ");
                    string link_ref = link.Attributes["href"].Value;

                    if (link_ref.Contains("Album"))
                    {
                        albums.Add(link_text + " " + link_ref);
                        Console.WriteLine(link_text + " " + link_ref);
                    }
                }
            }

            File.WriteAllLines(@"C:\Private\GitHub\Projects\ImageLoader\Image Loader Test\albums.txt", albums);
        }
    }
}
