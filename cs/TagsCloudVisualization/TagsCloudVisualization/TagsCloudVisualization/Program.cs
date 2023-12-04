using System.Drawing;
namespace TagsCloudVisualization;

class Program
{
    public static void Main(string[] args)
    {
        var sizes = Utils.GenerateSizes(500, 20, 100, 20, 100);
        var layouter = new CircularCloudLayouter(new Point(1000, 1000));
        layouter.PutRectangles(sizes);
        var rectanglesLayout = layouter.GetLayout();
        var bitmap = CloudLayoutDrawer.Draw(rectanglesLayout.ToArray(), 2000, 2000);
        bitmap.Save(@$"{Environment.CurrentDirectory}..\..\..\..\Images\cloud2.jpg");
    }
}