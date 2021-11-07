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
            var offset = GetMinPoint();
            var brush = new SolidBrush(Color.Red);
            foreach (var point in points)
                graphics.FillRectangle(brush, point.X - offset.X, point.Y - offset.Y, 1, 1);
        }

        public Size GetBitmapSize()
        {
            if (points.Count == 0)
                return new Size(1, 1);

            var min = GetMinPoint();
            var max = GetMaxPoint();

            return new Size(
                max.X - min.X + 1,
                max.Y - min.Y + 1);
        }

        private Point GetMinPoint() => points.Aggregate((min, current) =>
            new Point(Math.Min(current.X, min.X), Math.Min(current.Y, min.Y)));

        private Point GetMaxPoint() => points.Aggregate((max, current) =>
            new Point(Math.Max(current.X, max.X), Math.Max(current.Y, max.Y)));
    }
}