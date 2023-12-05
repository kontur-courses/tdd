using System.Drawing;

namespace TagsCloudVisualization;

public class RectangleDrawer : IRectangleDrawer
{
    private readonly Pen pen;
    private readonly int scale;
    
    public RectangleDrawer(Pen pen, int scale)
    {
        if (scale <= 0)
            throw new ArgumentException("Scale must be a positive number.");
        this.pen = pen ?? throw new ArgumentException("Pen must not be null.");
        this.scale = scale;
    }
    
    public Bitmap DrawRectangles(List<Rectangle> rectangles, Rectangle borders)
    {
        if (rectangles is null)
            throw new ArgumentException("The list of rectangles cannot be null");
        
        var bitmap = new Bitmap(borders.Width * scale, borders.Height * scale);
        var graphics = Graphics.FromImage(bitmap);
        
        var rectanglesWithShift = rectangles
            .Select(r =>
                new Rectangle(
                    (r.X - borders.X) * scale,
                    (r.Y - borders.Y) * scale, 
                    r.Width * scale,
                    r.Height * scale));
        
        foreach (var rectangle in rectanglesWithShift)
            graphics.DrawRectangle(pen, rectangle);

        return bitmap;   
    }
}