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

        public Bitmap Draw(Size imageSize, SizeF cloudScale)
        {
            return _drawer.Draw(_rectangles, imageSize, cloudScale);
        }
    }
}