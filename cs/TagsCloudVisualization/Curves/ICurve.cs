using System.Drawing;

namespace TagsCloudVisualization.Curves
{
    public interface ICurve
    {
        Point GetPoint(double t);
    }
}