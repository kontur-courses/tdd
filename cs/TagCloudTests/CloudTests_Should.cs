using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagCloud;

namespace TagCloudTests;

public class CloudTests_Should
{
    private CircularCloudLayouter layouter;

    [SetUp]
    public void Setup()
    {
        var center = new Point(0, 0);
        layouter = new CircularCloudLayouter(center);
    }

    [TearDown]
    public void Tear_Down()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            TagCloudDrawer.Draw(layouter);
    }

    [Test]
    public void ReturnEmptyList_WhenCreated()
    {
        layouter.Rectangles.Should().BeEmpty();
    }

    [Test]
    public void ReturnOneElementList_WhenAddOne()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(1);
    }

    [Test]
    public void ReturnTwoElementList_WhenAddTwo()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(2);
        NotIntersectedAssertation();
    }

    [TestCase(1, 1, 100, TestName = "WithSquareShape")]
    [TestCase(20, 10, 100, TestName = "WithRectangleShape")]
    public void AddManyRectangles_WithConstantSize(int width, int height, int count)
    {
        var size = new Size(width, height);
        for (int i = 0; i < count; i++)
            layouter.PutNextRectangle(size);

        layouter.Rectangles.Count().Should().Be(count);
        NotIntersectedAssertation();
    }

    [TestCase(5, 10, 2, 4, 100)]
    [TestCase(20, 40, 20, 40, 200)]
    public void AddManyRectangles_WithVariableSize(int widthMin, int widthMax, int heightMin, int heightMax, int count)
    {
        var rnd = new Random(DateTime.Now.Microsecond);

        for (int i = 0; i < count; i++)
        {
            var size = new Size(rnd.Next(widthMin, widthMax), rnd.Next(heightMin, heightMax));
            layouter.PutNextRectangle(size);
        }

        layouter.Rectangles.Count().Should().Be(count);
        NotIntersectedAssertation();
    }

    [TestCase(-1, 1, TestName = "WithNegativeWidth")]
    [TestCase(1, -1, TestName = "WithNegativeHeight")]
    [TestCase(-1, -1, TestName = "WithNegativeWidthAndHeight")]
    public void Throw_ThenTryPutRectangle(int width, int height)
    {
        Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
    }

    public void NotIntersectedAssertation()
    {
        layouter
            .Rectangles
            .HasIntersectedRectangles()
            .Should()
            .BeFalse();
    }

}