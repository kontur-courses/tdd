using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Segment
    {
        public readonly Point start;
        public readonly Point end;
        public readonly Type type;
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


        public enum Type
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}
