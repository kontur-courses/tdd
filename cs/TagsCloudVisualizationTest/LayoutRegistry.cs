using System.Drawing;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualizationTest;

public static class LayoutRegistry
{
    private const int RandomFilledCloudRectangleCount = 1000;

    public static List<Rectangle> RandomFilledCloudRectangles = [];

    private static readonly Random Random = new();

    public static CircularCloudLayouter GetRandomFilledCloud(int sizeFrom = 10, int sizeTo = 100)
    {
        var layouter = new CircularCloudLayouter();
        RandomFilledCloudRectangles = new List<Rectangle>(RandomFilledCloudRectangleCount);

        for (var i = 0; i < RandomFilledCloudRectangleCount; i++)
        {
            var size = new Size(Random.Next(sizeFrom, sizeTo), Random.Next(sizeFrom, sizeTo));
            RandomFilledCloudRectangles.Add(layouter.PutNextRectangle(size));
        }

        return layouter;
    }
}