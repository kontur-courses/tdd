using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Draw;
using TagsCloudVisualization.Extension;
using TagsCloudVisualization.RectangleGenerator;
using TagsCloudVisualization.Saver;

var center = new Point(0, 0);
var randomRectangles = RectangleGenerator.GenerateRandomRectangles(1000);
var layouter = new CircularCloudLayouter(center);
layouter.PutRectangles(randomRectangles);
var drawer = new RectangleDraftsman(1500, 1500);
const string filename = "CloudRectangles1000.png";
drawer.CreateImage(layouter.Rectangles);
drawer.SaveImageToFile(filename);