using System.Drawing;
using TagCloud;

void DrawTagCloud(string filename, IEnumerable<Size> sizes)
{
    var layouter = new CircularCloudLayouter(new Point(400, 300));
    foreach (var size in sizes)
        layouter.PutNextRectangle(size);

    using var bitmap = new Bitmap(800, 600);
    using var graphics = Graphics.FromImage(bitmap);
    var pen = new Pen(Color.Black, 1);

    graphics.Clear(Color.White);
    foreach (var rectangle in layouter.Rectangles)
        graphics.DrawRectangle(pen, rectangle);

    bitmap.Save($"{filename}.jpg");
}

var squareSizes = Enumerable.Range(1, 200)
    .Select(n => new Size(30, 30))
    .ToArray();
  
DrawTagCloud("squares", squareSizes);

var rnd = new Random();
var randomSizes = Enumerable.Range(1, 200)
    .Select(n => new Size(rnd.Next(15, 31), rnd.Next(15, 31)))
    .ToArray();

DrawTagCloud("random", randomSizes);
DrawTagCloud("sorted-random", randomSizes.OrderByDescending(s => s.Width * s.Height));