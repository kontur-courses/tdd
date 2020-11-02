using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization.Graphic
{
    public class BitmapSaver : IImageSaver
    {
        private readonly string directory;
        private readonly string filename;
        private const string Extension = "bmp";

        public BitmapSaver(string directory, string filename)
        {
            this.directory = directory;
            this.filename = filename;
        }

        public void Save(Image image)
        {
            var file = Path.ChangeExtension(filename, Extension);
            var path = Path.Combine(directory, file);
            path = file;
            Console.WriteLine($"saving to {path}");
            image.Save(path);
        }
    }
}