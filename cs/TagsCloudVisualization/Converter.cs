using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Converter
    {
        private Pen pen;

        public Converter()
        {
            pen = new Pen(Color.Red);
        }

        public void GetBitmapFromRectangles(List<Rectangle> rectangles, string fileName)
        {
            var size = GetSizeBitmapFromRectangles(rectangles);
            var bitmap = new Bitmap(size.Width + 1, size.Height + 1);
            var graphics = Graphics.FromImage(bitmap);

            rectangles
                .AsParallel()
                .ForAll(r => graphics.DrawRectangle(pen, r.X - size.X, r.Y - size.Y, r.Width, r.Height));
            bitmap.Save(fileName);
        }

        private Rectangle GetSizeBitmapFromRectangles(List<Rectangle> rectangles)
        {
            var minTop = rectangles.Min((r) => r.Top);
            var maxBottom = rectangles.Max((r) => r.Bottom);
            var minLeft = rectangles.Min((r) => r.Left);
            var maxRight = rectangles.Max((r) => r.Right);
            return new Rectangle(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }
    }
}
