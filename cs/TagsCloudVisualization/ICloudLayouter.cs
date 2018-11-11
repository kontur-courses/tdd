using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        IEnumerable<Rectangle> GetLayout();
    }
}