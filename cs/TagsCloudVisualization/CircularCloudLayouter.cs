using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Structures;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Rectangle canvas;
        private readonly IEnumerator<Point> spiral;
        public readonly Point Center;
        public readonly List<Point> SpiralPoints;

        protected QuadTree QuadTree { get; }

        public CircularCloudLayouter(Size canvasSize)
        {
            Center = new Point(canvasSize / 2);
            canvas = GetRectangleAtPositionOfCenter(Center, canvasSize);
            spiral = new Spiral(Center, Math.PI / 360, 2).GetEnumerator();
            SpiralPoints = new List<Point>();
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

                if (QuadTree.IntersectsWith(rectangle))
                    continue;

                CheckIfRectangleIsOutsideOfCanvas(rectangle);

                for (var i = 0; i < 4; i++)
                    rectangle = TryShiftToCenter(rectangle, i % 2 == 0);

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
            var oldDistance = rectangle.Center().GetDistanceSquareTo(Center);
            var oldLocation = rectangle.Location;
            while (true)
            {
                var newLocation = oldLocation;

                if (isVertical)
                    newLocation.Y += Math.Sign(Center.Y - rectangle.Center().Y);
                else
                    newLocation.X += Math.Sign(Center.X - rectangle.Center().X);


                var newRectangle = new Rectangle(newLocation, rectangle.Size);
                var newDistance = newRectangle.Center().GetDistanceSquareTo(Center);

                if (newDistance >= oldDistance)
                    break;
                
                if (!QuadTree.IntersectsWith(newRectangle))
                    rectangle = newRectangle;


                oldLocation = rectangle.Location;
                oldDistance = newDistance;
            }

            return rectangle;
        }
    }
}
