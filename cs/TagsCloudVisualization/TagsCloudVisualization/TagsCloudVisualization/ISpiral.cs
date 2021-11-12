using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public interface ISpiral
    {
        Point Center { get; }
        Point GetNextPosition();
        double AngleStep { get; }
        double Angle { get; }
        double DistanceBetweenTurns { get;}
    }
}
