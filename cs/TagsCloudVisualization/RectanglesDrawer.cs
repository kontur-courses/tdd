using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class RectanglesDrawer
{
    private readonly Rectangle[] rectangles;
    private Size imageSize;
    private Point center;
    
    public RectanglesDrawer(Rectangle[] rectangles, Point center)
    {
        this.center = center;
        this.rectangles = rectangles;
    }
    
    public void GenerateImage(String pathToSave)
    {
        if (string.IsNullOrWhiteSpace(pathToSave))
        {
            throw new ArgumentNullException("pathToSave can not be null or empty" );
        }

        if (rectangles.Length == 0)
        {
            throw new InvalidOperationException("rectangles can not be empty" );
        }
        
        var imageSize = GetSizeForImage();
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(Color.Black, 1);
        using var brush = new SolidBrush(Color.OrangeRed);
        
        var offsetX = imageSize.Width / 2 - center.X;
        var offsetY = imageSize.Height / 2 - center.Y;
        
        for (int i = 0; i < rectangles.Length; i++)
        {
            var offsetRectangle = rectangles[i];
            offsetRectangle.Offset(offsetX, offsetY);
            graphics.FillRectangle(brush, offsetRectangle);
            graphics.DrawRectangle(pen, offsetRectangle);
        }
        bitmap.Save(pathToSave, ImageFormat.Png);
    }
    
    private Size GetSizeForImage()
    {
        var maxHeight = 0;
        var maxWidth = 0;
        foreach (var rectangle in rectangles)
        {
            var maxDistYFromCenter = Math.Max(Math.Abs(center.Y - rectangle.Top), Math.Abs(center.Y - rectangle.Bottom));
            var maxDistXFromCenter = Math.Max(Math.Abs(center.X - rectangle.Left), Math.Abs(center.X - rectangle.Right));
            
            maxHeight = Math.Max(maxHeight, maxDistYFromCenter);
            maxWidth = Math.Max(maxWidth, maxDistXFromCenter);
        }
        return new Size(maxWidth*2, maxHeight*2);
    }
}