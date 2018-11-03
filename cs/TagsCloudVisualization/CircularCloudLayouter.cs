using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;

        public CircularCloudLayouter(Point center, IPointsGenerator pointsGenerator)
        {
            this.center = center;
        }

        public List<Rectangle> Rectangles { get; private set; }

        public void PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }
    }
}