using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : BaseCloudLayouter
{
    public CircularCloudLayouter(Point center) : base(center) {}
    
    public override Point FindPositionForRectangle(Size rectangleSize)
    {
        var centerWithOffset = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);

        return GetSpiralPoints(centerWithOffset, 5 * Math.PI / 180, 2)
            .FirstOrDefault(x => IsPlaceSuitableForRectangle(new Rectangle(x, rectangleSize)));;
    }
    
    private static IEnumerable<Point> GetSpiralPoints(Point center, double dtheta, double a)
    {
        for (var theta = 0d; ; theta += dtheta)
        {
            var point = center;
            point.Offset(PolarToCartesian(a * theta, theta));

            yield return point;
        }
    }
    
    private static Point PolarToCartesian(double distance, double angle)
    {
        return new Point((int)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)));
    }
}