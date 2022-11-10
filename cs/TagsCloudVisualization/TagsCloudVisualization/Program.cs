using System.Drawing;
using TagsCloudVisualization;

var drawer = new TagCloudDrawer(300, 300, 1, Color.Black);
drawer.DrawRandomRectangles(200,
    new Size(1, 1),
    new Size(10, 10));
drawer.SaveImage();