using System.Drawing;

namespace TagsCloudVisualization;

public static class Program
{
    public static void Main()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0), new CircularCloudBuilder(1, 0.1d));
        for (var i = 1; i <= 100; i++)
            layouter.PutNextRectangle(new Size(20, 10));

        var drawer = new TagsCloudDrawer(layouter, new Pen(Color.Red, 3), 3);

        var bitmap = drawer.DrawTagCloud();
        TagsCloudDrawer.SaveImage(bitmap, Directory.GetCurrentDirectory(), "image.jpeg");
    }
}