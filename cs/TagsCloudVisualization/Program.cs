using System.Drawing;
using TagsCloudVisualization;

class CircularCloudLayouter
{
    private Point cloudCenter;
    private List<Rectangle> rectangels = new List<Rectangle>();
    private PointsOnSpiral points;

    public CircularCloudLayouter(Point cloudCenter)
    {
        this.cloudCenter = cloudCenter;
        points = new PointsOnSpiral(cloudCenter);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        foreach (var point in points.GetPointsOnSpiral())
        {
            var rectangle = new Rectangle(point, rectangleSize);
            if (rectangels.Any(x => x.IntersectsWith(rectangle)))
                continue;
            rectangels.Add(rectangle);
            return rectangle;
        }

        return new Rectangle(cloudCenter, rectangleSize);
    }
}