using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudLayouter // так это получается паттерн стратегия
{
    public Point Start { get; }


    public Rectangle PutNextRectangle(Size rectangleSize);
}