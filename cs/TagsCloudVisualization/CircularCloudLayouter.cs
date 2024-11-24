using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly IPointDistributor distributor;
    private readonly List<RectangleF> rectangles = [];

    public CircularCloudLayouter(IPointDistributor distributor)
        => this.distributor = distributor;

    public RectangleF PutNextRectangle(SizeF rectangleSize)
    {
        PointF point;
        RectangleF rect;
        do
        {
            point = distributor.GetNextPoint();
            rect = RectangleFExtensions.GetRectangleFWithCenterIn(point, rectangleSize);
        } while (rectangles.Any(r => r.IntersectsWith(rect)));

        rectangles.Add(rect);
        return rect;
    }
}