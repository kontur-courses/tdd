using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class CloudLayouterUtilities
    {
        public static Random rnd = new Random();

        public static List<Rectangle> LayoutRectangles(Point center, List<Size> sizes)
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            foreach (var size in sizes)
                rectangles.Add(layouter.PutNextRectangle(size));

            return rectangles;
        }

        public static Bitmap GetBitmapFromRectangles(Size imageSize, List<Rectangle> rectangles)
        {
            var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bitmap);

            foreach (var rect in rectangles)
            {
                var brush = new SolidBrush(
                    Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                graphics.FillRectangle(brush, rect);
            }

            return bitmap;
        }

        public static Bitmap GetCenteredBitmapFromRectangles(Point center, List<Rectangle> rectangles)
        {
            var maxDistance = (int)Math.Sqrt(center.GetMaxSquaredDistanceTo(rectangles));
            maxDistance += rectangles.Select(rect => Math.Max(rect.Width, rect.Height)).Max();
            var imageSize = new Size(2 * maxDistance, 2 * maxDistance);
            var imageCenter = new Point(maxDistance, maxDistance);
            var imageRectangles = GetRectanglesShiftedToNewCenter(rectangles, center, imageCenter);

            return GetBitmapFromRectangles(imageSize, imageRectangles);
        }

        public static List<Rectangle> GetRectanglesShiftedToNewCenter(
            List<Rectangle> rectangles, Point oldCenter, Point newCenter)
        {
            var offset = new Point(newCenter.X - oldCenter.X, newCenter.Y - oldCenter.Y);
            var shiftedRectangles = new List<Rectangle>();
            foreach (var rect in rectangles)
            {
                rect.Offset(offset);
                shiftedRectangles.Add(rect);
            }
            return shiftedRectangles;
        }

        public static List<Rectangle> GenerateRandomLayout(Point center, int amount,
            int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var sizes = new List<Size>();
            while (amount-- > 0)
                sizes.Add(GetRandomSize(minWidth, maxWidth, minHeight, maxHeight));
            return LayoutRectangles(center, sizes);
        }

        public static Size GetRandomSize(int minWidth, int maxWidth,
                                  int minHeight, int maxHeight)
        {
            return new Size(rnd.Next(minWidth, maxWidth + 1),
                                rnd.Next(minHeight, maxHeight + 1));
        }
    }
}
