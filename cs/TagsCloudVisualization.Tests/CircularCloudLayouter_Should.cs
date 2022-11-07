using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Layouter;


namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouter_Should
{
    private CircularCloudLayouter layouter;
    private List<Size> sizes;

    [SetUp]
    public void SetUp()
    {
        var point = new Point(0, 0);
        layouter = new CircularCloudLayouter(point);
        sizes = new List<Size>
        {
            new(20, 10),
            new(10, 20),
            new(20, 30),
            new(20, 20),
            new(50, 30),
            new(10, 40)
        };
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure) return;
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
    public void PutNextRectangle_ThrowsArgumentException_WithIncorrectSize(int width, int height)
    {
        Action put = () => layouter.PutNextRectangle(new Size(width, height));
        put.Should().Throw<ArgumentException>().WithMessage("Size of rectangular must be positive");
    }

    [Test]
    public void PutNextRectangle_ShouldPlaceInRightPosition()
    {
        foreach (var size in sizes)
            layouter.PutNextRectangle(size);
        layouter.GetTagsLayout().First().Should().BeEquivalentTo(new Rectangle(0, 0, 20, 10));
        layouter.GetTagsLayout().Last().Should().BeEquivalentTo(new Rectangle(20, 16, 10, 40));
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
    public void ClearLayout_ShouldMakeTagsEmpty()
    {
        foreach (var size in sizes)
            layouter.PutNextRectangle(size);
        layouter.ClearLayout();
        layouter.GetTagsLayout().Should().BeEmpty();
    }

    [Test]
    public void Layout_ManyTags_WithoutIntersect()
    {
        var rnd = new Random(30);
        var tags = new List<Rectangle>();
        for (var i = 0; i < 400; i++)
            tags.Add(layouter.PutNextRectangle(new Size(rnd.Next(20, 50), rnd.Next(20, 50))));

        var tag = tags.Where(tag => tags.Where(t => t != tag).Any(t => t.IntersectsWith(tag)));
        tag.Should().BeEmpty();
    }

    [Test]
    public void GetTestLayout_ShouldBeEmpty_WhenInitialized()
    {
        layouter.GetTagsLayout().Should().BeEmpty();
    }
}