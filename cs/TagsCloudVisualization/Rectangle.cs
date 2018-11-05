using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class Rectangle
    {
        public Point Center { get; set; }
        public Size Size { get; }

        public double LeftXCoord => Center.X - Size.Width / 2;
        public double RightXCoord => Center.X + Size.Width / 2;
        public double TopYCoord => Center.Y + Size.Height / 2;
        public double BottomYCoord => Center.Y - Size.Height / 2;

        public Rectangle(Point center, Size size)
        {
            Center = center;
            Size = size;
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
            if (RightXCoord > Center.X && TopYCoord > Center.Y)
                result.Add(Quarter.TopRight);
            if (LeftXCoord < Center.X && TopYCoord > Center.Y)
                result.Add(Quarter.TopLeft);
            if (LeftXCoord < Center.X && BottomYCoord < Center.Y)
                result.Add(Quarter.BottomLeft);
            if (RightXCoord > Center.X && BottomYCoord < Center.Y)
                result.Add(Quarter.BottomRight);
            return result.ToArray();
        }

        public override string ToString()
        {
            return $"Rectangle (X: {Center.X}; Y: {Center.Y})";
        }
    }
}