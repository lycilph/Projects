using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace FilesystemControlsTest
{
    public partial class MainWindow
    {
        public ObservableCollection<FileSystemObjectInfo> Drives
        {
            get { return (ObservableCollection<FileSystemObjectInfo>)GetValue(DrivesProperty); }
            set { SetValue(DrivesProperty, value); }
        }
        public static readonly DependencyProperty DrivesProperty =
            DependencyProperty.Register("Drives", typeof(ObservableCollection<FileSystemObjectInfo>), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Drives = new ObservableCollection<FileSystemObjectInfo>();

            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
                Drives.Add(new FileSystemObjectInfo(drive));

            if (Drives.Any())
                Drives.First().IsSelected = true;
        }
    }
}
