namespace CircularCloudLayouter.WeightedLayouter.Forming;

public class FormFactor
{
    private readonly Func<int, int, int, int, int> _preferredStartCalculator;
    private readonly Func<int, double, double> _segmentScoreCalculator;

    public FormFactor(
        Func<int, int, int, int, int> preferredStartCalculator,
        Func<int, double, double> segmentScoreCalculator,
        double widthToHeightRatio = 1
    )
    {
        _preferredStartCalculator = preferredStartCalculator ??
                                    throw new ArgumentNullException(nameof(preferredStartCalculator));
        _segmentScoreCalculator = segmentScoreCalculator ??
                                  throw new ArgumentNullException(nameof(segmentScoreCalculator));
        WidthToHeightRatio = widthToHeightRatio;
    }

    public FormFactor WithRatio(double widthToHeightRatio) =>
        new(_preferredStartCalculator, _segmentScoreCalculator, widthToHeightRatio);

    public double WidthToHeightRatio { get; }

    public int GetPreferredStart(int min, int max, int sideLength, int middle)
    {
        if (max - min < sideLength)
            throw new ArgumentException("Not enough space to place side!");
        return _preferredStartCalculator(min, max, sideLength, middle);
    }

    public double GetSegmentScore(int weight, double distToCenter) =>
        _segmentScoreCalculator(weight, distToCenter);
}