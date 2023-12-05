using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class SpiralBuilderTests
{
    private CircularCloudBuilder builder = null!;
    private List<Rectangle> placedRectangles = null!;
    
    [SetUp]
    public void SetUp()
    {
        builder = new CircularCloudBuilder(new Point(500, 500), 1, 0.1d);
        placedRectangles = new List<Rectangle>();
    }
    
    [TestCase(-200, 300, false)]
    [TestCase(200, -300, false)]
    [TestCase(200, 300, true)]
    public void GetNextPosition_ThrowsArgumentException_OnIncorrectParameters(int width, int height, bool listIsNull)
    {
        Assert.Throws<ArgumentException>( () => builder.GetNextPosition(new Size(width, height),
            (listIsNull ? null : placedRectangles)!));
    }
}