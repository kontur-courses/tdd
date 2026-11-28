using System.Drawing;
using TagsCloudVisualization.Abstractions;

namespace TagsCloudVisualization
{
    public sealed class PlacementStrategy : IPlacementStrategy
    {
        private readonly int maxSpiralSteps;

        public PlacementStrategy(int maxSpiralSteps = 100_000)
        {
            if (maxSpiralSteps <= 0) throw new ArgumentOutOfRangeException(nameof(maxSpiralSteps));
            this.maxSpiralSteps = maxSpiralSteps;
        }

        public Rectangle FindPosition(
            Size size,
            List<Rectangle> placed,
            ISpiral spiral,
            Point center)
        {
            throw new NotImplementedException();
        }


    }
}