using System;

namespace TagsCloudVisualization
{
    public static class PointConverter
    {
        public static Tuple<float, float> TransformPolarToCartesian(float r, float theta)
        {
            return new Tuple<float, float>(
                (float)(r * Math.Cos(theta)),
                (float)(r * Math.Sin(theta)));
        }

        public static Tuple<float, float> TransformCartesianToPolar(float x, float y)
        {
            var theta = (float)Math.Atan2(y, x);
            var r = (float)Math.Sqrt(x * x + y * y);
            return new Tuple<float, float>(r, theta);
        }
    }
}