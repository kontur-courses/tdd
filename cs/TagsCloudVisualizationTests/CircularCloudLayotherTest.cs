using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.TagsCloudVisualizationTests;

public class CircularCloudLayotherTest
{
    private CircularCloudLayouter _circularCloudLayouter;

    [SetUp]
    public void SetUp()
    {
        _circularCloudLayouter =
            new CircularCloudLayouter(new Point(CloudLayouterConst.CloudCentreX, CloudLayouterConst.CloudCentreY));
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
        var testName = TestContext.CurrentContext.Test.Name;
        var filePath = TestContext.CurrentContext.WorkDirectory;
        var rectangles = _circularCloudLayouter.GetRectangles();
        var size = _circularCloudLayouter.getCloudSize();
        CloudDrawer.DrawRectangles(rectangles, size, $"{testName}.png");
        Console.WriteLine($"Tag cloud visualization saved to file  {filePath}/{testName}.png");
    }

    [TestCase(-2, 3, TestName = "Negative width")]
    [TestCase(2, -3, TestName = "Negative height")]
    [TestCase(0, 0, TestName = "zero width and height")]
    [TestCase(-2, -3, TestName = "Negative weight and height")]
    public void PutNextRectangle_ShouldThrowArgumentException_WithNegativeInput(int width, int height)
    {
        Action action = () => new CircularCloudLayouter(new Point()).PutNextRectangle(new Size(width, height));
        action.Should().Throw<ArgumentException>();
    }

    [TestCase(2, 3, TestName = "positive width and height")]
    public void PutNextRectangle_ShouldNotThrowArgumentException_WithCorrectInput(int width, int height)
    {
        Action action = () => new CircularCloudLayouter(new Point()).PutNextRectangle(new Size(width, height));
        action.Should().NotThrow<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ShouldAddNewRectangle()
    {
        var size = new Size(10, 20);
        var coordinateX = CloudLayouterConst.CloudCentreX - size.Width / 2;
        var coordinateY = CloudLayouterConst.CloudCentreY - size.Height / 2;
        _circularCloudLayouter.PutNextRectangle(size);
        _circularCloudLayouter.GetRectangles().Should().NotBeEmpty();
        _circularCloudLayouter.GetRectangles().Should()
            .Contain(new Rectangle(coordinateX, coordinateY, size.Width, size.Height));
    }

    [Test]
    public void CircularCloudLayouter_RectAngelsListShouldHaveCorrectSize()
    {
        _circularCloudLayouter.PutNextRectangle(new Size(40, 20));
        _circularCloudLayouter.PutNextRectangle(new Size(20, 40));
        _circularCloudLayouter.PutNextRectangle(new Size(50, 50));
        _circularCloudLayouter.GetRectangles().Count.Should().Be(3);
    }

    [Test]
    public void CircularCloudLayouter_RectAngelsShouldNoIntersectsWithOthers()
    {
        Random rnd = new Random();
        for (int i = 0; i < 100; i++)
        {
            int width = rnd.Next(15, 40);
            int height = rnd.Next(15, 40);
            _circularCloudLayouter.PutNextRectangle(new Size(width, height));
        }

        List<Rectangle> rectangels = _circularCloudLayouter.GetRectangles();
        foreach (Rectangle rectangle in rectangels)
        {
            rectangels.Where((_, j) => j != rectangels.IndexOf(rectangle))
                .All(r => !r.IntersectsWith(rectangle))
                .Should().BeTrue();
        }
    }

    [Test, MaxTime(5000)]
    public void CircularCloudLayouter_ShouldWorkInTime()
    {
        Random rnd = new Random();
        for (int i = 0; i < 10000; i++)
        {
            int width = rnd.Next(1, 10);
            int height = rnd.Next(1, 10);
            _circularCloudLayouter.PutNextRectangle(new Size(width, height));
        }
    }
}