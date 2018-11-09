using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public interface IReader
    {
        IEnumerable<Size> Read(string[] lines);
    }
}