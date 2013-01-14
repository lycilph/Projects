using System;
using System.Collections.Generic;

namespace CategorizationEngine
{
    public class Post : ObservableObject
    {
        public Account Account { get; set; }

        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }

        public List<Category> Categories { get; private set; }

        public Post()
        {
            Categories = new List<Category>();
        }
    }
}
