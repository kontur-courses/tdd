using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static readonly Point Center = new Point(500, 500);

        private static void Main()
        {
            DrawTagCloudWithEqualRectangles();
            DrawTagCloudWithIncreasingRectangles();
            DrawTagCloudWithDecreasingRectangles();
        }

        private static void DrawTagCloudWithEqualRectangles()
        {
            var sizes = Enumerable.Repeat(new Size(50, 50), 100).ToArray();
            var layouter = new CircularCloudLayouter(Center);
            var visualizer = new CircularCloudVisualizer();
            var bitmap = visualizer.Visualize(layouter, sizes);
            bitmap.Save("equal_rectangles.png");
        }

        private static void DrawTagCloudWithIncreasingRectangles()
        {
            using (var bitmap = CreateBitmap(50, i =>
            {
                var text = $"#{i + 1}";
                var fontSize = 8 + i;
                var font = new Font(FontFamily.GenericSansSerif, fontSize);
                return new Tag(MeasureText(text, fontSize), text, font);
            }))
            {
                bitmap.Save("increasing_rectangles.png");
            }
        }

        private static void DrawTagCloudWithDecreasingRectangles()
        {
            using (var bitmap = CreateBitmap(50, i =>
            {
                var text = $"#{i + 1}";
                var fontSize = 58 - i;
                var font = new Font(FontFamily.GenericSansSerif, fontSize);
                return new Tag(MeasureText(text, fontSize), text, font);
            }))
            {
                bitmap.Save("decreasing_rectangles.png");
            }
        }

        private static Bitmap CreateBitmap(int count, Func<int, Tag> fabric)
        {
            var layouter = new CircularCloudLayouter(Center);
            var tags = Enumerable.Range(0, count).Select(fabric).ToArray();
            var visualizer = new CircularCloudVisualizer();
            return visualizer.Visualize(layouter, tags);
        }

        private static Size MeasureText(string text, int fontSize) =>
            new Size(fontSize * (text.Length + 1), fontSize * 2);
    }
}