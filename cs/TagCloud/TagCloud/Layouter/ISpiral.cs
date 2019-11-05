using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public interface ISpiral
    {
        IEnumerable<Point> IterateBySpiralPoints();
    }
}