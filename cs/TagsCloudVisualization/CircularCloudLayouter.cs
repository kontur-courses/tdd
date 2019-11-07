using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Point center;
        private readonly ArchimedeanSpiral archimedeanSpiral;
        public int CloudRightBorder { get; private set; }
        public int CloudBottomBorder { get; private set; }
        public int CloudLeftBorder { get; private set; }
        public int CloudTopBorder { get; private set; }

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            archimedeanSpiral = new ArchimedeanSpiral(center);
            CloudRightBorder = 0;
            CloudTopBorder = 0;
            CloudLeftBorder = int.MaxValue;
            CloudTopBorder = int.MaxValue;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var point = archimedeanSpiral.GetNextPoint();
            var checkedRectangle = new Rectangle(point, rectangleSize);
            while (!IsCorrectToPlace(checkedRectangle))
            {
                point = archimedeanSpiral.GetNextPoint();
                checkedRectangle = new Rectangle(point, rectangleSize);
            }

            var adjustedRectangle = AdjustRectangle(checkedRectangle);
            UpdateBorders(adjustedRectangle);
            rectangles.Add(adjustedRectangle);
            return adjustedRectangle;
        }

        public List<Rectangle> GetRectangles()
        {
            return rectangles
                .Select(rectangle => new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height))
                .ToList();
        }

        private void UpdateBorders(Rectangle newRectangle)
        {
            CloudRightBorder = Math.Max(CloudRightBorder, newRectangle.Right);
            CloudLeftBorder = Math.Min(CloudLeftBorder, newRectangle.X);
            CloudBottomBorder = Math.Max(CloudBottomBorder, newRectangle.Bottom);
            CloudTopBorder = Math.Min(CloudTopBorder, newRectangle.Y);
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
    }
}