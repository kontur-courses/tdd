using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualizationTests
{
    public class PointsVisualizer : IVisualizer
    {
        private readonly List<Point> points;

        public PointsVisualizer(IEnumerable<Point> points)
        {
            this.points = points.ToList();
        }

        public void Draw(Graphics graphics)
        {
            var offset = PointHelper.GetTopLeftAge(points);
            var brush = new SolidBrush(Color.Red);
            foreach (var point in points)
                graphics.FillRectangle(brush, point.X - offset.X, point.Y - offset.Y, 1, 1);
        }

        public Size GetBitmapSize()
        {
            if (points.Count == 0)
                return new Size(1, 1);

            var topLeft = PointHelper.GetTopLeftAge(points);
            var bottomRight = PointHelper.GetBottomRightAge(points);

            return new Size(
                bottomRight.X - topLeft.X + 1,
                bottomRight.Y - topLeft.Y + 1);
        }
    }
}