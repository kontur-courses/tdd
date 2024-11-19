using Newtonsoft.Json.Linq;
using System.Drawing;

namespace TagsCloudVisualization.Task_2;

public class VisualizationCloudLayout
{
    private readonly int width;
    private readonly int height;
    private readonly Color backgroundColor;
    private readonly Color[] rectanglePalette;

    public VisualizationCloudLayout(int width, int height, Color backgroundColor, IEnumerable<Color> rectanglePalette)
    {
        this.width = width;
        this.height = height;
        this.backgroundColor = backgroundColor;
        this.rectanglePalette = rectanglePalette.ToArray();
    }

    public Bitmap CreateImage(CircularCloudLayouter cloudLayouter, int amountRectangles,
        Size minSize, Size maxSize)
    {
        var image = new Bitmap(width, height);
        var rectangles = CloudLayout.GenerateCloudLayout(
            amountRectangles,
            minSize,
            maxSize,
            cloudLayouter);

        DrawCloudLayout(Graphics.FromImage(image), rectangles);

        return image;
    }

    public Bitmap CreateImage(IEnumerable<Rectangle> rectangles)
    {
        var image = new Bitmap(width, height);

        DrawCloudLayout(Graphics.FromImage(image), rectangles);

        return image;
    }

    private void DrawCloudLayout(Graphics graphics, IEnumerable<Rectangle> rectangles)
    {
        var random = new Random();

        graphics.FillRectangle(new SolidBrush(backgroundColor), 0, 0, width, height);

        foreach (var rect in rectangles)
        {
            var color = rectanglePalette[random.Next(rectanglePalette.Length)];

            graphics.FillRectangle(new SolidBrush(color), rect);
            graphics.DrawRectangle(new Pen(Color.Black, 1), rect);
        }
    }
}
