using System.Drawing;
using TagCloud2;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private const double CoefficientRadius = 1.3;
    private const int CoefficientAngle = 14;
    private readonly List<Rectangle> rectangles;
    private double angle;
    public CircularCloudLayouter(SettingsTagCloud settingsTagCloud)
    {
        Validate(settingsTagCloud.Center);
        rectangles = [];
        Start = settingsTagCloud.Center;
    }

    public Point Start { get; }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rec = Rectangle.Empty;
        do
        {
            angle += Math.PI / CoefficientAngle;
            var radius = CoefficientRadius * angle;
            var x = (int)(Start.X + radius * Math.Cos(angle));
            var y = (int)(Start.Y + radius * Math.Sin(angle));
            rec = new Rectangle(new Point(x, y), rectangleSize);
        } while (AnyIntersectWithRec(rec));

        if (rectangles.Count != 0) rec = Sealing(rec);
        rectangles.Add(rec);
        return rec;
    }

    private static void Validate(Point center)
    {
        if (center.X < 0) throw new ArgumentException("X has value less than 0");
        if (center.Y < 0) throw new ArgumentException("Y has value less than 0");
    }

    private Rectangle Sealing(Rectangle rec)
    {
        while (Start.Y - rec.Bottom > 1 && !AnyIntersectWithRec(rec with { Y = rec.Y + 2 })) rec.Y += 1;
        while (rec.Top - Start.Y > 1 && !AnyIntersectWithRec(rec with { Y = rec.Y - 2 })) rec.Y -= 1;
        while (rec.Left - Start.X > 1 && !AnyIntersectWithRec(rec with { X = rec.X - 2 })) rec.X -= 1;
        while (Start.X - rec.Right > 1 && !AnyIntersectWithRec(rec with { X = rec.X + 2 })) rec.X += 1;

        return rec;
    }

    private bool AnyIntersectWithRec(Rectangle rec)
    {
        return rectangles.Any(rectangle => rectangle.IntersectsWith(rec));
    }
}