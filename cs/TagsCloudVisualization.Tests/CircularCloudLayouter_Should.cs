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
    public void PutNextRectangle_ShouldAddFirstRectangle_ToCenterOfCloud()
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
        var rnd = new Random(106);
        var countOfRectangles = 400;
        var maxWidth = 50;
        var maxHeight = 50;
        for (var i = 0; i < countOfRectangles; i++)
            layouter.PutNextRectangle(new Size(rnd.Next(20, maxWidth), rnd.Next(20, maxHeight)));
        var rectangles = layouter.GetRectanglesLayout().ToList();
        var lastLocation = rectangles.Last().Location;
        var radius = layouter.Center.GetDistance(lastLocation);
        var extendedRadius = radius + Math.Sqrt(maxHeight * maxHeight + maxWidth * maxWidth);
        var circle = new Circle(extendedRadius, layouter.Center);
        var rectanglesOutside = rectangles.Where(rec => !circle.ContainsRectangle(rec)).ToList();
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
    public void Layout_ManyRectangles_WithoutIntersect()
    {
        var rnd = new Random(30);
        var rectangles = new List<Rectangle>();
        for (var i = 0; i < 400; i++)
            rectangles.Add(layouter.PutNextRectangle(new Size(rnd.Next(20, 50), rnd.Next(20, 50))));
        var intersectRectangles = rectangles.Where(rect => rectangles.Any(t => t != rect && t.IntersectsWith(rect)));
        intersectRectangles.Should().BeEmpty();
    }

    [Test]
    public void GetTestLayout_ShouldBeEmpty_WhenInitialized()
    {
        layouter.GetRectanglesLayout().Should().BeEmpty();
    }
}