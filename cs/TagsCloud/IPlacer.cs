using System.Drawing;

namespace TagsCloud
{
    internal interface IPlacer<out T>
    {
        T Place(Point point, Size size);
    }
}