using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization;

public class CircularCloudLayouter_Should
{
    private CircularCloudLayouter? circularCloudLayouter;

    [SetUp]
    public void SetCircularCloudFieldToNull()
    {
        circularCloudLayouter = null;
    }

    [TearDown]
    public void CreateLayoutImage_IfTestFailed()
    {
        if (TestContext.CurrentContext.Result.FailCount < 1) return;

        var testName = TestContext.CurrentContext.Test.Name;
        var workDirectory = TestContext.CurrentContext.WorkDirectory;

        circularCloudLayouter?.CreateLayoutImage(testName, workDirectory);

        var filePath = $@"{workDirectory}\{testName}.png";

        TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
        TestContext.AddTestAttachment($"{testName}.png");
    }

    [Test]
    public void CreatesClassInstance_WithoutException()
    {
        var createCloudLayouter = () => new CircularCloudLayouter(new Point(0, 0));
        createCloudLayouter.Should().NotThrow();
    }

    [TestCase(-1, 0, TestName = "Negative width")]
    [TestCase(0, -1, TestName = "Negative height")]
    [TestCase(-5, -5, TestName = "Negative width and height")]
    public void PutNextRectangleThrowsArgumentException_WhenNegativeParameters(int rectWidth, int rectHeight)
    {
        circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
        var rectangleSize = new Size(rectWidth, rectHeight);
        var rectangleCreation = () => circularCloudLayouter.PutNextRectangle(rectangleSize);
        rectangleCreation.Should().Throw<ArgumentException>();
    }

    [TestCase(0, 0, 0, 0)]
    [TestCase(0, 0, 100, 100)]
    [TestCase(0, 0, 1, 1)]
    [TestCase(0, 0, 5, 4)]
    [TestCase(5, 4, 2, 1)]
    [TestCase(5, 4, 3, 6)]
    public void PutsFirstRectangle_InTheCenter(int centerX, int centerY, int rectWidth, int rectHeight)
    {
        circularCloudLayouter = new CircularCloudLayouter(new Point(centerX, centerY));
        var rectangle = circularCloudLayouter.PutNextRectangle(new Size(rectWidth, rectHeight));
        var halfWidth = (int)Math.Floor(rectWidth / 2.0);
        var halfHeight = (int)Math.Floor(rectHeight / 2.0);

        var expectedRectangleLeft = circularCloudLayouter.CenterPoint.X - halfWidth;
        var expectedRectagnleRight = circularCloudLayouter.CenterPoint.X + halfWidth + (rectWidth % 2);
        var expectedRectangleTop = circularCloudLayouter.CenterPoint.Y - halfHeight;
        var expectedRectangleBottom = circularCloudLayouter.CenterPoint.Y + halfHeight + (rectHeight % 2);

        rectangle.Left.Should().Be(expectedRectangleLeft);
        rectangle.Right.Should().Be(expectedRectagnleRight);
        rectangle.Top.Should().Be(expectedRectangleTop);
        rectangle.Bottom.Should().Be(expectedRectangleBottom);
    }

    [Test]
    [Timeout(1000)]
    public void Puts5SameSquares_InPlusShape_WhenAngleDeltaIsHalfPi()
    {
        const int radiusDelta = 1;
        const double angleDelta = Math.PI / 2;
        circularCloudLayouter = new CircularCloudLayouter(new Point(), radiusDelta, angleDelta);
        var rectangleSize = new Size(10, 10);
        var firstRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        var secondRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        var thirdRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        var fourthRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        var fifthRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);

        firstRectangle.GetRectangleCenterPoint().Should().Be(new Point(0, 0));
        secondRectangle.GetRectangleCenterPoint().Should().Be(new Point(10, 0));
        thirdRectangle.GetRectangleCenterPoint().Should().Be(new Point(0, 10));
        fourthRectangle.GetRectangleCenterPoint().Should().Be(new Point(-10, 0));
        fifthRectangle.GetRectangleCenterPoint().Should().Be(new Point(0, -10));
    }

    [Test]
    [Timeout(1000)]
    public void Puts11SameSquaresCloseToEachOther_WhenAngleDeltaIsPiOver60()
    {
        circularCloudLayouter = new CircularCloudLayouter(new Point());
        var expectedRectangleCenters = new List<Point>()
        {
            new (0, 0),
            new (4, 0),
            new (2, 4),
            new (-2, 4),
            new (-4, 0),
            new (-2, -4),
            new (2, -4),
            new (6, 4),
            new (-6, 4),
            new (-6, -4),
            new (6, -4),
        };
        foreach (var expectedRectangleCenter in expectedRectangleCenters)
        {
            var currentRectangle = circularCloudLayouter.PutNextRectangle(new Size(4, 4));
            currentRectangle.GetRectangleCenterPoint().Should().Be(expectedRectangleCenter);
        }
    }
}