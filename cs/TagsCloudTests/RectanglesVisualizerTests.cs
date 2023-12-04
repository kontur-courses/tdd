using System.Drawing;
using FluentAssertions;
using tagsCloud;

namespace TagsCloudTests;

[TestFixture]
public class RectanglesVisualizerTests
{
    [Test]
    public void RectanglesVisualizer_ShouldNotBeException()
    {
        var rectangles = new List<Rectangle>();
        var action = () =>
        {
            var visualizer = new RectanglesVisualizer(rectangles);
            visualizer.DrawTagCloud();
        };
        action.Should().NotThrow();
    }


    [Test]
    public void RectanglesVisualizer_DrawSomeRectangles_AllRectanglesInImage()
    {
        var count = 10;
        var rectangles = new List<Rectangle>();
        for (var i = 0; i < count; i++)
        {
            var locate = Utils.GetRandomLocation();
            var size = Utils.GetRandomSize();
            var rect = new Rectangle(locate, size);
            rectangles.Add(rect);
        }

        var visualizer = new RectanglesVisualizer(rectangles);
        var image = visualizer.DrawTagCloud();
        CheckImageBorders(rectangles, image).Should().BeTrue();
    }

    private bool CheckImageBorders(List<Rectangle> rectangles, Bitmap image)
    {
        return rectangles.Max(rectangle => rectangle.Bottom) < image.Height &&
               rectangles.Max(rectangle => rectangle.Right) < image.Width;
    }
}