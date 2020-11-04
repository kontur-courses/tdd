using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Should
{
    class TestedCloudLayouter
    {
        public readonly List<Rectangle> Rectangles;
        private readonly CircularCloudLayouter layouter;

        public TestedCloudLayouter(Point center)
        {
            layouter = new CircularCloudLayouter(center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size size)
        {
            var newRectangle = layouter.PutNextRectangle(size);
            Rectangles.Add(newRectangle);
            return newRectangle;
        }
    }
}
