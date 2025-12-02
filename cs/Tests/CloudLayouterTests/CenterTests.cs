using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Geometry.Extensions;
using TagsCloudVisualization.Layouters;

namespace Tests.CloudLayouterTests;

public class CenterTests : CircularCloudLayouterTestBase
{
    [TestCaseSource(nameof(CenterTestCases))]
    public void PutNextRectangle_DifferentCenters_ShouldPlaceFirstRectangleInCenter(Point center)
    {
        var spiral = new ArchimedeanSpiral(center);
        Layouter = new SpiralCloudLayouter(spiral);

        foreach (var size in SizesGenerator.GenerateRandomRectangles(10, 123)) Layouter.PutNextRectangle(size);

        var rectangle = Layouter.Rectangles.First();

        rectangle.GetCenter().Should().BeEquivalentTo(center);
    }

    private static IEnumerable<Point> GetTestCenters()
    {
        yield return new Point(0, 0);
        yield return new Point(1000, 500);
        yield return new Point(-100, -100);
    }

    private static IEnumerable<TestCaseData> CenterTestCases
    {
        get
        {
            foreach (var center in GetTestCenters())
                yield return new TestCaseData(center)
                    .SetName($"FirstRectangle_ShouldPlaceInCenter_({center.X},{center.Y})");
        }
    }
}