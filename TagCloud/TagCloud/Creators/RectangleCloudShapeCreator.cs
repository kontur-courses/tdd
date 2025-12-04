using System.Drawing;
using TagCloud.Shapes;

namespace TagCloud.Creators;
public class RectangleCloudShapeCreator : ICloudShapeCreator
{ 
    public ICloudShape Create(Point center, Size size)
    {
        var left = center.X - size.Width / 2;
        var top = center.Y - size.Height / 2;
        var rect = new Rectangle(new Point(left, top), size);
        return new RectangleCloudShape(rect);
    }
}