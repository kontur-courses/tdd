using System.Drawing;
using TagsCloudVisualization;

CircularCloudLayouter circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
var btm = new Bitmap(100, 100);
var g = Graphics.FromImage(btm);
var strings =
    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eu accumsan est. " +
    "Nunc vitae velit id eros rhoncus tincidunt a id ante. Fusce vestibulum dolor eget lorem scelerisque, in lobortis mauris auctor. " +
    "Ut accumsan metus eget justo venenatis vehicula. Nam ut augue et diam ultricies ultricies in at tellus. " +
    "Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. " +
    "Aenean sollicitudin, lacus eleifend dignissim sagittis, massa ex cursus neque, ut gravida massa lorem ac odio. " +
    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vel nulla dolor. Maecenas vitae egestas tellus, sit amet molestie ipsum. " +
    "Fusce interdum, odio a fringilla interdum, nibh felis efficitur quam, tristique maximus urna nisl nec mauris. " +
    "Cras auctor ultrices rhoncus. Morbi vehicula pulvinar justo, quis pulvinar ante finibus id. " +
    "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. " +
    "Etiam urna tellus, gravida id ultricies eu, tincidunt sit amet ex.";
foreach (var word in strings.Split(' '))
{
    var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(word.Length, 2));
    Console.WriteLine(rectangle1.ToString());
    g.DrawRectangle(new Pen(Color.Blue), rectangle1);
}
btm.Save(@"C:\Users\harle\source\repos\tdd\cs\TagsCloudVisualization\myBitmap.bmp");