using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;

    private readonly List<Rectangle> rectangles = new(50);

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }
    
}