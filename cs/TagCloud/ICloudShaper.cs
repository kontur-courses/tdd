using System.Drawing;

namespace TagCloud;

public interface ICloudShaper
{
    Point GetNextPossiblePoint();
}