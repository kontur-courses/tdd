using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.Extensions;

namespace TagsCloud.Visualization.Drawer
{
    public abstract class Drawer : IDrawer
    {
        public Image Draw([NotNull] Rectangle[] rectangles)
        {
            if (rectangles.Length == 0)
                throw new ArgumentException($"rectangles array cannot be empty");
            
            var (width, height) = GetWidthAndHeight(rectangles);
            var (widthWithOffset, heightWithOffset) = (width + 100, height + 100);
            var center = rectangles.First().GetCenter();
            var bitmap = new Bitmap(widthWithOffset, heightWithOffset);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.TranslateTransform(center.X + widthWithOffset / 2, center.Y + heightWithOffset / 2);

            Transform(graphics, rectangles);

            return bitmap;
        }

        protected abstract void Transform(Graphics graphics, Rectangle[] rectangles);

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