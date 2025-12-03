using System.Drawing;
using TagsCloudVisualization.CoordinateGenerators;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point _center;
    private readonly ICoordinateGenerator _generator;
    private readonly List<Rectangle> _rectangles;

    public CircularCloudLayouter(Point center, ICoordinateGenerator generator)
    {
        _center = center;
        _generator = generator;
        _rectangles = new();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        Rectangle rectangle;
        Point position;

        do
        {
            position = _generator.GetNextPosition();
            rectangle = new Rectangle(position.X - rectangleSize.Width / 2,
                position.Y - rectangleSize.Height / 2,
                rectangleSize.Width, rectangleSize.Height);
        } while (rectangle.HasIntersections(_rectangles));

        _rectangles.Add(rectangle);
        return rectangle;
    }

    public IReadOnlyCollection<Rectangle> Rectangles => _rectangles.AsReadOnly();
}