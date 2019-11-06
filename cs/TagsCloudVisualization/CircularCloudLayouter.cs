using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {

        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private Point center;
        private readonly Spiral spiral;
        public int RightBorder { get; private set; }
        public int BottomBorder { get; private set; }
        public int LeftBorder { get; private set; }
        public int TopBorder { get; private set; }

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            spiral = new Spiral(center);
            RightBorder = 0;
            TopBorder = 0;
            LeftBorder = int.MaxValue;
            TopBorder = int.MaxValue;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var point = spiral.GetNextPoint();
            var checkedRectangle = new Rectangle(point, rectangleSize);
            while (!IsCorrectToPlace(checkedRectangle))
            {
                point = spiral.GetNextPoint();
                checkedRectangle = new Rectangle(point, rectangleSize);
            }
            var adjustedRectangle = AdjustRectangle(checkedRectangle);
            UpdateBorders(adjustedRectangle);
            rectangles.Add(adjustedRectangle);
            return adjustedRectangle;

        }
        private void UpdateBorders(Rectangle newRectangle)
        {
            RightBorder = Math.Max(RightBorder, newRectangle.X + newRectangle.Width);
            LeftBorder = Math.Min(LeftBorder, newRectangle.X);
            BottomBorder = Math.Max(BottomBorder, newRectangle.Y + newRectangle.Height);
            TopBorder = Math.Min(TopBorder, newRectangle.Y);
        }

        private Rectangle AdjustRectangle(Rectangle rectangle)
        {
            rectangle = MoveRectangleHorizontally(rectangle);
            rectangle = MoveRectangleVertically(rectangle);
            return rectangle;
        }
        private Rectangle MoveRectangleHorizontally(Rectangle rectangle)
        {
            var stepSize = rectangle.X < center.X ? 1 : -1;
            var checkedRectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            while (IsCorrectToPlace(checkedRectangle) && checkedRectangle.X != center.X)
            {
                checkedRectangle.X += stepSize;
            }
            if (!IsCorrectToPlace(checkedRectangle))
            {
                checkedRectangle.X -= stepSize;
            }

            return checkedRectangle;
        }

        private Rectangle MoveRectangleVertically(Rectangle rectangle)
        {
            var stepSize = rectangle.Y < center.Y ? 1 : -1;
            var checkedRectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            while (IsCorrectToPlace(checkedRectangle) && checkedRectangle.Y != center.Y)
            {
                checkedRectangle.Y += stepSize;
            }

            if (!IsCorrectToPlace(checkedRectangle))
            {
                checkedRectangle.Y -= stepSize;
            }

            return checkedRectangle;
        }


        private bool IsCorrectToPlace(Rectangle checkedRectangle)
        {
            if (checkedRectangle.X < 0 || checkedRectangle.Y < 0)
            {
                return false;
            }

            return rectangles.All(rectangle => !rectangle.IntersectsWith(checkedRectangle));
        }


        public List<Rectangle> GetRectangles()
        {
            return rectangles.Select(rectangle => new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)).ToList();
        }
    }

}
