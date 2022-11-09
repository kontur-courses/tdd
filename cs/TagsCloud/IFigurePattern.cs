using System.Drawing;

namespace TagsCloud
{
    internal interface IFigurePattern
    {
        Point GetNextPoint();
        void Restart();
    }
}