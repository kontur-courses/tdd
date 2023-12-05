using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    public List<Rectangle> PlacedRectangles { get; } = new();
    private readonly ICircularCloudBuilder circularCloudBuilder;

    public CircularCloudLayouter(ICircularCloudBuilder circularCloudBuilder)
    {
        this.circularCloudBuilder = circularCloudBuilder;
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Height and width must be positive");

        var nextRectangle = circularCloudBuilder.GetNextPosition(rectangleSize, PlacedRectangles);
        
        PlacedRectangles.Add(nextRectangle);
        return nextRectangle;
    }
}