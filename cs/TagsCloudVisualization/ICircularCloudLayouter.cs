using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface ICircularCloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}