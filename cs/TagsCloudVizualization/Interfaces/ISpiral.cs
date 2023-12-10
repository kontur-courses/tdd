using System.Drawing;

namespace TagsCloudVizualization.Interfaces;

public interface ISpiral
{
    public IEnumerable<Point> GetPointsOnSpiral();
}