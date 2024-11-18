using System.Drawing;
using TagsCloudVisualization.SpiralLayouter.PointGenerator;

namespace TagsCloudVisualization.SpiralLayouter;

public class SpiralCloudLayouter : ICloudLayouter
{
    private List<Point> placedPoints;
    private IPointGenerator pointGenerator;
    private List<Rectangle> placedRectangles;

    public Point Center { get; }

    public SpiralCloudLayouter(Point center, IPointGenerator pointGenerator)
    {
        Center = center;
        placedPoints = [];
        placedRectangles = [];
        this.pointGenerator = pointGenerator;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        Rectangle placedRect;
        try
        {
            placedRect = pointGenerator.StartFrom(Center)
                .Except(placedPoints)
                .Select(p => CreateRectangle(p, rectangleSize))
                .First(r => !placedRectangles.Any(r.IntersectsWith));
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException("There are no more points in generator");
        }

        placedRectangles.Add(placedRect);
        placedPoints.Add(placedRect.Location - placedRect.Size / 2);
        
        return placedRect;
    }

    private Rectangle CreateRectangle(Point center, Size rectangleSize)
    {
        var rectangleUpperLeft = center - rectangleSize / 2;
        return new Rectangle(rectangleUpperLeft, rectangleSize);
    }
}