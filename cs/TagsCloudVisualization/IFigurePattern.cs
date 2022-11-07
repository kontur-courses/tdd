using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface IFigurePattern
    {
        Point GetNextPoint();
        void Restart();
    }
}