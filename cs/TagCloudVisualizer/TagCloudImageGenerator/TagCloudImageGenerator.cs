using System.Drawing;

namespace TagCloudVisualizer.TagCloudImageGenerator;

public static class TagCloudImageGenerator
{
    public static Bitmap GenerateImage(Rectangle[] rectangles, Size canvasSize)
    {
        var canvas = new Bitmap(canvasSize.Width, canvasSize.Height);

        using var graphics = Graphics.FromImage(canvas);
        using var backgroundBrush = new SolidBrush(Color.White);
        using var rectanglePen = new Pen(Color.Black);
        
        graphics.FillRectangle(backgroundBrush,0, 0, canvas.Width, canvas.Height);
        foreach (var rectangle in rectangles)
        {
            graphics.DrawRectangle(rectanglePen, rectangle);
        }

        return canvas;
    }
}