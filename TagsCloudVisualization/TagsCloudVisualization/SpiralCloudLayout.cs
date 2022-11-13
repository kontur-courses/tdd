using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class SpiralCloudLayout : ICloudLayout
    {
        public List<Rectangle> PlacedRectangles { get; } = new List<Rectangle>();

        private Point LastRectanglePoint = new Point(0, 0);
        private double LastAngle = 0;
        private double AngleStep = 0.2d;

        private Point center;

        public Rectangle GetBorders()
        {
            return new Rectangle(-100, -100, 200, 200);
        }

        public SpiralCloudLayout(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rect;
            if (PlacedRectangles.Count == 0)
            {
                rect = CreateRectangleByCenter(rectangleSize, center);
                PlacedRectangles.Add(rect);
                return rect;
            }

            rect = FindPlaceForRectangle(rectangleSize);
            rect.X += center.X;
            rect.Y += center.Y;
            PlacedRectangles.Add(rect);
            return rect;

        }

        private Rectangle CreateRectangleByCenter(Size rectangleSize, Point center)
        {
            return new Rectangle(
                center.X - rectangleSize.Width / 2,
                center.Y - rectangleSize.Height / 2,
                rectangleSize.Width,
                rectangleSize.Height);
        }

        private Rectangle FindPlaceForRectangle(Size rectangleSize)
        {
            Rectangle place;
            do
            {
                LastAngle += AngleStep;
                Point center = new Point(
                    (int) ((1 + LastAngle) * Math.Cos(LastAngle)),
                    (int) ((1 + LastAngle) * Math.Sin(LastAngle)));
                place = CreateRectangleByCenter(rectangleSize, center);
            } while (IntersectsWithOtherRectangles(place));

            return place;
        }

        private bool IntersectsWithOtherRectangles(Rectangle rect)
        {
            foreach (var other in PlacedRectangles)
            {
                if (other.IntersectsWith(rect)) return true;
            }

            return false;
        }
    }
}
