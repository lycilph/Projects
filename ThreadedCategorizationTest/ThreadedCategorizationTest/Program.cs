using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeBits;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadedCategorizationTest
{
    class Program
    {
        public static int matches;

        static void Main(string[] args)
        {
            #region Data generation
            Console.WriteLine("Data generation");
            Stopwatch sw = Stopwatch.StartNew();

            List<string> items = GenerateItems(5000, 30);
            Category root = GenerateCategories(3, 20);
            root.Items.AddRange(items);

            sw.Stop();
            Console.WriteLine(string.Format("Data generation {0} ms", sw.ElapsedMilliseconds));
            #endregion

            #region Categorize
            Console.WriteLine("Sequential categorization");
            for (int i = 0; i < 5; i++)
            {
                root.Reset();
                root.Items.AddRange(items);
                matches = 0;

                sw = Stopwatch.StartNew();
                Categorize(root);
                sw.Stop();

                Console.WriteLine(string.Format("Categorization {0} ms, matches found {1}", sw.ElapsedMilliseconds, matches));
            }
            #endregion

            #region Parallel categories
            Console.WriteLine("Parallel categorization");
            for (int i = 0; i < 5; i++)
            {
                root.Reset();
                root.Items.AddRange(items);
                matches = 0;

                sw = Stopwatch.StartNew();
                ParallelCategorize(root);
                sw.Stop();

                Console.WriteLine(string.Format("Categorization {0} ms, matches found {1}", sw.ElapsedMilliseconds, matches));
            }
            #endregion

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void ForceRegexCompilation(Category root)
        {
            foreach (Category c in root.Categories)
            {
                c.Pattern.IsMatch("");
                ForceRegexCompilation(c);
            }
        }

        private static void Categorize(Category root)
        {
            // Match items for this level
            foreach (Category c in root.Categories)
            {
                foreach (string i in root.Items)
                {
                    if (c.Match(i))
                    {
                        matches++;
                        c.Items.Add(i);
                    }
                }
            }

            // Recurse through child categories
            foreach (Category c in root.Categories)
                Categorize(c);
        }

        private static void ParallelCategorize(Category root)
        {
            // Match items for this level
            Parallel.ForEach(root.Categories,
                (c) =>
                {
                    foreach (string i in root.Items)
                    {
                        if (c.Match(i))
                        {
                            Interlocked.Add(ref matches, 1);
                            c.Items.Add(i);
                        }
                    }
                });

            // Recurse through child categories
            Parallel.ForEach(root.Categories, (c) => ParallelCategorize(c));
        }

        #region Test data generation

        private static List<string> GenerateItems(int count, int length)
        {
            List<string> items = new List<string>();
            for (int i = 0; i < count; i++)
                items.Add(PasswordGenerator.Generate(length, PasswordCharacters.AllLetters));
            return items;
        }

        private static Category GenerateCategories(int levels, int count)
        {
            Category root = new Category("Root", "");
            return GenerateCategories(levels, count, root);
        }

        private static Category GenerateCategories(int levels, int count, Category parent)
        {
            if (levels == 0)
                return parent;

            for (int i = 0; i < count; i++)
            {
                string name = PasswordGenerator.Generate(5, PasswordCharacters.AllLetters);
                string pattern = PasswordGenerator.Generate(2, PasswordCharacters.AllLetters);
                Category child = new Category(name, pattern);
                parent.Categories.Add(child);
                GenerateCategories(levels - 1, count, child);
            }

            return parent;
        } 

        #endregion
    }
}
