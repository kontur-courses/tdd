using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualization
    {
        private readonly Point center;
        private List<Rectangle> rectangles;

        public TagsCloudVisualization(Point center)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException();
            }
            this.center = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(center, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        public Point GetCenter()
        {
            return this.center;
        }
    }
}
