using SixLabors.ImageSharp;

namespace TagsCloudVisualization;

public interface ILayout
{
    public ICollection<RectangleF> PlacedFigures { get; }
    public void PutNextRectangle(SizeF rectSize);
}