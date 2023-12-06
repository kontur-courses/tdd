using System.Drawing;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization;

class Program
{
    public static void Main(string[] args)
    {
        var sizes = Utills.GenerateSizes(500, 20, 100, 20, 100);
        var layouter = new CircularCloudLayouter(new Point(1000, 1000));
        layouter.PutRectangles(sizes);
        var cloud = layouter.GetCloud();
        var bitmap = CloudDrawer.Draw(cloud, 2000, 2000);
        bitmap.Save(@$"{Environment.CurrentDirectory}..\..\..\..\Images\cloud000.jpg");
    }
}