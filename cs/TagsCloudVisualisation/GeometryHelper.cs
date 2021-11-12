using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualisation
{
    public static class GeometryHelper
    {
        public static Point ConvertFromPolarToDecartWithFlooring(double currentAngle, double currentRadius)
        {
            var x = Math.Cos(currentAngle) * currentRadius;
            var y = Math.Sin(currentAngle) * currentRadius;
            return new Point((int)x, (int)y);
        }
    }
}
