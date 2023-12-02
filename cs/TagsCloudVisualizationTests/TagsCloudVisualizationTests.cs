using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter circularCloudLayouter;

    [SetUp]
    public void CircularCloudLayouterSetUp()
    {
        var dict = WordsDataSet.CreateFrequencyDict(
            "../../../../TagsCloudVisualization/words.txt"
        );

        circularCloudLayouter = new CircularCloudLayouter(dict);
    }

    [TestCaseSource(typeof(TagsCloudVisualizationTestData),
        nameof(TagsCloudVisualizationTestData.RectanglesIntersection))]
    public void IntersectWithPlaced_ShouldReturn(
        Dictionary<string, Rectangle> rectanglesWithWords, Rectangle target, bool result)
    {
        CircularCloudLayouter.IntersectWithPlaced(rectanglesWithWords, target).Should().Be(result);
    }

    [TestCaseSource(typeof(TagsCloudVisualizationTestData), nameof(TagsCloudVisualizationTestData.PutRectangle))]
    public void PutNextRectangle_ShouldReturn(List<Point> coordinates, bool[] results)
    {
        for (var i = 0; i < coordinates.Count; i++)
            circularCloudLayouter.PutNextRectangle(coordinates[i]).Should().Be(results[i]);
    }

    [TestCaseSource(typeof(TagsCloudVisualizationTestData), nameof(TagsCloudVisualizationTestData.Algorithm))]
    public void AlgorithmShould(string filePath, bool result)
    {
        var dict = WordsDataSet.CreateFrequencyDict(filePath);

        var algorithmData = new CircularCloudLayouter(dict).Algorithm();

        (algorithmData.Count == dict.Count).Should().Be(result);
    }

    [Test]
    public void AlgorithmComplexity_LessOrEqualQuadratic()
    {
        // TODO
    }
}