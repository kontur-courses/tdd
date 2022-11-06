namespace CircularCloudLayouter.WeightedLayouter.Forming;

public static class StandardFormFactors
{
    public static readonly FormFactor Rectangle = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, _) => 1d / weight
    );

    public static readonly FormFactor Ellipse = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, distToCenter) => 1d / (weight * weight + 1.4 * distToCenter * distToCenter)
    );

    public static readonly FormFactor Cross = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, distToCenter) => weight / (weight * weight + 1.4 * distToCenter * distToCenter)
    );

    public static readonly FormFactor X = new(
        PreferredStartCalculators.CloserToEdges,
        (weight, distToCenter) => distToCenter / (weight * weight + distToCenter * distToCenter)
    );
}