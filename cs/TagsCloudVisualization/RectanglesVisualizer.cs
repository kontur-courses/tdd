using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectanglesVisualizer
    {
        public static Bitmap Visualize(IReadOnlyCollection<Rectangle> rectangles)
        {
            var (bitmapSize, offsetFromCenter) = GetNecessarySizeForBitmapAndOffset(rectangles);
            var center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);
            var offsetForRectangles = center.Add(offsetFromCenter);

            var canvas = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var penForRectangles = new Pen(Color.Blue, 1);
            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.Clear(Color.Black);
                foreach (var rectangle in rectangles)
                {
                    var newRectangle = new Rectangle(rectangle.Location.Add(offsetForRectangles), rectangle.Size);
                    graphics.DrawRectangle(penForRectangles, newRectangle);
                }
            }

            return canvas;
        }

        private static (Size, Point) GetNecessarySizeForBitmapAndOffset(IReadOnlyCollection<Rectangle> rectangles)
        {
            var leftTopCorner = new Point(int.MaxValue, int.MaxValue);
            var rightBottomCorner = new Point(int.MinValue, int.MinValue);

            foreach (var rectangle in rectangles)
            {
                if (rectangle.Bottom > rightBottomCorner.Y)
                    rightBottomCorner.Y = rectangle.Bottom;
                if (rectangle.Right > rightBottomCorner.X)
                    rightBottomCorner.X = rectangle.Right;
                if (rectangle.Top < leftTopCorner.Y)
                    leftTopCorner.Y = rectangle.Top;
                if (rectangle.Left < leftTopCorner.X)
                    leftTopCorner.X = rectangle.Left;
            }

            var width = rightBottomCorner.X - leftTopCorner.X + 1;
            var height = rightBottomCorner.Y - leftTopCorner.Y + 1;
            var dx = (rightBottomCorner.X + leftTopCorner.X + 1) / 2;
            var dy = (rightBottomCorner.Y + leftTopCorner.Y + 1) / 2;
            return (new Size(width, height), new Point(-dx, -dy));
        }
    }
}