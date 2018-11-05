using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private List<Rectangle> Rectangles;
        private readonly Spiral spiral;

        public CircularCloudLayouter()
        {
            Rectangles = new List<Rectangle>();
            spiral = new Spiral(this);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = spiral.GenerateNewRectangle(Rectangles, rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }
    }

}
