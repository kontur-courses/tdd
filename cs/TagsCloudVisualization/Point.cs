namespace TagsCloudVisualization
{
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static int PowDistance(Point p1, Point p2)
        {
            var dx = (p2.X - p1.X);
            var dy = (p2.Y - p1.Y);
            return dx * dx + dy * dy;
        }

        public bool Equals(Point p)
        {
            return (X == p.X) && (Y == p.Y);
        }
        
        public override int GetHashCode() 
        {
            return (X << 2) ^ Y;
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}