using System.Drawing;

namespace TagsCloudVisualizationTests
{
    public class VisualOutput
    {
        private readonly IVisualizer visualizer;

        public VisualOutput(IVisualizer visualizer)
        {
            this.visualizer = visualizer;
        }

        public void SaveToBitmap(string filename)
        {
            var bitmap = DrawToBitmap();
            bitmap.Save(filename);
        }

        public Bitmap DrawToBitmap()
        {
            var size = visualizer.GetBitmapSize();
            var bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);

            visualizer.Draw(graphics);
            graphics.Save();
            return bitmap;
        }
    }
}