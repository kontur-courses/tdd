namespace TagsCloudVisualization
{
    public class Rectangle
    {
        public Point Pos { get; }
        public Size Size { get; }

        public Rectangle(Point pos, Size size)
        {
            Pos = pos;
            Size = size;
        }

        public static bool IsOverlap(Rectangle a, Rectangle b)
        {
            var aBottomRightX = a.Pos.X + a.Size.Width;
            var aBottomRightY = a.Pos.Y + a.Size.Height;
            var bBottomRightX = b.Pos.X + b.Size.Width;
            var bBottomRightY = b.Pos.Y + b.Size.Height;

            return a.Pos.X < bBottomRightX &&
                   aBottomRightX > b.Pos.X &&
                   a.Pos.Y < bBottomRightY &&
                   aBottomRightY > b.Pos.Y;
        }
    }
}