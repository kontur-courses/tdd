using System.Drawing;
using TagsCloudVisualization;

var center = new Point(512, 512);
var layouter = new CircularCloudLayouter(center);
var rectSize = new Size(100, 40);
var iterations = 100;
var f = () => layouter.PutNextRectangle(rectSize);
var rand = new Random();
for (var i = 0; i < iterations; i++)
{
    f.Invoke();
    rectSize = new Size(rand.Next(60, 140), rand.Next(20, 80));
}

var radius = (int)layouter.Rectangles.Select(x => Math.Sqrt(Math.Pow(x.X - center.X, 2) + Math.Pow(x.Y - center.Y, 2))).Max();


var image = new Bitmap(1024, 1024);
var graphics = Graphics.FromImage(image);

graphics.DrawRectangles(Pens.Aqua, layouter.Rectangles.ToArray());
graphics.DrawEllipse(Pens.Brown, new Rectangle(new Point(center.X - radius, center.Y - radius), new Size(radius * 2, radius * 2)));
graphics.Save();
image.Save("abc.png");