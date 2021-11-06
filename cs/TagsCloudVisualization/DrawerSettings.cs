using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class DrawerSettings
    {
        private readonly TagsCloudDrawer _drawer;
        private readonly Rectangle[] _rectangles;

        public DrawerSettings(Rectangle[] rectangles, TagsCloudDrawer drawer)
        {
            _rectangles = rectangles ?? throw new ArgumentNullException(nameof(rectangles));
            _drawer = drawer ?? throw new ArgumentNullException(nameof(drawer));
        }

        public void Draw(Bitmap bitmap, SizeF cloudScale)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));
            if (cloudScale.Width <= 0 || cloudScale.Height <= 0)
                throw new ArgumentException(
                    $"{nameof(cloudScale)} expected positive dimensions, but actually negative");
            _drawer.Draw(bitmap, _rectangles, cloudScale);
        }
    }
}