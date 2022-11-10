using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public IReadOnlyList<Rectangle> Rectangles;
        public Point Center { get; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }
    }
}