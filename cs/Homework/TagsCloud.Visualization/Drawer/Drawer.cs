using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace TagsCloud.Visualization.Drawer
{
    public class Drawer : IDrawer
    {
        public Image Draw(Rectangle[] rectangles)
        {
            var (width, height) = GetWidthAndHeight(rectangles);
            var (widthWithOffset, heightWithOffset) = (width + 10, height + 10);
            var bitmap = new Bitmap(widthWithOffset, heightWithOffset);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.TranslateTransform(widthWithOffset / 2f, heightWithOffset / 2f);
            Transform(graphics, rectangles);

            return bitmap;
        }

        protected virtual void Transform(Graphics graphics, Rectangle[] rectangles)
        {
            var pen = new Pen(Color.Chocolate);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawRectangles(pen, rectangles);
            graphics.FillRectangles(Brushes.Chartreuse, rectangles);
        }

        private (int width, int height) GetWidthAndHeight(Rectangle[] rectangles)
        {
            var maxRight = rectangles.Max(x => x.Right);
            var minLeft = rectangles.Min(x => x.Left);
            var maxBottom = rectangles.Max(x => x.Bottom);
            var minTop = rectangles.Min(x => x.Top);

            return (Math.Abs(maxRight - minLeft), Math.Abs(maxBottom - minTop));
        }
    }
}