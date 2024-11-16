using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization;

var sizes = new List<Size>();

var random = new Random();
for (int i = 0; i < 50; i++)
{
    sizes.Add(new Size(random.Next(10, 50), random.Next(10, 50)));
}

var layout = new CircularCloudLayouterImpl(new Point(1000, 1000))
    .GenerateLayout(sizes.ToArray())
    .ToArray();

#pragma warning disable CA1416

var filename = "./test.png";
new Bitmap(2000, 2000)
    .DrawRectangles(layout, new Pen(Color.Blue))
    .Save(filename, ImageFormat.Png);
Console.WriteLine($"File was saved to: {Path.GetFullPath(filename)}");

#pragma warning restore CA1416