using SixLabors.ImageSharp;

namespace TagCloud.CoordinatesProviders;

public interface ICoordinatesProvider
{
    public IEnumerable<PointF> GetPoints();
}