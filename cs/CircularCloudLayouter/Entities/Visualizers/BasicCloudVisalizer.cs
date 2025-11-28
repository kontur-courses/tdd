using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualisation.Entities.Visualizer
{
    public class BasicCloudVisalizer : ICloudVisualizer
    {
        public void Visualize(List<Rectangle> rectangles, Point center, string filePath)
        {
            var allPoints = rectangles.SelectMany(r => new[]
            {
            new Point(r.Left, r.Top),
            new Point(r.Right, r.Bottom)
            }).Concat(new[] { center });

            int minX = allPoints.Min(p => p.X);
            int minY = allPoints.Min(p => p.Y);
            int maxX = allPoints.Max(p => p.X);
            int maxY = allPoints.Max(p => p.Y);

            int padding = 50;
            int width = maxX - minX + 2 * padding;
            int height = maxY - minY + 2 * padding;

            using (var bitmap = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.DarkGray);

                int offsetX = -minX + padding;
                int offsetY = -minY + padding;

                var color = Color.DarkBlue;
                
                foreach(var rectangle in rectangles)
                {
                    var drawRect = new Rectangle(
                        rectangle.X + offsetX,
                        rectangle.Y + offsetY,
                        rectangle.Width,
                        rectangle.Height);

                    using (var brush = new SolidBrush(color))
                    using (var pen = new Pen(Color.Black, 1))
                    {
                        graphics.FillRectangle(brush, drawRect);
                        graphics.DrawRectangle(pen, drawRect);
                    }
                }

                using (var centerBrush = new SolidBrush(Color.Red))
                {
                    var centerPoint = new Point(center.X + offsetX, center.Y + offsetY);
                    graphics.FillEllipse(centerBrush, centerPoint.X-2, centerPoint.Y-2, 4, 4);
                }

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    bitmap.Save(stream, ImageFormat.Png);
                }
            }
        }
    }
}
