using System.Drawing;

namespace TagsCloudVisualization;

public static class BitmapGenerator
{
    public static Bitmap GenerateWindowsBitmap(ICloudLayouter cloudLayouter, Size bitmapSize, in Size[] rectSizes)
    {
        var rects = rectSizes.Select(cloudLayouter.PutNextRectangle);
        return GenerateWindowsBitmap(rects.ToList(), bitmapSize);
    }

    public static Bitmap GenerateWindowsBitmap(List<Rectangle> rects, Size bitmapSize)
    {
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        var xOffset = bitmap.Width / 2 - rects.First().X;
        var yOffset = bitmap.Height / 2 - rects.First().Y;

        foreach (var rect in rects)
        {
            var offsetPos = new Point(rect.X + xOffset, rect.Y + yOffset);
            graphics.DrawRectangle(Pens.MediumPurple, new Rectangle(offsetPos, rect.Size));
        }
        return bitmap;
    }
}