using System.Drawing;
using TagsCloudVisualization;

if (!Directory.Exists("Images"))
    Directory.CreateDirectory("Images");

var random = new Random();
var squareRectLayout = new CircularCloudLayouter(new Point(0, 0), 0.01);

for (int i = 0; i < 100; i++)
{
    var squareWidth = random.Next(5, 100);
    squareRectLayout.PutNextRectangle(new Size(squareWidth, squareWidth));
}

new BitmapTagsCloudVisualization().SaveTagsCloud(squareRectLayout, "Images\\SquareRectLayout.bmp");


var horizontalRectLayout = new CircularCloudLayouter(new Point(0, 0), 0.01);

for (int i = 0; i < 100; i++)
{
    var height = random.Next(5, 25);
    var width = random.Next(50, 100);
    horizontalRectLayout.PutNextRectangle(new Size(width, height));
}

new BitmapTagsCloudVisualization().SaveTagsCloud(horizontalRectLayout, "Images\\HorizontalRectLayout.bmp");

var verticalRectLayout = new CircularCloudLayouter(new Point(0, 0), 0.01);

for (int i = 0; i < 100; i++)
{
    var width = random.Next(5, 25);
    var height = random.Next(50, 100);
    verticalRectLayout.PutNextRectangle(new Size(width, height));
}

new BitmapTagsCloudVisualization().SaveTagsCloud(verticalRectLayout, "Images\\VerticalRectLayout.bmp");


var singleSizeSquareRectLayout = new CircularCloudLayouter(new Point(0, 0), 0.01);
var squareSingleWidth = random.Next(5, 100);

for (int i = 0; i < 100; i++)
{
    singleSizeSquareRectLayout.PutNextRectangle(new Size(squareSingleWidth, squareSingleWidth));
}

new BitmapTagsCloudVisualization().SaveTagsCloud(singleSizeSquareRectLayout, "Images\\SingleSizeSquareRectLayout.bmp");