using System;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ILayouter
{
    private readonly List<Rectangle> rectangles;
    private Point center;
    private double spiralStep;
    private double angle;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new List<Rectangle>();
        spiralStep = 1;
    }

    public IReadOnlyList<Rectangle> AddedRectangles => rectangles;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
            throw new ArgumentException($"{nameof(rectangleSize)} should be with positive width and height");
        var location = GetPosition(rectangleSize);
        var rectangle = new Rectangle(location, rectangleSize);
        rectangles.Add(rectangle);
        return rectangle;
    }

    private Point GetPosition(Size rectangleSize)
    {
        if (rectangles.Count == 0)
        {
            center.Offset(new Point(rectangleSize / -2));
            return center;
        }

        return FindApproximatePosition(rectangleSize);
    }

    private Point FindApproximatePosition(Size rectangleSize)
    {
        var currentAngle = angle;
        while (true)
        {
            var candidateLocation = new Point(center.X + (int)(spiralStep * Math.Cos(currentAngle)),
                center.Y + (int)(spiralStep * Math.Sin(currentAngle)));
            var candidateRectangle = new Rectangle(candidateLocation, rectangleSize);

            if (!IntersectsWithAny(candidateRectangle))
            {
                rectangles.Add(candidateRectangle);
                angle = currentAngle;
                return candidateRectangle.Location;
            }

            currentAngle += GetAngleStep();
            if (currentAngle > Math.PI * 2)
            {
                currentAngle %= Math.PI * 2;
                UpdateSpiral();
            }
        }
    }

    private bool IntersectsWithAny(Rectangle candidateRectangle)
    {
        return rectangles
            .Any(candidateRectangle.IntersectsWith);
    }

    private void UpdateSpiral()
    {
        spiralStep += 1;
    }

    private double GetAngleStep()
    {
        var defaultStep = Math.PI / 10;
        var count = (int)spiralStep / 10;
        if (count > 0)
        {
            defaultStep /= count;
        }

        return defaultStep;
    }
}