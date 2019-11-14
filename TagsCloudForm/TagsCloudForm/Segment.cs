using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Segment
    {
        public Point start { get; private set; }
        public  Point end { get; private set; }
        public  Type type { get; private set; }
        public double Length  => Math.Sqrt(Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2)); 
        public Segment(Point start, Point end, Type type)
        {
            if (type == Type.Bottom || type == Type.Top)
            {
                if (start.Y != end.Y)
                    throw new ArgumentException("Wrong coordinates");
                if (start.X < end.X)
                {
                    this.start = start;
                    this.end = end;
                }
                else
                {
                    this.start = end;
                    this.end = start;
                }
                this.type = type;
            }
            else
            {
                if (start.X != end.X)
                    throw new ArgumentException("Wrong coordinates");
                if (start.Y < end.Y)
                {
                    this.start = start;
                    this.end = end;
                }
                else
                {
                    this.start = end;
                    this.end = start;
                }
                this.type = type;
            }
        }
        public Segment(int startX, int startY, int endX, int endY, Type type) : this(new Point(startX, startY), new Point(endX, endY), type)
        {
        }

        public bool Parallel(Segment segment)
        {
            if (segment.type == Type.Top && type == Type.Top
                || segment.type == Type.Bottom && type == Type.Top
                || segment.type == Type.Top && type == Type.Bottom
                || segment.type == Type.Bottom && type == Type.Bottom)
                return true;
            if (segment.type == Type.Left && type == Type.Left
                || segment.type == Type.Right && type == Type.Left
                || segment.type == Type.Left && type == Type.Right
                || segment.type == Type.Right && type == Type.Right)
                return true;
            return false;
        }

        public bool Opposite(Segment segment)
        {
            if (segment.type == Type.Bottom && type == Type.Top
                || segment.type == Type.Top && type == Type.Bottom)
                return true;
            if (segment.type == Type.Right && type == Type.Left
                || segment.type == Type.Left && type == Type.Right)
                return true;
            return false;
        }

        public bool Horizontal()
        {
            if (type == Type.Bottom || type == Type.Top)
                return true;
            return false;
        }

        public bool Contains(Segment segment)
        {
            if (!this.Parallel(segment))
                return false;
            if (segment.Horizontal())
                if (this.start.X < segment.start.X && this.end.X > segment.end.X)
                    return true;
                else
                    return false;
            else
                if (this.start.Y < segment.start.Y && this.end.Y > segment.end.Y)
                return true;
            else
                return false;
        }

        public bool Intersects(Segment segment)
        {
            if (this.Horizontal() && segment.Horizontal())
            {
                if ((this.start.X >= segment.start.X && this.start.X < segment.end.X)
                            || (this.end.X > segment.start.X && this.end.X <= segment.end.X)
                            || (this.start.X <= segment.start.X && this.end.X >= segment.end.X)
                            || (this.start.X >= segment.start.X && this.end.X <= segment.end.X))
                    return true;
                return false;
            }
            if (!this.Horizontal() && !segment.Horizontal())
            {
                if ((this.start.Y >= segment.start.Y && this.start.Y < segment.end.Y)
                            || (this.end.Y > segment.start.Y && this.end.Y <= segment.end.Y)
                            || (this.start.Y <= segment.start.Y && this.end.Y >= segment.end.Y)
                            || (this.start.Y >= segment.start.Y && this.end.Y <= segment.end.Y))
                    return true;
                return false;
            }
            return false;
        }


        public static Type OppositeType(Type type)
        {
            switch (type)
            {
                case Type.Top:
                    return Type.Bottom;
                case Type.Bottom:
                    return Type.Top;
                case Type.Left:
                    return Type.Right;
                case Type.Right:
                    return Type.Left;
                default:
                    return Type.Top;
            }
        }

        public enum Type
        {
            Top,
            Bottom,
            Left,
            Right
        }

    }
}
