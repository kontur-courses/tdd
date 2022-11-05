using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using TagsCloudVisualization;

// Console.WriteLine("Hello, World!");

var layouter = new CircularCloudLayouter(new Point(150, 150));
var bitmap = new Bitmap(300, 300);
var g = Graphics.FromImage(bitmap);
var pen = new Pen(Color.Black, 2);
var pen2 = new Pen(Color.Red, 2);
var rand = new Random();
for (int i = 0; i < 100; i++)
{
    g.DrawRectangle(pen,layouter.PutNextRectangle(new Size(10 + rand.Next(-5, 5), 20+ rand.Next(-10, 10))));
    //g.DrawLine(pen2, layouter.Center, layouter.CenterMass);
    //g.DrawLine(pen2, layouter.CenterMass, Point.Empty);
}
    // g.DrawRectangle(pen, layouter.PutNextRectangle(new Size(10, 5)));
    
bitmap.Save("..\\..\\..\\Results\\result.bmp");
