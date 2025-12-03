using System.Drawing;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

public class VisualizerTestsBase
{
    private int imageWidth ; 
    private int imageHeight;
    private string[] words;
    private string fileName;
    private Font font;
    private Color textColor;
    private Color backgroundColor;

    public void SetVisualizationData(
        int imageWidth,
        int imageHeight,
        string[] words,
        string fileName,
        Font font,
        Color textColor,
        Color backgroundColor)
    {
        this.imageWidth = imageWidth;
        this.imageHeight = imageHeight;
        this.words = words;
        this.fileName = fileName;
        if (font != null) this.font = font;
        if (textColor != null) this.textColor = textColor;
        if (backgroundColor != null) this.backgroundColor = backgroundColor;
    }

    [TearDown]
    public void TearDown()
    {
        var result = TestContext.CurrentContext.Result;
        if (result.Outcome.Status != TestStatus.Failed)
            return;
        var directory = TestContext.CurrentContext.TestDirectory;
        var path = Path.Combine(directory, fileName);
        var visualizer = new Visualizer(new CircularCloudLayouter(new Point(imageWidth / 2, imageHeight / 2)));

        visualizer.DrawCloud(
            words,
            path,
            imageWidth,
            imageHeight,
            textColor,
            backgroundColor,
            font);

        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
    }
}