using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> cloudRectangles = new();
    private readonly double spiralStep;
    private readonly double radiusOffset;
    private readonly int indent;

    public CircularCloudLayouter(Point center, double spiralStep = 0.05, double radiusOffset = 3.0, int indent = 0)
    {
        this.center = center;
        this.spiralStep = spiralStep;
        this.radiusOffset = radiusOffset;
        this.indent = indent;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (cloudRectangles.Count == 0)
        {
            var firstRectangle = CreateRectangleWithCenter(center, rectangleSize);
            cloudRectangles.Add(firstRectangle);
            return firstRectangle;
        }

        for (var spiralShift = 0.0;; spiralShift += spiralStep)
        {
            var radius = radiusOffset * spiralShift;
            var candidateCenter = new PointF(
                center.X + (float)(radius * Math.Cos(spiralShift)),
                center.Y + (float)(radius * Math.Sin(spiralShift)));

            var candidate = CreateRectangleWithCenter(candidateCenter, rectangleSize);
            if (IntersectsAny(candidate))
            {
                continue;
            }

            candidate = CompactRectangle(candidate, rectangleSize);

            cloudRectangles.Add(candidate);
            return candidate;
        }
    }

    public CircularCloudLayouter WithRectangle(Size rectangleSize)
    {
        PutNextRectangle(rectangleSize);
        return this;
    }

    private Rectangle CreateRectangleWithCenter(PointF center, Size size)
    {
        var location = new Point((int)Math.Round(center.X - size.Width / 2.0), (int)Math.Round(center.Y - size.Height / 2.0));
        return new Rectangle(location, size);
    }

    private bool IntersectsAny(Rectangle rectangle)
    {
        var rectangleWithIndent = new Rectangle(
            rectangle.X - indent,
            rectangle.Y - indent,
            rectangle.Width + indent * 2,
            rectangle.Height + indent * 2);
        return cloudRectangles.Any(r => r.IntersectsWith(rectangleWithIndent));
    }

    private Rectangle CompactRectangle(Rectangle rectangle, Size size)
    {
        var rectangleXCenter = rectangle.X + rectangle.Width / 2.0;
        var rectangleYCenter = rectangle.Y + rectangle.Height / 2.0;
        var dx = center.X - rectangleXCenter;
        var dy = center.Y - rectangleYCenter;
        var distanceToCenter = Math.Sqrt(dx * dx + dy * dy);
        if (distanceToCenter < 1) return rectangle;

        var ux = dx / distanceToCenter;
        var uy = dy / distanceToCenter;

        while (true)
        {
            var nextXCenter = rectangleXCenter + ux;
            var nextYCenter = rectangleYCenter + uy;
            var nextRectangle = CreateRectangleWithCenter(new PointF((float)nextXCenter, (float)nextYCenter), size);

            if (IntersectsAny(nextRectangle)) return rectangle;
            if (nextRectangle == rectangle) return rectangle;

            rectangle = nextRectangle;
            rectangleXCenter = rectangle.X + rectangle.Width / 2.0;
            rectangleYCenter = rectangle.Y + rectangle.Height / 2.0;
        }
    }

    [SupportedOSPlatform("windows")]
    public void Save(string path, int x, int y)
    {
        using var bitmap = new Bitmap(x, y);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        foreach (var rectangle in cloudRectangles)
        {
            if (rectangle.Width >= 2 && rectangle.Height >= 2)
            {
                graphics.DrawRectangle(Pens.Black, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
            }
            else
            {
                graphics.FillRectangle(Brushes.Black, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }
        graphics.FillRectangle(Brushes.Red, center.X, center.Y, 1, 1);
        bitmap.Save(path, ImageFormat.Png);
    }
}
