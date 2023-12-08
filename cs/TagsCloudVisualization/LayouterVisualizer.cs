using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class LayouterVisualizer: IDisposable
{
    private bool isDisposed;

    public LayouterVisualizer(Size imageSize)
    {
        bitmap = new Bitmap(imageSize.Width, imageSize.Height);
    }

    public void VisualizeRectangle(Rectangle rectangle)
    {
        using var g = Graphics.FromImage(bitmap);
        using var brush = new SolidBrush(Color.White);
        
        var pen = new Pen(brush, 3); ;
        g.DrawRectangle(pen, rectangle);
    }

    public void VisualizeRectangles(IReadOnlyCollection<Rectangle> rectangles)
    {
        foreach (var rect in rectangles)
        {
            VisualizeRectangle(rect);
        }
    }

    public void SaveImage(string file, ImageFormat format)
    {
        bitmap.Save(file, format);
        Console.WriteLine($"Tag cloud visualization saved to {file}");
    }

    ~LayouterVisualizer()
    {
        Dispose(false);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool fromDisposeMethod)
    {
        if (isDisposed) return;
        if (fromDisposeMethod)
        {
            Bitmap.Dispose();
            Pen.Dispose();
        }

        isDisposed = true;
    }
}