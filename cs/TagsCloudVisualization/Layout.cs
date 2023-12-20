

using Aspose.Drawing;

namespace TagsCloudVisualization;

public class Layout
{
    public static IEnumerable<Point> GetNextCoord(Point center)
    {
        yield return center + new Size(0, 0);
        var offset = 1;

        while (true)
        {
            for (var dx = -offset; dx <= offset; dx++)
            for (var dy = -offset; dy <= offset; dy++)
                if (Math.Abs(dx) == offset || Math.Abs(dy) == offset)
                    yield return center + new Size(dx, dy);

            offset++;
        }
    }
}