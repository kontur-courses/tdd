using System.Drawing;

namespace TagsCloudVisualization;

public interface ISpiral
{
    Point GetCartesianPoint(int degree);
}