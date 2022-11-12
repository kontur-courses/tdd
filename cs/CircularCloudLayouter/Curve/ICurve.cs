using System.Collections.Generic;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public interface ICurve : IEnumerable<Point>
    {
        void ChangeCenterPoint(Point newCenter);

        Point Center { get; }
    }
}