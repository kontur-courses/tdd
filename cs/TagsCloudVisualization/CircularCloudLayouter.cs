using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        private ArchimedesSpiral spiral;
        private readonly List<Rectangle> arrangedRectangles= new List<Rectangle>();
        
        public CircularCloudLayouter(Point center)
        {
            if(center.X <= 0 || center.Y <= 0)
                throw new ArgumentException("Tags Cloud center coordinates should be positive.");
            Center = center;
            spiral = new ArchimedesSpiral(center,1, 1);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Tags Cloud rectangle size parameters should be positive.");
            var tempRect = new Rectangle(spiral.GetNextCoordinates(), rectangleSize);
            while (arrangedRectangles.Any(r => r.IntersectsWith(tempRect)))
                tempRect.Location = spiral.GetNextCoordinates();
            arrangedRectangles.Add(tempRect);
            return tempRect;
        }
    }
}