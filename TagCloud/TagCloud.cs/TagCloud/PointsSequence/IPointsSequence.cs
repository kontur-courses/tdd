using System.Drawing;

namespace TagCloud
{
    public interface IPointsSequence
    {
        Point GetNextPoint();

        void Reset();

        void SetCenter(Point center);
    }
}