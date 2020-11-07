using System.Drawing;

namespace CloudTag
{
    public static class RectangleExtensions
    {
        public static Rectangle SetCenter(this Rectangle rectangle, Point centerPoint)
        {
            var x = centerPoint.X - rectangle.Width / 2;
            var y = centerPoint.Y - rectangle.Height / 2;
            return new Rectangle(new Point(x,y), rectangle.Size);
        }
    }
}