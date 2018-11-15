using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static int GetRectangleArea(this Rectangle rectangle)
        {
            return rectangle.Height * rectangle.Width;
        }

        public static Point GetCenter(this Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        public static Point[] GetCornersCoordinates(this Rectangle rectangle)
        {
            Point[] cornersCoordinates = new Point[4]
            {
                rectangle.Location,
                new Point(rectangle.X + rectangle.Width, rectangle.Y),
                new Point(rectangle.X, rectangle.Y + rectangle.Height),
                new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height)
            };
            return cornersCoordinates;
        }

        public static Rectangle MakeShift(this Rectangle rectangle, Point shearCoordinates)
        {
            rectangle.Location += new Size(shearCoordinates);
            return rectangle;
        }
    }
}