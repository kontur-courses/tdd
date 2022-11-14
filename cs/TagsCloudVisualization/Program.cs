using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var bitmap = new Bitmap(600, 600);
            var layouter = new CircularCloudLayouter(new Point(bitmap.Width / 2, bitmap.Height / 2));
            RectangleVisualisator visualisator = new RectangleVisualisator(layouter, bitmap);
            visualisator.CreateImage();
        }
    }
}