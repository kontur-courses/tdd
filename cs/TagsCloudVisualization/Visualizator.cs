using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public static class Visualizator
    {
        public static readonly Color BackgroundColor = Color.AliceBlue;
        public static readonly Color ObjectsColor = Color.Blue;

        public static void VizualizeToFile(
            IEnumerable<Rectangle> rectangles,
            int width,
            int height,
            string path)
        {
            var bitmap = new Bitmap(width * 2 + 2, height * 2 + 2);
            var rectsToDraw = rectangles
                .Select(r => NormalizeToDrawing(r, width, height))
                .Select(ConvertRectangleToRectangleF)
                .ToArray();
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(BackgroundColor);
                using (var pen = new Pen(ObjectsColor))
                {
                    graphics.DrawRectangles(pen, rectsToDraw);
                }
            }

            bitmap.Save(path, ImageFormat.Png);
        }

        public static Rectangle NormalizeToDrawing(Rectangle rectangle, int width, int height)
        {
            var size = rectangle.Size;
            var origin = rectangle.Origin;
            var center = rectangle.Center;
            var centerOffset = new Point(width - size.Width / 2, height - size.Height / 2);
            var newCenter = center - origin + centerOffset;
            return new Rectangle(origin, newCenter, size);
        }

        public static RectangleF ConvertRectangleToRectangleF(Rectangle rectangle)
        {
            var location = ConvertPointToPointF(rectangle.Center);
            var size = ConvertSizeToSizeF(rectangle.Size);

            return new RectangleF(location, size);
        }

        public static PointF ConvertPointToPointF(Point point)
            => new PointF((float)point.X, (float)point.Y);

        public static SizeF ConvertSizeToSizeF(Size size)
            => new SizeF((float)size.Width, (float)size.Height);
    }
}
