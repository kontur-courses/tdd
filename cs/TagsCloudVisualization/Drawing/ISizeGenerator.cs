using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Drawing
{
    public interface ISizeGenerator
    {
        IEnumerable<Size> GenerateSizes(int count);
    }
}