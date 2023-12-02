using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter circularCloudLayouter = null!;
    private const string FilePath = "testFile";

    [SetUp]
    public void CircularCloudLayouterSetUp()
    {
        var dict = WordsDataSet.CreateFrequencyDict(
            "../../../../TagsCloudVisualization/words.txt"
        );

        circularCloudLayouter = new CircularCloudLayouter(dict);
    }

    [TestCaseSource(typeof(TagsCloudVisualizationTestData), nameof(TagsCloudVisualizationTestData.IntersectionData))]
    public void IntersectWithPlaced_ShouldReturn(Point coordinate, Rectangle target, bool result)
    {
        circularCloudLayouter.PutNextRectangle(coordinate);

        circularCloudLayouter.IntersectWithPlaced(target).Should().Be(result);
    }

    [Test]
    public void PutNextRectangle_Should_PlaceWord_When_Free()
    {
        var size = circularCloudLayouter.PlacedWords.Count;
        circularCloudLayouter.PutNextRectangle(new Point(0, 0), true);

        (circularCloudLayouter.PlacedWords.Count != size).Should().BeTrue();
    }
    
    [Test]
    public void PutNextRectangle_Should_NotPlaceWord_When_SpaceOccupied()
    {
        circularCloudLayouter.PutNextRectangle(new Point(0, 0), true);
        var size = circularCloudLayouter.PlacedWords.Count;
        circularCloudLayouter.PutNextRectangle(new Point(0, 0), true);

        (circularCloudLayouter.PlacedWords.Count != size).Should().BeFalse();
    }

    [Test]
    public void GenerateTagCloudShouldCreateFile()
    {
        circularCloudLayouter.GenerateTagCloud(FilePath);

        File.Exists($"../../../../TagsCloudVisualization/{FilePath}.jpg").Should().BeTrue();
    }

    [Test]
    public void GenerateTagCloudShould_PlaceAllWords()
    {
        var dict = WordsDataSet.CreateFrequencyDict(
            "../../../../TagsCloudVisualization/words.txt"
        );

        var cloudLayouter = new CircularCloudLayouter(dict);
        cloudLayouter.GenerateTagCloud(FilePath);

        (cloudLayouter.PlacedWords.Count == dict.Count).Should().BeTrue();
    }

    [Test]
    public void AlgorithmComplexity_LessOrEqualQuadratic()
    {
        // TODO
    }
}