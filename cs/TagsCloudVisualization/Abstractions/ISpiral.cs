using System.Drawing;

namespace TagsCloudVisualization.Abstractions
{
    public interface ISpiral
    {
        public IEnumerable<Point> GetPoints();

    }
}
