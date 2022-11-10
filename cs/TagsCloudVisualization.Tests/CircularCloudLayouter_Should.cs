using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Layouter;


namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouter_Should
{
    private CircularCloudLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        var point = new Point(0, 0);
        layouter = new CircularCloudLayouter(point);
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
            return;
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Tests_fail");
        var drawer = new Drawing.CloudDrawer(layouter, path);
        var name = TestContext.CurrentContext.Test.Name + ".bmp";
        drawer.DrawCloud(name, Pens.Red);
        Console.WriteLine("Tag cloud visualization saved to file " + path + name);
    }

    [Test]
    public void PutNextRectangle_ShouldAdd_NewRectangular()
    {
        var rectangle = layouter.PutNextRectangle(new Size(1, 2));
        rectangle.Should().BeEquivalentTo(new Rectangle(0, 0, 1, 2));
    }

    [Test]
    public void PutNextRectangle_ShouldAddFirstTag_ToCenterOfTagCloud()
    {
        var rectangle = layouter.PutNextRectangle(new Size(1, 2));
        rectangle.Location.Should().BeEquivalentTo(layouter.Center);
    }

    [TestCase(0, 1, TestName = "zero width")]
    [TestCase(1, 0, TestName = "zero height")]
    [TestCase(-1, 1, TestName = "negative width")]
    [TestCase(1, -1, TestName = "negative height")]
    public void PutNextRectangle_ThrowsException_WithIncorrectSize(int width, int height)
    {
        Action put = () => layouter.PutNextRectangle(new Size(width, height));
        put.Should().Throw<IncorrectSizeException>();
    }

    [Test]
    public void Rectangles_ShouldFormCircularFigure()
    {
        var rnd = new Random(30);
        var countOfRectangles = 4000;
        for (var i = 0; i < countOfRectangles; i++)
            layouter.PutNextRectangle(new Size(rnd.Next(20, 50), rnd.Next(20, 50)));
        var rectangles = layouter.GetRectanglesLayout().ToList();
        var lastAddedRectangle = rectangles.Last();
        var radius = lastAddedRectangle.GetPoints().Select(p => layouter.Center.GetDistance(p)).Max();
        var circle = new Circle(radius, layouter.Center);
        var rectanglesOutside = rectangles.Where(rec => !circle.ContainsRectangle(rec)).ToList();
        if (rectanglesOutside.Count < countOfRectangles * 0.30)
            rectanglesOutside = rectanglesOutside.Where(rec => !circle.ContainsMostPartOfRectangle(rec, 50)).ToList();
        rectanglesOutside.Should().BeEmpty();
    }

    [Test]
    public void HaveCorrect_Center()
    {
        layouter.Center.Should().BeEquivalentTo(new Point(0, 0));
    }

    [Test]
    public void PutNextRectangle_SavesRectangleSize()
    {
        var size = new Size(1, 2);
        layouter.PutNextRectangle(size).Size.Should().Be(size);
    }

    [Test]
    public void ClearLayout_ShouldMakeRectanglesEmpty()
    {
        layouter.PutNextRectangle(new Size(1, 2));
        layouter.ClearRectanglesLayout();
        layouter.GetRectanglesLayout().Should().BeEmpty();
    }

    [Test]
    public void Layout_ManyTags_WithoutIntersect()
    {
        var rnd = new Random(30);
        var rectangles = new List<Rectangle>();
        for (var i = 0; i < 400; i++)
            rectangles.Add(layouter.PutNextRectangle(new Size(rnd.Next(20, 50), rnd.Next(20, 50))));
        var intersectRectangles = rectangles.Where(tag => rectangles.Any(t => t != tag && t.IntersectsWith(tag)));
        intersectRectangles.Should().BeEmpty();
    }

    [Test]
    public void GetTestLayout_ShouldBeEmpty_WhenInitialized()
    {
        layouter.GetRectanglesLayout().Should().BeEmpty();
    }
}