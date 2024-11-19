using System.Drawing;

namespace TagsCloudVisualization.Interfaces;

public interface ISizesGenerator
{
    public IEnumerable<Size> GenerateSize();
}