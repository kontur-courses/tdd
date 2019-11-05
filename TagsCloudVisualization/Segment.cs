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
            this.start = start;
            this.end = end;
            this.type = type;
        }
        public Segment(int startX, int startY, int endX, int endY, Type type)
        {
            this.start = new Point(startX, startY);
            this.end = new Point(endX, endY);
            this.type = type;
        }

        public static Segment operator -(Segment obj1, Segment obj2)
        {
            if (obj1.start == obj2.start)
                return new Segment(obj1.end, obj2.end, obj1.type);
            if (obj1.end == obj2.end)
                return new Segment(obj1.start, obj2.start, obj1.type);
            throw new ArgumentException();
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
