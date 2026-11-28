using System.Drawing;
using TagsCloudVisualization.Abstractions;
namespace TagsCloudVisualization
{
    public sealed class ArchimedeanSpiral : ISpiral
    {
        private readonly Point center;
        private readonly double radiusStep;
        private readonly double angleStep;

        public ArchimedeanSpiral(Point center, double radiusStep = 0.5, double angleStep = Math.PI / 32)
        {
            if (radiusStep <= 0) throw new ArgumentOutOfRangeException(nameof(radiusStep));
            if (angleStep <= 0) throw new ArgumentOutOfRangeException(nameof(angleStep));

            this.center = center;
            this.radiusStep = radiusStep;
            this.angleStep = angleStep;
        }

        public IEnumerable<Point> GetPoints()
        {
            throw new NotImplementedException();

        }
    }
}