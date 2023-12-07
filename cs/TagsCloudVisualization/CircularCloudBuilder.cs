using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudBuilder : ICircularCloudBuilder
{
    private readonly int radiusIncrement;
    private readonly double angleIncrement;

    public CircularCloudBuilder(int radiusIncrement, double angleIncrement)
    {
        if (radiusIncrement == 0 || Math.Abs(angleIncrement - 0) < 1e-3)
            throw new ArgumentException("Parameters for the algorithm must not be null");

        this.radiusIncrement = radiusIncrement;
        this.angleIncrement = angleIncrement;
    }

    private static void ValidateArguments(Size rectangleSize, List<Rectangle> placedRectangles)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Height and width must not be negative.");
        if (placedRectangles is null)
            throw new ArgumentException("The list of placed rectangles cannot be null.");
    }

    public Rectangle GetNextPosition(Point center, Size rectangleSize, List<Rectangle> placedRectangles)
    {
        ValidateArguments(rectangleSize, placedRectangles);

        var radius = 0d;
        var angle = 0d;
        var rectangle = new Rectangle(Point.Empty, rectangleSize);
        do
        {
            var x = center.X + radius * Math.Cos(angle);
            var y = center.Y + radius * Math.Sin(angle);

            angle += angleIncrement;

            if (Math.Abs(angle) >= 2 * Math.PI)
            {
                angle = 0;
                radius += radiusIncrement;
            }

            rectangle.Location = new Point((int) x, (int) y);
        } while (placedRectangles.AnyIntersectsWith(rectangle));

        return rectangle;
    }
}