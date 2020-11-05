using System.Drawing;

namespace TagCloud
{
    public interface IDiscreteCurve
    {
        void Next();
        Point CurrentPoint { get; }
    }
}