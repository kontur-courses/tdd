using System.Drawing;

namespace TagsCloudVisualization.Layouter;

public class CircularCloudLayouter : ICloudLayouter
{
    public Point Center { get; }
    private readonly List<Rectangle> tags;
    private Point currentPoint;
    private double angle;

    public CircularCloudLayouter(Point center)
    {
        Center = center;
        tags = new List<Rectangle>();
        currentPoint = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size of rectangular must be positive");
        var rectangle = new Rectangle(currentPoint, rectangleSize);
        while (!CanBePlaced(rectangle))
        {
            CalculateNewPoint();
            rectangle.Location = currentPoint;
        }

        tags.Add(rectangle);
        return rectangle;
    }

    public Rectangle[] GetTagsLayout()
    {
        return tags.ToArray();
    }

    public void ClearLayout()
    {
        tags.Clear();
        angle = 0.0;
    }

    private void CalculateNewPoint()
    {
        currentPoint.X = (int)(Math.Cos(angle) * angle + Center.X);
        currentPoint.Y = (int)(Math.Sin(angle) * angle + Center.Y);
        angle += 0.001;
    }

    private bool CanBePlaced(Rectangle rectangle)
    {
        return tags.All(tag => !tag.IntersectsWith(rectangle));
    }
}