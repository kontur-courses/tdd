using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class TagsCloudDrawer
{
    private readonly ICloudLayouter layouter;

    public TagsCloudDrawer(ICloudLayouter layouter)
    {
        this.layouter = layouter;
    }

    public Bitmap DrawRectangles(Pen pen, int scale)
    {
        if (scale <= 0)
            throw new ArgumentException("Scale must be a positive number.");
        if (pen is null)
            throw new ArgumentException("Pen must not be null.");
        
        var borders = layouter.GetCloudBorders();
        var bitmap = new Bitmap(borders.Width * scale, borders.Height * scale);
        var graphics = Graphics.FromImage(bitmap);
        
        var rectanglesWithShift = layouter.PlacedRectangles
            .Select(r =>
                new Rectangle(
                    (r.X - borders.X) * scale,
                    (r.Y - borders.Y) * scale, 
                    r.Width * scale,
                    r.Height * scale));
        
        foreach (var rectangle in rectanglesWithShift)
            graphics.DrawRectangle(pen, rectangle);

        return bitmap;
    }
    
    public static void SaveImage(Bitmap bitmap, string dirPath, string filename)
    {
        if (string.IsNullOrWhiteSpace(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            throw new ArgumentException("The provided filename is not valid.");

        try
        {
            Directory.CreateDirectory(dirPath);
        }
        catch (Exception)
        {
            throw new ArgumentException("The provided directory path is not valid.");
        }

        bitmap.Save(Path.Combine(dirPath, filename), ImageFormat.Jpeg);
    }
}