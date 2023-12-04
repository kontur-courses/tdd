using SixLabors.ImageSharp;

namespace TagsCloudVisualization;

public class CircularCloudLayout : ILayout
{
    private PointF center;
    private readonly List<RectangleF> placedRectangles = new();
    private readonly Spiral spiral;
    
    public CircularCloudLayout(PointF center, float distanceDelta, float angleDelta)
    {
        this.center = center;
        spiral = new Spiral(distanceDelta, angleDelta);
    }
    
    public RectangleF PutNextRectangle(SizeF rectSize)
    {
        var rectangle = GetCorrectlyPlacedRectangle(rectSize);
        placedRectangles.Add(rectangle);
        
        return rectangle;
    }

    private RectangleF GetCorrectlyPlacedRectangle(SizeF rectSize)
    {
        var point = spiral.GetNextPoint();
        var rectangle = new RectangleF(point, rectSize);

        while (true)
        {
            point.Center(ref center);
            point.ApplyOffset(-rectSize.Width / 2, -rectSize.Height / 2);
            
            (rectangle.X, rectangle.Y) = point;
            
            if (placedRectangles.All(rect => !rect.IntersectsWith(rectangle)))
                break;

            point = spiral.GetNextPoint();
        }

        return rectangle;
    }
}