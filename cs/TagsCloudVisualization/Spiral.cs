using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private static Dictionary<Size, int> sizeToParameter = new();

        public static IEnumerable<Point> GetCoordinates(Size tagSize, double spiralPitch)
        {
            var parameter = Math.PI / 2;
            if (sizeToParameter.ContainsKey(tagSize))
                parameter = sizeToParameter[tagSize];
            while (true)
            {
                //yield return GetArchimedeanSpiral(parameter, spiralPitch);    
                parameter += Math.PI / (((int)parameter + 1) * 4);
            }
        }

        

        private static void PolarToCartesian(float r, float theta,
            out float x, out float y)
        {
            x = (float)(r * Math.Cos(theta));
            y = (float)(r * Math.Sin(theta));
        }

        private static Point GetArchimedeanSpiral(double paramteter, double pitch)
        {
            var coefficient = pitch / 2 * Math.PI;
            return new Point((int)(coefficient * paramteter * Math.Cos(paramteter)),
                (int)(coefficient * paramteter * Math.Sin(paramteter)));
        }
    }
}