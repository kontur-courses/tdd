using System.Drawing;

namespace TagsCloudLib.Abstractions.Factories;

public interface ISpiralFactory
{
    ISpiral Create(Point center, double spiralStep = 0.2, double radiusStep = 0.5);
}