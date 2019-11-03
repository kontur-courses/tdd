using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center;

        public CircularCloudLayouter(Point center) => Center = center;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return Rectangle.Empty;
        }

        public Point GetRectangleCenter(Rectangle rectangle)
        {
            var horizontalCenter = (rectangle.X + rectangle.Width) / 2.0;
            var verticalCenter = (rectangle.Y + rectangle.Height) / 2.0;
            return new Point(horizontalCenter, verticalCenter);
        }
    }
}