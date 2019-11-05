using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point center_cloud;
        private List<Rectangle> rectangles;
        private const double radiusStep = 1;
        private const double angleStep = 1;
        private double angle = 1;

        public CircularCloudLayouter(Point center)
        {
            center_cloud = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetNextRectangle(rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var rectangle = GetRectangle(rectangleSize);
            while (CheckIntersect(rectangle))
            {
                rectangle = GetRectangle(rectangleSize);
            }
            return rectangle;
        }

        private bool CheckIntersect(Rectangle rectangle)
        {
            foreach (var r in rectangles)
                if (r.IntersectsWith(rectangle))
                    return true;
            return false;
        }

        private Rectangle GetRectangle(Size rectangleSize)
        {
            var x = (int)(Math.Cos(angle) * radiusStep * angle + center_cloud.X);
            var y = (int)(Math.Sin(angle) * radiusStep * angle + center_cloud.Y);
            var rectangle = new Rectangle(new Point(x, y), rectangleSize);
            angle += angleStep / angle;
            return rectangle;
        }

        private Rectangle GetSize()
        {
            if (rectangles.Count == 0)
                return new Rectangle(0, 0, 0, 0);
            var minTop = rectangles[0].Top;
            var maxBottom = rectangles[0].Bottom;
            var minLeft = rectangles[0].Left;
            var maxRight = rectangles[0].Right;

            foreach (var r in rectangles)
            {
                minTop = minTop > r.Top ? r.Top : minTop;
                maxBottom = maxBottom < r.Bottom ? r.Bottom : maxBottom;
                minLeft = minLeft > r.Left ? r.Left : minLeft;
                maxRight = maxRight < r.Right ? r.Right : maxRight;
            }
            return new Rectangle(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }

        public void SaveImage()
        {
            var size = GetSize();
            var bitmap = new Bitmap(size.Width + 1, size.Height + 1);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Red);
            foreach (var r in rectangles)
            {
                graphics.DrawRectangle(pen, r.X - size.X, r.Y - size.Y, r.Width, r.Height);
            }
            bitmap.Save("image.bmp");
        }
    }
}
