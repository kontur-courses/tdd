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
        private readonly List<Rectangle> rectangles;
        private RoundSpiralPositionGenerator positionGenerator;

        public TagsCloudVisualization(Point center)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException();
            }
            this.center = center;
            rectangles = new List<Rectangle>();
            positionGenerator = new RoundSpiralPositionGenerator(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(positionGenerator.Next(), rectangleSize);
            while (IntersectsWithPrevious(rectangle))
            {
                rectangle.MoveToPosition(positionGenerator.Next());
            }
            rectangles.Add(rectangle);
            return rectangle;
        }

        private bool IntersectsWithPrevious(Rectangle rectangle)
        {
            return rectangles.Any(previousRectangle => previousRectangle.IntersectsWith(rectangle));
        }

        public Point GetCenter()
        {
            return this.center;
        }
    }
}
