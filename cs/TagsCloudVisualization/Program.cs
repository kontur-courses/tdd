using System.Drawing;

namespace TagsCloudVisualization;

public static class Program
{
    public static void Main()
    {
        var random = new Random();

        var layouter = new CircularCloudLayouter(new CircularCloudBuilder(new Point(500, 500), 1, 0.1d));
        for (var i = 1; i <= 100; i++)
            layouter.PutNextRectangle(new Size(random.Next(201), random.Next(201)));

        var rectangleDrawer = new RectangleDrawer(new Pen(Color.Red, 3), 3);
        var drawer = new TagsCloudDrawer(layouter, rectangleDrawer);
        
        var bitmap = drawer.DrawTagCloud();
        TagsCloudDrawer.SaveImage(bitmap, Directory.GetCurrentDirectory(), "image.jpeg");
    }
}