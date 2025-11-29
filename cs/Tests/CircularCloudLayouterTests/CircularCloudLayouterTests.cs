using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TestTagsCloudVisualisation.CircularCloudLayouterTests;

[TestFixture]
public class Tests
{
    private CircularCloudLayouter cloudLayouter;
    private Point center;

    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        cloudLayouter = new CircularCloudLayouter(center);
    }

    [Test]
    public void PutNextRectangle_ShouldThrowArgumentException_WhenInvalidSize()
    {
        var createZeroSize = () => cloudLayouter.PutNextRectangle(new Size(0, 0));
        var createNegativeSize  = () => cloudLayouter.PutNextRectangle(new Size(-10, -10));
        
        createZeroSize.Should().Throw<ArgumentException>();
        createNegativeSize.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void PutNextRectangle_ShouldNotIntersects_WhenSameSize()
    {   
        var rectangle1 = cloudLayouter.PutNextRectangle(new Size(10,10));
        var rectangle2 = cloudLayouter.PutNextRectangle(new Size(10,10));

        rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
    }
    
    [Test]
    public void PutNextRectangle_ShouldBeInCenter_WhenPutFirstRectangle()
    {   
        var rectangle = cloudLayouter.PutNextRectangle(new Size(10,10));
        var point = new Point(rectangle.X, rectangle.Y);
        
        point.Should().Be(center);
    }
    
    [Test]
    public void PutNextRectangle_ShouldNotIntersects_WhenPutManyRectangles()
    {
        var rectangles = new List<Rectangle>();
        
        for (int i = 1; i < 100; i++)
        {
            cloudLayouter.PutNextRectangle(new Size(i, i));
        }

        foreach (var rectangle1 in rectangles)
        {
            foreach (var rectangle2 in rectangles)
            {
                if (rectangle1 != rectangle2)
                    rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
            }
        }
    }
    
    [Test]
    public void PutNextRectangle_ShouldHaveCorrectSize_WhenPutRectangle()
    {
        var size = new Size(15, 20);
        
        var rect = cloudLayouter.PutNextRectangle(size);

        rect.Width.Should().Be(size.Width);
        rect.Height.Should().Be(size.Height);
    }
    
}

