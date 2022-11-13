using System.Collections.Generic;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public interface ICurve : IEnumerable<Point>
    {
        Point Center { get; }
    }
}