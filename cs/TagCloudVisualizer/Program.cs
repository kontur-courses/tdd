using System.Drawing;
using System.Drawing.Imaging;
using TagCloudVisualizer.CloudLayouter;
using TagCloudVisualizer.TagCloudImageGenerator;

var rectangleCount = 300;

if (args.Length > 0)
    int.TryParse(args[0], out rectangleCount);

var canvasSize = new Size(500, 500);
var layouter = new CircularCloudLayouter(new Point(canvasSize.Width / 2, canvasSize.Height / 2));
var random = new Random();
var rectangles = new Rectangle[rectangleCount];

for (var i = 0; i < rectangleCount; i++)
{
    rectangles[i] = layouter.PutNextRectangle(new Size(random.Next(10, 30), random.Next(10, 30)));
}

var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
var folderName = "TagCloud";
var now = DateTime.Now;
var filename = $"TagCloud_{now:yyyy-MM-dd-HH-mm-ss}.png";

if (!Directory.Exists(path))
    Directory.CreateDirectory(path);

var fullFilePath = Path.Combine(path, folderName, filename);

var image = TagCloudImageGenerator.GenerateImage(rectangles, canvasSize);
image.Save(fullFilePath, ImageFormat.Png);

Console.WriteLine($"Image saved as {fullFilePath}");