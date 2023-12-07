using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public const int ImageWidth = 800;
        public const int ImageHeight = 800;

        public static void Main(string[] args)
        {
            new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2), new Spiral());
        }
    }
}
