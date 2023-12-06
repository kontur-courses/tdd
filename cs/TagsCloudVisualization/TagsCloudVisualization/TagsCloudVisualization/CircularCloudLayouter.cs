using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private Cloud Cloud { get; set; }
    private IRectanglesPlacer CircularPlacer { get; set; }

    public CircularCloudLayouter(Point center)
    {
        Cloud = new Cloud(center, new List<Rectangle>());
        CircularPlacer = new ArchimedeanSpiralPlacer(center, 0.1, 0.5, 1);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("either width or height of rectangle size is not possitive");

        var nextRectangle = new Rectangle(CircularPlacer.GetNextPoint(), rectangleSize);
        while (Cloud.Rectangles.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
            nextRectangle = new Rectangle(CircularPlacer.GetNextPoint(), rectangleSize);

        Cloud.AddRectangle(nextRectangle);
        return nextRectangle;
    }

    public void PutRectangles(List<Size> sizes)
    {
        if (sizes.Count == 0)
            throw new ArgumentException("пустые размеры");
        foreach (var size in sizes)
            PutNextRectangle(size);
    }

    public Cloud GetCloud() => Cloud;
}