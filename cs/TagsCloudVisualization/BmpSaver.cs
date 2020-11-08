using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class BmpSaver : ISaver
    {
        public void SaveImage(Bitmap bitmap, string fileName)
        {
            if (!fileName.All(char.IsLetter))
                throw new ArgumentException("File name contains invalid characters");

            var path = Directory.GetCurrentDirectory();

            bitmap.Save($"{path}\\{fileName}.bmp", ImageFormat.Bmp);
        }
    }
}
