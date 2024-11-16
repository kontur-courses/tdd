using System.Drawing;

namespace TagsCloudVisualization.Visualizers;

public class CartesianVisualizer(Size bitmapSize) : IVisualizer
{
    private const int MinColorComponentValue = 0;
    private const int MaxColorComponentValue = 255;
    private readonly Random random = new();
    private readonly Point centerOffset = new(bitmapSize.Width / 2, bitmapSize.Height / 2);
    
    public Bitmap CreateBitmap(IEnumerable<Rectangle> rectangles)
    {
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        foreach (var rectangle in rectangles)
        {
            rectangle.Offset(centerOffset);
            graphics.DrawRectangle(GetRandomPen(), rectangle);
        }
        
        return bitmap;
    }

    private Pen GetRandomPen() =>
        new
        (Color.FromArgb(
            GetRandomArgbColorComponent(),
            GetRandomArgbColorComponent(),
            GetRandomArgbColorComponent())
        );

    private int GetRandomArgbColorComponent() => 
        random.Next(MinColorComponentValue, MaxColorComponentValue);
}