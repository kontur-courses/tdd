using System.Drawing;
using TagsCloudVisualization;

var center = new Point(512, 512);
var layouter = new CircularCloudLayouter(center, 1 * Math.PI / 180d, 0.5d);
var rand = new Random();
for (var i = 0; i < 100; i++)
    layouter.PutNextRectangle(new Size(rand.Next(60, 140), rand.Next(20, 80)));

var visualizator = new TagsCloudVisualizator(layouter, new Size(1024, 1024));
visualizator.FillBackground();
visualizator.DrawRectangles();
visualizator.DrawShape();
visualizator.SaveImage("layout.png");