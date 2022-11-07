using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Structures;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Rectangle canvas;
        private readonly List<Rectangle> rectangles;
        private readonly IEnumerator<Point> spiral;
        private readonly Point center;
        public readonly List<Point> SpiralPoints;

        protected QuadTree QuadTree { get; }

        public CircularCloudLayouter(Point center)
        {
            canvas = GetRectangleAtPositionOfCenter(center, 2 * new Size(center));
            rectangles = new();
            spiral = new Spiral(center, Math.PI / 360, 2).GetEnumerator();
            this.center = center;
            SpiralPoints = new();

            QuadTree = new QuadTree(canvas);
        }

        private Rectangle GetRectangleAtPositionOfCenter(Point position, Size rectangleSize)
        {
            return new Rectangle(position - rectangleSize / 2, rectangleSize);
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Given rectangle size must be positive");

            while (spiral.MoveNext())
            {
                var rectangle = GetRectangleAtPositionOfCenter(spiral.Current, rectangleSize);
                SpiralPoints.Add(spiral.Current);

                if (QuadTree.HasContent(rectangle))
                    continue;

                CheckIfRectangleIsOutsideOfCanvas(rectangle);

                for (var i = 0; i < 4; i++)
                    rectangle = TryShiftToCenter(rectangle, i % 2 == 0);

                rectangles.Add(rectangle);
                QuadTree.Insert(rectangle);
                return rectangle;
            }

            throw new Exception("Given rectangle couldn't be placed");
        }

        private void CheckIfRectangleIsOutsideOfCanvas(Rectangle rectangle)
        {
            var copy = rectangle;
            copy.Intersect(canvas);
            if (copy != rectangle)
                throw new Exception("Rectangle was placed out side of canvas");
        }

        private Rectangle TryShiftToCenter(Rectangle rectangle, bool isVertical)
        {
            var oldDistance = rectangle.Center().GetDistanceSquareTo(center);
            var oldLocation = rectangle.Location;
            while (true)
            {
                var newLocation = oldLocation;

                if (isVertical)
                    newLocation.Y += Math.Sign(center.Y - rectangle.Center().Y);
                else
                    newLocation.X += Math.Sign(center.X - rectangle.Center().X);


                var newRectangle = new Rectangle(newLocation, rectangle.Size);
                var newDistance = newRectangle.Center().GetDistanceSquareTo(center);

                if (newDistance >= oldDistance)
                    break;
                
                if (!QuadTree.HasContent(newRectangle))
                    rectangle = newRectangle;


                oldLocation = rectangle.Location;
                oldDistance = newDistance;
            }

            return rectangle;
        }
    }
}
