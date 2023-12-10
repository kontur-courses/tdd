using System.Drawing;
using TagsCloudVisualization;

using var imageGenerator = new ImageGenerator(
    FileHandler.GetOutputRelativeFilePath("words.jpg"),
    FileHandler.GetSourceRelativeFilePath("JosefinSans-Regular.ttf"),
    30, 1920, 1080);

new TagCloudVisualizer(
    new CircularCloudLayouter(new Point(960, 540)),
    imageGenerator
).GenerateTagCloud(new WordsDataSet(FileHandler.ReadText("words")));