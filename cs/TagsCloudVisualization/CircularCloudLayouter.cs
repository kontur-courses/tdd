using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double AzimuthDelta = Math.PI / 18;

        private readonly Size centralOffset;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly ArchimedeanSpiral polarSpiral = new ArchimedeanSpiral();

        public CircularCloudLayouter(Point center) => centralOffset = new Size(center);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty) throw new ArgumentException();

            Rectangle newRectangle;

            while (true)
            {
                var currentArchimedeanSpiralPoint = ConvertPointFromPolarToCartesian(polarSpiral.Radius,
                                                                                     polarSpiral.Azimuth);
                newRectangle = new Rectangle(currentArchimedeanSpiralPoint, rectangleSize);
                if (rectangles.All(rectangle => !rectangle.IntersectsWith(newRectangle)))
                    break;

                polarSpiral.Azimuth += AzimuthDelta;
            }

            if (rectangles.Count == 0)
            {
                var firstRectangleOffset = new Point(-rectangleSize.Width / 2,
                                                     -rectangleSize.Height / 2);
                newRectangle.Offset(firstRectangleOffset);
            }

            rectangles.Add(newRectangle);
            polarSpiral.Azimuth += AzimuthDelta;

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