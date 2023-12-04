using System.Drawing;
using System.Drawing.Imaging;

namespace tagsCloud;

public class MainProgram
{
    public static void Main(string[] args)
    {
        var layout = new CircularCloudLayouter(new Point(100, 100));
        for (var i = 0; i < 10; i++)
        {
            var rectangle = layout.PutNextRectangle(Utils.GetRandomSize());
        }

        var visualizer = new RectanglesVisualizer(layout.Rectangles);
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        var path = projectDirectory + @"\images\";
        var imageName = "10rect";
        var image = visualizer.DrawTagCloud();
        image.Save($"{path}{imageName}.png", ImageFormat.Png);
    }
}