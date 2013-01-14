using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CategorizationEngine.Filters;

namespace CategorizationEngine
{
    public class DataGenerator
    {
        private static readonly Random rand = new Random();

        public static Profile GenerateProfile()
        {
            var profile = new Profile() {Name = Wordlist.Instance.Random()};
            // Add accounts
            profile.Accounts.AddRange(RandomAccounts(rand.Next(1,11)));
            // Add posts to accounts
            foreach (var account in profile.Accounts)
                account.Add(RandomPosts(rand.Next(100,1001)));
            // Add categories
            foreach (var category in FixedCategories(profile.AggregatedPosts()))
                profile.RootCategory.Add(category);
            return profile;
        }

        public static Account RandomAccount()
        {
            return new Account() {Name = Wordlist.Instance.Random()};
        }

        public static List<Account> RandomAccounts(int count)
        {
            var result = new List<Account>();
            for (var i = 0; i < count; i++)
                result.Add(RandomAccount());
            return result;
        }

        public static Post RandomPost()
        {
            var p = new Post();

            // Generate random date
            var start = new DateTime(2000, 1, 1);
            var range = (DateTime.Today - start).Days;
            p.Date = start.AddDays(rand.Next(range));

            // Generate random text
            p.Text = Wordlist.Instance.Random(3);

            // Generate random value
            p.Value = rand.NextDouble() * 1000;

            return p;
        }

        public static List<Post> RandomPosts(int count)
        {
            var result = new List<Post>();
            for (var i = 0; i < count; i++)
                result.Add(RandomPost());
            return result;
        }

        public static List<Category> FixedCategories(List<Post> posts)
        {
            var result = new List<Category>();

            // Temp category
            Category c;

            var regex_category = new Category("Regex");
            c = new Category("Word1");
            c.Filters.Add(new RegexFilter(new Regex(Wordlist.Instance.Words[0])));
            regex_category.Add(c);
            c = new Category("Word2");
            c.Filters.Add(new RegexFilter(new Regex(Wordlist.Instance.Words[100])));
            regex_category.Add(c);
            c = new Category("Word3");
            c.Filters.Add(new RegexFilter(new Regex(Wordlist.Instance.Words[200])));
            regex_category.Add(c);
            result.Add(regex_category);

            var date_category = new Category("Date");
            c = new Category("Before 1-1-2008");
            c.Filters.Add(new DateFilter { Date1 = new DateTime(2008, 1, 1), Operator = DateOperator.Before });
            date_category.Add(c);
            c = new Category("Equal 1-1-2008");
            c.Filters.Add(new DateFilter { Date1 = new DateTime(2008, 1, 1), Operator = DateOperator.Equal });
            date_category.Add(c);
            c = new Category("After 1-1-2008");
            c.Filters.Add(new DateFilter { Date1 = new DateTime(2008, 1, 1), Operator = DateOperator.After });
            date_category.Add(c);
            c = new Category("Range 1-1-2008 to 1-1-2009");
            c.Filters.Add(new DateFilter { Date1 = new DateTime(2008, 1, 1), Date2 = new DateTime(2009, 1, 1), Operator = DateOperator.Range });
            date_category.Add(c);
            c = new Category("1-1-2008 to 1-1-2009");
            c.Filters.Add(new DateFilter { Date1 = new DateTime(2008, 1, 1), Operator = DateOperator.After });
            c.Filters.Add(new DateFilter { Date1 = new DateTime(2009, 1, 1), Operator = DateOperator.Before });
            date_category.Add(c);
            result.Add(date_category);

            var value_category = new Category("Value");
            c = new Category("Less than 542");
            c.Filters.Add(new ValueFilter { Value1 = 542, Operator = ValueOperator.Less });
            value_category.Add(c);
            c = new Category("Around 542");
            c.Filters.Add(new ValueFilter { Value1 = 542, Epsilon = 5, Operator = ValueOperator.Around });
            value_category.Add(c);
            c = new Category("Greater than 542");
            c.Filters.Add(new ValueFilter { Value1 = 542, Operator = ValueOperator.Greater });
            value_category.Add(c);
            c = new Category("Range 123 - 321");
            c.Filters.Add(new ValueFilter { Value1 = 123, Value2 = 321, Operator = ValueOperator.Range });
            value_category.Add(c);
            result.Add(value_category);

            var hash_category = new Category("Items");
            var f = new HashFilter();
            f.Set.Add(posts[0]);
            f.Set.Add(posts[17]);
            f.Set.Add(posts[23]);
            f.Set.Add(posts[42]);
            hash_category.Filters.Add(f);
            result.Add(hash_category);

            return result;
        }
    }
}
