using System.Drawing;
using TagsCloudVisualisation.Entities.Layouters;
using TagsCloudVisualisation.Entities.Visualizer;

namespace TagsCloudVisualisation
{
    public class Program()
    {
        public static void Main(string[] args)
        {
            var center = new Point(100, 100);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = layouter.GenerateRandomRectangles(50);

            var visualizer = new BasicCloudVisalizer();

            var tempDir = Path.Combine(Path.GetTempPath(), "CloudLayouts");
            Directory.CreateDirectory(tempDir);

            var fileName = $"cloud_{DateTime.Now:yyyyMMdd_HHmmssfff}.png";
            var filePath = Path.Combine(tempDir, fileName);

            visualizer.Visualize(rectangles, center, filePath);

            Console.WriteLine($"Saved in {filePath}");
        }
    }
}