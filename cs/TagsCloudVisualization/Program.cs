// See https://aka.ms/new-console-template for more information

using System.Drawing;
using TagsCloudVisualization;

using var tagCloudVisualizer = new TagCloudVisualizer(
    new CircularCloudLayouter(new Point(960, 540)),
    new ImageGenerator(FileHandler.GetRelativeFilePath("out/rectangles.jpg"), 30, 1920, 1080),
    new WordsDataSet("words")
);
tagCloudVisualizer.GenerateTagCloud();