using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.CloudFactories
{
    public abstract class TagCloudFactory
    {
        public abstract Size CanvasSize { get; }
        public abstract Color BackgroundColor { get; }

        public Graphics GetGraphics(Bitmap tagCloudBitmap)
        {
            var graphics = Graphics.FromImage(tagCloudBitmap);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            return graphics;
        }

        public abstract Action<Tag> GetTagDrawer(Graphics graphics);

        public abstract IEnumerable<Tag> GetTags(string[] cloudStrings, Graphics graphics,
                                                 ICloudLayouter circularCloudLayouter);

        protected static Brush GetBrush(Color color, Dictionary<Color, Brush> brushByColor)
        {
            if (brushByColor.TryGetValue(color, out var brush))
                return brush;
            return brushByColor[color] = new SolidBrush(color);
        }
    }
}