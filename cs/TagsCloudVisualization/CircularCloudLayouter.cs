using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center
        { get; set; }
        /*
        public Point CurrentPoint
        { get; set; }
        */

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
            Point rectangleCenterLocation = LayouterSpiral.GetNextPoint();
            var rectangleLocation = new Point(rectangleCenterLocation.X - rectangleSize.Width / 2, rectangleCenterLocation.Y - rectangleSize.Height / 2);
            var rectangle = new Rectangle(rectangleLocation, rectangleSize);
            //CurrentPoint = rectangleLocation;
            return rectangle;
        }

        
    }

    class Program
    {
        public static void Main()
        {

        }
    }
}
