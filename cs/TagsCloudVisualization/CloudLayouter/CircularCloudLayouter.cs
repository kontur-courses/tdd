using SixLabors.ImageSharp;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly PointF center;
    private readonly ArchimedeanSpiralPointGenerator spiral;
    public List<RectangleF> Rectangles { get; }

    public CircularCloudLayouter(PointF center)
    {
        this.center = center;
        Rectangles = new List<RectangleF>();
        spiral = new ArchimedeanSpiralPointGenerator(center);
    }

    public RectangleF PutNextRectangle(SizeF rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size should be positive value", nameof(rectangleSize));

        RectangleF newRectangle;
        do
        {
            var location = spiral.GetNextPoint();
            var rectangleLocation = new PointF(location.X - rectangleSize.Width / 2,
                location.Y - rectangleSize.Height / 2);
            newRectangle = new RectangleF(rectangleLocation, rectangleSize);

            newRectangle = ShiftRectangleTowardsCenter(newRectangle);
        } while (newRectangle.IntersectsWithAny(Rectangles));

        Rectangles.Add(newRectangle);
        return newRectangle;
    }

    private RectangleF ShiftRectangleTowardsCenter(RectangleF rectangle)
    {
        if (Rectangles.Count == 0)
            return rectangle;

        var shiftFactor = 0.001f;
        var canShiftX = true;
        var canShiftY = true;

        while (canShiftX || canShiftY)
        {
            var centerShift = new PointF(center.X - rectangle.X, center.Y - rectangle.Y);

            var shiftX = canShiftX ? centerShift.X * shiftFactor : 0;
            var shiftY = canShiftY ? centerShift.Y * shiftFactor : 0;

            var shiftedRectangle = new RectangleF(
                rectangle.X + shiftX,
                rectangle.Y + shiftY,
                rectangle.Width,
                rectangle.Height);

            if (shiftedRectangle.IntersectsWithAny(Rectangles))
            {
                if (canShiftX)
                    canShiftX = false;
                if (canShiftY)
                    canShiftY = false;
            }
            else
            {
                rectangle = shiftedRectangle;
            }
        }

        return rectangle;
    }
}