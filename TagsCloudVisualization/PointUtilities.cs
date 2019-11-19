using System;

namespace TagsCloudVisualization
{
    public static class PointConverter
    {
        public static (float x, float y) TransformPolarToCartesian(float r, float theta)
        {
            return (x: (float) (r * Math.Cos(theta)), y: (float) (r * Math.Sin(theta)));
        }

        public static (float r, float theta) TransformCartesianToPolar(float x, float y)
        {
            return (r: (float) Math.Sqrt(x * x + y * y), theta: (float) Math.Atan2(y, x));
        }
    }
}