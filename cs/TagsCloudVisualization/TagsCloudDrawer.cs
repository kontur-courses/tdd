using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class TagsCloudDrawer
{
    private readonly ICloudLayouter layouter;
    private readonly IRectangleDrawer drawer;

    public TagsCloudDrawer(ICloudLayouter layouter, IRectangleDrawer drawer)
    {
        this.layouter = layouter;
        this.drawer = drawer;
    }

    public Bitmap DrawTagCloud()
    {
        var borders = layouter.GetCloudBorders();

        return drawer.DrawRectangles(layouter.PlacedRectangles, borders);
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