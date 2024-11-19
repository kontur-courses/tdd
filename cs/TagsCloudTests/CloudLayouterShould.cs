using FluentAssertions;
using System.Drawing;
using FluentAssertions.Execution;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

[assembly: Parallelizable(ParallelScope.Children)]

namespace TagsCloudTests;

[TestFixture]
public class CloudLayouterShould
{
    private readonly Point _defaultCenter = new(0, 0);
    private readonly Random _random = new();
    private readonly ISizesGenerator _defaultSizesGenerator = new RandomSizesGenerator();

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleWithExpectedLocation_AfterFirstExecution()
    {
        var expectedCenter = new Point(_random.Next(-10, 10), _random.Next(-10, 10));
        var rectangleSize = _defaultSizesGenerator
            .GenerateSize()
            .Take(1)
            .First();
        var cloudLayouter = new CircularCloudLayouter(expectedCenter);

        var actualRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

        actualRectangle
            .GetCentralPoint()
            .Should()
            .BeEquivalentTo(expectedCenter);
    }

    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(0, 0)]
    public void PutNextRectangle_ThrowArgumentOutOfRangeException_AfterExecutionWith(int width, int height)
    {
        var rectangleSize = new Size(width, height);
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);

        var executePutNewRectangle = () =>
            circularCloudLayouter
                .PutNextRectangle(rectangleSize);

        executePutNewRectangle
            .Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    [Repeat(5)]
    public void PutNextRectangle_ReturnRectangleThatNotIntersectsWithOther_AfterManyExecution()
    {
        var rectangleSizes = _defaultSizesGenerator
            .GenerateSize()
            .Take(_random.Next(10, 200));
        var cloudLayouter = new CircularCloudLayouter(_defaultCenter);

        var rectangles = rectangleSizes
            .Select(size => cloudLayouter.PutNextRectangle(size))
            .ToArray();

        for (var i = 0; i < rectangles.Length; i++)
        for (var j = i + 1; j < rectangles.Length; j++)
            rectangles[i]
                .IntersectsWith(rectangles[j])
                .Should()
                .BeFalse();
    }

    [Test]
    [Repeat(20)]
    public void PutNextRectangle_ReturnRectanglesInCircle_AfterManyExecution()
    {
        var rectangleSizes = _defaultSizesGenerator
            .GenerateSize()
            .Take(_random.Next(100, 200));
        var circularCloudLayouter = new CircularCloudLayouter(_defaultCenter);

        var rectanglesList = rectangleSizes
            .Select(rectangleSize => circularCloudLayouter
                .PutNextRectangle(rectangleSize))
            .ToList();

        var circleRadius = rectanglesList
            .Select(rectangle => rectangle.GetCentralPoint())
            .Max(pointOnCircle => pointOnCircle.GetDistanceTo(_defaultCenter));
        var fromRectangleToCenterDistances =
            rectanglesList
                .Select(rectangle => rectangle
                    .GetCentralPoint()
                    .GetDistanceTo(_defaultCenter));

        var sumRectanglesSquare = rectanglesList.Sum(rectangle => rectangle.Width * rectangle.Height);
        var circleSquare = circleRadius * circleRadius * Math.PI;
        var precision = circleSquare * 0.375;

        using (new AssertionScope())
        {
            circleSquare
                .Should()
                .BeApproximately(sumRectanglesSquare, precision);
            foreach (var distanceToCenter in fromRectangleToCenterDistances)
                distanceToCenter
                    .Should()
                    .BeLessOrEqualTo(circleRadius);
        }
    }
}