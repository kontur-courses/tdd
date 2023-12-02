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
        public readonly Point CenterPoint;
        private readonly SpiralGenerator spiralGenerator;
        private readonly List<Rectangle> createdRectangles = new();
        public CircularCloudLayouter(Point center)
        {
            CenterPoint = center;
            spiralGenerator = new SpiralGenerator(center);
        }

        public CircularCloudLayouter(Point center, int radiusDelta, double angleDelta)
        {
            CenterPoint = center;
            spiralGenerator = new SpiralGenerator(center, radiusDelta, angleDelta);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Rectangle can't have negative width or height");

            while (true)
            {
                var nextPoint = spiralGenerator.GetNextPoint();

                var locationForRect = new Point(nextPoint.X - rectangleSize.Width / 2,
                    nextPoint.Y - rectangleSize.Height / 2);

                var newRect = new Rectangle(locationForRect, rectangleSize);
                if (createdRectangles.Any(rectangle => rectangle.IntersectsWith(newRect))) continue;

                createdRectangles.Add(newRect);
                return newRect;
            }
        }
    }
}
