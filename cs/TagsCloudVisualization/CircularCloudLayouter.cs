using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : TagCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Points spiralPoints;

        public CircularCloudLayouter(Point center) : base(center) =>
            spiralPoints = new SpiralPoints(center);

        public override Rectangle PutNextRectangle(Size rectangleSize)
        {
            var halfRectangleSize = new Size(rectangleSize.Width / 2, rectangleSize.Height / 2);
            foreach (var point in spiralPoints.GetPoints())
            {
                var rectangle = new Rectangle(point - halfRectangleSize, rectangleSize);
                if (rectangles.Any(x => x.IntersectsWith(rectangle)))
                    continue;

                rectangles.Add(rectangle);
                return rectangle;
            }

            return new Rectangle();
        }
    }
}