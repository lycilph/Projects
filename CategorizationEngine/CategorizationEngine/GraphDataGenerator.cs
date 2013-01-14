using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ClosedXML.Excel;

namespace CategorizationEngine
{
    public class GraphDataGenerator
    {
        private readonly Profile profile;

        public GraphDataGenerator(Profile profile)
        {
            this.profile = profile;
        }

        // Generate data
        public List<KeyValuePair<string, double>> ByYear(Category category)
        {
            var result = new List<KeyValuePair<string, double>>();

            var posts = category.AggregatePosts().Distinct().OrderBy(p => p.Date).ToList();
            var date = posts.First().Date;
            var last_date = posts.Last().Date;
            while (date.Year <= last_date.Year)
            {
                var total = posts.Where(p => p.Date.Year == date.Year).Sum(p => p.Value);
                result.Add(new KeyValuePair<string, double>(date.Year.ToString(), total));
                date = date.AddYears(1);
            }

            return result;
        }

        // Generate data and save to excel
        public long Execute()
        {
            var sw = Stopwatch.StartNew();
            var workbook = new XLWorkbook();

            WriteAccountsSheet(workbook);
            WritePostsSheet(workbook);
            WriteCategoriesByYear(workbook);
            WriteCategoriesByMonth(workbook);

            workbook.SaveAs("graph_data.xlsx");

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private void WriteAccountsSheet(XLWorkbook workbook)
        {
            var accounts = profile.Accounts;
            var ws = workbook.AddWorksheet("Accounts");

            ws.Row(1).Cell(1).SetValue("Name");
            ws.Row(1).Cell(2).SetValue("# of posts");

            for (int i = 0; i < accounts.Count; i++)
            {
                ws.Row(i + 2).Cell(1).SetValue(accounts[i].Name);
                ws.Row(i + 2).Cell(2).SetValue(accounts[i].Posts.Count);
            }
        }

        private void WritePostsSheet(XLWorkbook workbook)
        {
            var posts = profile.AggregatedPosts();
            var ws = workbook.AddWorksheet("Posts");

            ws.Row(1).Cell(1).SetValue("Account");
            ws.Row(1).Cell(2).SetValue("Date");
            ws.Row(1).Cell(3).SetValue("Text");
            ws.Row(1).Cell(4).SetValue("Value");
            ws.Row(1).Cell(5).SetValue("Categories");
            
            for (int i = 0; i < posts.Count; i++)
            {
                ws.Row(i + 2).Cell(1).SetValue(posts[i].Account.Name);
                ws.Row(i + 2).Cell(2).SetValue(posts[i].Date);
                ws.Row(i + 2).Cell(3).SetValue(posts[i].Text);
                ws.Row(i + 2).Cell(4).SetValue(posts[i].Value);

                for (int j = 0; j < posts[i].Categories.Count; j++)
                    ws.Row(i + 2).Cell(5 + j).SetValue(posts[i].Categories[j].Name);
            }
        }

        private void WriteCategoriesByYear(XLWorkbook workbook)
        {
            var ws = workbook.AddWorksheet("Categorization (by year)");

            var all_posts = profile.AggregatedPosts().OrderBy(p => p.Date);
            var first_date = all_posts.First().Date;
            var last_date = all_posts.Last().Date;

            // Find posts per category
            var all_categories = Category.Flatten(profile.RootCategory);
            var posts_by_category = all_categories.ToDictionary(category => category, category => category.AggregatePosts().Distinct().ToList());

            ws.Row(1).Cell(1).SetValue("Year");
            for (int i = 0; i < all_categories.Count; i++)
            {
                ws.Row(1).Cell(i + 2).SetValue(all_categories[i].Name);
            }

            for (int i = first_date.Year; i <= last_date.Year; i++)
            {
                var row = i - first_date.Year + 2;
                ws.Row(row).Cell(1).SetValue(i);

                for (int j = 0; j < all_categories.Count; j++)
                {
                    var total = posts_by_category[all_categories[j]].Where(p => p.Date.Year == i).Sum(p => p.Value);
                    ws.Row(row).Cell(j + 2).SetValue(total);
                }
            }
        }

        private void WriteCategoriesByMonth(XLWorkbook workbook)
        {
            var ws = workbook.AddWorksheet("Categorization (by month)");

            var all_posts = profile.AggregatedPosts().OrderBy(p => p.Date);
            var first_date = all_posts.First().Date;
            var last_date = all_posts.Last().Date;

            var all_categories = Category.Flatten(profile.RootCategory);
            var posts_by_category = all_categories.ToDictionary(category => category, category => category.AggregatePosts().Distinct().ToList());

            ws.Row(1).Cell(1).SetValue("Date");
            for (int i = 0; i < all_categories.Count; i++)
            {
                ws.Row(1).Cell(i + 2).SetValue(all_categories[i].Name);
            }

            var current_date = new DateTime(first_date.Year, first_date.Month, 1);
            last_date = new DateTime(last_date.Year, last_date.Month, 1).AddMonths(1);
            var current_row = 2;
            while (current_date != last_date)
            {
                ws.Row(current_row).Cell(1).SetValue(current_date.ToShortDateString());

                for (int j = 0; j < all_categories.Count; j++)
                {
                    var total = posts_by_category[all_categories[j]].Where(p => p.Date.Year == current_date.Year && p.Date.Month == current_date.Month).Sum(p => p.Value);
                    ws.Row(current_row).Cell(j + 2).SetValue(total);
                }

                current_row++;
                current_date = current_date.AddMonths(1);
            }
        }
    }
}
