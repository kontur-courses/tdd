//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;

//namespace TagsCloudVisualization
//{
//    public static class RectangleExtension
//    {
//        public static bool Contact(this Rectangle firstRectangle, Rectangle secondRectangle) =>
//            firstRectangle
//                .GetExtremePoints()
//                .Any(point => point.);
 

//        public static IEnumerable<Point> GetExtremePoints(this Rectangle rectangle)
//        {
//            yield return new Point(rectangle.X, rectangle.Y);
//            yield return new Point(rectangle.X + rectangle.Width, rectangle.Y);
//            yield return new Point(
//                rectangle.X + rectangle.Width,
//                rectangle.Y + rectangle.Height
//            );
//            yield return new Point(rectangle.X, rectangle.Y + rectangle.Height);
//        }

//        private static bool PointOnRectangleBorder(Rectangle rectangle, Point point) =>
//            (point.X >= rectangle.X && rectangle.Y <= point.X + rectangle.Width)
//            (point.)
//    }
//}
