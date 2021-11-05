using System;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;
#pragma warning disable CA1416

namespace TagsCloudVisualization
{
    public class TagsCloudDrawer
    {
        private readonly int _rectanglesCount;
        private readonly CircularCloudLayouter _layouter;
        private readonly Func<Size> _sizeFactory;
        private readonly IColorGenerator _colorGenerator;

        public TagsCloudDrawer(int rectanglesCount, CircularCloudLayouter layouter, Func<Size> sizeFactory,
            IColorGenerator colorGenerator)
        {
            if (rectanglesCount <= 0)
            {
                throw new ArgumentException($"{nameof(rectanglesCount)} should be positive");
            }

            _rectanglesCount = rectanglesCount;
            _layouter = layouter ?? throw new ArgumentNullException(nameof(layouter));
            _sizeFactory = sizeFactory ?? throw new ArgumentNullException(nameof(sizeFactory));
            _colorGenerator = colorGenerator ?? throw new ArgumentNullException(nameof(colorGenerator));
        }

        public Bitmap Draw(Size imageSize, SizeF cloudScale)
        {
            Validate(imageSize, cloudScale);
            var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new SolidBrush(new Color());

            var rectangles = GenerateRectangles(_rectanglesCount);

            var bounds = GetBoundingSize(rectangles);
            var scaleX = (bounds.Width) / (bitmap.Width * cloudScale.Width);
            var scaleY = (bounds.Height) / (bitmap.Height * cloudScale.Height);

            graphics.FillRectangle(Brushes.Gray, 0, 0, bitmap.Width, bitmap.Height);
            graphics.TranslateTransform(bitmap.Width / 2f, bitmap.Height / 2f);
            graphics.ScaleTransform(1 / scaleX, 1 / scaleY);
            
            foreach (var rectangle in rectangles)
            {
                brush.Color = _colorGenerator.Generate();
                graphics.FillRectangle(brush, rectangle);
            }

            return bitmap;
        }

        private Rectangle[] GenerateRectangles(int n)
        {
            return Enumerable.Range(0, n)
                .Select(x => _layouter.PutNextRectangle(_sizeFactory()))
                .ToArray();
        }
        
        private static void Validate(Size imageSize, SizeF cloudScale)
        {
            if (imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                throw new ArgumentException($"{nameof(imageSize)} expected positive dimensions, but actually negative");
            }

            if (cloudScale.Width <= 0 || imageSize.Height <= 0)
            {
                throw new ArgumentException($"{nameof(cloudScale)} expected positive dimensions, but actually negative");
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
