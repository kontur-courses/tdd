using System.Drawing;
namespace TagsCloudVisualization;
public static class CloudLayoutDrawer
{
    public static Bitmap Draw(Rectangle[] rectanglesLayout, int width, int height)
    {
        if (rectanglesLayout == null || rectanglesLayout.Length == 0)
            throw new ArgumentNullException("rectangles are empty");
        if (width <= 0 || height <= 0)
            throw new ArgumentException("either width or height of rectangle size is not possitive");

        var bitmap = new Bitmap(width, height);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        var pen = new Pen(Color.Black);
        graphics.DrawRectangles(pen, rectanglesLayout);
        return bitmap;
    }
}
