using System.Drawing;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public class DrawingExamples
{
    static void Main()
    {
        DrawImage_DecreasingRectangles120();
        DrawImage_MixedRectangles320();
        DrawImage_EqualsRectangles250();
    }

    public static void DrawImage_EqualsRectangles250()
    {
        var tempLayouter = new CircularCloudLayouter(new Point(400, 400));
        for (int i = 0; i < 250; i++)
            tempLayouter.PutNextRectangle(new Size(10, 5));
        DrawingTagsCloud drawingTagsCloud = new DrawingTagsCloud(new Point(tempLayouter.CenterCloud.X * 2, tempLayouter.CenterCloud.Y * 2), tempLayouter.GetRectangles);
        drawingTagsCloud.SaveToFile("EqualsRectangles250.png");
    }

    public static void DrawImage_MixedRectangles320()
    {
        var tempLayouter = new CircularCloudLayouter(new Point(400, 400));
        var rectanglesSizes = new List<Size>
        {
            new Size(10, 5),
            new Size(8, 8),
            new Size(12, 3),
            new Size(6, 10)
        };
        for (int i = 0; i < 80; i++)
        {
            foreach (var size in rectanglesSizes)
                tempLayouter.PutNextRectangle(size);
        }
        DrawingTagsCloud drawingTagsCloud = new DrawingTagsCloud(new Point(tempLayouter.CenterCloud.X * 2, tempLayouter.CenterCloud.Y * 2), tempLayouter.GetRectangles);
        drawingTagsCloud.SaveToFile("MixedRectangles320.png");
    }

    public static void DrawImage_DecreasingRectangles120()
    {
        var tempLayouter = new CircularCloudLayouter(new Point(400, 400));
        tempLayouter.PutNextRectangle(new Size(160, 180));
        for (int i = 0; i < 80; i++)
        {
            tempLayouter.PutNextRectangle(new Size(60, 40));
        }
        for (int i = 0; i < 39; i++)
        {
            tempLayouter.PutNextRectangle(new Size(20, 25));
        }
        DrawingTagsCloud drawingTagsCloud = new DrawingTagsCloud(new Point(tempLayouter.CenterCloud.X * 2, tempLayouter.CenterCloud.Y * 2), tempLayouter.GetRectangles);
        drawingTagsCloud.SaveToFile("DecreasingRectangles120.png");
    }
}