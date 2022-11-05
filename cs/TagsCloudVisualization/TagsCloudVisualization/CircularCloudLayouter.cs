using System.Drawing;
using System.Numerics;

namespace TagsCloudVisualization;

class CircularCloudLayouter
{
    private RectanglesNet _rectanglesNet;

    public CircularCloudLayouter(Point center)
    {
        _rectanglesNet = new RectanglesNet(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return _rectanglesNet.AddRectToNet(rectangleSize);
    }
}