using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization_Tests;

public static class ErrorImageSaver
{
    private const string ResultsFolderPath = "errors";
    private const string FileNameDateTimeFormat = "yy-MM-dd hh-mm-ss";
    private static readonly Brush Brush = new SolidBrush(Color.Black);

    public static string SaveErrorResult(List<Rectangle> rects, string testName)
    {
        var bitmapSize = GetBitmapSize(rects);
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

        DrawRectangles(rects, bitmap);
        return SaveToFile(bitmap, testName);
    }

    private static Size GetBitmapSize(IEnumerable<Rectangle> rects)
    {
        var minLeft = int.MaxValue;
        var minTop = int.MaxValue;
        var maxRight = int.MinValue;
        var maxBottom = int.MinValue;
        foreach (var rectangle in rects)
        {
            if (rectangle.Left < minLeft)
                minLeft = rectangle.Left;
            if (rectangle.Top < minTop)
                minTop = rectangle.Top;
            if (rectangle.Right > maxRight)
                maxRight = rectangle.Right;
            if (rectangle.Bottom > maxBottom)
                maxBottom = rectangle.Bottom;
        }

        return new Size(maxRight - minLeft, maxBottom - minTop);
    }

    private static void DrawRectangles(IEnumerable<Rectangle> rects, Image bitmap)
    {
        var ghx = Graphics.FromImage(bitmap);
        ghx.TranslateTransform(
            bitmap.Width / 2f,
            bitmap.Height / 2f
        );

        foreach (var rect in rects)
            ghx.FillRectangle(Brush, rect);
    }

    private static string SaveToFile(Image bitmap, string testName)
    {
        if (!Directory.Exists(ResultsFolderPath))
            Directory.CreateDirectory(ResultsFolderPath);

        var resultName = DateTime.Now.ToString(FileNameDateTimeFormat) + $" ({testName}).png";
        var resultPath = Path.Combine(ResultsFolderPath, resultName);

        bitmap.Save(resultPath, ImageFormat.Png);
        return Path.GetFullPath(resultPath);
    }
}