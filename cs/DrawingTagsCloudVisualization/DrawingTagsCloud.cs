using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System.Drawing;

namespace DrawingTagsCloudVisualization;

public class DrawingTagsCloud
{
    private readonly System.Drawing.Point centercloud;
    private readonly List<Rectangle> rectangles;

    public DrawingTagsCloud(System.Drawing.Point center, List<Rectangle> rectanglesInput)
    {
        this.centercloud = center;
        this.rectangles = rectanglesInput;
    }

    public void SaveToFile(string filePath)
    {
        using var bitmapContext = new SkiaBitmapExportContext(800, 800, 2.0f);

        var canvas = bitmapContext.Canvas;
        canvas.FontColor = Colors.Black;
        canvas = Draw(canvas);
        using var image = bitmapContext.Image;
        using var stream = File.OpenWrite(filePath);
        image.Save(stream);
    }

    private ICanvas Draw(ICanvas canvas)
    {
        canvas.FillColor = Colors.Blue;

        foreach (var rect in rectangles)
        {
            canvas.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        return canvas;
    }
}
