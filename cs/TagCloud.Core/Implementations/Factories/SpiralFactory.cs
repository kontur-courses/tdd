using System.Drawing;
using TagsCloudLib.Abstractions;
using TagsCloudLib.Abstractions.Factories;

namespace TagsCloudLib.Implementations.Factories;

public class SpiralFactory() : ISpiralFactory
{
    public ISpiral Create(Point center, double spiralStep = 0.2, double radiusStep = 0.5)
    {
        return new ArchimedeanSpiral(center, spiralStep, radiusStep);
    }
}