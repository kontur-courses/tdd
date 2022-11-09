using System.Drawing;
using TagsCloudVisualization;

var rnd = new Random();
var cloud = new CircularCloudLayouter(new Point(750, 750));
for (int i = 0; i < 25; i++)
{
    cloud.PutNextRectangle(new Size(rnd.Next(50, 150), rnd.Next(50, 150)));
}
Drawer.CreateImage(1500, 1500, cloud.GetRectangles(), "first layout");