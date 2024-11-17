using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Renderers;

public class SimpleRenderer : IRenderer
{
    private Graphics graphics;
    private Bitmap bitmap;
    private readonly Pen pen;
    private readonly int widthOffset;
    private readonly int heightOffset;
    
    public SimpleRenderer(Size size, Pen? pen = null)
    {
        bitmap = new Bitmap(size.Width, size.Height);
        graphics = Graphics.FromImage(bitmap);
        widthOffset = bitmap.Width / 2;
        heightOffset = bitmap.Height / 2;
        this.pen = pen ?? new Pen(Color.Black);
    }

    public void AddRectangle(Rectangle rectangle)
    {
        graphics.DrawRectangle(pen, rectangle.X + widthOffset, rectangle.Y + heightOffset, rectangle.Width, rectangle.Height);
    }
    
    public void AddRectangles(Rectangle[] rectangles)
    {
        foreach (var rect in rectangles)
        {
            AddRectangle(rect);
        }
    }

    public void SaveImage(string filename)
    {
        bitmap.Save(filename, ImageFormat.Png);
    }

    public void Clear()
    {
        bitmap = new Bitmap(bitmap.Width, bitmap.Height);
        graphics = Graphics.FromImage(bitmap);
    }
}