using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualizationTests
{
    public class PointsVisualizer
    {
        private readonly Size size;
        private readonly List<Point> points;

        public PointsVisualizer(Size bitmapSize, IEnumerable<Point> points)
        {
            size = bitmapSize;
            this.points = points.ToList();
        }

        public void SaveToBitmap(string filename)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);

            DrawPoints(graphics);
            graphics.Save();
            bitmap.Save(filename);
        }

        private void DrawPoints(Graphics graphics)
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