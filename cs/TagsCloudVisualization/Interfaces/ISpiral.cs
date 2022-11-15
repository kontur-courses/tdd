using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ISpiral
{
    Point GetCartesianPoint(int degree);
}