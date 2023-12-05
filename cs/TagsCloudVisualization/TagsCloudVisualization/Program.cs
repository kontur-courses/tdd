using System.Drawing;
using TagsCloudVisualization;

var rnd = new Random();
var sizes = new Size[500];
for (var i = 0; i < sizes.Length; i++)
{
    sizes[i] = new(10 + rnd.Next(40), 1 + rnd.Next(40));
}
var tagCloudLayouter = new CircularCloudLayouter(new(500, 500));
foreach (var size in sizes)
{
    tagCloudLayouter.PutNextRectangle(size);
}
TagCloudSaver.Save(tagCloudLayouter.TagCloud, "image.png");