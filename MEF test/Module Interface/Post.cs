using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Interface
{
    public class Post
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }

        public Post(DateTime Date, string Text)
        {
            this.Date = Date;
            this.Text = Text;
        }
    }
}
