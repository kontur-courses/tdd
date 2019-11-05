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
            if (rectangles.Length == 0)
                return new Rectangle(0, 0, 0, 0);
            var minTop = rectangles[0].Top;
            var maxBottom = rectangles[0].Bottom;
            var minLeft = rectangles[0].Left;
            var maxRight = rectangles[0].Right;

            foreach (var r in rectangles)
            {
                minTop = minTop > r.Top ? r.Top : minTop;
                maxBottom = maxBottom < r.Bottom ? r.Bottom : maxBottom;
                minLeft = minLeft > r.Left ? r.Left : minLeft;
                maxRight = maxRight < r.Right ? r.Right : maxRight;
            }
            return new Rectangle(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }
    }
}
