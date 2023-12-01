using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private Point center;
    private Dictionary<string, int> frequencyDict;
        
    public CircularCloudLayouter(Point center, Dictionary<string, int> frequencyDict)
    {
        this.center = center;
        this.frequencyDict = frequencyDict;
    }

    public Rectangle PutNextRectangle(int width = 10, int height = 10)
    {
        return PutNextRectangle(new Size(width, height));
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return new Rectangle();
    }
}