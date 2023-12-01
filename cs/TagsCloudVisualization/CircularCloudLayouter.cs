using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private Point center;

    public int Quantity
    { private set; get; }
        
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(int width = 10, int height = 10)
    {
        return PutNextRectangle(new Size(width, height));
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        Quantity++;
        
        return new Rectangle();
    }
}