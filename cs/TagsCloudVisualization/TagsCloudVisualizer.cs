using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization;

public class TagsCloudVisualizer : ITagsCloudVisualizer
{
    private bool isDisposed;
    private readonly Bitmap bitmap;

    public TagsCloudVisualizer(Size imageSize)
    {
        bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        isDisposed = false;
    }

    public void AddVisualizationRectangles(IEnumerable<Rectangle> rectangles)
    {
        using var graphics = Graphics.FromImage(bitmap);
        VisualizeRectangles(graphics, rectangles);
        VisualizeCenter(graphics, rectangles.First());
    }

    public void Save(string outputFilePath, ImageFormat imageFormat)
    {
        bitmap.Save(outputFilePath, imageFormat);
    }

    private void VisualizeRectangles(Graphics graphics, IEnumerable<Rectangle> rectangles)
    {
        foreach (var rectangle in rectangles)
        {
            VisualizeRectangle(graphics, rectangle);
        }
    }

    private void VisualizeRectangle(Graphics graphics, Rectangle rectangle)
    {
        graphics.FillRectangle(Brushes.Blue, rectangle);
        graphics.DrawRectangle(Pens.Black, rectangle);
    }

    private void VisualizeCenter(Graphics graphics, Rectangle firstRectangle)
    {
        var center = firstRectangle.Center();
        var centerRectangle = CalculateCentralRectangle(center);
        VisualizeCenterPoint(graphics, centerRectangle);
    }

    private Rectangle CalculateCentralRectangle(Point center)
    {
        var size = new Size(2, 2);
        return new Rectangle(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
    }

    private void VisualizeCenterPoint(Graphics graphics, Rectangle centerRectangle)
    {
        graphics.FillEllipse(Brushes.Red, centerRectangle);
    }

    ~TagsCloudVisualizer()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool fromDisposeMethod)
    {
        if (isDisposed)
        {
            return;
        }

        if (fromDisposeMethod)
        {
            bitmap.Dispose();
        }

        isDisposed = true;
    }
}