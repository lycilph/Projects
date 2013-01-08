using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CategorizationEngine
{
    public class Wordlist
    {
        private static readonly Wordlist instance = new Wordlist();
        private static readonly Random rand = new Random();

        private Wordlist()
        {
            Words = new List<string>();

            using(var reader = new StreamReader("american-words.35"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Words.Add(line);
                }
            }
        }

        public static Wordlist Instance
        {
            get { return instance; }
        }

        public List<string> Words { get; private set; }

        public string Random()
        {
            var index = rand.Next(0, Words.Count);
            return Words[index];
        }

        public string Random(int n)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < n; i++)
            {
                var index = rand.Next(0, Words.Count);
                sb.Append(Words[index] + " ");
            }
            return sb.ToString();
        }
    }
}
