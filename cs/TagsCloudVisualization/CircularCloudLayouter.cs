using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException("Center coords should be positive");
            }

            this.Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("height and width should be positive");
            var spiralAngle = 0.0;

            Func<double, Point> spiralEquasion = t =>
                new Point(Center.X + (int) (t * Math.Cos(t)),
                    Center.Y + (int) (t * Math.Sin(t)));

            while (true)
            {
                var tempPoint = spiralEquasion(spiralAngle);
                spiralAngle += 0.1;
                var rectangleCenterPoint = new Point(tempPoint.X - rectangleSize.Width / 2,
                    tempPoint.Y - rectangleSize.Height / 2);

                var currentRectangle = new Rectangle(rectangleCenterPoint, rectangleSize);
                if (!Rectangles.Any(rect => rect.IntersectsWith(currentRectangle)))
                {
                    Rectangles.Add(currentRectangle);
                    return currentRectangle;
                }
            }
        }
 
    }
}