using System.Drawing;
using TagCloud.Shapes;

namespace TagCloud.Creators;
public interface ICloudShapeCreator
{
    ICloudShape Create(Point center, Size size);
}