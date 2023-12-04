using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    public Point Center { get; private set; }
    public List<Rectangle> Rectangles { get; private set; }
    public IRectanglesPlacer CircularPlacer { get; private set; }

    public CircularCloudLayouter(Point center)
    {
        Center = center;
        Rectangles = new List<Rectangle>();
        CircularPlacer = new ArchimedeanSpiralPlacer(center, 0.1, 0.5, 1);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("either width or height of rectangle size is not possitive");

        var nextRectangle = CircularPlacer.GetNextRectangle(rectangleSize);
        while (Rectangles.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
            nextRectangle = CircularPlacer.GetNextRectangle(rectangleSize);

        Rectangles.Add(nextRectangle);
        return nextRectangle;
    }

    public List<Rectangle> GetLayout() => Rectangles;
}