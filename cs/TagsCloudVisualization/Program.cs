// See https://aka.ms/new-console-template for more information

using System.Drawing;
using TagsCloudVisualization;

var resolution = new Size(1920, 1080);
using var tagCloudVisualizer = new TagCloudVisualizer(
    new CircularCloudLayouter(new Point(960, 540), resolution),
    new ImageGenerator("rectangles", 30, resolution.Width, resolution.Height, 100)
);
tagCloudVisualizer.GenerateTagCloud();