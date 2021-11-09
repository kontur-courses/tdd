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
        public Point GetNextPosition();
        double Radius { get; set; }
        double AngleStep { get; set; }
    }
}
