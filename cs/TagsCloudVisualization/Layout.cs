using System.Drawing;

namespace TagsCloudVisualization;

public class Layout
{
    

    public Layout(Point center)
    {
        Center = center;
    }
    
    public Point Center { get; }
    
    public IEnumerable<Point> GetNextCoord()
    {
        yield return Center + new Size(0, 0);
        var offset = 1;

        while (true)
        {
            for (var dx = -offset; dx <= offset; dx++)
            for (var dy = -offset; dy <= offset; dy++)
                if (Math.Abs(dx) == offset || Math.Abs(dy) == offset)
                    yield return Center + new Size(dx, dy);

            offset++;
        }
    }
}