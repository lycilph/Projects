using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FilesystemControlsTest
{
    public class FileSystemObjectInfo : ObservableObject
    {
        public ObservableCollection<FileSystemObjectInfo> Children { get; private set; }

        private ImageSource image_source;
        public ImageSource ImageSource
        {
            get { return image_source; }
            private set
            {
                if (image_source != value)
                {
                    image_source = value;
                    NotifyPropertyChanged("ImageSource");
                }
            }
        }

        private FileSystemInfo info;
        public FileSystemInfo Info
        {
            get { return info; }
            private set
            {
                if (info != value)
                {
                    info = value;
                    NotifyPropertyChanged("Info");
                }
            }
        }

        private bool expanded = false;
        public bool IsExpanded
        {
            get { return expanded; }
            set
            {
                if (expanded != value)
                {
                    expanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        private bool selected = false;
        public bool IsSelected
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        private DriveInfo Drive { get; set; }

        public FileSystemObjectInfo(FileSystemInfo info)
        {
            if (this is DummyFileSystemObjectInfo) return;

            Children = new ObservableCollection<FileSystemObjectInfo>();

            Info = info;
            if (Info is DirectoryInfo)
            {
                ImageSource = Interop.FolderManager.GetImageSource(info.FullName, Interop.ItemState.Close);
                AddDummy();
            }
            else if (Info is FileInfo)
            {
                //ImageSource = FileManager.GetImageSource(info.FullName);
                throw new ArgumentException("Only directories is supported");
            }
        }

        public FileSystemObjectInfo(DriveInfo drive) : this(drive.RootDirectory)
        {
            Drive = drive;
        }

        private void AddDummy()
        {
            Children.Add(new DummyFileSystemObjectInfo());
        }

        private bool HasDummy()
        {
            return GetDummy() != null;
        }

        private DummyFileSystemObjectInfo GetDummy()
        {
            var list = Children.OfType<DummyFileSystemObjectInfo>().ToList();
            if (list.Count > 0) return list.First();
            return null;
        }

        private void RemoveDummy()
        {
            Children.Remove(GetDummy());
        }


        public override void PreNotifyPropertyChanged(string property_name)
        {
            if (Info is DirectoryInfo && string.Equals(property_name, "IsExpanded", StringComparison.CurrentCultureIgnoreCase))
            {
                if (IsExpanded)
                {
                    ImageSource = Interop.FolderManager.GetImageSource(Info.FullName, Interop.ItemState.Open);
                    if (HasDummy())
                    {
                        RemoveDummy();
                        ExploreDirectories();
                        //ExploreFiles();
                    }
                }
                else
                {
                    ImageSource = Interop.FolderManager.GetImageSource(Info.FullName, Interop.ItemState.Close);
                }
            }
        }

        private void ExploreDirectories()
        {
            if (Drive != null && !Drive.IsReady) return;

            try
            {
                if (Info is DirectoryInfo)
                {
                    var directories = ((DirectoryInfo)Info).GetDirectories();
                    foreach (var dir in directories.Where(d => !d.IsHidden() && !d.IsSystem()).OrderBy(d => d.Name))
                        Children.Add(new FileSystemObjectInfo(dir));
                }
            }
            catch
            {
                /*throw;*/
            }
        }

        //private void ExploreFiles()
        //{
        //    if (!object.ReferenceEquals(this.Drive, null))
        //    {
        //        if (!this.Drive.IsReady) return;
        //    }
        //    try
        //    {
        //        if (this.FileSystemInfo is DirectoryInfo)
        //        {
        //            var files = ((DirectoryInfo)this.FileSystemInfo).GetFiles();
        //            foreach (var file in files.OrderBy(d => d.Name))
        //            {
        //                if (!object.Equals((file.Attributes & FileAttributes.System), FileAttributes.System) &&
        //                    !object.Equals((file.Attributes & FileAttributes.Hidden), FileAttributes.Hidden))
        //                {
        //                    this.Children.Add(new FileSystemObjectInfo(file));
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        /*throw;*/
        //    }
        //}
    }

    public class DummyFileSystemObjectInfo : FileSystemObjectInfo
    {
        public DummyFileSystemObjectInfo() : base(new DirectoryInfo("DummyFileSystemObjectInfo")) {}
    }
}
