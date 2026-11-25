using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Density;
using TagsCloudVisualization.Spiral;

namespace TagsCloudVisualization.Layout
{
    public class CircularCloudLayouter : ILayouter
    {
        private readonly PointF center;
        private readonly ISpiral spiral;
        private readonly IRectangleFCompactor compactor;
        private readonly List<RectangleF> placedRectangles;

        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
            spiral = new ArchimedeanSpiral(center);
            compactor = new CenterwardCompactor();
            placedRectangles = new();
        }

        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            EnsureValidSize(rectangleSize);

            var candidate = FindFreePosition(rectangleSize);
            var compacted = compactor.Compact(candidate, placedRectangles, center);

            placedRectangles.Add(compacted);

            return compacted;
        }

        private RectangleF FindFreePosition(SizeF rectangleSize)
        {
            while (true)
            {
                var point = spiral.GetNextPoint();

                var rectangleCentre = new PointF(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2);
                var candidate = new RectangleF(rectangleCentre, rectangleSize);

                if (!placedRectangles.Select(rectangle => rectangle.IntersectsWith(candidate)).Any())
                    return candidate;
            }
        }

        private void EnsureValidSize(SizeF rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException(
                    $"Rectangle size must be positive, got {rectangleSize}.");
            }
        }
    }
}
