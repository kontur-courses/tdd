using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {

        private static long GetDistanceToRange(int number, int minValue, int maxValue)
        {
            if (number < minValue)
                return (long)minValue - number;
            return number > maxValue
                ? (long)number - maxValue
                : 0;
        }

        // Тесты для этого метода уже написаны в RectangleExtensions
        public static double DistanceTo(this Point point, Rectangle rectangle)
        {
            long differenceX = GetDistanceToRange(point.X, rectangle.Left, rectangle.Right);
            long differenceY = GetDistanceToRange(point.Y, rectangle.Top, rectangle.Bottom);

            long square = differenceX * differenceX + differenceY * differenceY;
            return Math.Sqrt(square);
        }
    }
}
