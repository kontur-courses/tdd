using System.Drawing;

namespace TagsCloudVisualization.Abstractions
{
    public interface IPlacementStrategy
    {
        public Rectangle FindPosition(
            Size size,
            List<Rectangle> placed,
            ISpiral spiral,
            Point center
            );
    }
}
