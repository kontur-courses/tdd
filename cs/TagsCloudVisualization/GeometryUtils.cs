using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class GeometryUtils
    {
        public static Point ConvertPolarToIntegerCartesian(double r, double phi)
        {
            var x = (int)Math.Round(r * Math.Cos(phi));
            var y = (int)Math.Round(r * Math.Sin(phi));
            return new Point(x, y);
        }

        public static Rectangle[] GetPossibleRectangles(Point point, Size size)
        {
            var delta = new[] { 1, 0 };
            return delta
                .SelectMany(dx =>
                    delta.Select(dy =>
                        new Point(point.X - dx * size.Width, point.Y - dy * size.Height)))
                .Select(p => new Rectangle(p, size)).ToArray();

        }

        public static bool RectanglesAreIntersected(Rectangle firstRectangle, Rectangle secondRectangle)
        {
            return Segment.SegmentsAreIntersected(
                       new Segment(firstRectangle.Left, firstRectangle.Right),
                       new Segment(secondRectangle.Left, secondRectangle.Right))
                   && Segment.SegmentsAreIntersected(
                       new Segment(firstRectangle.Top, firstRectangle.Bottom),
                       new Segment(secondRectangle.Top, secondRectangle.Bottom));
        }


        public static Segment[] GetRectangleSides(Rectangle rectangle)
        {
            var topLeft = new Point(rectangle.Left, rectangle.Top);
            var topRight = new Point(rectangle.Right, rectangle.Top);
            var bottomLeft = new Point(rectangle.Left, rectangle.Bottom);
            var bottomRight = new Point(rectangle.Right, rectangle.Bottom);

            return new[]
            {
                new Segment(topLeft, topRight),
                new Segment(topLeft, bottomLeft),
                new Segment(topRight, bottomRight),
                new Segment(bottomLeft, bottomRight)
            };
        }
    }
}
