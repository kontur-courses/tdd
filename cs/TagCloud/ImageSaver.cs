using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloud
{
    public static class ImageSaver
    {
        public static void SaveBitmap (Bitmap bitmap, string fullPath)
        {
            bitmap.Save(fullPath);
        }

        public static void SaveBitmap(Bitmap bitmap, DirectoryInfo directoryToSave, string fileName)
        {
            bitmap.Save(Path.Combine(directoryToSave.FullName, fileName));
        }
    }
}
