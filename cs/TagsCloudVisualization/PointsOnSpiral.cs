using System.Drawing;

namespace TagsCloudVisualization;

public class PointsOnSpiral
{
    private Point center;

    public PointsOnSpiral(Point center)
    {
        this.center = center;
    }

    public IEnumerable<Point> GetPointsOnSpiral()
    {
        var radius = 0;
        while (true)
        {
            for (var i = 0; i < 360; i++)
            {
                yield return new Point((int)(Math.Cos(2 * Math.PI * i / 360) * radius) + center.X,
                    (int)(Math.Sin(2 * Math.PI * i / 360) * radius) + center.Y);
            }

            radius++;
        }
    }
}