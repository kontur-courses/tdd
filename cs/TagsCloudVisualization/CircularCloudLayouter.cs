using System.Drawing;

namespace TagsCloudVisualization;

class CircularCloudLayouter
{
    private readonly Point Center;
    private readonly List<Rectangle> Rectangles = new();
    
    public CircularCloudLayouter(Point center)
    {
        Center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var x = Center.X - rectangleSize.Width / 2;
        var y = Center.Y - rectangleSize.Height / 2;
        var newRectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
        
        while (Rectangles.Any(existingRectangle => existingRectangle.IntersectsWith(newRectangle)))
        {
            newRectangle.Offset(0, 1);
        }
        
        Rectangles.Add(newRectangle);
        return newRectangle;
    }
}
