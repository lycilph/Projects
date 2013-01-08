using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using CategorizationEngine;

namespace Gui
{
    public class MainWindowViewModel : ObservableObject
    {
        private ObservableCollection<Post> _Posts;
        public ObservableCollection<Post> Posts
        {
            get { return _Posts; }
            set
            {
                if (_Posts == value) return;
                _Posts = value;
                NotifyPropertyChanged("Posts");
            }
        }

        private Category _RootCategory;
        public Category RootCategory
        {
            get { return _RootCategory; }
            set
            {
                if (_RootCategory == value) return;
                _RootCategory = value;
                NotifyPropertyChanged("RootCategory");
            }
        }

        public MainWindowViewModel()
        {
            Posts = new ObservableCollection<Post>(Post.Random(10000));

            RootCategory = new Category("Root");

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
            RootCategory.Categories.Add(c1);

            var c2 = new Category("C2");
            var c21 = new Category("C2.1");
            c21.Patterns.Add(new DatePattern() { Date = new DateTime(2008, 1, 1), Operator = DateOperator.Before });
            c2.Categories.Add(c21);
            var c22 = new Category("C2.2");
            c22.Patterns.Add(new DatePattern() { Date = new DateTime(2008, 1, 1), Operator = DateOperator.Equal });
            c2.Categories.Add(c22);
            var c23 = new Category("C2.3");
            c23.Patterns.Add(new DatePattern() { Date = new DateTime(2008, 1, 1), Operator = DateOperator.After });
            c2.Categories.Add(c23);
            RootCategory.Categories.Add(c2);

            var c3 = new Category("C3");
            var c31 = new Category("C3.1");
            c31.Patterns.Add(new ValuePattern() { Value = 542, Operator = ValueOperator.Less });
            c3.Categories.Add(c31);
            var c32 = new Category("C3.2");
            c32.Patterns.Add(new ValuePattern() { Value = 542, Operator = ValueOperator.Equal });
            c3.Categories.Add(c32);
            var c33 = new Category("C3.3");
            c33.Patterns.Add(new ValuePattern() { Value = 542, Operator = ValueOperator.Greater });
            c3.Categories.Add(c33);
            RootCategory.Categories.Add(c3);

            var c4 = new Category("C4");
            var c4p = new HashPattern();
            c4p.Set.Add(Posts[0]);
            c4p.Set.Add(Posts[17]);
            c4p.Set.Add(Posts[23]);
            c4p.Set.Add(Posts[42]);
            c4.Patterns.Add(c4p);
            RootCategory.Categories.Add(c4);

            foreach (var post in Posts)
            {
                foreach (var category in Category.Flatten(RootCategory).Where(c => c.Patterns.Count > 0))
                {
                    if (category.IsMatch(post))
                    {
                        category.Posts.Add(post);
                        post.Categories.Add(category);
                    }
                }
            }
        }
    }
}
