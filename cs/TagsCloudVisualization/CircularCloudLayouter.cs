using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Put rectangles in cloud in order of distance from center
    /// </summary>
    class CircularCloudLayouter
    {
        public CircularCloudLayouter(ISpiral spiral, Point center)
        {
            Rectangles = new List<Rectangle>();
            Center = center;
            this.spiral = spiral;
        }

        private readonly ISpiral spiral;

        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = spiral.GetRectangleInCurrentSpiralPosition(rectangleSize);
            while (IntersectsWithPrevious(rectangle))
                rectangle = spiral.GetRectangleInCurrentSpiralPosition(rectangleSize);

            Rectangles.Add(rectangle);
            return rectangle;
        }

        private bool IntersectsWithPrevious(Rectangle rectangle)
        {
            return Rectangles.Any(rectangle.IntersectsWith);
        }
    }
}

