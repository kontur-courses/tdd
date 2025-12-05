using System.Drawing;
using TagsCloudVisualization.PointGenerator;


namespace TagsCloudVisualization.CircularCloudLayouter;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> rectangles = [];

    private readonly IPointGenerator pointGenerator;

    private readonly int maxPointsPerRectangle;

    public CircularCloudLayouter(Point center, int maxPointsPerRectangle, IPointGenerator pointGenerator)
    {
        ArgumentNullException.ThrowIfNull(pointGenerator);

        if (center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("Center should be greater than 0");

        if (maxPointsPerRectangle <= 0)
            throw new ArgumentException("maxPointsPerRectangle must be greater than 0");

        this.maxPointsPerRectangle = maxPointsPerRectangle;
        this.pointGenerator = pointGenerator;
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive");

        var points = pointGenerator
            .GetPoints()
            .Take(maxPointsPerRectangle);


        foreach (var point in points)
        {
            var rectangle = new Rectangle(point, rectangleSize);
            for (var i = rectangles.Count - 1; i >= 0; i--)
            {
                if (rectangles[i].IntersectsWith(rectangle))
                    goto NextPoint;
            }

            rectangles.Add(rectangle);
            return rectangle;
            NextPoint: ;
        }

        throw new ArgumentException($"Failed to find place for the {rectangles.Count} rectangle");
    }
}