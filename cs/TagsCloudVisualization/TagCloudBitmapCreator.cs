using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TagsCloudVisualization
{
    public static class TagCloudBitmapCreator
    {
        public static Bitmap CreateBitmap(string[] cloudStrings, Size bitmapSize, Color bitmapBackgroundColor,
                                          CircularCloudLayouter cloudLayouter, ITagFactory tagFactory)
        {
            var brushByColor = new Dictionary<Color, Brush>();

            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            graphics.FillRectangle(new SolidBrush(bitmapBackgroundColor), new Rectangle(Point.Empty, bitmapSize));

            foreach (var tag in tagFactory.GetTags(cloudStrings, graphics, cloudLayouter))
                graphics.DrawString(tag.Text, tag.Style.Font, GetBrush(tag.Style.TextColor),
                                    tag.TagBox, TagStyle.TextFormat);

            return bitmap;

            Brush GetBrush(Color color)
            {
                if (brushByColor.TryGetValue(color, out var brush))
                    return brush;
                return brushByColor[color] = new SolidBrush(color);
            }
        }
    }
}