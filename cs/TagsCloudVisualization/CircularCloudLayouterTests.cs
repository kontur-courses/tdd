using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization;

public class CircularCloudLayouterTests
{
    [Test]
    public void PlacesFirstRectangleAtCloudCenter()
    {
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var rectangle = layouter.PutNextRectangle(new Size(20, 10));
        Assert.That(rectangle, Is.EqualTo(new Rectangle(90, 95, 20, 10)));
    }

    [TestCase(20, 10)]
    [TestCase(60, 30)]
    public void PlacesSecondRectangleWithoutOverlap(int width, int height)
    {
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var rectangle1 = layouter.PutNextRectangle(new Size(20, 10));
        var rectangle2 = layouter.PutNextRectangle(new Size(width, height));
        Assert.That(rectangle1.IntersectsWith(rectangle2), Is.False);
    }
}
