using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> TagCloud { get; } = new();

        private double step = Math.PI / 100;
        private double radiusDelta = 2;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            var iteration = 0;
            do
            {
                var angle = iteration * step;
                var radius = radiusDelta * angle / (2 * Math.PI);
                var position = new Point(
                    Center.X + (int)Math.Round(Math.Cos(angle) * radius),
                    Center.Y + (int)Math.Round(Math.Sin(angle) * radius));

                rectangle = new Rectangle(position, rectangleSize);
                iteration += 1;
            }
            while (IsIntersect(rectangle));
            TagCloud.Add(rectangle);

            return rectangle;
        }

        private bool IsIntersect(Rectangle rectangle)
        {
            return TagCloud.Any(rect => rect.IntersectsWith(rectangle));
        }
    }
}
