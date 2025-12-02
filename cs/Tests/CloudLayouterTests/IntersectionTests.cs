using System.Drawing;
using FluentAssertions;

namespace Tests.CloudLayouterTests;

public class IntersectionTests : CircularCloudLayouterTestBase
{
    [TestCaseSource(nameof(IntersectionRectanglesTestCases))]
    public void PutNextRectangle_ManyRectangles_ShouldNotIntersects(IEnumerable<Size> sizes)
    {
        foreach (var size in sizes) Layouter.PutNextRectangle(size);

        var rectangles = Layouter.Rectangles.ToList();

        foreach (var r1 in rectangles)
        foreach (var r2 in rectangles.Where(r2 => r2 != r1))
            r1.IntersectsWith(r2).Should().BeFalse();
    }

    private static IEnumerable<TestCaseData> IntersectionRectanglesTestCases
    {
        get
        {
            yield return new TestCaseData(
                    SizesGenerator.GenerateSquares(100, TestSeed))
                .SetName("PlacingSquares_ShouldNotCauseIntersections");
            yield return new TestCaseData(
                    SizesGenerator.GenerateTallRectangles(100, TestSeed))
                .SetName("PlacingTallRectangles_ShouldNotCauseIntersections");
            yield return new TestCaseData(
                    SizesGenerator.GenerateLongRectangles(100, TestSeed))
                .SetName("PlacingLongRectangles_ShouldNotCauseIntersections");
            yield return new TestCaseData(
                    SizesGenerator.GenerateRandomRectangles(100, TestSeed))
                .SetName("PlacingRandomRectangles_ShouldNotCauseIntersections");
            yield return new TestCaseData(
                    SizesGenerator.GenerateBigRectangles(100, TestSeed))
                .SetName("PlacingBigRectangles_ShouldNotCauseIntersections");
        }
    }
}