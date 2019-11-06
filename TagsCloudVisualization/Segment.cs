using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Segment
    {
        public Point start;
        public Point end;
        public Type type;
        public double Length
        {
            get
            {
                return Math.Sqrt(Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2));
            }
        }
        public Segment(Point start, Point end, Type type)
        {
            if (type == Type.Bottom || type == Type.Top)
            {
                if (start.Y != end.Y)
                    throw new ArgumentException("Wrong coordinates");
                else
                {
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
            }
            else
            {
                if (start.X != end.X)
                    throw new ArgumentException("Wrong coordinates");
                else
                {
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
        }
        public Segment(int startX, int startY, int endX, int endY, Type type):this(new Point(startX, startY), new Point(endX, endY), type)
        {
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
