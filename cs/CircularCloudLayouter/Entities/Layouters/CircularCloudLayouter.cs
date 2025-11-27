using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualisation.Entities.Layouters
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private double currentAngle;
        private double currentRadius;

        private double angleStep = 0.1;
        private double radiusStep = 0.5;

        public CircularCloudLayouter(Point center)
        {
            if (center.X <= 0 || center.Y <= 0) throw new ArgumentException("Center coordinates must be positives");
            this.center = center;
            rectangles = new List<Rectangle>();
            currentAngle = 0;
            currentRadius = 0;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0) throw new ArgumentException("Rectangle sizes must be positives");

            if (rectangles.Count == 0)
            {
                return PutFirstRectangle(rectangleSize);
            }

            return PlaceNextRectange(rectangleSize);
        }

        private Rectangle PlaceNextRectange(Size rectangleSize)
        {
            while (true)
            {
                double x = center.X + currentRadius * Math.Cos(currentAngle);
                double y = center.Y + currentRadius * Math.Sin(currentAngle);

                var potentialCenter = new Point((int)(x - rectangleSize.Width / 2), (int)(y - rectangleSize.Height / 2));
                var potentialRectangle = new Rectangle(potentialCenter, rectangleSize);

                if (!IntersectWithAny(potentialRectangle))
                {
                    rectangles.Add(potentialRectangle);
                    return potentialRectangle;
                }

                currentAngle += angleStep;
                if (currentAngle >= 2 * Math.PI)
                {
                    currentAngle = 0;
                    currentRadius += radiusStep;
                }
            }
        }

        private bool IntersectWithAny(Rectangle potentialRectangle)
        {
            return rectangles.Any(rect => rect.IntersectsWith(potentialRectangle));
        }

        private Rectangle PutFirstRectangle(Size rectangleSize)
        {
            var firstCenter = new Point((int)(center.X - rectangleSize.Width / 2), (int)(center.Y - rectangleSize.Width / 2));
            var first = new Rectangle(firstCenter, rectangleSize);
            currentRadius = rectangleSize.Height / 2;
            rectangles.Add(first);
            return first;
        }

        public List<Rectangle> GenerateRandomRectangles(int count)
        {
            var rectangles = new List<Rectangle>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var size = new Size(random.Next(20, 21), random.Next(20, 21));
                rectangles.Add(PutNextRectangle(size));
            }

            return rectangles;
        }
    }
}