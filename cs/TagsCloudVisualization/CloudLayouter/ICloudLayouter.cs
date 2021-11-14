using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter
{
    public interface ICloudLayouter
    {
        SizeF Size { get;}
        PointF Center { get; }
        RectangleF PutNextRectangle(Size rectangleSize);
    }
}