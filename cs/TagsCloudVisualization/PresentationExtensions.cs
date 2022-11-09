using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TagsCloudVisualization;

[SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
public class CircularCloudLayouterPresentationProxy : ICircularCloudLayouter, IDisposable
{
    private readonly Bitmap bitmap;
    private readonly Graphics graphics;
    private readonly CircularCloudLayouter layouter;
    private readonly Font numbersFont;
    private readonly Pen linePen;
    private readonly Pen rectanglePen;
    private Point previous = Point.Empty;
    private int counter = 0;
    
    public CircularCloudLayouterPresentationProxy(Point center)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new NotImplementedException("implemented only for windows");
        layouter = new(center);
        bitmap = new(center.X * 2, center.Y * 2);
        graphics = Graphics.FromImage(bitmap);
        graphics.FillRectangle(Brushes.White, new(Point.Empty, new Size(center) * 2));

        numbersFont = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular, GraphicsUnit.Pixel);
        linePen = new(Color.Blue, 1);
        rectanglePen = new(Color.Red, 1);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = layouter.PutNextRectangle(rectangleSize);

        var current = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        if (!previous.IsEmpty) graphics.DrawLine(linePen, previous, current);

        previous = current;

        graphics.DrawString((counter++).ToString(), numbersFont, Brushes.Black, rectangle);
        graphics.DrawRectangle(rectanglePen, rectangle);

        return rectangle;
    }

    public void Dispose()
    {
        numbersFont.Dispose();
        rectanglePen.Dispose();
        linePen.Dispose();
        graphics.Dispose();
        bitmap.Dispose();
        GC.SuppressFinalize(this);
    }

    public void SaveToFile(string fileName)
    {
        graphics.Save();
        bitmap.Save(fileName);
    }
}