using Microsoft.Extensions.DependencyInjection;
using TagsCloudLib.Abstractions;
using TagsCloudLib.Abstractions.Factories;
using TagsCloudLib.Abstractions.Visualizer;
using TagsCloudLib.Implementations;
using TagsCloudLib.Implementations.Factories;
using TagsCloudLib.Implementations.Visualizer;

namespace TagsCloudLib;

public static class ServiceRegistration
{
    public static IServiceCollection AddTagCloudServices(this IServiceCollection services,
        TagCloudVisualizationConfig config)
    {
        services.AddSingleton(config);

        services.AddSingleton<ISpiralFactory, SpiralFactory>();
        services.AddSingleton<ICenterShifter, CenterShifter>();
        services.AddSingleton<ILayouterFactory, LayouterFactory>();

        services.AddScoped<IBoundingBoxCalculator, BoundingBoxCalculator>();
        services.AddScoped<ICanvasSizeCalculator, CanvasSizeCalculator>();
        services.AddScoped<ICoordinateTransformer, CoordinateTransformer>();
        services.AddScoped<IRender, BitmapRender>();
        services.AddScoped<IScalingCalculator, ScalingCalculator>();
        services.AddScoped<TagCloudVisualizer>();

        return services;
    }
}