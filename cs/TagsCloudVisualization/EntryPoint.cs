using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace TagsCloudVisualization
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(1000, 1000));
            layouter.PutNextRectangles(Enumerable.Range(50, 50).Select((n, i) => new Size(n * 2 + i, n + i)));
            DrawRectangles(layouter.Rectangles, expandPercent: 1200);
        }

        public static void DrawRectangles(IEnumerable<Rectangle> rectangles, string path = @"../../../image.png", int expandPercent = 120)
        {
            rectangles = rectangles.ToList();
            var maxX = rectangles.Max(rect => rect.Right);
            var minX = rectangles.Max(rect => rect.Left);
            var maxY = rectangles.Max(rect => rect.Bottom);
            var minY = rectangles.Max(rect => rect.Top);

            var width = maxX - minX;
            var height = maxY - minY;
            width *= expandPercent / 100;
            height *= expandPercent / 100;

            var image = new Bitmap(width, height);
            var graphics = Graphics.FromImage(image);

            foreach (var (i, rectangle) in rectangles.Select((rectangle, i) => (i, rectangle)))
            {
                var change = (i + 25) % 256;
                var topColor = Color.FromArgb(change, Color.LimeGreen);
                var bottomColor = Color.FromArgb(255 - change, Color.Yellow);

                graphics.DrawRectangle(Pens.Blue, rectangle);
                graphics.FillRectangle(new LinearGradientBrush(rectangle, bottomColor, topColor, LinearGradientMode.BackwardDiagonal),
                                       rectangle);
            }

            image.Save(path);
        }
    }
}
