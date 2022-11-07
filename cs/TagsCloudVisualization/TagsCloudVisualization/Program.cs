using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using TagsCloudVisualization;

var layouter = new CircularCloudLayouter(new Point(150, 150));
var bitmap = new Bitmap(300, 300);
var g = Graphics.FromImage(bitmap);
var pen = new Pen(Color.Black, 2);
var pen2 = new Pen(Color.Red, 2);
var rand = new Random();
for (int i = 0; i < 100; i++)
{
    g.DrawRectangle(pen,layouter.PutNextRectangle(new Size( 20+ rand.Next(-10, 10),20+ rand.Next(-10, 10))));
}
bitmap.Save("..\\..\\..\\..\\..\\..\\Results\\result2.bmp");
