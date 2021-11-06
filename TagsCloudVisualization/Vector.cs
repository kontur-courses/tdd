using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Vector
    {
        public Point Begin;
        public Point End;

        public Vector(Point begin, Point end)
        {
            Begin = begin;
            End = end;
        }
    }
}
