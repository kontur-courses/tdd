using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly Cloud cloud;
    private readonly IFormPointer circularPlacer;

    public CircularCloudLayouter(Point center)
    {
        cloud = new Cloud(center, new List<Rectangle>());
        circularPlacer = new ArchimedeanSpiralPointer(center, 0.1, 0.5, 1);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("either width or height of rectangle size is not possitive");

        var nextRectangle = Utils.Utils.GetRectangleFromCenter(circularPlacer.GetNextPoint(), rectangleSize);
        while (cloud.Rectangles.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
            nextRectangle = Utils.Utils.GetRectangleFromCenter(circularPlacer.GetNextPoint(), rectangleSize);

        cloud.AddRectangle(nextRectangle);

        return nextRectangle;
    }

    public void PutRectangles(List<Size> sizes)
    {
        if (sizes.Count == 0)
            throw new ArgumentException("пустые размеры");
        foreach (var size in sizes)
            PutNextRectangle(size);
    }

    public Cloud GetCloud() => cloud;
}