using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface IPlacer<out T>
    {
        T Place(Point point, Size size);
    }
}