using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudLib;
using TagsCloudLib.Abstractions;
using TagsCloudLib.Abstractions.Factories;
using TagsCloudLib.Extensions;
using TagsCloudLib.Implementations;
using TagsCloudLib.Implementations.Visualizer;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouterBase layouter;
    private TagCloudVisualizer visualizer;
    private TagCloudVisualizationConfig config;
    
    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        var center = new Point(0, 0);
        config = new TagCloudVisualizationConfig
        {
            CanvasWidth = 3000,
            CanvasHeight = 3000,
            CanvasBackgroundColor = Color.Black,
            ShapeBorderColor = Color.Cyan,
            ShapeBorderThickness = 3,
            ShapeFillColor = Color.FromArgb(80, Color.Cyan)
        };
        services.AddTagCloudServices(config);
        var provider = services.BuildServiceProvider();
        var layouterFactory = provider.GetRequiredService<ILayouterFactory>();
        layouter = layouterFactory.Create(center);
        visualizer = provider.GetRequiredService<TagCloudVisualizer>();
    }
    


    [TearDown]
    public void TearDown()
    {
        var result = TestContext.CurrentContext.Result.Outcome.Status;

        if (result != NUnit.Framework.Interfaces.TestStatus.Failed) return;
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var folderName = Path.Combine(desktopPath, "tests");
        Directory.CreateDirectory(folderName);
        var path = Path.Combine(folderName, $"{TestContext.CurrentContext.Test.Name}_failed.png");

        try
        {
            visualizer.Draw(layouter.Rectangles, path, config);
            TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"Failed to save tag cloud visualization: {ex.Message}");
        }
    }

    [TestCaseSource(nameof(GenerateInvalidSizes))]
    public void PutNextRectangle_ShouldThrow_WhenSizeIsInvalid(Size size)
    {
        var act = () => layouter.PutNextRectangle(size);
        act.Should().Throw<ArgumentException>();
    }

    [TestCaseSource(nameof(GenerateDifferentSizes))]
    public void PutNextRectangle_ShouldHaveCorrectSize(Size size)
    {
        var rect = layouter.PutNextRectangle(size);
        rect.Width.Should().Be(size.Width);
        rect.Height.Should().Be(size.Height);
    }

    [TestCaseSource(nameof(GenerateSpiralPoints))]
    public void PutNextRectangle_ShouldPlaceRectanglesAtExpectedPoints(Point[] expectedPoints)
    {
        var fakeSpiral = A.Fake<ISpiral>();
        var queue = new Queue<Point>(expectedPoints);
        A.CallTo(() => fakeSpiral.GetNextPoint()).ReturnsLazily(() => queue.Dequeue());

        var fakeLayouter = new CircularCloudLayouter(
            new Point(0, 0),
            fakeSpiral,
            new CenterShifter()
        );

        foreach (var point in expectedPoints)
        {
            var rect = fakeLayouter.PutNextRectangle(new Size(10, 10));
            rect.Center().Should().Be(point);
        }

        fakeLayouter.Rectangles.Should().HaveCount(expectedPoints.Length);
    }

    [TestCaseSource(nameof(GenerateRectanglesCount))]
    public void PutNextRectangle_ShouldPlaceManyRectanglesWithoutIntersections(int count)
    {
        for (var i = 0; i < count; i++)
        {
            layouter.PutNextRectangle(new Size(10, 10));
        }

        layouter.Rectangles.Should().HaveCount(count);

        foreach (var r1 in layouter.Rectangles)
        {
            foreach (var r2 in layouter.Rectangles.Where(r2 => r1 != r2))
            {
                r1.IntersectsWith(r2).Should().BeFalse();
            }
        }
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBeExactlyAtCenter()
    {
        var rect = layouter.PutNextRectangle(new Size(10, 10));
        rect.Center().Should().Be(layouter.Center);
    }

    [Test]
    public void PutNextRectangle_ShouldCallCenterShifter()
    {
        var fakeShifter = A.Fake<ICenterShifter>();
        var fakeSpiral = A.Fake<ISpiral>();
        A.CallTo(() => fakeSpiral.GetNextPoint()).Returns(new Point(0, 0));
        var layouterWithFake = new CircularCloudLayouter(
            new Point(0, 0),
            fakeSpiral,
            fakeShifter
        );

        layouterWithFake.PutNextRectangle(new Size(10, 10));

        A.CallTo(() => fakeShifter.ShiftToCenter(A<Rectangle>._, A<Point>._, A<List<Rectangle>>._))
            .MustHaveHappened();
    }

    [Test]
    public void PutNextRectangle_ShouldTryMultipleSpiralPointsUntilFree()
    {
        var fakeSpiral = A.Fake<ISpiral>();
        var fakeShifter = new CenterShifter();

        var occupiedPoint = new Point(0, 0);
        var freePoint = new Point(10, 10);
        var callCount = 0;

        A.CallTo(() => fakeSpiral.GetNextPoint()).ReturnsLazily(() =>
        {
            callCount++;
            return callCount == 1 ? occupiedPoint : freePoint;
        });

        var layouterWithFake = new CircularCloudLayouter(
            new Point(0, 0),
            fakeSpiral,
            fakeShifter
        );

        layouterWithFake.PutNextRectangle(new Size(10, 10));
        var rect = layouterWithFake.PutNextRectangle(new Size(10, 10));
        rect.Center().Should().Be(freePoint);
    }
        
    public static IEnumerable<TestCaseData> GenerateRectanglesCount()
    {
        yield return new TestCaseData(1)
            .SetName("PutNextRectangle_SingleRectangle_NoIntersection")
            .SetDescription("Placing LayouterFactory single rectangle should succeed without intersections.");
        yield return new TestCaseData(2)
            .SetName("PutNextRectangle_TwoRectangles_NoIntersection")
            .SetDescription("Placing two rectangles should succeed without intersections.");
        yield return new TestCaseData(20)
            .SetName("PutNextRectangle_TwentyRectangles_NoIntersection")
            .SetDescription("Placing 20 rectangles should succeed without intersections.");
        yield return new TestCaseData(200)
            .SetName("PutNextRectangle_TwoHundredRectangles_NoIntersection")
            .SetDescription("Placing 200 rectangles should succeed without intersections.");
        for (var i = 50; i <= 200; i *= 2)
            yield return new TestCaseData(i)
                .SetName($"PutNextRectangle_{i}Rectangles_NoOverlap")
                .SetDescription($"Placing {i} rectangles should not produce overlaps.");
    }

    public static IEnumerable<TestCaseData> GenerateSpiralPoints()
    {
        var points = Enumerable.Range(0, 20).Select(i => new Point(i * 10, i * 10)).ToArray();
        yield return new TestCaseData(points)
            .SetName("PutNextRectangle_ShouldTryMultipleSpiralPointsUntilFree")
            .SetDescription("Rectangles should be placed at expected points along the spiral.");
    }

    public static IEnumerable<TestCaseData> GenerateDifferentSizes()
    {
        yield return new TestCaseData(new Size(10, 10))
            .SetName("PutNextRectangle_Size10x10")
            .SetDescription("Rectangle with width=10, height=10 should be placed correctly.");
        yield return new TestCaseData(new Size(20, 5))
            .SetName("PutNextRectangle_Size20x5")
            .SetDescription("Rectangle with width=20, height=5 should be placed correctly.");
        yield return new TestCaseData(new Size(5, 30))
            .SetName("PutNextRectangle_Size5x30")
            .SetDescription("Rectangle with width=5, height=30 should be placed correctly.");
        yield return new TestCaseData(new Size(15, 25))
            .SetName("PutNextRectangle_Size15x25")
            .SetDescription("Rectangle with width=15, height=25 should be placed correctly.");
    }

    public static IEnumerable<TestCaseData> GenerateInvalidSizes()
    {
        yield return new TestCaseData(new Size(0, 10))
            .SetName("PutNextRectangle_Throws_WhenWidthIsZero")
            .SetDescription("CanvasWidth is zero, should throw ArgumentException.");
        yield return new TestCaseData(new Size(10, 0))
            .SetName("PutNextRectangle_Throws_WhenHeightIsZero")
            .SetDescription("CanvasHeight is zero, should throw ArgumentException.");
        yield return new TestCaseData(new Size(-5, 10))
            .SetName("PutNextRectangle_Throws_WhenWidthIsNegative")
            .SetDescription("CanvasWidth is negative, should throw ArgumentException.");
        yield return new TestCaseData(new Size(10, -5))
            .SetName("PutNextRectangle_Throws_WhenHeightIsNegative")
            .SetDescription("CanvasHeight is negative, should throw ArgumentException.");
        yield return new TestCaseData(new Size(-5, -5))
            .SetName("PutNextRectangle_Throws_WhenWidthAndHeightNegative")
            .SetDescription("CanvasWidth and height are negative, should throw ArgumentException.");
    }
}