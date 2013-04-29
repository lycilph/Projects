using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LunchViewer.Resources
{
    public class PathTrimmingTextBlock : TextBlock
    {
        private FrameworkElement container;

        public PathTrimmingTextBlock()
        {
            Loaded += PathTrimmingTextBlock_Loaded;
        }

        void PathTrimmingTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (Parent == null) throw new InvalidOperationException("PathTrimmingTextBlock must have a container such as a Grid.");

            container = (FrameworkElement)Parent;
            container.SizeChanged += ContainerSizeChanged;

            Text = GetTrimmedPath(ActualWidth);
        }

        private void ContainerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Text = GetTrimmedPath(ActualWidth);
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(PathTrimmingTextBlock), new UIPropertyMetadata(""));

        private string GetTrimmedPath(double width)
        {
            if (string.IsNullOrWhiteSpace(Path))
                return string.Empty;

            var filename = System.IO.Path.GetFileName(Path);
            var directory = System.IO.Path.GetDirectoryName(Path);
            bool widthOK;
            var changedWidth = false;

            do
            {
                FormattedText formatted = new FormattedText(
                    string.Format("{0}...\\{1}", directory, filename),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    FontFamily.GetTypefaces().First(),
                    FontSize,
                    Foreground);

                widthOK = formatted.Width < (width * 0.95);

                if (widthOK) continue;

                changedWidth = true;
                if (directory == null) return "...\\" + filename;

                directory = directory.Substring(0, directory.Length - 1);
                if (directory.Length == 0) return "...\\" + filename;
            } while (!widthOK);

            return !changedWidth ? Path : string.Format("{0}...{1}", directory, filename);
        }
    }
}
