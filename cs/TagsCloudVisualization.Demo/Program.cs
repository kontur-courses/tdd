using System.Drawing;
using TagsCloudVisualization;

var center = new Point(500, 500);

var projectDirectory = FindProjectDirectory(AppContext.BaseDirectory);
var imagesDirectory = Path.Combine(projectDirectory, "Images");

Directory.CreateDirectory(imagesDirectory);

var seeds = new[] { 42, 100, 999 };

for (int i = 0; i < seeds.Length; i++)
{
    var random = new Random(seeds[i]);
    var layouter = new CircularCloudLayouter(center);

    for (int j = 0; j < 100; j++)
    {
        layouter.PutNextRectangle(new Size(random.Next(20, 100), random.Next(20, 100)));
    }

    var fileName = $"cloud_{i + 1}.png";
    var filePath = Path.Combine(imagesDirectory, fileName);

    CloudVisualizer.SaveToFile(layouter.Rectangles, center, filePath);
}

static string FindProjectDirectory(string startDirectory)
{
    var directory = new DirectoryInfo(startDirectory);

    while (directory != null)
    {
        if (directory.GetFiles("*.csproj").Any())
            return directory.FullName;

        directory = directory.Parent;
    }

    throw new DirectoryNotFoundException("Project directory not found");
}