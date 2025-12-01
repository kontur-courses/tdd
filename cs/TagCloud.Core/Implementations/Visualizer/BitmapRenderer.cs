using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudLib.Abstractions.Visualizer;
using TagsCloudLib.Models;

namespace TagsCloudLib.Implementations.Visualizer;

public class BitmapRender : IRender
{
    public void Render(TransformResult layout, CanvasSize canvas, TagCloudVisualizationConfig config, string outputPath)
    {
        using var bitmap = new Bitmap(canvas.Width, canvas.Height);
        using var g = Graphics.FromImage(bitmap);

        g.Clear(config.CanvasBackgroundColor);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var penWidth = Math.Max(0.5f, config.ShapeBorderThickness * layout.Scale);

        using var pen = new Pen(config.ShapeBorderColor, penWidth);
        using var brush = new SolidBrush(config.ShapeFillColor);

        foreach (var r in layout.Rects)
        {
            if (config.ShapeUseFill)
            {
                g.FillRectangle(brush, r);
            }

            g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
        }

        bitmap.Save(outputPath, ImageFormat.Png);
    }
}