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
        
        private double LastAngle = 0;
        private double AngleStep = 0.2d;

        private Point center;
        private Point leftUpperCorner;
        private Point rightBottomCorner;


        public SpiralCloudLayout(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rect;
            if (PlacedRectangles.Count == 0)
            {
                rect = CreateRectangleByCenter(rectangleSize, new Point(0, 0));
                ResizeBorders(rect);
            }
            else
            {
                rect = FindPlaceForRectangle(rectangleSize);
                ResizeBorders(rect);

            }
            rect.Offset(center);
            PlacedRectangles.Add(rect);
            return rect;

        }

        private void ResizeBorders(Rectangle rectangle)
        {
            leftUpperCorner.X = Math.Min(rectangle.Left, leftUpperCorner.X);
            leftUpperCorner.Y = Math.Min(rectangle.Top, leftUpperCorner.Y);
            rightBottomCorner.X = Math.Max(rectangle.Right, rightBottomCorner.X);
            rightBottomCorner.Y = Math.Max(rectangle.Bottom, rightBottomCorner.Y);
        }
        public Rectangle GetBorders()
        {
            return new Rectangle(leftUpperCorner.X + center.X, leftUpperCorner.Y + center.Y,
                rightBottomCorner.X - leftUpperCorner.X, rightBottomCorner.Y - leftUpperCorner.Y);
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
            rect.Offset(center);
            foreach (var other in PlacedRectangles)
            {
                if (other.IntersectsWith(rect)) return true;
            }

            return false;
        }
    }
}
