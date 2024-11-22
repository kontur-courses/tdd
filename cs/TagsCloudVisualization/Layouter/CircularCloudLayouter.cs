using SkiaSharp;

namespace TagsCloudVisualization.Layouter
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly List<SKRect> rectangles;
        private readonly SKPoint center;
        private double angle;
        private const double Step = 0.1;

        public CircularCloudLayouter(SKPoint center)
        {
            rectangles = new List<SKRect>();
            this.center = center;
        }

    public SKRect PutNextRectangle(SKSize rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive", nameof(rectangleSize));

        SKRect rectangle;

        do
        {
            var centerOfRectangle = GetNextPosition();
            var rectanglePosition = new SKPoint(centerOfRectangle.X - rectangleSize.Width / 2,
                centerOfRectangle.Y - rectangleSize.Height / 2);
            rectangle = new SKRect(
                rectanglePosition.X,
                rectanglePosition.Y,
                rectanglePosition.X + rectangleSize.Width,
                rectanglePosition.Y + rectangleSize.Height);
        } while (rectangles.Any(r => r.IntersectsWith(rectangle)));

            rectangles.Add(rectangle);
            return rectangle;
        }

        public SKRect[] GetRectangles()
        {
            return rectangles.ToArray();
        }

        private SKPoint GetNextPosition()
        {
            var radius = Step * angle;
            var x = (float)(center.X + radius * Math.Cos(angle));
            var y = (float)(center.Y + radius * Math.Sin(angle));

            angle += Step;

            return new SKPoint(x, y);
        }
    }
}