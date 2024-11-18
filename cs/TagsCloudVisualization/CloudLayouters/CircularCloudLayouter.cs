using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualization.CloudLayouters;

public class CircularCloudLayouter(Point layoutCenter, IPointsGenerator pointsGenerator) : ICircularCloudLayouter
{
    private const string FiniteGeneratorExceptionMessage =
        "В конструктор CircularCloudLayouter был передан конечный генератор точек";
    private readonly IEnumerator<Point> pointEnumerator = pointsGenerator
        .GeneratePoints(layoutCenter)
        .GetEnumerator();
    private readonly List<Rectangle> layoutRectangles = [];

    public CircularCloudLayouter(Point layoutCenter, double radius, double angleOffset) :
        this(layoutCenter, new FermatSpiralPointsGenerator(radius, angleOffset))
    {
        
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = pointEnumerator
            .ToIEnumerable()
            .Select(point => new Rectangle()
                .CreateRectangleWithCenter(point, rectangleSize))
            .FirstOrDefault(rectangle => !layoutRectangles.Any(rectangle.IntersectsWith));
        
        if (rectangle.IsEmpty)
            throw new InvalidOperationException(FiniteGeneratorExceptionMessage);

        layoutRectangles.Add(rectangle);
        return rectangle;
    }

}