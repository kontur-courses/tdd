using System.Drawing;

namespace TagCloud;

public interface ICloudShaper
{
    Rectangle GetNextPossibleRectangle(Size size);
}