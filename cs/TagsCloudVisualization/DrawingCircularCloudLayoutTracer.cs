using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TagsCloudVisualization;

[SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
public class DrawingCircularCloudLayoutTracer : ICircularCloudLayoutTracer, IDisposable
{
    private readonly Bitmap bitmap;
    private readonly Point center;
    private readonly Pen circlePen;
    private readonly Graphics graphics;
    private readonly Rectangle imageRectangle;
    private readonly Pen linePen;
    private readonly Brush numbersColor;
    private readonly Font numbersFont;
    private readonly Pen rectanglePen;
    private readonly List<Rectangle> rectangles = new();
    private readonly Pen shiftingPen;
    private int counter;
    private Point previousRectangleCenter;
    private double radius;


    public DrawingCircularCloudLayoutTracer(
        int width,
        int height,
        Point center,
        Pen linePen,
        Pen circlePen,
        Font numbersFont,
        Brush numbersColor,
        Pen rectanglePen,
        Brush backgroundColor,
        Pen shiftingPen)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new NotImplementedException("implemented only for windows");
        bitmap =
            bitmap = new(width, height);
        graphics = Graphics.FromImage(bitmap);
        imageRectangle = new(0, 0, width, height);
        graphics.FillRectangle(backgroundColor, imageRectangle);
        this.center = center;
        this.linePen = linePen;
        this.circlePen = circlePen;
        this.numbersFont = numbersFont;
        this.numbersColor = numbersColor;
        this.rectanglePen = rectanglePen;
        this.shiftingPen = shiftingPen;
    }

    public DrawingCircularCloudLayoutTracer(int width, int height, Point center) : this(
        width,
        height,
        center,
        new(Color.Blue, 1),
        new(Color.Yellow, 0.5f),
        new(FontFamily.GenericMonospace, 10, FontStyle.Regular, GraphicsUnit.Pixel),
        Brushes.Black,
        new(Color.Red, 1),
        Brushes.White,
        new(Color.Gray, 0.7f))
    {
    }

    public void TraceRectangle(Rectangle rectangle)
    {
        if (imageRectangle.Contains(rectangle))
            rectangles.Add(rectangle);
    }

    public void TraceCirclePoint(Point point)
    {
        if (imageRectangle.Contains(point))
            radius = Math.Max(radius, point.DistanceTo(center));
    }

    public void TraceShifting(Point from, Point to)
    {
        if (imageRectangle.Contains(from) && imageRectangle.Contains(to))
            graphics.DrawLine(shiftingPen, from, to);
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
        foreach (var rectangle in rectangles)
        {
            var rectangleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            if (!previousRectangleCenter.IsEmpty) graphics.DrawLine(linePen, previousRectangleCenter, rectangleCenter);
            previousRectangleCenter = rectangleCenter;

            graphics.DrawString((counter++).ToString(), numbersFont, numbersColor, rectangle);
            graphics.DrawRectangle(rectanglePen, rectangle);
        }

        graphics.Save();
        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Jpeg);
        File.WriteAllBytes(fileName, ms.ToArray());
    }
}