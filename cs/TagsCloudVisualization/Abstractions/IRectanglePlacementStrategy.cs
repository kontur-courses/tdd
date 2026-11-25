using System.Drawing;

namespace TagsCloudVisualization.Abstractions
{
    public interface IRectanglePlacementStrategy
    {
        public Rectangle FindPosition(
            Size size,
            List<Rectangle> placed,
            ISpiral spiral,
            Point center
            );
    }
}
