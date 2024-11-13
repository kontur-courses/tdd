using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class TagsCloudRenderer
{
    private const int FrameSize = 50;
    private readonly IEnumerable<Rectangle> rectangles;
    private readonly Bitmap bitmap;

    private TagsCloudRenderer(IEnumerable<Rectangle> rectangles, Bitmap bitmap)
    {
        this.rectangles = rectangles;
        this.bitmap = bitmap;
    }

    public static TagsCloudRenderer Create(ICloudLayouter cloudLayouter)
    {
        if (cloudLayouter == null || cloudLayouter.GetRectangles().Count == 0)
            throw new ArgumentException("The cloud layout is empty");

        var cloudSize = cloudLayouter.GetCloudSize();
        var bitmap = new Bitmap(cloudSize.Width + FrameSize, cloudSize.Height + FrameSize);
        var rectangles = cloudLayouter.GetRectangles().Select(rec =>
            rec with { X = rec.X + bitmap.Width / 2, Y = rec.Y + bitmap.Height / 2 });
        
        return new TagsCloudRenderer(rectangles, bitmap);
    }
    
    public void Render()
    {
        using var graphics = Graphics.FromImage(bitmap);
        DrawRectangles(graphics);
    }

    private void DrawRectangles(Graphics graphics)
    {
        var rnd = new Random();
        foreach (var rect in rectangles)
        {
            var color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            var brush = new SolidBrush(color);
            graphics.FillRectangle(brush, rect);
        }
    }

    public void SaveToPath(string fileName, string imagesPath)
    {
        if (fileName == null)
            throw new ArgumentException("The file name cannot be null.");
        
        if (imagesPath == null)
            throw new ArgumentException("The images path cannot be null.");
        
        if (!Directory.Exists(imagesPath))
            Directory.CreateDirectory(imagesPath);

        var safeFileName = GetSafeFileName(fileName);
        bitmap.Save(Path.Combine(imagesPath, safeFileName));
    }

    private static string GetSafeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Concat(fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
    }
}