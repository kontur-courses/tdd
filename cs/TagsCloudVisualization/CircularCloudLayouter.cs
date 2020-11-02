using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;
        private double maxDiagonalOfRectangleAtCurrentTurn;
        private double coefOfSpiralEquation;
        private int turnNumber;
        private Rectangle lastRectangle;
        private double anglePhi;
        private double deltaOfAnglePhi = Math.PI / 36;

        public CircularCloudLayouter(Point point)
        {
            center = point;
            anglePhi = 2 * Math.PI;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException();
            if (turnNumber == 0)
            {
                turnNumber++;
                return PutFirstRectangle(rectangleSize);
            }

            var rectangle = lastRectangle;
            while (RectanglesAreCrossed(rectangle, lastRectangle))
            {
                if (anglePhi >= 2 * Math.PI * turnNumber)
                {
                    var distanceBetweenCenterAndRectangleAtPreviousTurn =
                        2 * Math.PI * coefOfSpiralEquation * (turnNumber - 1);
                    coefOfSpiralEquation =
                        (maxDiagonalOfRectangleAtCurrentTurn + 5 + distanceBetweenCenterAndRectangleAtPreviousTurn) /
                        (2 * Math.PI * turnNumber);
                    turnNumber++;
                    maxDiagonalOfRectangleAtCurrentTurn = 0;
                }
                var coordinates = GetCoordinatesOfRectangle(rectangleSize);
                rectangle = new Rectangle(new Point(coordinates.Item1, coordinates.Item2), rectangleSize);
                anglePhi += deltaOfAnglePhi;
            }
            var diagonal = GetDiagonalOfRectangle(rectangle);
            if (diagonal > maxDiagonalOfRectangleAtCurrentTurn)
                maxDiagonalOfRectangleAtCurrentTurn = diagonal;
            lastRectangle = rectangle;
            return rectangle;
        }

        private Tuple<int, int> GetCoordinatesOfRectangle(Size rectangleSize)
        {
            var x = (int)Math.Round(coefOfSpiralEquation * anglePhi * Math.Cos(anglePhi) + center.X);
            var y = (int)Math.Round(coefOfSpiralEquation * anglePhi * Math.Sin(anglePhi) + center.Y);
            if (x >= center.X && y <= center.Y)
            {
                y -= rectangleSize.Height;
            }
            else if (x <= center.X && y <= center.Y)
            {
                y -= rectangleSize.Height;
                x -= rectangleSize.Width;

            }
            else if (x <= center.X && y >= center.Y)
            {
                x -= rectangleSize.Width;
            }

            return Tuple.Create(x, y);
        }

        private double GetDiagonalOfRectangle(Rectangle rectangle)
        {
            return Math.Sqrt(rectangle.Height * rectangle.Height + rectangle.Width * rectangle.Width);
        }

        private bool RectanglesAreCrossed(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (Math.Min(rectangle1.Right, rectangle2.Right) - Math.Max(rectangle1.Left, rectangle2.Left) >= 0)
                   && (Math.Min(rectangle1.Bottom, rectangle2.Bottom) - Math.Max(rectangle1.Top, rectangle2.Top) >= 0);
        }

        private Rectangle PutFirstRectangle(Size rectangleSize)
        {
            var x = center.X;
            var y = center.Y;
            var rectangle = new Rectangle(new Point(x, y), rectangleSize);
            maxDiagonalOfRectangleAtCurrentTurn = GetDiagonalOfRectangle(rectangle);
            lastRectangle = rectangle;
            return rectangle;
        }
    }
}
