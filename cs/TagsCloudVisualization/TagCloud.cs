using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class TagCloud(ICloudLayouter layouter)
{
    private readonly ICloudLayouter _layouter = layouter;
    public List<Rectangle> Rectangles { get; } = new();
    private int _maxRight = 0;
    private int _maxBottom = 0;
    private int _minLeft = int.MaxValue;
    private int _minTop = int.MaxValue;

    public void AddNextRectangleWith(Size size)
    {
        var nextRectangle = _layouter.PutNextRectangle(size);
        _maxRight = Math.Max(_maxRight, nextRectangle.Right);
        _maxBottom = Math.Max(_maxBottom, nextRectangle.Bottom);
        _minLeft = Math.Min(_minLeft, nextRectangle.Left);
        _minTop = Math.Min(_minTop, nextRectangle.Top);
        Rectangles.Add(nextRectangle);
    }

    public int Width =>
        Rectangles.Count == 0 ? 0 : _maxRight - _minLeft;

    public int Height =>
        Rectangles.Count == 0 ? 0 : _maxBottom - _minTop;

    public int LeftBound =>
        _minLeft;

    public int TopBound =>
        _minTop;
}