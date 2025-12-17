using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudVisualization;

public class TagCloudVisualizer
{
    private readonly Color backgroundColor;
    private readonly Color rectangleColor;
    private readonly Color centerColor;
    private readonly int padding;
    
    private const int MinImageSize = 400;

    public TagCloudVisualizer(
        Color backgroundColor = default,
        Color rectangleColor = default,
        Color centerColor = default,
        int padding = 30)
    {
        this.backgroundColor = backgroundColor.IsEmpty ? Color.White : backgroundColor;
        this.rectangleColor = rectangleColor.IsEmpty ? Color.Blue : rectangleColor;
        this.centerColor = centerColor.IsEmpty ? Color.Red : centerColor;
        this.padding = padding;
    }

    public void CreateVisualization(
        List<Rectangle> rectangles,
        Point center,
        string filePath)
    {
        ValidateInput(rectangles, filePath);
        
        var imageSize = CalculateImageSize(rectangles, center);
        var offset = CalculateCenteringOffset(imageSize, center);
        
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        DrawLayout(graphics, rectangles, center, offset);
        
        EnsureDirectoryExists(filePath);
        bitmap.Save(filePath, ImageFormat.Png);
    }

    private void ValidateInput(List<Rectangle> rectangles, string filePath)
    {
        if (rectangles == null)
            throw new ArgumentNullException(nameof(rectangles));
        
        if (rectangles.Count == 0)
            throw new ArgumentException("Rectangles list cannot be empty");
        
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty");
    }

    private Size CalculateImageSize(List<Rectangle> rectangles, Point center)
    {
        if (rectangles.Count == 0)
            return new Size(MinImageSize, MinImageSize);
        
        int maxLeft = 0, maxRight = 0, maxTop = 0, maxBottom = 0;
        
        foreach (var rect in rectangles)
        {
            maxLeft = Math.Max(maxLeft, center.X - rect.Left);
            maxRight = Math.Max(maxRight, rect.Right - center.X);
            maxTop = Math.Max(maxTop, center.Y - rect.Top);
            maxBottom = Math.Max(maxBottom, rect.Bottom - center.Y);
        }
        
        var maxWidth = Math.Max(maxLeft, maxRight) * 2 + 2 * padding;
        var maxHeight = Math.Max(maxTop, maxBottom) * 2 + 2 * padding;
        
        var maxSide = Math.Max(maxWidth, maxHeight);
        maxSide = Math.Max(maxSide, MinImageSize);
        
        return new Size(maxSide, maxSide);
    }

    private Point CalculateCenteringOffset(Size imageSize, Point cloudCenter)
    {
        var imageCenter = new Point(imageSize.Width / 2, imageSize.Height / 2);
        
        return new Point(
            imageCenter.X - cloudCenter.X,
            imageCenter.Y - cloudCenter.Y
        );
    }

    private void DrawLayout(Graphics graphics, List<Rectangle> rectangles, Point center, Point offset)
    {
        graphics.Clear(backgroundColor);
        
        DrawRectangles(graphics, rectangles, offset);
        DrawCloudCenter(graphics, center, offset);
        DrawInfo(graphics, rectangles.Count);
    }

    private void DrawRectangles(Graphics graphics, List<Rectangle> rectangles, Point offset, float penWidth = 1.5f)
    {
        using var pen = new Pen(rectangleColor, penWidth);
        using var brush = new SolidBrush(Color.FromArgb(40, rectangleColor));
        
        foreach (var rect in rectangles)
        {
            var drawRect = new Rectangle(
                rect.X + offset.X,
                rect.Y + offset.Y,
                rect.Width,
                rect.Height);
            
            graphics.FillRectangle(brush, drawRect);
            graphics.DrawRectangle(pen, drawRect);
        }
    }

    private void DrawCloudCenter(Graphics graphics, Point center, Point offset)
    {
        var centerPoint = new Point(
            center.X + offset.X,
            center.Y + offset.Y);
        
        using var centerBrush = new SolidBrush(centerColor);
        var markerSize = 8;
        graphics.FillEllipse(centerBrush,
            centerPoint.X - markerSize / 2,
            centerPoint.Y - markerSize / 2,
            markerSize, markerSize);
    }

    private void DrawInfo(Graphics graphics, int rectangleCount)
    {
        using var font = new Font("Arial", 10);
        using var textBrush = new SolidBrush(Color.Black);
        
        var info = $"Rectangles: {rectangleCount}";
        graphics.DrawString(info, font, textBrush, 10, 10);
    }

    private void EnsureDirectoryExists(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }
}