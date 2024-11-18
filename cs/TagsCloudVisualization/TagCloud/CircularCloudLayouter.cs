using System.Drawing;

namespace TagsCloudVisualization.TagCloud;

public class CircularCloudLayouter(Point center, IPositionCalculator calculator)
{
    public List<Rectangle> Rectangles { get; private set; } = [];

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0)
            throw new ArgumentException("Size width must be positive number");
        if (rectangleSize.Height <= 0)
            throw new ArgumentException("Size height must be positive number");

        var temp = calculator.CalculateNextPosition(rectangleSize)
            .First(rectangle => calculator.IsRectanglePositionValid(Rectangles, rectangle));
        Rectangles.Add(temp);
        return temp;
    }
}