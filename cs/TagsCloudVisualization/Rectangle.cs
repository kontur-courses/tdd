using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class Rectangle
    {
        public Point Center { get; set; }
        public Size Size { get; }
        public Point Origin { get; }

        public double LeftXCoord => Center.X - Size.Width / 2;
        public double RightXCoord => Center.X + Size.Width / 2;
        public double TopYCoord => Center.Y + Size.Height / 2;
        public double BottomYCoord => Center.Y - Size.Height / 2;

        public Rectangle(Point origin, Point center, Size size)
        {
            Center = center;
            Size = size;
            Origin = origin;
        }

        public bool Intersects(Rectangle otherRectangle)
        {
            return !(LeftXCoord > otherRectangle.RightXCoord
                     || RightXCoord < otherRectangle.LeftXCoord
                     || TopYCoord < otherRectangle.BottomYCoord
                     || BottomYCoord > otherRectangle.TopYCoord);
        }

        public Quarter[] GetQuarters()
        {
            var result = new List<Quarter>();
            if (RightXCoord > Origin.X && TopYCoord > Origin.Y)
                result.Add(Quarter.TopRight);
            if (LeftXCoord < Origin.X && TopYCoord > Origin.Y)
                result.Add(Quarter.TopLeft);
            if (LeftXCoord < Origin.X && BottomYCoord < Origin.Y)
                result.Add(Quarter.BottomLeft);
            if (RightXCoord > Origin.X && BottomYCoord < Origin.Y)
                result.Add(Quarter.BottomRight);
            return result.ToArray();
        }

        public override string ToString()
        {
            return $"Rectangle (X: {Center.X}; Y: {Center.Y})";
        }
    }
}