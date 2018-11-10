using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center { get; }
        private readonly List<Rectangle> cloudRectangles = new List<Rectangle>();
        private readonly IEnumerable<Point> generatorPoints;

        public CircularCloudLayouter(Point center, IEnumerable<Point> generatorPoints)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException("Center coords should be positive");
            }

            this.generatorPoints = generatorPoints;
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("height and width should be positive");

            var currentRectangle = new Rectangle();
            foreach(var point in generatorPoints)
            {

                var rectangleCenterPoint = new Point(
                    point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2);

                currentRectangle = new Rectangle(rectangleCenterPoint, rectangleSize);
                if (!cloudRectangles.Any(rect => rect.IntersectsWith(currentRectangle)))
                {
                    cloudRectangles.Add(currentRectangle);
                    break;
                }
            }
            return currentRectangle;
        }
 
    }
}