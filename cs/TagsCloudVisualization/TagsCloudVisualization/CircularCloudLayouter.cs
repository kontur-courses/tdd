using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter(Point center) : ICircularCloudLayouter
    {
        public Point Center { get; } = center;

        private const double ANGLE_STEP = Math.PI / 100;
        private const double RADIUS_DELTA = 2;

        public Rectangle PutNextRectangle(Size rectangleSize, ICollection<Rectangle> existingRectangles)
        {
            ArgumentNullException.ThrowIfNull(existingRectangles);

            Rectangle rectangle;
            var iteration = 0;
            do
            {
                var angle = iteration * ANGLE_STEP;
                var radius = RADIUS_DELTA * angle / (2 * Math.PI);
                var position = new Point(
                    Center.X + (int)Math.Round(Math.Cos(angle) * radius),
                    Center.Y + (int)Math.Round(Math.Sin(angle) * radius));

                rectangle = new Rectangle(position, rectangleSize);
                iteration += 1;
            }
            while (existingRectangles.Any(r => r.IntersectsWith(rectangle)));
            existingRectangles.Add(rectangle);

            return rectangle;
        }
    }
}
