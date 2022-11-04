using System.Drawing;

namespace TagCloudVisualizer.TagCloudImageGenerator;

public class TagCloudImageGenerator
{
    public static Bitmap GenerateImage(Rectangle[] rectangles, Size canvasSize)
    {
        var canvas = new Bitmap(canvasSize.Width, canvasSize.Height);
        var graphics = Graphics.FromImage(canvas);

        var backgrouldBrush = Brushes.White;
        var rectanglePen = Pens.Black;

        graphics.FillRectangle(backgrouldBrush,0, 0, canvas.Width, canvas.Height);
        foreach (var rectangle in rectangles)
        {
            graphics.DrawRectangle(rectanglePen, rectangle);
        }

        return canvas;
    }
}