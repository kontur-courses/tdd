using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    public List<Rectangle> PlacedRectangles { get; } = new();
    private readonly Point center;
    private readonly ICircularCloudBuilder circularCloudBuilder;

    public CircularCloudLayouter(Point center, ICircularCloudBuilder circularCloudBuilder)
    {
        this.circularCloudBuilder = circularCloudBuilder;
        this.center = center;
    }

    private Rectangle PlaceFirstRectangle(Size rectangleSize)
    {
        var firstRectangleLocation = new Point(center.X - rectangleSize.Width / 2,
            center.Y - rectangleSize.Height / 2);
        return new Rectangle(firstRectangleLocation, rectangleSize);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Height and width must be positive");

        if (PlacedRectangles.Count == 0)
        {
            var firstRectangle = PlaceFirstRectangle(rectangleSize);
            PlacedRectangles.Add(firstRectangle);
            return firstRectangle;
        }

        var rectangle = circularCloudBuilder.GetNextPosition(center, rectangleSize, PlacedRectangles);

        PlacedRectangles.Add(rectangle);
        return rectangle;
    }
}