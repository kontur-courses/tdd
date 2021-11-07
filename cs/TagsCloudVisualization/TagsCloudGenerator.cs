using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsCloudGenerator
    {
        private readonly TagsCloudDrawer _drawer;
        private readonly CircularCloudLayouter _layouter;
        private readonly int _rectanglesCount;
        private readonly SizeF _cloudScale;
        private readonly Func<Size> _sizeFactory;

        public TagsCloudGenerator(
            int rectanglesCount, 
            SizeF cloudScale,
            CircularCloudLayouter layouter, 
            Func<Size> sizeFactory,
            TagsCloudDrawer drawer)
        {
            if (cloudScale.Width <= 0 || cloudScale.Height <= 0)
                throw new ArgumentException(
                    $"{nameof(cloudScale)} expected positive dimensions, but actually negative");
            if (rectanglesCount <= 0) throw new ArgumentException(
                $"{nameof(rectanglesCount)} should be positive");
            _rectanglesCount = rectanglesCount;
            _cloudScale = cloudScale;
            _layouter = layouter ?? throw new ArgumentNullException(nameof(layouter));
            _sizeFactory = sizeFactory ?? throw new ArgumentNullException(nameof(sizeFactory));
            _drawer = drawer ?? throw new ArgumentNullException(nameof(drawer));
        }

        public void Generate(Bitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));
            _drawer.Draw(bitmap, GenerateRectangles(), _cloudScale);
        }

        private Rectangle[] GenerateRectangles()
        {
            return Enumerable.Range(0, _rectanglesCount)
                .Select(x => _layouter.PutNextRectangle(_sizeFactory()))
                .ToArray();
        }
    }
}