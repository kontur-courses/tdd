using SixLabors.ImageSharp;
using TagsCloudVisualization;

// Layout work demonstration.

var random = new Random();

// Determine screen center:
var center = new PointF((float)1920 / 2, (float)1080 / 2);

// Create random set of sizes:
var sizes = Enumerable
    .Range(0, 75)
    .Select(rect => new SizeF(random.Next(50, 100), random.Next(50, 150)))
    .ToArray();

// Try sorting for better distribution
Array.Sort(sizes, new SizeFComparer(false));

// Create Archimedes spiral as layoutFunction:
var layoutFunction = new Spiral(0.1f, (float)Math.PI / 180);

// Create layout using layout function and screen center:
var layout = new Layout(layoutFunction, center);

// Put rectangles in layout:
foreach (var size in sizes) layout.PutNextRectangle(size);

// Save image of created layout:
layout.SaveVisualization(
    new Size(1920, 1080),
    Color.Yellow,
    1.2f,
    Color.Red,
    "best_layout");