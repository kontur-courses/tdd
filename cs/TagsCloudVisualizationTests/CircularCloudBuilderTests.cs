using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudBuilderTests
{
    private CircularCloudBuilder builder = null!;
    private List<Rectangle> placedRectangles = null!;

    [SetUp]
    public void SetUp()
    {
        builder = new CircularCloudBuilder(1, 0.1d);
        placedRectangles = new List<Rectangle>();
    }

    [TestCase(0, 0.1)]
    [TestCase(1, 0)]
    public void Constructor_ThrowsArgumentException_OnInvalidArguments(int radiusIncrement, double angleIncrement)
    {
        Assert.Throws<ArgumentException>(() => new CircularCloudBuilder(radiusIncrement, angleIncrement));
    }

    [TestCase(-200, 300, false)]
    [TestCase(200, -300, false)]
    [TestCase(200, 300, true)]
    public void GetNextPosition_ThrowsArgumentException_OnInvalidArguments(int width, int height, bool isListNull)
    {
        Assert.Throws<ArgumentException>(() => builder.GetNextPosition(new Point(500, 500),
            new Size(width, height),
            (isListNull ? null : placedRectangles)!));
    }
}