using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly IEnumerator<Point> _pointGeneratorIterator;
    private readonly List<Rectangle> _rectangles = new();

    public CircularCloudLayouter(Point center)
    {
        var pointGenerator = new SpiralPointGenerator(center);
        _pointGeneratorIterator = pointGenerator
            .GeneratePoint()
            .GetEnumerator();
    }

    public Rectangle PutNextRectangle(Size size)
    {
        if (size.Width < 1 || size.Height < 1)
            throw new ArgumentOutOfRangeException(
                $"{nameof(size.Width)} and {nameof(size.Height)} should be greater than zero");

        Rectangle newRectangle;
        do newRectangle = GetNextRectangle(size);
        while (_rectangles.Any(rec => rec.IntersectsWith(newRectangle)));

        _rectangles.Add(newRectangle);
        return newRectangle;
    }

    private Rectangle GetNextRectangle(Size rectangleSize) =>
        new(GetNextRectangleCenter(rectangleSize), rectangleSize);

    private Point GetNextRectangleCenter(Size rectangleSize)
    {
        _pointGeneratorIterator.MoveNext();
        var rectangleCenter = ShiftRectangleLocationBy(rectangleSize);
        return _pointGeneratorIterator
            .Current
            .MoveTo(rectangleCenter);
    }

    private static Size ShiftRectangleLocationBy(Size rectangleSize) =>
        new(-rectangleSize.Width / 2, rectangleSize.Height / 2);
}