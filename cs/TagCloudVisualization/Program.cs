using System.Drawing;
using TagCloud;

void DrawTagCloud(IEnumerable<Size> sizes, string filename)
{
    var layouter = new CircularCloudLayouter(new Point(0, 0));
    foreach (var size in sizes)
        layouter.PutNextRectangle(size);
    var directory = new DirectoryInfo("./Visualization");
    if (!directory.Exists) directory.Create();
    new TagCloudDrawer().DrawTagCloud(layouter)
        .Save(Path.Join(directory.FullName, filename));
}

var squareSizes = Enumerable.Range(1, 200)
    .Select(n => new Size(30, 30));

DrawTagCloud(squareSizes, "squares.jpg");

var rnd = new Random();
var randomSizes = Enumerable.Range(1, 200)
    .Select(n => new Size(rnd.Next(15, 31), rnd.Next(15, 31)))
    .ToArray();

DrawTagCloud(randomSizes, "random.jpg");
DrawTagCloud(randomSizes.OrderByDescending(s => s.Width * s.Height), "sorted-random.jpg");