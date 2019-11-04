using System.Drawing;

namespace TagsCloudVisualization
{
    public struct FilledArea
    {
        public Point TopLeft { get; }
        public Point BottomRight { get; }
        public Point Center { get; } 

        public FilledArea(Rectangle rectangle)
        {
            TopLeft = rectangle.Location;
            BottomRight = GetBottomRightPoint(rectangle);
            Center = GetCenter(rectangle);
        }

        public FilledArea(Point topLeft, Point bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
            var size = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            Center = GetCenter(new Rectangle(topLeft, size));
        }

        private static Point GetCenter(Rectangle rectangle)
        {
            var centerX = rectangle.X + rectangle.Width / 2;
            var centerY = rectangle.Y + rectangle.Height / 2;
            return new Point(centerX, centerY);
        }
        
        private static Point GetBottomRightPoint(Rectangle rectangle)
        {
            var bottomRightX = rectangle.Location.X + rectangle.Width;
            var bottomRightY = rectangle.Location.Y - rectangle.Height;
            return new Point(bottomRightX, bottomRightY);
        }

        public bool IntersectsWith(FilledArea area) =>
            !(area.TopLeft.X > BottomRight.X && area.BottomRight.X > BottomRight.X ||
              area.TopLeft.X < TopLeft.X && area.BottomRight.X < TopLeft.X ||
              area.TopLeft.Y > TopLeft.Y && area.BottomRight.Y > TopLeft.Y || 
              area.TopLeft.Y < BottomRight.Y && area.BottomRight.Y < BottomRight.Y);
    }
}