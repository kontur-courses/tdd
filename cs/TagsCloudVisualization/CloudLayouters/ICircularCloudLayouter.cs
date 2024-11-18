using System.Drawing;

namespace TagsCloudVisualization.CloudLayouters;

public interface ICircularCloudLayouter
{
    public Rectangle PutNextRectangle(Size rectangleSize);
}