using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rectangle) =>
            new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);

        public static Rectangle MoveToHavePointInCenter(this Rectangle rectangle, Point center)
        {
            var newLocation = center - new Size(rectangle.Width / 2, rectangle.Height / 2);
            return new Rectangle(newLocation, rectangle.Size);
        }
    }
}