using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TagsCloudVisualization;

public class Visualizer
{
    private readonly IEnumerable<RectangleF> rectangles;

    public Visualizer(IEnumerable<System.Drawing.RectangleF> rectangles)
        => this.rectangles = rectangles.Select(rect => new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));

    public Visualizer(IEnumerable<RectangleF> rectangles)
        => this.rectangles = rectangles;

    public void SaveVisualizationAsPng(string fileName)
    {
        var size = GetImageSize();
        using var image = new Image<Rgba32>(size.Width, size.Height);
        image.Mutate(ctx =>
        {
            ctx.Clear(Color.Azure);

            foreach (var rectangle in rectangles)
                ctx.Draw(Color.DarkRed, 1.5f, rectangle);
        });
        image.SaveAsPng(fileName);
    }

    private Size GetImageSize()
    {
        if (!rectangles.Any())
            return new Size(100, 100);
        var imageWidth = rectangles
            .Select(rect => Math.Max(Math.Abs(rect.Right), Math.Abs(rect.Left)))
            .Max();
        var imageHeight = rectangles
            .Select(rect => Math.Max(Math.Abs(rect.Top), Math.Abs(rect.Bottom)))
            .Max();

        return new Size((int)imageWidth * 2, (int)imageHeight * 2);
    }
}