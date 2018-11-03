using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    class CircularCloudVisualization
    {
        private const int CountOfRectangles = 100;
        private const int MinSizeOfRectangle = 10;
        private const int MaxSizeOfRectangle = 100;
        private const int BitmapWidth = 1000;
        private const int BitmapHeight = 1000;
        private const string BitmapName = "CircularCloud";

        public static void Main()
        {
            var center = new Point(BitmapWidth/2, BitmapHeight/2);
            var cloud = new CircularCloudLayouter(center);

            var rectangles = GenerateRectanglesOfCloud(cloud);
            var bitmap = GetBitmapWithRectangles(center, rectangles);

            bitmap.Save($"..\\..\\Images\\{BitmapName}{CountOfRectangles}.png", ImageFormat.Png);
        }

        private static Bitmap GetBitmapWithRectangles(Point center, List<Rectangle> rectangles)
        {
            var bitmap = new Bitmap(BitmapWidth, BitmapHeight);
            var pen = new Pen(Color.Black);

            var maxDist = (int)rectangles.Select(x => GetDistanceFromRectangleToPoint(x, center)).Max();

            foreach (var rectangle in rectangles)
            {
                var dist = GetDistanceFromRectangleToPoint(rectangle, center);

                var r = (int)(dist / maxDist * 255 * 0.9);
                var g = (int)(dist / maxDist * 255 * 0.7);
                var b = (int)(dist / maxDist * 255 * 0.5);

                var brush = new SolidBrush(Color.FromArgb(r, g, b));

                Graphics.FromImage(bitmap).FillRectangle(brush, rectangle);
                Graphics.FromImage(bitmap).DrawRectangle(pen, rectangle);
            }

            return bitmap;
        }

        private static List<Rectangle> GenerateRectanglesOfCloud(CircularCloudLayouter cloud)
        {
            var rnd = new Random();

            var rectangles = new List<Rectangle>();

            for (var i = 0; i < CountOfRectangles; i++)
            {
                var width = rnd.Next(MinSizeOfRectangle, MaxSizeOfRectangle);
                var height = rnd.Next(MinSizeOfRectangle, MaxSizeOfRectangle);

                var size = new Size(width, height);

                rectangles.Add(cloud.PutNextRectangle(size));
            }

            return rectangles;
        }

        private static double GetDistanceFromRectangleToPoint(Rectangle rectangle, Point center)
        {
            return Math.Sqrt((GetCenterOfRectangle(rectangle).X - center.X) *
                             (GetCenterOfRectangle(rectangle).X - center.X) +
                             (GetCenterOfRectangle(rectangle).Y - center.Y) *
                             (GetCenterOfRectangle(rectangle).Y - center.Y));
        }

        private static Point GetCenterOfRectangle(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }
    }
}
