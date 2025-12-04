using System.Drawing;

namespace TagCloud;

public interface ISpiral
{
    IEnumerable<Point> GetPointsOnSpiral();
}