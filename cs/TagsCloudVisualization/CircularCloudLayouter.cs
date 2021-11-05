using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get; set; }

        public Spiral LayouterSpiral { get; set; }

        //public bool NoInrtersections { get; set; }

        public List<Rectangle> RectangleList { get; set; }


        public CircularCloudLayouter(Point center)
        {
            Center = center;
            LayouterSpiral = new Spiral(Center);
            RectangleList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            Rectangle nextRectangle;
            if (RectangleList.Count == 0)
            {
                nextRectangle = new Rectangle(Center, rectangleSize);
                RectangleList.Add(nextRectangle);
                return nextRectangle;
            }
            nextRectangle = CreteNewRectangle(rectangleSize);
            while (RectangleList.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
                nextRectangle = CreteNewRectangle(rectangleSize);
            RectangleList.Add(nextRectangle);
            return nextRectangle;
        }

        private Rectangle CreteNewRectangle(Size rectangleSize)
        {
            var rectangleCenterLocation = LayouterSpiral.GetNextPoint();
            var rectangleX = rectangleCenterLocation.X - rectangleSize.Width / 2;
            var rectangleY = rectangleCenterLocation.Y - rectangleSize.Height / 2;
            var rectangleLocation = new Point(rectangleX,rectangleY);
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
