using System;
using System.Drawing;


namespace TagsCloudVisualizer
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
