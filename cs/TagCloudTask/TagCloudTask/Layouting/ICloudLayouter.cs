using System.Drawing;

namespace TagCloudTask.Layouting
{
    public interface ICloudLayouter : ILayouter
    {
        Point Center { get; }

        int GetCloudBoundaryRadius();
    }
}