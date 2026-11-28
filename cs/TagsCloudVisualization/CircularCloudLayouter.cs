using System.Drawing;

namespace TagsCloudVisualization;

/// <summary>
/// Располагает прямоугольники в виде облака вокруг заданного центра.
/// </summary>
/// <param name="center">Центр облака тегов.</param>
/// <param name="provider">Провайдер, определяющий последовательность центров,
/// на которых располагаются прямоугольники</param>
public class CircularCloudLayouter(ICenterPointProvider provider)
{
    private readonly List<Rectangle> rectangles = [];

    public CircularCloudLayouter(Point center)
        : this(ArchimedeanSpiralDefaults.Create(center))
    {
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Height);

        // пока самый простой вариант - если точка не подошла, то берем новую
        var newRectangleCenter = provider.GetNextCenterPosition();
        var potentialRectangle = new Rectangle(
            GetRectangleLeftTopPoint(newRectangleCenter, rectangleSize), 
            rectangleSize);
        
        while (rectangles.Any(r => r.IntersectsWith(potentialRectangle)))
        {
            newRectangleCenter = provider.GetNextCenterPosition();
            potentialRectangle = new Rectangle(
                GetRectangleLeftTopPoint(newRectangleCenter, rectangleSize), 
                rectangleSize);
        }
        
        rectangles.Add(potentialRectangle);
        return potentialRectangle;
    }
    
    private static Point GetRectangleLeftTopPoint(Point rectCenter, Size rectSize)
    {
        return new Point(
            rectCenter.X - rectSize.Width / 2,
            rectCenter.Y - rectSize.Height / 2
        );
    }
}