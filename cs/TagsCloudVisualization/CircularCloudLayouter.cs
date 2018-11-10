using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double SpiralAngleInterval = 0.1;
        private const double MaxSpiralAngle = double.MaxValue;
        private const double SpiralTurnsInterval = 0.5;

        private Point origin;
        private List<Rectangle> rectanglesList;
        private int width, height;
        private double currentSpiralAngle;

        public CircularCloudLayouter(Point origin)
        {
            this.origin = origin;
            rectanglesList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = PutOnSpiral(rectangleSize);
            if (rectangle == null)
                return null;
            rectangle = MakeCloserToCenter(rectangle);
            rectanglesList.Add(rectangle);
            UpdateWidthAndHeight(rectangle);
            return rectangle;
        }

        public void Visualize(string path)
            => Visualizator.VizualizeToFile(rectanglesList, width, height, path);

        private Rectangle MakeCloserToCenter(Rectangle rectangle)
        {
            var directionToCenter = new Vector(rectangle.Center, origin).Normalized();
            var currentDirection = directionToCenter;
            var previousPosition = new Point(0, 0);
            while (directionToCenter.IsCollinear(currentDirection) 
                   && !rectangle.IsIntersectsWithAnyRect(rectanglesList))
            {
                previousPosition = rectangle.Center;
                rectangle.Center += directionToCenter;
                currentDirection = new Vector(rectangle.Center, origin).Normalized();
            }

            rectangle.Center = previousPosition;
            return rectangle;
        }

        private void UpdateWidthAndHeight(Rectangle rectangle)
        {
            var newWidth = Math.Abs(rectangle.RightXCoord) > Math.Abs(rectangle.LeftXCoord) ? Math.Abs(rectangle.RightXCoord) : Math.Abs(rectangle.LeftXCoord);
            var newHeight = Math.Abs(rectangle.TopYCoord) > Math.Abs(rectangle.BottomYCoord) ? Math.Abs(rectangle.TopYCoord) : Math.Abs(rectangle.BottomYCoord);
            if (newWidth > width)
                width = (int)newWidth;
            if (newHeight > height)
                height = (int)newHeight;
        }

        private Rectangle PutOnSpiral(Size rectangleSize)
        {
            var newRectangle = new Rectangle(origin, origin, rectangleSize);
            for (var angle = currentSpiralAngle; angle < MaxSpiralAngle; angle += SpiralAngleInterval)
            {
                currentSpiralAngle += SpiralAngleInterval;
                var rectCenter = ArithmeticSpiral(angle, SpiralTurnsInterval);
                newRectangle.Center = rectCenter;
                if (!newRectangle.IsIntersectsWithAnyRect(rectanglesList))
                    return newRectangle;
            }

            return null;
        }

        private Point ArithmeticSpiral(double angle, double turnsInterval)
        {
            var x = (origin.X + turnsInterval * angle) * Math.Cos(angle);
            var y = (origin.Y + turnsInterval * angle) * Math.Sin(angle);

            return new Point(x, y);
        }
    }
}
