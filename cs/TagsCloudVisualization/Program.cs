using System.Drawing;
using TagsCloudVisualization;

var random = new Random();
var Sizes = new List<Size>();

for (int i = 0; i < 50; i++) 
{
    var width = random.Next(10, 100); 
    //var height = random.Next(10, 100);
    Sizes.Add(new Size(width, width));
}

Sizes = Sizes.OrderByDescending(s => s.Width * s.Height).ToList();
var Layouter = new CircularCloudLayouter(new Point(400, 400));
foreach (var size in Sizes)
    Layouter.PutNextRectangle(size);

var drawer = new ImageCreater(Layouter.Center.X * 2, Layouter.Center.Y * 2);
var directory = AppDomain.CurrentDomain.BaseDirectory + "tagCloud3.jpeg";
drawer.Generate(Layouter.Rectangles, directory);
Console.WriteLine($"Saved to {directory}");