using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Geometry.Extensions;

namespace Tests.CloudLayouterTests;

public class DensityTests : CircularCloudLayouterTestBase
{
    [TestCaseSource(nameof(DensityRectanglesTestCases))]
    public void PutNextRectangle_ManyRectangles_ShouldTightlyDistribute(IEnumerable<Size> sizes, double expectedDensity)
    {
        foreach (var size in sizes) Layouter.PutNextRectangle(size);

        var rectangles = Layouter.Rectangles.ToList();
        var rectanglesArea = rectangles.Select(r => r.GetArea()).Sum();

        var circumscribedCircleRadius = GetCircumscribedCircleRadius(Center, rectangles);
        var circumscribedCircleArea = circumscribedCircleRadius * circumscribedCircleRadius * Math.PI;

        var actualDensityCoefficient = rectanglesArea / circumscribedCircleArea;

        actualDensityCoefficient.Should().BeGreaterThanOrEqualTo(expectedDensity);
    }

    private static IEnumerable<TestCaseData> DensityRectanglesTestCases
    {
        get
        {
            yield return new TestCaseData(
                    SizesGenerator.GenerateSquares(10, TestSeed), 0.3)
                .SetName("Placing10Squares_ShouldAchieveAtLeast30PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateTallRectangles(10, TestSeed), 0.3)
                .SetName("Placing10TallRectangles_ShouldAchieveAtLeast30PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateLongRectangles(10, TestSeed), 0.3)
                .SetName("Placing10LongRectangles_ShouldAchieveAtLeast30PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateRandomRectangles(10, TestSeed), 0.3)
                .SetName("Placing10RandomRectangles_ShouldAchieveAtLeast30PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateBigRectangles(10, TestSeed), 0.3)
                .SetName("Placing10BigRectangles_ShouldAchieveAtLeast30PercentDensity");

            yield return new TestCaseData(
                    SizesGenerator.GenerateSquares(100, TestSeed), 0.48)
                .SetName("Placing100Squares_ShouldAchieveAtLeast48PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateTallRectangles(100, TestSeed), 0.48)
                .SetName("Placing100TallRectangles_ShouldAchieveAtLeast48PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateLongRectangles(100, TestSeed), 0.48)
                .SetName("Placing100LongRectangles_ShouldAchieveAtLeast48PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateRandomRectangles(100, TestSeed), 0.48)
                .SetName("Placing100RandomRectangles_ShouldAchieveAtLeast48PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateBigRectangles(100, TestSeed), 0.48)
                .SetName("Placing100BigRectangles_ShouldAchieveAtLeast48PercentDensity");

            yield return new TestCaseData(
                    SizesGenerator.GenerateSquares(1000, TestSeed), 0.64)
                .SetName("Placing1000Squares_ShouldAchieveAtLeast64PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateTallRectangles(1000, TestSeed), 0.64)
                .SetName("Placing1000TallRectangles_ShouldAchieveAtLeast64PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateLongRectangles(1000, TestSeed), 0.64)
                .SetName("Placing1000LongRectangles_ShouldAchieveAtLeast64PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateRandomRectangles(1000, TestSeed), 0.64)
                .SetName("Placing1000RandomRectangles_ShouldAchieveAtLeast64PercentDensity");
            yield return new TestCaseData(
                    SizesGenerator.GenerateBigRectangles(1000, TestSeed), 0.64)
                .SetName("Placing1000BigRectangles_ShouldAchieveAtLeast64PercentDensity");
        }
    }

    private static double GetCircumscribedCircleRadius(Point center, IEnumerable<Rectangle> rectangles)
    {
        var radius = 0d;

        foreach (var rectangle in rectangles)
        foreach (var vertex in rectangle.GetVertices())
        {
            var distance = center.DistanceTo(vertex);
            radius = Math.Max(radius, distance);
        }

        return radius;
    }
}