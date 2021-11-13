using System;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public static class BitmapExtensions
    {
        public static Bitmap GetScaledBitmap(this Bitmap bmp, double scale)
        {
            if (scale <= 0)
                throw new ArgumentException($"{nameof(scale)} should be positive", nameof(scale));
            if (scale == 1)
                return bmp;
            using (bmp)
            {
                var scaledSize = new Size((int)(bmp.Width * scale), (int)(bmp.Height * scale));
                return new Bitmap(bmp, scaledSize);
            }
        }

        public static void Save(this Bitmap bmp, string fileName, DirectoryInfo dir = null)
        {
            dir = dir ?? new DirectoryInfo(Environment.CurrentDirectory);
            if (!dir.Exists) dir.Create();
            var path = Path.Combine(dir.FullName, fileName);
            SaveToPath(bmp, path);
        }

        private static void SaveToPath(Bitmap bmp, string path)
        {
            try
            {
                bmp.Save(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't save file to: {path}", ex);
            }
        }
    }
}