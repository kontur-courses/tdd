using System.Drawing;

namespace TagsCloudVisualization;

public static class CloudDrawer
{
    public static Bitmap Draw(Cloud cloud, int imageWidth, int imageHeight)
    {
        if (cloud.Rectangles.Count == 0)
            throw new ArgumentException("rectangles are empty");
        if (imageWidth <= 0 || imageHeight <= 0)
            throw new ArgumentException("either width or height of rectangle size is not possitive");

        var bitmap = new Bitmap(imageWidth, imageHeight);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        var pen = new Pen(Color.Black);
        graphics.DrawRectangles(pen, cloud.Rectangles.ToArray());
        return bitmap;
    }
}
