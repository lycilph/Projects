using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ThreadedCategorizationTest
{
    public class Category
    {
        public Regex Pattern { get; private set; }

        public string Name { get; set; }
        public List<Category> Categories { get; private set; }
        public List<string> Items { get; private set; }

        public Category(string name, string pattern)
        {
            Name = name;
            Categories = new List<Category>();
            Items = new List<string>();

            Pattern = new Regex(pattern);
        }

        public void Reset()
        {
            Items.Clear();

            foreach (Category c in Categories)
                c.Reset();
        }

        public bool Match(string s)
        {
            return Pattern.IsMatch(s);
        }
    }
}
