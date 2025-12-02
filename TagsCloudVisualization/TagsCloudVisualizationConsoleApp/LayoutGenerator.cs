using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationApp;

public class LayoutGenerator
{
    private ILayouter _layouter;
    private Point _center;

    private IEnumerable<Rectangle> GenerateLayout(Func<int, Size> sizeSelector)
    {
        _center = new Point(GenerationConstants.CenterX, GenerationConstants.CenterY);
        _layouter = new CircularCloudLayouter(_center);
        for (var i = 0; i < GenerationConstants.DefaultRectNumber; i++)
        {
            var size = sizeSelector(i);
            _layouter.PutNextRectangle(size);
        }
        return _layouter.GetLayout();
    }

    public IEnumerable<Rectangle> GenerateBaseLayout()
        => GenerateLayout(_ => new Size(
            GenerationConstants.DefaultRectWidth,
            GenerationConstants.DefaultRectHeight));

    public IEnumerable<Rectangle> GenerateRandomLayout()
    {
        var random = new Random();
        return GenerateLayout(_ =>
        {
            var width = random.Next(
                GenerationConstants.DefaultRectWidth,
                GenerationConstants.DefaultRectHeight);
            var height = random.Next(
                GenerationConstants.DefaultRectWidth,
                GenerationConstants.DefaultRectHeight + 1);
            return new Size(width, height);
        });
    }

    public void DrawLayout(string filename)
        => CloudVisualizer.DrawRectangles(_layouter.GetLayout(), _center, filename);
}