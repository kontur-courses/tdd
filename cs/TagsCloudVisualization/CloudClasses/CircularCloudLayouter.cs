using System.Drawing;
using System.Runtime.CompilerServices;
using TagsCloudVisualization.CloudClasses.Interfaces;

namespace TagsCloudVisualization.CloudClasses
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public readonly IRayMover RayMover;

        public List<Rectangle> Rectangles => rectangles;

        public CircularCloudLayouter(IRayMover rayMover)
        {
            RayMover = rayMover;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("The height and width of the Rectangle must be greater than 0");

            foreach (var point in RayMover.MoveRay())
            {
                var location = new Point(point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2);

                var rectangle = new Rectangle(location, rectangleSize);

                // Проверяем, пересекается ли новый прямоугольник с уже существующими
                if (!rectangles.Any(r => r.IntersectsWith(rectangle)))
                {
                    rectangles.Add(rectangle);
                    return rectangle;
                }
            }

            throw new InvalidOperationException("No suitable location found for the rectangle.");
        }
    }
}
