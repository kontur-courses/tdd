using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace TagsCloudVisualization
{
    static class Program
    {
        private const string Filename =
            @"C:\Users\Night\Desktop\Languages\C#\Shpora\2.tdd\cs\TagsCloudVisualization\\image.bmp";

        private static readonly Size bitmapSize = new Size(800, 600);
        private static readonly Color bitmapBackgroundColor = Color.FromArgb(0, 34, 43);

        private static void Main() => Visualize();

        private static void Visualize()
        {
            var center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);

            var circularCloudLayouter = new CircularCloudLayouter(center);

            Random rnd = new Random(3);
            var shuffledTagStrings = TagCloudsContent
                                     .WebCloudStrings.Take(1)
                                     .Concat(TagCloudsContent.WebCloudStrings.Skip(1).OrderBy(x => rnd.Next()))
                                     .ToArray();

            var bitmap = TagCloudBitmapCreator.CreateBitmap(shuffledTagStrings, bitmapSize, bitmapBackgroundColor,
                                                            circularCloudLayouter, new TagFactory());
            bitmap.Save(Filename);
        }

        private static void DrawBlocks()
        {
            var center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);

            var circularCloudLayouter = new CircularCloudLayouter(center);
            var random = new Random();
            var rectangles = Enumerable.Range(0, 100)
                                       .Select(i => circularCloudLayouter.PutNextRectangle(
                                                   i > 0
                                                       ? new Size(random.Next(50, 100), random.Next(30, 80))
                                                       : new Size(250, 100)))
                                       .ToArray();

            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            graphics.FillRectangle(new SolidBrush(bitmapBackgroundColor), new Rectangle(Point.Empty, bitmapSize));

            graphics.FillRectangle(Brushes.Black, rectangles[0]);
            graphics.FillRectangles(Brushes.Aquamarine, rectangles.Skip(1).ToArray());

            bitmap.Save(Filename);
        }
    }
}