using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly double _spiralStep;
    private readonly ArithmeticSpiral _arithmeticSpiral;
    private readonly List<Rectangle> _rectangles;

    public IReadOnlyCollection<Rectangle> Rectangles => _rectangles;

    public CircularCloudLayouter(Point center, double spiralStep = 1)
    {
        if (spiralStep <= 0)
            throw new ArgumentException("Zero or negative  spiral step are not allowed");

        _spiralStep = spiralStep;
        _arithmeticSpiral = new ArithmeticSpiral(center);
        _rectangles = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.IsEmpty)
            return new Rectangle(0, 0, 0, 0);

        if (rectangleSize.Width < 1 || rectangleSize.Height < 1)
            return new Rectangle(0, 0, 0, 0);

        var currentLength = 0d;
        var nextPoint = _arithmeticSpiral.GetPoint(currentLength);
        var rectangle = new Rectangle(nextPoint, rectangleSize);


        while (_rectangles.Any(rect => rect.IntersectsWith(rectangle)))
        {
            currentLength += _spiralStep;

            nextPoint = _arithmeticSpiral.GetPoint(currentLength);
            rectangle = new Rectangle(nextPoint, rectangleSize);
        }

        _rectangles.Add(rectangle);
        return rectangle;
    }
}