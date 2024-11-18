using System.Drawing;

namespace TagsCloudVisualization.Visualizers;

public class DefaultVisualizer(Size bitmapSize) : IVisualizer
{
    private Point bitmapCenter = new(bitmapSize.Width / 2, bitmapSize.Height / 2);
    
    public Bitmap CreateBitmap(IEnumerable<Rectangle> rectangles)
    {
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
        
        var deltaWidth = bitmapCenter.X - rectangles.First().X;
        var deltaHeight = bitmapCenter.Y - rectangles.First().Y;
        
        using var graphics = Graphics.FromImage(bitmap);
        foreach (var rectangle in rectangles)
        {
            var pen = new Pen(Color.DeepPink);
            graphics.DrawRectangle(pen, 
                CenterRectangle(rectangle, deltaWidth, deltaHeight));
        }

        return bitmap;
    }

    private Rectangle CenterRectangle(Rectangle rectangle, int deltaWidth, int deltaHeight)
    {
        var centeredRectanglePos = new Point(rectangle.Location.X + deltaWidth, 
            rectangle.Location.Y + deltaHeight);
        return new Rectangle(centeredRectanglePos, rectangle.Size);
    }
}