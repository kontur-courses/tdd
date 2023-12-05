using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter(Point centerPoint)
    {
        private readonly List<Rectangle> rectangles = new();
        private readonly ArchimedeanSpiral spiral = new(centerPoint);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("rectangleSize with zero or negative height or width is prohibited!");
            while (true)
            {
                var rectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
                if (rectangles.Any(x => x.IntersectsWith(rectangle))) continue;
                rectangles.Add(rectangle);
                break;
            }

            return rectangles[^1];
        }

        public IReadOnlyList<Rectangle> GetRectangles() => rectangles.AsReadOnly();
    }
}