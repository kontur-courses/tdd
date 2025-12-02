using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TagCloud.Visualizers;

public class TagCloudVisualizer(Color rectangleColor, Color technicalFiguresColor)
{
    public void DrawRectangles(List<Rectangle> rectangles, Size canvasSize, string savePath)
    {
        var image = new Image<Rgba32>(canvasSize.Width, canvasSize.Height);
        var pen = Pens.Dot(rectangleColor, 1);
        
        DrawCenterDot(image, canvasSize);
        DrawLimitingCircle(image, canvasSize);
        
        foreach (var rectangle in rectangles)
            DrawRectangle(image, pen, new Rectangle(rectangle.Location + canvasSize / 2, rectangle.Size));
        
        image.Save(savePath);
    }

    private void DrawCenterDot(Image image, SizeF canvasSize)
    {
        var size = new SizeF(10, 10);
        var center = new PointF(canvasSize.Width / 2, canvasSize.Height / 2);
        var ellipse = new EllipsePolygon(center, size);
        
        image.Mutate(x => x.Fill(technicalFiguresColor, ellipse));
    }

    private void DrawLimitingCircle(Image image, SizeF canvasSize)
    {
        var pen = Pens.Dot(technicalFiguresColor, 1);
        var center = new PointF(canvasSize.Width / 2, canvasSize.Height / 2);
        var radius = canvasSize.Height / 2;
        var ellipse = new EllipsePolygon(center, radius);
        
        image.Mutate(x => x.Draw(pen, ellipse));
    }

    private void DrawRectangle(Image image, Pen pen, Rectangle rectangle)
    {
        var rectanglePoly = new RectangularPolygon(rectangle);
        
        image.Mutate(x => x.Draw(pen, rectanglePoly));
    }
}