using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Bitmap bitmap = new Bitmap(600, 600);
            RectangleVisualisator visualisator = new RectangleVisualisator(new Point(bitmap.Width / 2, bitmap.Height / 2), bitmap);
            visualisator.CreateImage();
        }
    }
}