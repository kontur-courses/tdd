using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class MathHelper
    {
        public static int MaxAbs(int val1, int val2) =>
            Math.Abs(val1) == Math.Max(Math.Abs(val1), Math.Abs(val2)) ? val1 : val2;
    }
}
