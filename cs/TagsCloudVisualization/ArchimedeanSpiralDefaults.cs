using System.Drawing;

namespace TagsCloudVisualization;

public static class ArchimedeanSpiralDefaults
{
    private const double AngleStepRadians = 0.1;
    private const double RadiusCoefficient = 1.0;
    private const double StartRadius = 1.0;

    public static ArchimedeanSpiral Create(Point center) =>
        new(AngleStepRadians, RadiusCoefficient, StartRadius, center);
}