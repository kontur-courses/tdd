using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        private Point nextRectanglePos;
        
        public List<Rectangle> Rectangles { get; private set; }
        
        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            Center = center;
            nextRectanglePos = Center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(nextRectanglePos, rectangleSize);
            Rectangles.Add(rectangle);
            setNextRectanglePos(rectangleSize);
            return rectangle;
        }

        private void setNextRectanglePos(Size currentRectangleSize)
        {
            nextRectanglePos.X += currentRectangleSize.Width;
        }
    }
}