using System;
using System.Drawing;
using System.IO;
using TagsCloudVisualization;

var center = new Point(500, 500);
var baseDirectory = AppContext.BaseDirectory;
var projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "../../../"));
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
