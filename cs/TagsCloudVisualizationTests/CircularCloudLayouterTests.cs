using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layouter = null!;

    [SetUp]
    public void SetUp()
    {
        layouter = new CircularCloudLayouter(new Point(500, 500));
    }

    [TestCase(-200, -300)]
    [TestCase(-200, 300)]
    [TestCase(200, -300)]
    public void PutNextRectangle_ThrowsArgumentException_WhenRectanlgeSizeContainsNegativeParameters(int width,
        int height)
    {
        Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
    }
    
    [Test]
    public void PutNextRecangle_PlacesRectangleWithoutIntersection_OnMultipleRectangles()
    {
        var rectangles = new List<Rectangle>();

        for (var i = 1; i < 50; i++)
        {
            var newRectangle = layouter.PutNextRectangle(new Size(20, 10));
            
            foreach (var rectangle in rectangles)
                Assert.That(rectangle.IntersectsWith(newRectangle), Is.False);

            rectangles.Add(newRectangle);
        }
    }
}