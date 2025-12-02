using System.Drawing;
using System.Runtime.Versioning;

namespace TagsCloudVisualization;

public class Visualizer
{
    [SupportedOSPlatform("Windows")]
    public static void GenerateImageFromCircularCloudLayouter(int imageSize, string filePath, CircularCloudLayouter circularCloudLayouter)
    {
        using var bitmap = PaintImage(imageSize, circularCloudLayouter);
        
        bitmap.Save(filePath);
    }
    
    [SupportedOSPlatform("Windows")]
    public static void GenerateImageCloudLayouter(int imageSize, string fileName, double step, double parameterA)
    {
        var spiral = new ArchimedeanSpiral(new Point(imageSize / 2, imageSize / 2), step, parameterA);
        var circularCloudLayouter = GenerateCloudLayouterWithRectangles(imageSize, spiral);
        using var bitmap = PaintImage(imageSize, circularCloudLayouter);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var imagesDirectory = Path.Combine(desktopPath, "Images");
        
        Directory.CreateDirectory(imagesDirectory);

        var filePath = Path.Combine(imagesDirectory, fileName);
        bitmap.Save(filePath);
    }

    [SupportedOSPlatform("Windows")]
    private static Bitmap PaintImage(int imageSize, CircularCloudLayouter circularCloudLayouter)
    {
        var bitmap = new Bitmap(imageSize, imageSize);
        using var graphics = Graphics.FromImage(bitmap);
        
        graphics.Clear(Color.White);
        
        using var penForPointCenter = new Pen(Color.Blue, 2);
        var radius = 1;
        
        graphics.DrawEllipse(penForPointCenter, circularCloudLayouter.Center.X - radius, circularCloudLayouter.Center.Y - radius, radius * 2, radius * 2);
    
        using var pen = new Pen(Color.Black, 1);
        
        foreach (var rectangle in circularCloudLayouter.Rectangles)
        {
            graphics.DrawRectangle(pen, rectangle);
        }

        return bitmap;
    }

    private static CircularCloudLayouter GenerateCloudLayouterWithRectangles(int imageSize, ISpiral spiral)
    {
        var circularCloudLayouter = new CircularCloudLayouter(new Point(imageSize /2, imageSize / 2));
        var rectangleSize = imageSize / 10;
        var random = new Random();
        
        circularCloudLayouter.SetNewSpiral(spiral);

        for (var i = 0; i < 100; i++)
        {
            var width = random.Next(2, rectangleSize);
            var height = random.Next(2, width);
            circularCloudLayouter.PutNextRectangle(new Size(width, height));
        }
        return circularCloudLayouter;
    }
}