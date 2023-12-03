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
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        bitmap.Save(Path.Combine(dirPath, filename), ImageFormat.Jpeg);
    }
}