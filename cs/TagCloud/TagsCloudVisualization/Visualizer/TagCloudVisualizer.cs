using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizer : IVisualizer
{
    private const int DefaultWidth = 800;
    private const int DefaultHeight = 600;
    private const int Padding = 200;
    
    private readonly IImageSaver _imageSaver;

    public TagCloudVisualizer(IImageSaver imageSaver)
    {
        _imageSaver = imageSaver ?? throw new ArgumentNullException(nameof(imageSaver));
    }

    public Bitmap CreateVisualization(IEnumerable<Rectangle> rectangles, Point center, TagCloudVisualizationConfig config)
    {
        var actualImageSize = config.ImageSize ?? CalculateOptimalImageSize(rectangles);
        var bitmap = new Bitmap(actualImageSize.Width, actualImageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        graphics.Clear(config.BackgroundColor);
        
        using var pen = new Pen(config.RectangleColor, config.PenWidth);
        using var brush = new SolidBrush(Color.FromArgb(config.RectangleFillAlpha, config.RectangleColor));
        
        foreach (var rectangle in rectangles)
        {
            graphics.FillRectangle(brush, rectangle);
            graphics.DrawRectangle(pen, rectangle);
        }
        
        using var centerBrush = new SolidBrush(config.CenterColor);
        var centerRect = new Rectangle(
            center.X - config.CenterPointSize / 2, 
            center.Y - config.CenterPointSize / 2, 
            config.CenterPointSize, 
            config.CenterPointSize);
        graphics.FillEllipse(centerBrush, centerRect);
        
        return bitmap;
    }

    public string SaveVisualization(IEnumerable<Rectangle> rectangles, Point center, TagCloudVisualizationConfig config)
    {
        using var bitmap = CreateVisualization(rectangles, center, config);
        return _imageSaver.SaveBitmap(bitmap, config.OutputFileName);
    }

    private Size CalculateOptimalImageSize(IEnumerable<Rectangle> rectangles)
    {
        var rectList = rectangles.ToList();
        
        if (!rectList.Any())
            return new Size(DefaultWidth, DefaultHeight);

        var minX = rectList.Min(r => r.Left);
        var maxX = rectList.Max(r => r.Right);
        var minY = rectList.Min(r => r.Top);
        var maxY = rectList.Max(r => r.Bottom);

        var width = Math.Max(DefaultWidth, (maxX - minX) + Padding);
        var height = Math.Max(DefaultHeight, (maxY - minY) + Padding);

        return new Size(width, height);
    }
}