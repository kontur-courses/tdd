using System.Drawing;

namespace TagsCloudVisualization;

public interface IRectangleSizeProvider
{
    IEnumerable<Size> GetSizes(int count, Size minSize, Size maxSize);
}