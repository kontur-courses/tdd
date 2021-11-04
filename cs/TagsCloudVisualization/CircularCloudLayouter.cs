using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center
        { get; set; }

        public Spiral LayouterSpiral
        { get; set; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            LayouterSpiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            var rectangleCenterLocation = LayouterSpiral.GetNextPoint();
            var rectangleLocation = new Point(rectangleCenterLocation.X - rectangleSize.Width / 2, rectangleCenterLocation.Y - rectangleSize.Height / 2);
            var rectangle = new Rectangle(rectangleLocation, rectangleSize);
            return rectangle;
        }

        
    }

    internal class Program
    {
        public static void Main()
        {}
    }
}
