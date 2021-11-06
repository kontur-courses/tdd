using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ILayouter<out TFigure>
    {
        TFigure PutNextRectangle(Size rectangleSize);
    }
}