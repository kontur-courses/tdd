using System.Drawing;
using TagsCloudVisualization.Abstractions;
namespace TagsCloudVisualization
{
    public sealed class CentripetalCompactor : ICentripetalCompactor
    {
        private readonly int step;
        private readonly int maxIterations;
        public CentripetalCompactor(int step = 1, int maxIterations = 10_000)
        {
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));
            if (maxIterations <= 0) throw new ArgumentOutOfRangeException(nameof(maxIterations));
            this.step = step;
            this.maxIterations = maxIterations;
        }

        public Rectangle Compact(Rectangle candidate,
            List<Rectangle> placed,
            Point center)
        {
            throw new NotImplementedException();
        }
    }
}