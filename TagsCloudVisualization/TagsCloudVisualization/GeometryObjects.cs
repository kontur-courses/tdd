using System;

namespace GeometryObjects
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    public class Rectangle
    {
        public Point RightTopVertex => new Point(LeftBottomVertex.X + Size.Width,
            LeftBottomVertex.Y + Size.Height);
        public Point LeftTopVertex => new Point(LeftBottomVertex.X, 
            RightTopVertex.Y);
        public Point RightBottomVertex => new Point(RightTopVertex.X,
            LeftBottomVertex.Y);

        public Point LeftBottomVertex { get; set; }

        public Size Size { get; private set; }

        public Rectangle(Point leftBottomVertex, Size size)
        {
            LeftBottomVertex = leftBottomVertex;
            Size = size;
        }

        static public bool AreRectanglesIntersected(Rectangle rec1, Rectangle rec2)
        {
            return !((rec1.RightTopVertex.X <= rec2.LeftBottomVertex.X ||
                    rec1.LeftBottomVertex.X >= rec2.RightTopVertex.X) ||
                   (rec1.RightTopVertex.Y <= rec2.LeftBottomVertex.Y ||
                    rec1.LeftBottomVertex.Y >= rec2.RightTopVertex.Y));
        }
    }


    public class Size
    {
        private int width;
        private int height;

        public int Width
        {
            get { return width; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Witdh must be positive");
                width = value;
            }
        }

        public int Height
        {
            get { return height; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Height must be positive");
                height = value;
            }
        }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class DistancesCalculator
    {
        public static double GetPointToPointDistance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }


        public static double GetPointToRectangleDistance(Point p, Rectangle r)
        {
            if (r.LeftBottomVertex.X <= p.X && p.X <= r.RightTopVertex.X)
                return Math.Min(Math.Abs(p.Y - r.LeftBottomVertex.Y),
                    Math.Abs(p.Y - r.RightTopVertex.Y));
            if (r.LeftBottomVertex.Y <= p.Y && p.Y <= r.RightTopVertex.Y)
                return Math.Min(Math.Abs(p.X - r.LeftBottomVertex.X),
                    Math.Abs(p.X - r.RightTopVertex.X));
            return Math.Min(Math.Min(GetPointToPointDistance(p, r.LeftBottomVertex),
                    GetPointToPointDistance(p, r.LeftTopVertex)),
                Math.Min(GetPointToPointDistance(p, r.RightBottomVertex),
                    GetPointToPointDistance(p, r.RightTopVertex)));
        }
    }
}