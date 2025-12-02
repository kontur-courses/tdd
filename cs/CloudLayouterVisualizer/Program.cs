using CloudLayouterVisualizer;
using System.Drawing;

const int minWidth = 20;
const int maxWidth = 60;
const int minHeight = 15;
const int maxHeight = 40;

var center = Point.Empty;

var binDir = AppDomain.CurrentDomain.BaseDirectory;
var projectDir = Directory.GetParent(binDir)!.Parent!.Parent!.FullName;
var imagesDir = Path.Combine(projectDir, "Images");
Directory.CreateDirectory(imagesDir);

GenerateCloud(
    fileName: "cloud_50_30x20.png",
    rectanglesCount: 50,
    sizeFactory: _ => new Size(30, 20),
    scale: 10,
    padding: 5);

var random = new Random();
GenerateCloud(
    fileName: "cloud_400_various.png",
    rectanglesCount: 400,
    sizeFactory: _ =>
    {
        var width = random.Next(minWidth, maxWidth);
        var height = random.Next(minHeight, maxHeight);
        return new Size(width, height);
    },
    scale: 3,
    padding: 10);

void GenerateCloud(
    string fileName,
    int rectanglesCount,
    Func<int, Size> sizeFactory,
    double scale,
    int padding)
{
    // Закомментировал из-за задачи 3. До него этот проект зависел от TagsCloudVisualization, сейчас наоборот 
    //var layouter = new CircularCloudLayouter(center);
    //var rectangles = new List<Rectangle>();

    //for (var i = 0; i < rectanglesCount; i++)
    //{
    //    var size = sizeFactory(i);
    //    rectangles.Add(layouter.PutNextRectangle(size));
    //}

    //var filePath = Path.Combine(imagesDir, fileName);
    //var visualizer = new CloudVisualizer(rectangles, filePath, scale, padding);
    //visualizer.Visualize();
}
