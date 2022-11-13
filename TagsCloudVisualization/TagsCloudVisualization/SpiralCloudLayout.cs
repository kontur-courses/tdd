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

        public Rectangle GetBorders()
        {
            throw new NotImplementedException();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (PlacedRectangles.Count == 0)
            {
                Rectangle rect = CreateRectangleByCenter(rectangleSize, new Point(0, 0));
                PlacedRectangles.Add(rect);
            }

            PlacedRectangles.Add(FindPlaceForRectangle(rectangleSize));
            return PlacedRectangles.Last();

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
            LastAngle += AngleStep;
            Point center = new Point(
                (int) ((1 + LastAngle) * Math.Cos(LastAngle)),
                (int) ((1 + LastAngle) * Math.Sin(LastAngle)));
            return CreateRectangleByCenter(rectangleSize, center);
        }
    }
}
