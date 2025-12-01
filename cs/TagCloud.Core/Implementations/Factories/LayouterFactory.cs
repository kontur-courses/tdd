using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudLib.Abstractions;
using TagsCloudLib.Abstractions.Factories;

namespace TagsCloudLib.Implementations.Factories;

public class LayouterFactory(IServiceProvider provider) : ILayouterFactory
{
    public CircularCloudLayouterBase Create(Point center, int maxAttempts, double spiralStep = 0.2, double radiusStep = 0.5)
    {
        var spiral = provider.GetRequiredService<ISpiralFactory>();
        var shifter = provider.GetRequiredService<ICenterShifter>();
        return new CircularCloudLayouter(center, spiral.Create(center, spiralStep, radiusStep), shifter, maxAttempts);
    }
}
