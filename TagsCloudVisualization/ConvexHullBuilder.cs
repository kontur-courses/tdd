using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class ConvexHullBuilder
    {
        public static int GetRotationDirection(Vector vector, Point point)
        {
            var vectorProduct = (vector.End.X - vector.Begin.X) * (point.Y - vector.Begin.Y) 
                                - (vector.End.Y - vector.Begin.Y) * (point.X - vector.Begin.X);
            return Math.Sign(vectorProduct);
        }
    }
}
