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
        private readonly SpiralPointsCreator pointsCreator;

        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            pointsCreator = new SpiralPointsCreator(center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size size)
        {
            if (size.Height <= 0 || size.Width <= 0)
                throw new ArgumentException("All size params should be greater than zero!");

            Rectangle nextRectangle;
            do
            {
                nextRectangle = new Rectangle(pointsCreator.GetNextPoint(), size);
            } while (Rectangles.Any(r => r.IntersectsWith(nextRectangle)));
            Rectangles.Add(nextRectangle);
            return nextRectangle;
        }
    }
}
