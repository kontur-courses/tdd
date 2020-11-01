using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Infrastructure.Environment
{
    public class BruteForceCollisionDetector : ICollisionDetector<Rectangle>
    {
        private readonly Index<Rectangle> index;
        public BruteForceCollisionDetector(Index<Rectangle> index)
        {
            this.index = index;
        }
        public bool IsColliding(Rectangle rectangle)
        {
            return index.Any(placedRectangle => placedRectangle.IntersectsWith(rectangle));
        }
    }
}