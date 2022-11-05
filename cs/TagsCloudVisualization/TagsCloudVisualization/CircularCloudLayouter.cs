using System.Drawing;
using System.Numerics;

namespace TagsCloudVisualization;

class CircularCloudLayouter
{
    private RectanglesNet _rectanglesNet;
    
    public Point CenterMass
    {
        get { return _rectanglesNet.CenterMass; }
    }
    
    public Point Center
    {
        get { return _rectanglesNet.Center; }
    }

    public CircularCloudLayouter(Point center)
    {
        _rectanglesNet = new RectanglesNet(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return _rectanglesNet.AddRectToNet(rectangleSize);
    }
}