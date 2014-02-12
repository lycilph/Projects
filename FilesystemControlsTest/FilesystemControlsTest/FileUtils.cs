using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesystemControlsTest
{
    public static class FileUtils
    {
        public static bool IsHidden(this FileSystemInfo info)
        {
            return info.Attributes.HasFlag(FileAttributes.Hidden);
        }

        public static bool IsSystem(this FileSystemInfo info)
        {
            return info.Attributes.HasFlag(FileAttributes.System);
        }
    }
}
