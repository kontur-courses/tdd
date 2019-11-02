using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        public readonly List<Rectangle> Items;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Items = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newItem = new Rectangle(default, rectangleSize);
            Items.Add(newItem);
            ReallocRectangles();
            return newItem;
        }

        private void ReallocRectangles()
        {
        }
    }
}
