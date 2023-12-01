using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private Point center;
    private Dictionary<string, int> frequencyDict;
    private Size displayResolution;
    
    public CircularCloudLayouter(Point center, Dictionary<string, int> frequencyDict, Size displayResolution)
    {
        this.center = center;
        this.frequencyDict = frequencyDict;
        this.displayResolution = displayResolution;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return new Rectangle();
    }
}