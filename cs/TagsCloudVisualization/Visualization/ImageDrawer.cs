using System.Drawing;

namespace TagsCloudVisualization.Visualization;

#pragma warning disable CA1416
public class ImageDrawer
{
    public static Bitmap DrawLayout(List<Rectangle> rectangles, int paddingFromBorders)
    {
        var minX = rectangles.Min(rectangle => rectangle.Left);
        var minY = rectangles.Min(rectangle => rectangle.Top);
        var maxX = rectangles.Max(rectangle => rectangle.Right);
        var maxY = rectangles.Max(rectangle => rectangle.Bottom);
        var width = maxX - minX + paddingFromBorders;
        var height = maxY - minY + paddingFromBorders;

        var random = new Random();
        
        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        
        graphics.Clear(Color.White);
        
        foreach (var rectangle in rectangles)
        {
            var shiftedRectangle = rectangle with
            {
                X = rectangle.X - minX + paddingFromBorders, 
                Y = rectangle.Y - minY + paddingFromBorders
            };
            
            var randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            
            var brush = new SolidBrush(randomColor);
            graphics.FillRectangle(brush, shiftedRectangle);
            graphics.DrawRectangle(Pens.Black, shiftedRectangle);
        }

        return bitmap;
    }
}