using System.Drawing;
using TagsCloudVisualization;

List<int> countOfRectangles = new List<int>() {50, 100, 400};

foreach (var i in countOfRectangles)
{
    CircularCloudLayouter circularCloudLayouter =
        new CircularCloudLayouter(new Point(CloudLayouterConst.CloudCentreX, CloudLayouterConst.CloudCentreY));
    var current = GenerateRectangles(i, circularCloudLayouter);
    var size = circularCloudLayouter.getCloudSize();
    CloudDrawer.DrawRectangles(current, size, $"Rectangles {i}.png");
}


static List<Rectangle> GenerateRectangles(int count, CircularCloudLayouter circularCloudLayouter)
{
    Random rnd = new Random();
    for (int i = 0; i < count; i++)
    {
        int width = rnd.Next(10, 25);
        int height = rnd.Next(10, 25);
        circularCloudLayouter.PutNextRectangle(new Size(width, height));
    }

    return circularCloudLayouter.GetRectangles();
}