using System.Drawing;

namespace CloudTag
{
    public static class RectangleExtensions
    {
        public static void SetCenter(this ref Rectangle rectangle, Point centerPoint)
        {
            var x = centerPoint.X - rectangle.Width / 2;
            var y = centerPoint.Y - rectangle.Height / 2;
            rectangle.Location = new Point(x, y);
        }
    }
}