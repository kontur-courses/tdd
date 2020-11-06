using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CircularCloud
{
    public class CircularCloudLayouter
    {
        protected ArchimedeanSpiral Spiral;
        protected List<Rectangle> Rectangles = new List<Rectangle>();
        protected List<Point> FreePoints = new List<Point>();

        public Point Center => Spiral.Center;

        public CircularCloudLayouter(Point center) => Spiral = new ArchimedeanSpiral(center);

        public CircularCloudLayouter(int x, int y) => Spiral = new ArchimedeanSpiral(x, y);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            FreePoints.RemoveAll(point => Rectangles.Any(rectangle => rectangle.Contains(point)));
            Point point;
            var result = Rectangle.Empty;
            if (FreePoints.Count > 0 && Rectangles.Count > 0)
            {
                point = FreePoints.FirstOrDefault(point =>
                    CouldPutRectangle(GetRectangleWithCenterInPoint(point, rectangleSize)));
                if (point != Point.Empty) result = GetRectangleWithCenterInPoint(point, rectangleSize);
            }

            while (result == Rectangle.Empty)
            {
                point = Spiral.GetNextPoint();
                if (CouldPutRectangle(GetRectangleWithCenterInPoint(point, rectangleSize)))
                    result = GetRectangleWithCenterInPoint(point, rectangleSize);

                if (!Rectangles.Any(rectangle => rectangle.Contains(point)))
                    FreePoints.Add(point);
            }

            Rectangles.Add(result);
            return result;
        }

        private bool CouldPutRectangle(Rectangle rectangle)
        {
            return !(Rectangles.Count > 0 && Rectangles.Any(rect =>
                rect.IntersectsWith(rectangle)));
        }

        public static Rectangle GetRectangleWithCenterInPoint(Point point, Size rectangleSize)
        {
            point.X -= rectangleSize.Width / 2;
            point.Y -= rectangleSize.Height / 2;
            return new Rectangle(point, rectangleSize);
        }

        public Rectangle[] GetAllRectangles()
        {
            return Rectangles.ToArray();
        }
    }
}