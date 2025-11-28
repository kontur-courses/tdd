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
            var layouter = new CircularCloudLayouter();

            var layoutWithDifferentRectangles = layouter.WithCenterAt(center)
                .WithWidthInRange(15, 30)
                .WithHeightInRange(15, 20)
                .WithCountOfRectangles(50)
                .Generate();

            //We can reset layouter to default configuration if needed
            layouter.Reset();

            var layoutWithRectanglesOfOneSize = layouter.WithCenterAt(center)
                .WithWidth(30)
                .WithHeight(30)
                .WithCountOfRectangles(50)
                .Generate();

            var visualizer = new BasicCloudVisalizer();

            var tempDir = Path.Combine(Path.GetTempPath(), "CloudLayouts");
            Directory.CreateDirectory(tempDir);

            var firstFile = $"cloud_{DateTime.Now:yyyyMMdd_HHmmssfff}.png";
            var secondFile = $"cloud_{DateTime.Now:yyyyMMdd_HHmmssfff}.png";

            var firstPath = Path.Combine(tempDir, firstFile);
            var secondPath = Path.Combine(tempDir, secondFile);

            visualizer.Visualize(layoutWithDifferentRectangles, center, firstPath);
            visualizer.Visualize(layoutWithRectanglesOfOneSize, center, secondPath);

            Console.WriteLine($"Saved in {tempDir}");
        }
    }
}