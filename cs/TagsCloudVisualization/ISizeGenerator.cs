using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ISizeGenerator
    {
        IEnumerable<Size> GenerateSizes(int count);
    }
}