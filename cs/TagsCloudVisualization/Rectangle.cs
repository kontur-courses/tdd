namespace TagsCloudVisualization
{
    public class Rectangle
    {
        public Point Pos { get; set; }
        public Size Size { get; }

        public Rectangle(Point pos, Size size)
        {
            Pos = pos;
            Size = size;
        }

        public Point bottmRightPoint => new Point(Pos.X + Size.Width, Pos.Y + Size.Height);

        public static bool IsOverlap(Rectangle a, Rectangle b)
        {
            var aBottomRight = a.bottmRightPoint;
            var bBottomRight = b.bottmRightPoint;

            return a.Pos.X < bBottomRight.X &&
                   aBottomRight.X > b.Pos.X &&
                   a.Pos.Y < bBottomRight.Y &&
                   aBottomRight.Y > b.Pos.Y;
        }
    }
}