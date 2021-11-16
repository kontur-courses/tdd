using System.Drawing;

namespace TagsCloudVisualization.Layouters
{
    internal interface IPointLayouter
    {
        public PointF CurrentPoint { get; }
        public void GetNextPoint();
    }
}