using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public ArchimedesSpiral ArchimedesSpiral { get; }

        public List<Rectangle> Rectangles { get; }

        public CircularCloudLayouter(Point center)
        {
            ArchimedesSpiral = new ArchimedesSpiral(center, 1);
            Rectangles = new List<Rectangle>();
        }
        

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.IsEmpty)
                throw new ArgumentException();
            var rect = new Rectangle(Point.Empty, rectangleSize);
            var rectangleLocation = GetNextRectangleLocation(rectangleSize);
            var centeredLocation = rect.GetRectangleCenteredLocation(rectangleLocation);
            rect.Location = centeredLocation;
            Rectangles.Add(rect);
            return rect;
        }

        private Point GetNextRectangleLocation(Size rectangleSize)
        {
            foreach (var point in ArchimedesSpiral.GetSpiralPoints())
            {
                var rectangleToPlace = new Rectangle(Point.Empty, rectangleSize);
                var centeredLocation = rectangleToPlace.GetRectangleCenteredLocation(point);
                rectangleToPlace.Location = centeredLocation;
                if (!rectangleToPlace.IntersectsWithAny(Rectangles))
                    return point;
            }
            return default;
        }
    }
}