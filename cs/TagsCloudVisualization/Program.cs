using System.Drawing;
using TagsCloudVisualization;

var firstLayouter = new CircularCloudLayouter(new Point(250,250));
for (int i = 0; i < 50; i++)
    firstLayouter.PutNextRectangle(new Size(20, 5));

var secondLayouter = new CircularCloudLayouter(new Point(100,100));
for (int i = 0; i < 150; i++)
{
    secondLayouter.PutNextRectangle(new Size(20, 5));
}

var thridLayouter = new CircularCloudLayouter(new Point(250, 250));
for (int i = 0; i < 500; i++)
{
    thridLayouter.PutNextRectangle(new Size(10, 5));
}

LayoutSaver.SaveFailedLayoutImageAsJpeg(@"C:\Users\harle\source\repos\tdd\cs\TagsCloudVisualization\Images\LayoutImage1.jpg",new Size(500,500),firstLayouter.Rectangles);
LayoutSaver.SaveFailedLayoutImageAsJpeg(@"C:\Users\harle\source\repos\tdd\cs\TagsCloudVisualization\Images\LayoutImage2.jpg", new Size(500, 500), secondLayouter.Rectangles);
LayoutSaver.SaveFailedLayoutImageAsJpeg(@"C:\Users\harle\source\repos\tdd\cs\TagsCloudVisualization\Images\LayoutImage3.jpg", new Size(500, 500), thridLayouter.Rectangles);

