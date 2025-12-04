using System.Drawing;

namespace TagCloud.Shapes;
public interface ICloudShape
{
    Point Center { get; }
    bool IntersectsWith(ICloudShape otherShape);
    ICloudShape Shift(int dx, int dy);
}