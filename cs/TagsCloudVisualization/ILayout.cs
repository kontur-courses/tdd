using SixLabors.ImageSharp;

namespace TagsCloudVisualization;

public interface ILayout
{
    public IList<RectangleF> PlacedFigures { get; }
    public void PutNextRectangle(SizeF rectSize);
}