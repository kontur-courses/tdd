using System.Drawing;

namespace TagsCloud.Core
{
    internal interface ICircularCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}