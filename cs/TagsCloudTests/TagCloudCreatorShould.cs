using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudTests;

[TestFixture]
public class TagCloudCreatorShould
{
    private readonly Point _defaultCenter = new(0, 0);
    private readonly Random _random = new();

    [Test]
    [Repeat(5)]
    public void Create_ReturnTagCloudEquivalentToConstructor_AfterExecution()
    {
        var sizesGenerator = new RandomSizesGenerator();
        var rectangleSizes = sizesGenerator
            .GenerateSize()
            .Take(_random.Next(10, 200))
            .ToArray();
        var currCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var expectedCloudLayouter = new CircularCloudLayouter(_defaultCenter);
        var defaultTagCloud = new TagCloud(currCloudLayouter);
        foreach (var rectangleSize in rectangleSizes)
            defaultTagCloud.AddNextRectangleWith(rectangleSize);

        var expectedTagCloud = TagCloudCreator.Create(rectangleSizes, expectedCloudLayouter);

        defaultTagCloud
            .Should()
            .BeEquivalentTo(expectedTagCloud);
    }
}