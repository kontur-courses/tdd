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
        private readonly ArchimedeanSpiral spiral = new ArchimedeanSpiral(AzimuthDelta);

        public CircularCloudLayouter(Point center) => centralOffset = new Size(center);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty)
                throw new ArgumentException("Got argument is empty.", nameof(rectangleSize));

            Rectangle newRectangle = Rectangle.Empty;

            foreach (var currentArchimedeanSpiralPoint in spiral.GetPoints())
            {
                Point newRectangleLocation = currentArchimedeanSpiralPoint;

                newRectangle = new Rectangle(newRectangleLocation, rectangleSize);

                if (!newRectangle.IntersectsWithAnyOf(rectangles)) break;

                if (currentArchimedeanSpiralPoint.X <= 0)
                {
                    newRectangle.Offset(new Point(-rectangleSize.Width, -rectangleSize.Height));

                    if (!newRectangle.IntersectsWithAnyOf(rectangles)) break;
                }
            }

            if (rectangles.Count == 0)
            {
                var firstRectangleOffset = new Point(-rectangleSize.Width / 2,
                                                     -rectangleSize.Height / 2);
                newRectangle.Offset(firstRectangleOffset);
            }

            rectangles.Add(newRectangle);

            return newRectangle.CreateMovedCopy(centralOffset);
        }
    }
}