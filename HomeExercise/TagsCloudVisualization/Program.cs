using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var maxSize = new Size(150, 50);
            var minSize = new Size(50, 50);
            var rectangles = RectanglesGenerator.GenerateRectangles(100, maxSize, minSize);
            var image = CloudVisualizer.Visualize(rectangles.ToArray());
            image.Save("layout4.png");
        }
    }
}