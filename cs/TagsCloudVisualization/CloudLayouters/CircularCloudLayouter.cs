using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.CloudLayouters
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private const double AzimuthDelta = Math.PI / 18;

        private readonly Size centralOffset;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly ArchimedeanSpiral spiral = new ArchimedeanSpiral();

        public CircularCloudLayouter(Point center) => centralOffset = new Size(center);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty) throw new ArgumentException();

            Rectangle newRectangle;

            while (true)
            {
                var currentArchimedeanSpiralPoint = ConvertPointFromPolarToCartesian(spiral.Radius, spiral.Azimuth);
                Point newRectangleLocation = currentArchimedeanSpiralPoint;

                newRectangle = new Rectangle(newRectangleLocation, rectangleSize);

                if (newRectangle.IntersectsWith(rectangles)) break;

                if (currentArchimedeanSpiralPoint.X <= 0)
                {
                    newRectangle.Offset(new Point(-rectangleSize.Width, -rectangleSize.Height));

                    if (newRectangle.IntersectsWith(rectangles)) break;
                }

                spiral.Azimuth += AzimuthDelta;
            }

            if (rectangles.Count == 0)
            {
                var firstRectangleOffset = new Point(-rectangleSize.Width / 2,
                                                     -rectangleSize.Height / 2);
                newRectangle.Offset(firstRectangleOffset);
            }

            rectangles.Add(newRectangle);
            spiral.Azimuth += AzimuthDelta;

            return newRectangle.CreateMovedCopy(centralOffset);
        }

        private static Point ConvertPointFromPolarToCartesian(double radius, double azimuth)
        {
            var x = (int)Math.Round(radius * Math.Cos(azimuth), MidpointRounding.AwayFromZero);
            var y = (int)Math.Round(radius * Math.Sin(azimuth), MidpointRounding.AwayFromZero);

            return new Point(x, y);
        }
    }
}