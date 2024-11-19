using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class TagCloudVisualization
{
    private static readonly Random _random = new();

    public static void SaveTagCloudAsBitmap(TagCloud tagCloud, string file)
    {
        const int rectangleOutline = 1;

        using var bitmap = new Bitmap(
            tagCloud.Width + rectangleOutline,
            tagCloud.Height + rectangleOutline);
        var frameShift = new Size(-tagCloud.LeftBound, -tagCloud.TopBound);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            foreach (var rectangle in tagCloud.Rectangles)
            {
                var rectangleInFrame = MoveRectangleToImageFrame(rectangle, frameShift);
                graphics.DrawRectangle(GetRandomPen(), rectangleInFrame);
            }
        }

        bitmap.Save(file);
    }

    private static Rectangle MoveRectangleToImageFrame(Rectangle rectangle, Size imageCenter) =>
        new(rectangle.Location.MoveTo(imageCenter), rectangle.Size);

    private static Pen GetRandomPen()
    {
        return new Pen(Color.FromArgb(
            _random.Next(0, 255),
            _random.Next(0, 255),
            _random.Next(0, 255)));
    }
}