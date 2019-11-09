using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ArchimedesSpiral archimedesSpiral;

        public List<Rectangle> Rectangles { get; }

        public CircularCloudLayouter(Point center)
        {
            archimedesSpiral = new ArchimedesSpiral(center, 0.1);
            Rectangles = new List<Rectangle>();
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.IsEmpty)
                throw new ArgumentException("Empty rectangle size");
            var rectangle = GetNextRectangle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            foreach (var point in archimedesSpiral.GetSpiralPoints())
            {
                var rectangleToPlace = new Rectangle(point, rectangleSize).GetCentered(point);
                if (!rectangleToPlace.IntersectsWithAny(Rectangles))
                    return rectangleToPlace;
            }
            return default;
        }
    }
}