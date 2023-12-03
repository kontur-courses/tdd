using System.Drawing;

namespace TagsCloudVisualization;

public static class Program
{
    public static void Main()
    {
        var random = new Random();

        var layouter = new CircularCloudLayouter(new Point(-500, -500));
        for (var i = 1; i <= 100; i++)
        {
            layouter.PutNextRectangle(new Size(10,10));
        }

        var drawer = new TagsCloudDrawer(layouter);
        
        var bitmap = drawer.DrawRectangles(new Pen(Color.Red, 1), 10);
        TagsCloudDrawer.SaveImage(bitmap, Directory.GetCurrentDirectory(), "image.jpeg");
    }
}