using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TagCloud.Tests")]

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private readonly Spiral spiral;
        internal readonly List<Rectangle> Rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newRect = FindPlaceForRect(rectangleSize);
            Rectangles.Add(newRect);
            return newRect;
        }

        private Rectangle FindPlaceForRect(Size rectangleSize)
        {
            var resultRect = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            while (resultRect.IntersectsWith(Rectangles))
                resultRect = new Rectangle(spiral.GetNextPoint(), rectangleSize);

            return resultRect;
        }
    }
}