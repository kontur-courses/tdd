using System;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

#pragma warning disable CA1416

namespace TagsCloudVisualization
{
    public class TagsCloudDrawer
    {
        private static readonly Color BackgroundColor = Color.Gray;
        private readonly IColorGenerator _colorGenerator;

        public TagsCloudDrawer(IColorGenerator colorGenerator)
        {
            _colorGenerator = colorGenerator ?? throw new ArgumentNullException(nameof(colorGenerator));
        }

        public void Draw(Bitmap bitmap, Rectangle[] rectangles, SizeF cloudScale)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));
            if (cloudScale.Width <= 0 || cloudScale.Height <= 0)
                throw new ArgumentException(
                    $"{nameof(cloudScale)} expected positive dimensions, but actually negative");
            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(BackgroundColor);
            graphics.TranslateTransform(bitmap.Width / 2f, bitmap.Height / 2f);

            if (rectangles.Length > 0)
            {
                ScaleClouds(graphics, bitmap.Size, cloudScale, rectangles);
                FillWithRectangles(graphics, rectangles);
            }
        }

        private void ScaleClouds(Graphics graphics, Size imageSize, SizeF cloudScale, Rectangle[] rectangles)
        {
            var bounds = GetBoundingSize(rectangles);
            var scaleX = bounds.Width / (imageSize.Width * cloudScale.Width);
            var scaleY = bounds.Height / (imageSize.Height * cloudScale.Height);
            graphics.ScaleTransform(1 / scaleX, 1 / scaleY);
        }

        private void FillWithRectangles(Graphics graphics, Rectangle[] rectangles)
        {
            var brush = new SolidBrush(new Color());
            foreach (var rectangle in rectangles)
            {
                brush.Color = _colorGenerator.Generate();
                graphics.FillRectangle(brush, rectangle);
            }
        }

        private static Size GetBoundingSize(Rectangle[] rectangles)
        {
            var top = rectangles.Min(x => x.Top);
            var bottom = rectangles.Max(x => x.Bottom);
            var left = rectangles.Min(x => x.Left);
            var right = rectangles.Max(x => x.Right);
            return new Size(right - left, bottom - top);
        }
    }
}
#pragma warning restore CA1416