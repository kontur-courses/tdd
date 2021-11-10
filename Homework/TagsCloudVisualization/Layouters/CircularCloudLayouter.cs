using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualization.Layouters
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }
    }
}