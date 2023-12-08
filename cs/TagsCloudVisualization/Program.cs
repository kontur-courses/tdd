using SixLabors.ImageSharp;
using TagsCloudVisualization;

// Layout work demonstration.

var random = new Random();

// Determine screen size and center:
var screenSize = new Size(1920, 1080);
var center = new PointF((float)screenSize.Width / 2, (float)screenSize.Height / 2);

// Create random set of sizes:
var sizes = Enumerable
    .Range(0, 50)
    .Select(rect => new SizeF(random.Next(25, 150), random.Next(25, 150)))
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
    screenSize,
    Color.White,
    1.2f,
    Color.Blue,
    "best_layout.png");