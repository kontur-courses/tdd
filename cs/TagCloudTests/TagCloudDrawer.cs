using System.Drawing;

namespace TagCloud;

public static class TagCloudDrawer
{
    private const string RelativePathToFailDirectory = @"..\..\..\Fails";
    
    public static void Draw(CircularCloudLayouter layouter)
    {
        var rectangles = layouter.Rectangles;
        if (rectangles.Count() == 0)
            return;

        var minX = rectangles.Min(rect => rect.X);
        var maxX = rectangles.Max(rect => rect.Right);
        var minY = rectangles.Min(rect => rect.Top);
        var maxY = rectangles.Max(rect => rect.Bottom);

        using var bitmap = new Bitmap(maxX - minX + 2, maxY - minY + 2);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.DrawRectangles(new Pen(Color.Red, 1),
            rectangles.Select(rect => rect with { X = -minX + rect.X, Y = -minY + rect.Y })
                .ToArray());

        var pathToFile = @$"{RelativePathToFailDirectory}\{TestContext.CurrentContext.Test.FullName}.jpg";
        var absolutePath = Path.GetFullPath(pathToFile);
        bitmap.Save(pathToFile);
        Console.WriteLine($"Tag cloud visualization saved to file {absolutePath}");
    }
}