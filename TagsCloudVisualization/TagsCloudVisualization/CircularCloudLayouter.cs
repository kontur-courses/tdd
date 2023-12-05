using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : BaseCloudLayouter
{
    private readonly double spiralDeltaAngle;
    private readonly double spiralDistance;

    public CircularCloudLayouter(Point center, double spiralDeltaAngle = 5 * Math.PI / 180, double spiralDistance = 2)
        : base(center)
    {
        this.spiralDeltaAngle = spiralDeltaAngle;
        this.spiralDistance = spiralDistance;
    }
    
    public override Point FindPositionForRectangle(Size rectangleSize)
    {
        var centerWithOffset = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);

        return GetSpiralPoints(centerWithOffset, spiralDeltaAngle, spiralDistance)
            .FirstOrDefault(x => IsPlaceSuitableForRectangle(new Rectangle(x, rectangleSize)));;
    }
    
    private static IEnumerable<Point> GetSpiralPoints(Point center, double deltaAngle, double distance)
    {
        for (var angle = 0d; ; angle += deltaAngle)
        {
            var point = center;
            point.Offset(PolarToCartesian(distance * angle, angle));

            yield return point;
        }
    }
    
    private static Point PolarToCartesian(double distance, double angle)
    {
        return new Point((int)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)));
    }
}