namespace CircularCloudLayouter.WeightedLayouter.Forming;

public static class FormFactors
{
    public static readonly FormFactor Square = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, distToCenter) => 1d / weight
    );

    public static readonly FormFactor Circle = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, distToCenter) => 1d / (weight * weight + distToCenter * distToCenter)
    );

    public static readonly FormFactor FourLeafClover = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, distToCenter) => weight / (weight * weight + distToCenter * distToCenter)
    );

    public static readonly FormFactor X = new(
        PreferredStartCalculators.CloserToEdges,
        (weight, distToCenter) => distToCenter / (weight * weight + distToCenter * distToCenter)
    );

    public static readonly FormFactor Cross = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, distToCenter) => 1d / (Math.Pow(weight, 4) + Math.Pow(distToCenter, 5))
    );

    // Понятия не имею что это, но выглядит интересно
    public static readonly FormFactor Something = new(
        PreferredStartCalculators.CloserToMiddle,
        (weight, distToCenter) => 1d / (-weight + distToCenter)
    );
}