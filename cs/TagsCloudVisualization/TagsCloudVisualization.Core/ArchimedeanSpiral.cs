using System.Drawing;

namespace TagsCloudVisualization.Core
{
    public class ArchimedeanSpiral
    {
        private readonly Point _center;
        private readonly HashSet<Point> _oldPoints = new HashSet<Point>();

        public ArchimedeanSpiral(Point center)
        {
            _center = center;
        }
        public Point GetNextPoint()
        {
            throw new NotImplementedException();
        }
    }
}
