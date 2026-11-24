using System.Drawing;

namespace TagsCloudVisualization;

public class Program
{
    public static void Main(string[] args)
    {
        var random = new Random();
        var layouter = new CircularCloudLayouter(new Point(200, 200), indent: 1);
        for (var i = 0; i < 300; i++)
        {
            layouter.PutNextRectangle(new Size(random.Next(20), random.Next(20)));
        }
        layouter.Save("output.png", 400, 400);
    }
}