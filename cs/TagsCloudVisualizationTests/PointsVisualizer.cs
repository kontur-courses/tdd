using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualizationTests
{
    public class PointsVisualizer : Visualizer
    {
        private readonly List<Point> points;

        public PointsVisualizer(Size bitmapSize, IEnumerable<Point> points) : base(bitmapSize)
        {
            this.points = points.ToList();
        }

        protected override void Draw(Graphics graphics)
        {
            var pen = new Pen(Color.OrangeRed, 1);
            var pointsList = points.ToList();
            var startPoint = pointsList.First();
            foreach (var point in pointsList.Skip(1))
            {
                graphics.DrawLine(pen, startPoint, point);
                startPoint = point;
            }
        }
    }
}