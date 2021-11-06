using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private static Dictionary<Size, int> sizeToParameter = new();

        public static IEnumerable<PointF> GetCoordinates(Size tagSize, float spiralPitch)
        {
            float parameter = (float)(Math.PI / 2);
            if (sizeToParameter.ContainsKey(tagSize))
                parameter = sizeToParameter[tagSize];
            foreach (var p in GetArchimedeanSpiral(parameter, spiralPitch))
            {
                yield return p;
            }    
        }


        private static void PolarToCartesian(float r, float theta,
            out float x, out float y)
        {
            x = (float)(r * Math.Cos(theta));
            y = (float)(r * Math.Sin(theta));
        }

        private static IEnumerable<PointF> GetArchimedeanSpiral(float paramteter, float pitch)
        {
            for (float p = 0;; p += 0.1f)
            {
                PolarToCartesian(p, pitch, out var x, out var y);
                yield return new PointF(x, y);
            }
        }
    }
}