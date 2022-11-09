using System.Drawing;

namespace TagsCloudVisualization
{
    //Позиции для прямоугольников выбираем по спирали, которая задаётся формулой: r = a + (b * angle)
    public class CircularCloudLayouter
    {
        private readonly Point _center;
        private readonly ArchimedeanSpiral _archimedeanSpiral;
        private List<Rectangle> _rectangles = new();
        public CircularCloudLayouter(Point center, double step, double density, double start)
        {
            _center = center;
            _archimedeanSpiral = new ArchimedeanSpiral(step, density, start);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException("Width and Height must be positive!");
            }

            foreach (var rectangleCenter in _archimedeanSpiral.GetNextSpiralPoint())
            {
                var applicantLocation = CalculateRectanglePosition(rectangleCenter, rectangleSize);
                var applicantRectangle = new Rectangle(applicantLocation, rectangleSize);
                if (!applicantRectangle.CheckForIntersectionWithRectangles(_rectangles))
                {
                    _rectangles.Add(applicantRectangle);
                    return applicantRectangle;
                }
            }

            throw new ArgumentException("Rectangle doesnt fit in circle");
        }

        private Point CalculateRectanglePosition(Point rectangleCenter, Size rectangleSize)
        {
            var X = (_center.X + rectangleCenter.X) - rectangleSize.Width / 2;
            var Y = (_center.Y + rectangleCenter.Y) - rectangleSize.Height / 2;

            return new Point(X, Y);
        }

        
    }
}

