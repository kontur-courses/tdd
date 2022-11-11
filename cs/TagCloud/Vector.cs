using System.Drawing;
using System.Security.Principal;

namespace TagCloud
{
    public class Vector
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsPerpendicularTo(Vector other)
        {
            return X * other.X == 0 && Y * other.Y == 0;
        }

        public static Vector GetVectorBetweenPoints(Point startPoint, Point endPoint)
        {
            int x = endPoint.X - startPoint.X;

            int y = endPoint.Y - startPoint.Y;

            return new Vector(x, y);
        }
    }
}