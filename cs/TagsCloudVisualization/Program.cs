using System.Drawing;
using TagsCloudVisualization.CloudClasses;
using TagsCloudVisualization.CloudClasses.Interfaces;

namespace TagsCloudVisualization.Visualisation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using var tagCloudGenerator = new TagCloudImageGenerator(new Visualiser(Color.Black, 1));

            var center = new Point(500, 500);
            var bitmapSize = new Size(1000, 1000);

            var firstSizes = GenerateRectangleSizes(40, 60, 80, 60, 80);
            var secondSizes = GenerateRectangleSizes(100, 10, 40, 20, 30);
            var thirdSizes = GenerateRectangleSizes(30, 80, 130, 80, 130);

            var firstLayouter = SetupCloudLayout(center, firstSizes);
            var secondLayouter = SetupCloudLayout(center, secondSizes);
            var thirdLayouter = SetupCloudLayout(center, thirdSizes);

            using var bitmap1 = tagCloudGenerator.CreateNewBitmap(bitmapSize, firstLayouter.Rectangles);
            using var bitmap2 = tagCloudGenerator.CreateNewBitmap(bitmapSize, secondLayouter.Rectangles);
            using var bitmap3 = tagCloudGenerator.CreateNewBitmap(bitmapSize, thirdLayouter.Rectangles);

            BitmapSaver.SaveToCorrect(bitmap1, "bitmap_1.png");
            BitmapSaver.SaveToCorrect(bitmap2, "bitmap_2.png");
            BitmapSaver.SaveToCorrect(bitmap3, "bitmap_3.png");
        }

        private static IEnumerable<Size> GenerateRectangleSizes(int count, int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var sizeBuilder = SizeBuilder.Configure()
                .SetCount(count)
                .SetWidth(minWidth, maxWidth)
                .SetHeight(minHeight, maxHeight)
                .Generate();

            return sizeBuilder;
        }

        private static CircularCloudLayouter SetupCloudLayout(Point center, IEnumerable<Size> sizes)
        {
            var layouter = new CircularCloudLayouter(new SpiralRayMover(center));

            sizes.ToList()
                .ForEach(size => layouter.PutNextRectangle(size));

            return layouter;
        }
    }
}
