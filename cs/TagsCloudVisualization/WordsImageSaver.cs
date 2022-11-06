using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public static class WordsImageSaver
{
    private const string ResultsFolderPath = "results";

    private const int MaxWidth = 10000;
    private const int MaxHeight = 10000;

    private const string FileNameDateTimeFormat = "yy-MM-dd hh-mm-ss";

    public static void SaveResult(WordsDrawingDataHandler dataHandler)
    {
        var bitmapSize = GetBitmapSize(dataHandler);
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

        DrawWords(dataHandler, bitmap);
        SaveToFile(bitmap);
    }

    private static Size GetBitmapSize(WordsDrawingDataHandler dataHandler)
    {
        var minLeft = int.MaxValue;
        var minTop = int.MaxValue;
        var maxRight = int.MinValue;
        var maxBottom = int.MinValue;
        foreach (var rectangle in dataHandler.SavedDrawingDatas.Select(data => data.Rectangle))
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

        return new Size(
            Math.Min(maxRight - minLeft + 50, MaxWidth),
            Math.Min(maxBottom - minTop + 50, MaxHeight)
        );
    }

    private static void DrawWords(WordsDrawingDataHandler dataHandler, Image bitmap)
    {
        var ghx = Graphics.FromImage(bitmap);
        ghx.TranslateTransform(
            bitmap.Width / 2f - dataHandler.Center.X,
            bitmap.Height / 2f - dataHandler.Center.Y
        );

        foreach (var data in dataHandler.SavedDrawingDatas)
            ghx.DrawString(data.Text, data.Font, data.Brush, data.Rectangle, data.Format);
    }

    private static void SaveToFile(Image bitmap)
    {
        if (!Directory.Exists(ResultsFolderPath))
            Directory.CreateDirectory(ResultsFolderPath);

        var resultName = DateTime.Now.ToString(FileNameDateTimeFormat) + ".png";
        var resultPath = Path.Combine(ResultsFolderPath, resultName);

        bitmap.Save(resultPath, ImageFormat.Png);
    }
}