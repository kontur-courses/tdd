using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class ConvexHullBuilder
    {
        /// <summary>
        /// Returns the integer that indicates point location relative to the vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="point"></param>
        /// <returns>A number that indicates <paramref name="point"/> location relative to the
        /// <paramref name="vector"/>.
        /// Return value meaning: -1 <paramref name="point"/> is located to the right
        /// of the <paramref name="vector"/>.
        /// 0 <paramref name="point"/> is located on the <paramref name="vector"/>.
        /// 1 <paramref name="point"/> is located to the left of the <paramref name="vector"/>.
        /// </returns>
        public static int GetRotationDirection(Vector vector, Point point)
        {
            var vectorProduct = (vector.End.X - vector.Begin.X) * (point.Y - vector.Begin.Y) 
                                - (vector.End.Y - vector.Begin.Y) * (point.X - vector.Begin.X);
            return Math.Sign(vectorProduct);
        }
    }
}
