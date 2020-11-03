using System.Drawing;
using System.Linq;
using TagsCloud.Core;

namespace TagsCloud
{
    public class Program
    {
        public const int ImageWidth = 1920;
        public const int ImageHeight = 1080;
        private const int CountRectangles = 1000;

        private static void Main()
        {
            var cloud = new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2));

            var rectangles = new SizeGenerator(25, 40, 10, 20)
                .GenerateSize(CountRectangles)
                .Select(cloud.PutNextRectangle);

            var image = CircularCloudVisualization.CreateImage(rectangles, ImageWidth, ImageHeight);
            image.Save($"../../Images/{CountRectangles}rectangles.jpg");
        }
    }
}