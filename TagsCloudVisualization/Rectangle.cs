using System;

namespace TagsCloudVisualization
{
    public class Rectangle
    {
        public Size Size { get; }
        public Point Point { get; }

        public Rectangle(double x, double y, int width,int height)
        {
            Point = new Point(x,y);
            Size = new Size(width,height);
        }

        public Rectangle(Size size, Point point)
        {
            if (size == null)
            {
                throw new NullReferenceException();
            }
            if (point == null)
            {
                throw new NullReferenceException();
            }
            Size = size;
            Point = point;
        }
    }
}