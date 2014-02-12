namespace Image_Loader
{
    public class Album
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public string AlbumLink
        {
            get { return RemoveLastPart(Link); }
        }

        public string SlidesLink 
        {
            get { return RemoveLastPart(Link) + "slides/"; }
        }

        public string ThumbsLink
        {
            get { return RemoveLastPart(Link) + "thumbs/"; }
        }

        public Album(string name, string link)
        {
            Name = name;
            Link = link;
        }

        private static string RemoveLastPart(string url)
        {
            int i = url.LastIndexOf('/');
            return url.Substring(0, i+1);
        }
    }
}
