namespace CircularCloudLayouter.Segments;

public record WeightedSegmentsOptimizationOptions(int MaxLengthToRemove, int MaxWeightDeltaToCombine)
{
    public static readonly WeightedSegmentsOptimizationOptions Default = new(0, 0);
}