using System.Drawing;

namespace ProjectCircularCloudLayouter
{
    public interface ISpiral
    {
        Point Center { get; }
        Point GetNewSpiralPoint();
    }
}