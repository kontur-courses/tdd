using System.Drawing;
using TagsCloudLib.Models;

namespace TagsCloudLib.Abstractions.Visualizer;

public interface IBoundingBoxCalculator
{
    LayoutBounds Calculate(IEnumerable<Rectangle> rectangles);
}