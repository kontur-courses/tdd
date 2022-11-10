using System.Drawing;
using TagCloud;

void DrawTagCloud(string filename, IEnumerable<Size> sizes)
{
    var layouter = new CircularCloudLayouter(new Point(400, 300));
    foreach (var size in sizes)
        layouter.PutNextRectangle(size);
    layouter.SaveAsImage(filename, new Size(800, 600));
}

var squareSizes = Enumerable.Range(1, 200)
    .Select(n => new Size(-30, 30));

DrawTagCloud("squares.jpg", squareSizes);

var rnd = new Random();
var randomSizes = Enumerable.Range(1, 200)
    .Select(n => new Size(rnd.Next(15, 31), rnd.Next(15, 31)))
    .ToArray();

DrawTagCloud("random.jpg", randomSizes);
DrawTagCloud("sorted-random.jpg", randomSizes.OrderByDescending(s => s.Width * s.Height));