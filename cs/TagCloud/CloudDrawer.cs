using System.Drawing;

namespace TagCloud;

public class CloudDrawer
{
    static void Main(string[] args)
    {
        var layouter = new CircularCloudLayouter(new Point(1920 / 2, 1080 / 2));

        var random = new Random();

        for (var i = 0; i < 50; i++)
        {
            layouter.PutNextRectangle(new Size(50 + random.Next(0, 100), 50 + random.Next(0, 100)));
        }

        var bitmap = new Bitmap(1920, 1080);
        var graphics = Graphics.FromImage(bitmap);
        graphics.DrawRectangles(new Pen(Color.Red, 1),
            layouter.Rectangles.Select(rect => rect).ToArray());
        var path = @$"{Environment.CurrentDirectory}\..\..\..\Sample.jpg";
        var absolutePath = Path.GetFullPath(path);
        bitmap.Save(absolutePath);
    }
}