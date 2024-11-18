using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class CircularCloudLayouterWorker
    {
        private static Random _random = new Random();

        public static Size GetNextRectangleSize(
            int minRectangleWidth,
            int maxRectangleWidth,
            int minRectangleHeight,
            int maxRectangleHeight)
        {
            if (minRectangleWidth <= 0 || maxRectangleWidth <= 0
                || minRectangleHeight <= 0 || maxRectangleHeight <= 0)
            {
                throw new ArgumentException(
                    "Ширина или высота прямоугольника должна быть положительной");
            }

            if (minRectangleWidth > maxRectangleWidth
                || minRectangleHeight > maxRectangleHeight)
            {
                throw new ArgumentException(
                    "Минимальное значение ширины или высоты не может быть больше максимального");
            }

            var width = _random.Next(minRectangleWidth, maxRectangleWidth);
            var height = _random.Next(minRectangleHeight, maxRectangleHeight);
            return new Size(width, height);
        }
    }
}