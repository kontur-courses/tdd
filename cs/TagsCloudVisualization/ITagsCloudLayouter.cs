using System.Drawing;

namespace TagsCloudVisualization
{
    interface ITagsCloudLayouter
    {
        ITagsCloud TagsCloud { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}