using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly ICloudDistribution distribution;
    private readonly List<Rectangle> rectangles = [];
    private readonly Point center = new();
    
    public CircularCloudLayouter(ICloudDistribution distribution)
    {
        this.distribution = distribution;
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("The rectangle size must be greater than zero.");
        
        var newRectangle = new Rectangle(distribution.GetNextPoint(), rectangleSize);
        
        while (IsIntersecting(newRectangle))
            newRectangle.Location = distribution.GetNextPoint();
        
        rectangles.Add(newRectangle);
        return newRectangle;
    }

    public List<Rectangle> GetRectangles() => rectangles;
    
    public Point GetCenter() => center; 

    public Size GetCloudSize()
    {
        if (rectangles.Count == 0)
            throw new ArgumentException("Cloud cannot be empty.");
        
        var maxX = rectangles.Select(rec => rec.X + rec.Width).Max();
        var minX = rectangles.Select(rec => rec.X).Min();
        var maxY = rectangles.Select(rec => rec.Y + rec.Height).Max();
        var minY = rectangles.Select(rec => rec.Y).Min();
        
        return new Size(maxX - minX, maxY - minY);
    }
    
    private bool IsIntersecting(Rectangle newRectangle) 
        => rectangles.Any(r => r.IntersectsWith(newRectangle));
}