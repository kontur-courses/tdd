using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests;

public class TagCloud_Should
{
    [TestCase(0, 0)]
    [TestCase(10, 10)]
    [TestCase(10, -10)]
    [TestCase(-10, 10)]
    [TestCase(-10, -10)]
    public void Constructor_DontThrowException(int x, int y)
    {
        Action act = () => _ = new CircularCloudLayouter(new Point(x, y));
        act.Should().NotThrow<Exception>();
    }
    
    [TestCase(0, 0, TestName = "{m}isEmpty")]
    [TestCase(10, 0, TestName = "{m}WithZeroWidth")]
    [TestCase(0, 10, TestName = "{m}WithZeroHeight")]
    public void PutNextRectangle_ThrowArgumentException_OnSize(int width, int height)
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(width, height);
        var act = () => _ = layouter.PutNextRectangle(size);
        act.Should().Throw<ArgumentException>().WithMessage("Area of rectangle can't be zero");
    }
    
    [TestCase(1, TestName = "{m}CorrectSize")]
    [TestCase(2, TestName = "{m}TwoSizes")]
    [TestCase(10, TestName = "{m}ManySizes")]
    public void PutNextRectangle_DontThrowException_On(int rectanglesCount)
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(10, 10);
        var act = () =>
        {
            for (var i = 0; i < rectanglesCount; i++)
                layouter.PutNextRectangle(size);
        };
        act.Should().NotThrow<Exception>();
    }

    [Test]
    public void PutNextRectangle_ReturnedRectangle_HasTheInputSize()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(10, 10);
        layouter.PutNextRectangle(size).Size.Should().Be(size);
    }
    
    [Test]
    public void PutNextRectangle_ReturnRectangles_DontIntersect()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(10, 10);
        var rect1 = layouter.PutNextRectangle(size);
        var rect2 = layouter.PutNextRectangle(size);
        rect1.IntersectsWith(rect2).Should().BeFalse();
    }
}