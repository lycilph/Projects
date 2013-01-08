using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using ClosedXML.Excel;

namespace CategorizationEngine
{
    class Program
    {
        private static void Save(List<Post> posts, Category root)
        {
            var sw = Stopwatch.StartNew();

            var workbook = new XLWorkbook();

            var ws = workbook.AddWorksheet("Posts");
            for (var i = 0; i < posts.Count; i++)
            {
                var p = posts[i];
                ws.Row(i + 1).Cell(1).SetValue(p.Date);
                ws.Row(i + 1).Cell(2).SetValue(p.Text);
                ws.Row(i + 1).Cell(3).SetValue(p.Value);

                for (var j = 0; j < p.Categories.Count; j++)
                {
                    var c = p.Categories[j];
                    ws.Row(i + 1).Cell(4 + j).SetValue(c.Name);
                }
            }

            ws = workbook.AddWorksheet("Categories");
            var categories = Category.Flatten(root);
            for (var i = 0; i < categories.Count; i++)
            {
                var c = categories[i];
                ws.Row(i + 1).Cell(1).SetValue(c.Name);
            }
            
            workbook.SaveAs("Output.xlsx");

            sw.Stop();
            Console.WriteLine("Elapsed (save): " + sw.ElapsedMilliseconds);
        }

        private static void Print(Category category, int indent = 0)
        {
            var str = category.ToString();
            Console.WriteLine(str.PadLeft(str.Length + indent*2));
            foreach (var c in category.Categories)
                Print(c, indent+1);
        }

        static void Main(string[] args)
        {
            var posts = Post.Random(10000);

            var root = new Category("Root");

            var c1 = new Category("C1");
            var c11 = new Category("C1.1");
            c11.Patterns.Add(new RegexPattern(new Regex(Wordlist.Instance.Words[0])));
            c1.Categories.Add(c11);
            var c12 = new Category("C1.2");
            c12.Patterns.Add(new RegexPattern(new Regex(Wordlist.Instance.Words[100])));
            c1.Categories.Add(c12);
            var c13 = new Category("C1.3");
            c13.Patterns.Add(new RegexPattern(new Regex(Wordlist.Instance.Words[200])));
            c1.Categories.Add(c13);
            root.Categories.Add(c1);

            var c2 = new Category("C2");
            var c21 = new Category("C2.1");
            c21.Patterns.Add(new DatePattern(){Date = new DateTime(2008,1,1),Operator = DateOperator.Before});
            c2.Categories.Add(c21);
            var c22 = new Category("C2.2");
            c22.Patterns.Add(new DatePattern() { Date = new DateTime(2008, 1, 1), Operator = DateOperator.Equal });
            c2.Categories.Add(c22);
            var c23 = new Category("C2.3");
            c23.Patterns.Add(new DatePattern() { Date = new DateTime(2008, 1, 1), Operator = DateOperator.After });
            c2.Categories.Add(c23);
            root.Categories.Add(c2);

            var c3 = new Category("C3");
            var c31 = new Category("C3.1");
            c31.Patterns.Add(new ValuePattern(){Value = 542, Operator = ValueOperator.Less});
            c3.Categories.Add(c31);
            var c32 = new Category("C3.2");
            c32.Patterns.Add(new ValuePattern() { Value = 542, Operator = ValueOperator.Equal });
            c3.Categories.Add(c32);
            var c33 = new Category("C3.3");
            c33.Patterns.Add(new ValuePattern() { Value = 542, Operator = ValueOperator.Greater });
            c3.Categories.Add(c33);
            root.Categories.Add(c3);

            var c4 = new Category("C4");
            var c4p = new HashPattern();
            c4p.Set.Add(posts[0]);
            c4p.Set.Add(posts[17]);
            c4p.Set.Add(posts[23]);
            c4p.Set.Add(posts[42]);
            c4.Patterns.Add(c4p);
            root.Categories.Add(c4);

            // Print hierarchical
            Print(root);
            // Print flattened
            var flattened = Category.Flatten(root).Where(c => c.Patterns.Count > 0);
            foreach (var category in flattened)
                Console.WriteLine(category);

            // Categories all posts
            var sw = Stopwatch.StartNew();
            foreach (var post in posts)
            {
                foreach (var category in flattened)
                {
                    if (category.IsMatch(post))
                    {
                        category.Posts.Add(post);
                        post.Categories.Add(category);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Elapsed (categorize): " + sw.ElapsedMilliseconds);

            // Save data to excel file
            Save(posts, root);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
