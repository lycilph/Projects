using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Loader
{
    public static class Cache
    {
        private static string MAIN_URL = @"http://www.skovboernehave.dk/";
        private static string BASE_DIR = AppDomain.CurrentDomain.BaseDirectory;

        public static string ALBUM_LIST_URL = "Foto.htm";

        private static string GetCacheName(string url)
        {
            return url + ".txt";
        }

        public static Task<string> GetString(string url)
        {
            string cache_url = GetCacheName(url);

            if (File.Exists(cache_url))
                return Task.Factory.StartNew(() => File.ReadAllText(cache_url));
            else
            {

            }
        }
    }
}
