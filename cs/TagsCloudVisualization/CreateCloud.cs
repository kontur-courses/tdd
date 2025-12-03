using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class CreateCloud
{
    private readonly List<Rectangle> rectangles;
    private readonly AppSettings? appSettings;
    private readonly List<WordData> wordDataList;
    
    public CreateCloud(List<Rectangle> rectangles, AppSettings appSettings, List<WordData> wordDataList)
    {
        this.rectangles = rectangles;
        this.appSettings = appSettings;
        this.wordDataList = wordDataList;
    }

    public Bitmap DrawCloud()
    {
        var bitmap = new Bitmap(appSettings.ImageSize.Width, appSettings.ImageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        using var wordBrush = new SolidBrush(appSettings.WordColor);
        using var contourPen = new Pen(appSettings.ContourColor, 1);
    
        graphics.Clear(appSettings.BackgroundColor);

        for (int i = 0; i < rectangles.Count; i++)
        {
            var rect = rectangles[i];
            var data = wordDataList[i];
            graphics.DrawString(data.Word, data.WordFont, wordBrush, rect.Location);
            graphics.DrawRectangle(contourPen, rect);
        }
    
        return bitmap;
    }
    
    public void SaveImage(string path, ImageFormat format)
    {
        using var bitmap = DrawCloud();
        bitmap.Save(path, format);
    }
}