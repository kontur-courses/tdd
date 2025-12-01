using System.Drawing;

namespace TagsCloudLib.Abstractions.Factories;

public interface ILayouterFactory
{
    CircularCloudLayouterBase Create(Point center, int maxAttempts = 1500, double spiralStep = 0.2, double radiusStep = 0.5);
}
