using System.Drawing;

namespace TagsCloudVisualizationTests
{
    public abstract class Visualizer
    {
        private readonly Size size;

        protected Visualizer(Size bitmapSize)
        {
            size = bitmapSize;
        }

        public void SaveToBitmap(string filename)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);

            Draw(graphics);
            graphics.Save();
            bitmap.Save(filename);
        }

        protected abstract void Draw(Graphics graphics);
    }
}