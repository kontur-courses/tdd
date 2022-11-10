using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloud;

namespace TagCloudTests;

public class CircularCloudLayouter_Should
{
    private readonly Dictionary<string, CircularCloudLayouter> layouterByTestId = new();

    [SetUp]
    public void SetUp()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var testId = TestContext.CurrentContext.Test.ID;
        layouterByTestId[testId] = layouter;
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure) return;

        var testId = TestContext.CurrentContext.Test.ID;
        if (!layouterByTestId.ContainsKey(testId)) return;
        
        var filename = $"{TestContext.CurrentContext.Test.Name}.jpg";
        var directory = new DirectoryInfo("../../../FallingTestsImages");
        if (!directory.Exists) directory.Create();
            
        var layouter = layouterByTestId[testId];
        new TagCloudDrawer().DrawTagCloud(layouter, filename, directory);
            
        Console.WriteLine($"Tag cloud visualization saved to file {directory.FullName}\\{filename}");
    }

    [TestCase(0, 0)]
    [TestCase(10, 10)]
    [TestCase(10, -10)]
    [TestCase(-10, 10)]
    [TestCase(-10, -10)]
    public void Constructor_DontThrowException(int x, int y)
    {
        var point = new Point(x, y);

        var act = () => new CircularCloudLayouter(point);

        act.Should().NotThrow<Exception>();
    }

    [TestCase(0, 0, TestName = "{m}IsEmpty")]
    [TestCase(0, 10, TestName = "{m}WithZeroWidth")]
    [TestCase(10, 0, TestName = "{m}WithZeroHeight")]
    [TestCase(-10, 10, TestName = "{m}WithNegativeWidth")]
    [TestCase(10, -10, TestName = "{m}WithNegativeHeight")]
    public void PutNextRectangle_ThrowArgumentException_OnSize(int width, int height)
    {
        var layouter = layouterByTestId[TestContext.CurrentContext.Test.ID];
        var size = new Size(width, height);

        var act = () => layouter.PutNextRectangle(size);

        act.Should().Throw<ArgumentException>()
            .WithMessage($"Width and height of the rectangle must be positive, but {size}");
    }

    [TestCase(1, TestName = "{m}CorrectSize")]
    [TestCase(5, TestName = "{m}MultipleRectangles")]
    public void PutNextRectangle_DontThrowException_On(int rectanglesCount)
    {
        var layouter = layouterByTestId[TestContext.CurrentContext.Test.ID];
        var size = new Size(10, 10);

        var act = () =>
        {
            for (var i = 0; i < rectanglesCount; i++)
                layouter.PutNextRectangle(size);
        };

        act.Should().NotThrow<Exception>();
    }

    [Test]
    public void PutNextRectangle_CenterRectangle_HasCorrectParameters()
    {
        var layouter = layouterByTestId[TestContext.CurrentContext.Test.ID];
        var cloudCenter = layouter.Center;
        var size = new Size(10, 10);
        var expectedPosition = new Point(cloudCenter.X - size.Width / 2, cloudCenter.Y - size.Height / 2);
        var expectedRectangle = new Rectangle(expectedPosition, size);

        var rectangle = layouter.PutNextRectangle(size);

        rectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [TestCase(2)]
    [TestCase(1000)]
    public void PutNextRectangle_ReturnRectangles_DontIntersect(int rectanglesCount)
    {
        var layouter = layouterByTestId[TestContext.CurrentContext.Test.ID];
        var random = new Random();
        var sizes = Enumerable.Range(1, rectanglesCount)
            .Select(n => new Size(random.Next(10, 31), random.Next(10, 31)));

        foreach (var size in sizes)
            layouter.PutNextRectangle(size);

        foreach (var rect1 in layouter.Rectangles)
        foreach (var rect2 in layouter.Rectangles.Where(r => r != rect1))
            rect1.IntersectsWith(rect2).Should().BeFalse();
    }
}