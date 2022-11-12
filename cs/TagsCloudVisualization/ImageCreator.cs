using System.Drawing;

namespace TagsCloudVisualization
{
    public class ImageCreator
    {
        private readonly Bitmap bitmap;

        public int Width { get; }
        public int Height { get; }
        public Graphics Graphics { get; }

        public ImageCreator(int width, int height)
        {
            Width = width;
            Height = height;

            bitmap = new Bitmap(width, height);
            Graphics = Graphics.FromImage(bitmap);
        }

        public void Save(string path) => bitmap.Save(path);
    }
}