using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get; set; } //????

        public Spiral LayouterSpiral { get; set; }

        //public bool NoInrtersections { get; set; }

        public List<Rectangle> RectangleList { get; set; }


        public CircularCloudLayouter(Point center)
        {
            Center = center;
            LayouterSpiral = new Spiral(Center);
            //NoInrtersections = true;
            RectangleList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            var nextRectangle = CreteNewRectangle(rectangleSize);
            while (RectangleList.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
            {
                nextRectangle = CreteNewRectangle(rectangleSize);
            }

            RectangleList.Add(nextRectangle);
            //NoInrtersections = !LayouterSpiral.CheckOutIntersections(RectangleList);
            //var result = (NoInrtersections) ? nextRectangle : PutNextRectangle(nextRectangle.Size);
            return nextRectangle;
        }

        private Rectangle CreteNewRectangle(Size rectangleSize)
        {
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
