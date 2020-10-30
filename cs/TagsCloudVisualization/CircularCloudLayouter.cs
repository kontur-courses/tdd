using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point center;
        
        public List<Rectangle> Rectangles { get; private set; }
        
        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(center, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }
    }
}