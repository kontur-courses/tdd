using System.Drawing;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualization.TagCloud
{
    public interface ICloudLayouter
    {
        public Point Center { get; }
        public IPointGenerator PointGenerator { get; }
        public int AddedRectanglesCount { get; }
        public Rectangle PutNextRectangle(Size rectangleSize);
        public Rectangle GetAddedRectangle(int index);
    }
}