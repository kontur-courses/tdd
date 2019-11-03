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
        private readonly RoundSpiralPositionGenerator positionGenerator;

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
            var nextPosition = positionGenerator.Next();
            var rectangle = new Rectangle(nextPosition, rectangleSize);
            while (IntersectsWithPrevious(rectangle) || rectangle.X < 0 || rectangle.Y < 0)
            {
                nextPosition = positionGenerator.Next();
                rectangle.MoveToPosition(nextPosition);
            }
            rectangles.Add(rectangle);
            return rectangle;
        }

        private bool IntersectsWithPrevious(Rectangle rectangle)
        {
            return rectangles.Any(previousRectangle => previousRectangle.IntersectsWith(rectangle));
        }
    }
}
