using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main()
        {
            Visualize(100, 300, 100);
            Visualize(100, 300, 100, false);
            Visualize(100, 200, 500, false);
        }

        private static void Visualize(int minRectSize, int maxRectSize, int count, bool isSameSized = true)
        {
            var rectangles = isSameSized
                ? RectanglesGenerator.GenerateSameRectanglesWithSize(count, minRectSize, maxRectSize)
                : RectanglesGenerator.GenerateDifferentRectangles(count, minRectSize, maxRectSize);
            var bmpVisualizer = new BitmapVisualizer(rectangles);
            bmpVisualizer.DrawRectangles(Color.Black, Color.White);
            var isSameSizedString = isSameSized ? "SameSized" : "Different";
            var fileName = $@"{count}{isSameSizedString}RectanglesWithSizesFrom{minRectSize}To{maxRectSize}.png";
            var dirPath = @"../../../ExamplePictures";
            bmpVisualizer.Save(fileName, new DirectoryInfo(dirPath));
            bmpVisualizer.Dispose();
        }
    }
}