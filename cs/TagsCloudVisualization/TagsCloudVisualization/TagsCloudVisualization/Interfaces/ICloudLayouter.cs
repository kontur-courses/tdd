using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ICloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    Cloud GetCloud();
    void PutRectangles(List<Size> sizes);
}