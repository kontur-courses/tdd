using System.Drawing;

public interface IPointsSequence
{
    Point GetNextPoint();

    void Reset();
}