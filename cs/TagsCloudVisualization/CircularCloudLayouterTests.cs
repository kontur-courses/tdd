using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouterTests
{
    [Test]
    public void PutNextRectangle_FirstRectangleIsCenteredAtCloudCenter()
    {
        var center = new Point(100, 100);
        var rectangleSize = new Size(40, 20);
        var layouter = new CircularCloudLayouter(center);

        var rectangle = layouter.PutNextRectangle(rectangleSize);

        Assert.That(
            rectangle,
            Is.EqualTo(new Rectangle(80, 90, 40, 20)));
    }
}