using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateImages("../../Example Images", 100, 150, 250);
        }

        private static void GenerateImages(string path, params int[] rectanglesCount)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var layouter = new CircularCloudLayouter(Point.Empty);
            var generator = new SizeGenerator(10, 50, 10, 50);
            
            for (var i = 0; i < rectanglesCount.Length; i++)
            {
                layouter = new CircularCloudLayouter(Point.Empty);
                var rectangles = generator.GenerateSizes(rectanglesCount[i])
                    .Select(size => layouter.PutNextRectangle(size));
                Drawer.DrawRectangles(rectangles, $"{path}/image{i}.png");
            }
        }
    }
}