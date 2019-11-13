using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudDrawer
    {
        private static readonly Font FakeFont = new Font(FontFamily.GenericSerif, 1);
        private readonly Color backgroundColor;
        private readonly Brush tagBrush;
        private readonly Pen pen;
        private readonly StringFormat stringFormat;

        public CircularCloudDrawer(Color backgroundColor, Brush tagBrush, Brush rectBrush)
        {
            this.backgroundColor = backgroundColor;
            this.tagBrush = tagBrush;
            pen = new Pen(rectBrush);
            stringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
            };
        }

        public void DrawRectangles(IEnumerable<Rectangle> rectangles, string filename)
        {
            DrawCloud(
                rectangles.Select(rect => new TagInfo("", FakeFont, rect)),
                filename);
        }

        public void DrawCloud(IEnumerable<TagInfo> tags, string filename)
        {
            var imageSize = GetSuitableImageSize(tags);
            var center = new Point(imageSize.Width / 2, imageSize.Height / 2);
            var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(backgroundColor);
            foreach (var tag in tags)
            {
                var movedToCenterRect = tag.Rectangle.ShiftLocation(center);
                graphics.DrawString(tag.Value, tag.Font, tagBrush, movedToCenterRect, stringFormat);
                graphics.DrawRectangle(pen, movedToCenterRect);
            }

            bitmap.Save(filename);
        }

        private Size GetSuitableImageSize(IEnumerable<TagInfo> tags)
        {
            var rectanglesSquare = tags
                .Select(tag => tag.Rectangle.Width * tag.Rectangle.Height).Sum();
            var increasedDiameter = (int) (Math.Sqrt(rectanglesSquare / Math.PI) * 1.5) * 2;
            return new Size(increasedDiameter, increasedDiameter);
        }
    }
}