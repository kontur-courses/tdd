using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators
{
    public interface IPointGenerator
    {
        public Point Center { get; }
        public Size CanvasSize { get; }
        public Point GetNextPoint();
        public void StartOver();
    }
}