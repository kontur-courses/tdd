using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface ISizeGenerator
    {
        IEnumerable<Size> GenerateSizes(int count);
    }
}