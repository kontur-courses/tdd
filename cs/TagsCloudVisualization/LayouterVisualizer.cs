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
        Bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        Pen = new Pen(Color.White, 3);
    }

    public Bitmap Bitmap { get; }

    public Pen Pen { get; set; }

    public void VisualizeRectangle(Rectangle rectangle)
    {
        using var g = Graphics.FromImage(Bitmap);
        g.DrawRectangle(Pen, rectangle);
    }

    public void VisualizeRectangles(IEnumerable<Rectangle> rectangles)
    {
        foreach (var rect in rectangles)
        {
            VisualizeRectangle(rect);
        }
    }

    public void SaveImage(string file, ImageFormat format)
    {
        if (TrySaveImage(file, format))
        {
            Console.WriteLine($"Tag cloud visualization saved to {file}");
        }
    }

    private bool TrySaveImage(string file, ImageFormat format)
    {
        try
        {
            Bitmap.Save(file, format);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while saving the image: " + ex.Message);
            return false;
        }
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