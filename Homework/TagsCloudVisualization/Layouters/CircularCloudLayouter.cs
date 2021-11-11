using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Layouters
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<RectangleF> rectangles;
        private readonly Spiral spiral;
        
        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
        }

        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            var rect = FindRectanglePosition(rectangleSize);
            
            rectangles.Add(rect);
            
            return rect;
        }

        private RectangleF FindRectanglePosition(Size rectangleSize)
        {
            var rect = new RectangleF(spiral.CurrentPoint, rectangleSize);
            
            while (IsRectangleIntersectsAny(rect))
            {
                spiral.IncreaseSize();
            }

            return rect;
        }

        private bool IsRectangleIntersectsAny(RectangleF rect) => 
            rectangles.Any(r => r.IntersectsWith(rect));
    }
}