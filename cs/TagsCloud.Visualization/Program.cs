using System.Drawing;
using TagsCloud.Core;

namespace TagsCloud.Visualization
{
    internal class Program
    {
        public const int ImageWidth = 1920;
        public const int ImageHeight = 1080;
        private const int CountWords = 1500;

        private static void Main()
        {
            var cloud = new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2));

            for (var i = 0; i < CountWords; ++i)
                cloud.PutNextRectangle(SizeGenerator.GenerateSize(25, 60, 10, 20));

            var image = CircularCloudVisualization.SaveImage(cloud.Rectangles, ImageWidth, ImageHeight);
            image.Save($"../../Images/{CountWords}rectangles.jpg");
        }
    }
}