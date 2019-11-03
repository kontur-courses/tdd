using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    interface IFigure
    {
        Point Center { get; set; }
        Point Location { get; set; }
        bool Contains(Point point);
        bool IntersectsWith(IFigure figure);
    }
}
