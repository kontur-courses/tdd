using Aspose.Drawing;
using Aspose.Drawing.Imaging;

namespace TagsCloudVisualization;

public class CircularCloudVisualizer
{
    private readonly VisualizerParams _visualizerParams;
    
    public CircularCloudVisualizer(VisualizerParams visualizerParams)
    {
        _visualizerParams = visualizerParams;
    }

    public void Visualize(CircularCloudLayouter layouter)
    {
        Visualize(layouter.GetRectangles);
    }

    public void Visualize(IEnumerable<Rectangle> rectangles)
    {
        var bitmap = GetImage(rectangles);
        SaveImage(bitmap);
    }

    private Image GetImage(IEnumerable<Rectangle> rectangles)
    {
        using var bitmap = new Bitmap(_visualizerParams.Width, _visualizerParams.Height);
        using var graphics = Graphics.FromImage(bitmap);
        using var brush = new SolidBrush(_visualizerParams.BgColor);
        using var pen = new Pen(_visualizerParams.RectangleColor);
        
        graphics.FillRectangle(brush, new Rectangle(0, 0, _visualizerParams.Width, _visualizerParams.Height));
        
        graphics.DrawRectangles(pen, rectangles.ToArray());
                
        return bitmap;
    }

    private void SaveImage(Image bitmap)
    {
        if (!Directory.Exists(_visualizerParams.PathToFile))
            Directory.CreateDirectory(_visualizerParams.PathToFile);
        
        bitmap.Save(_visualizerParams.PathWithFileName, ImageFormat.Png);
    }
}