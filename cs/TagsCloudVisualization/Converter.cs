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

        public Bitmap GetBitmapFromRectangles(Rectangle[] rectangles)
        {
            var size = GetSizeBitmapFromRectangles(rectangles);
            var bitmap = new Bitmap(size.Width + 1, size.Height + 1);
            var graphics = Graphics.FromImage(bitmap);
            
            foreach (var r in rectangles)
            {
                graphics.DrawRectangle(pen, r.X - size.X, r.Y - size.Y, r.Width, r.Height);
            }
            return bitmap;
        }

        private Rectangle GetSizeBitmapFromRectangles(Rectangle[] rectangles)
        {
            var minTop = rectangles.Min((Rectangle r) => r.Top);
            var maxBottom = rectangles.Max((Rectangle r) => r.Bottom);
            var minLeft = rectangles.Min((Rectangle r) => r.Left);
            var maxRight = rectangles.Max((Rectangle r) => r.Right);
            return new Rectangle(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }
    }
}
