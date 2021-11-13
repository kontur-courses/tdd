using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouterLogic;
using TagsCloudVisualization.Drawing;
using TagsCloudVisualization.FileManagers;

namespace TagsCloudDebug
{
    static class TagsCloudExamplesGenerator
    {
        static void Main(string[] args)
        {
            args = new[] { "100", "150", "200" };
            if (args.Length == 0)
            {
                GenerateImage();
            }

            for (var i = 0; i < args.Length; i++)
            {
                GenerateImage(int.Parse(args[i]), $"Example{i}.bmp");
            }
        }

        private static void GenerateImage(int rectanglesCount = 150, string fileName = "Example.bmp")
        {
            var cloudLayouter = new CircularCloudLayouter(new ArchimedeanSpiral(Point.Empty));
            var generator = new SizeGenerator(10, 50, 10, 50);
            var drawer = new Drawer();
            var saver = new ImageSaver();

            var rectangles = generator.GenerateSizes(rectanglesCount)
                .Select(size => cloudLayouter.PutNextRectangle(size));

            using var image = drawer.DrawRectangles(rectangles, new Size(700, 700));
            saver.SaveImage(image, fileName, ImageFormat.Png);
        }
    }
}