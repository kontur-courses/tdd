using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = Enumerable.Repeat(new Size(30, 30), 40);
            var rectangleLayout = rectangles
                .Select(layouter.PutNextRectangle)
                .ToArray();
            var image = new RectangleLayoutVisualizer().Vizualize(new Size(700, 700),
                rectangleLayout, new Point(50, 100));
            new ImageSaver().Save(image, "picture.png");
        }
    }
}
