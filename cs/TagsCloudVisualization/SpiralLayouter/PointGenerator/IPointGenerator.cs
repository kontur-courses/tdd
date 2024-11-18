using System.Drawing;

namespace TagsCloudVisualization.SpiralLayouter.PointGenerator;

public interface IPointGenerator
{
    /*
     * Infinite point generator
     */
    public IEnumerable<Point> StartFrom(Point startPoint);
}