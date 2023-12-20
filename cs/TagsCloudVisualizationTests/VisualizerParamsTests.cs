namespace TagsCloudVisualizationTests;

public class VisualizerParamsTests
{
    [TestCase(1, 0, "Height must be positive number", TestName = "ZeroWidth")]
    [TestCase(-1, 1, "Width must be positive number", TestName = "NegativeHeight")]
    public void ThrowsArgumentExceptionOn(int w, int h, string message)
    {
        new Action(() => new VisualizerParams(width: w, height: h))
            .Should()
            .ThrowExactly<ArgumentException>()
            .Where(e => e.Message.Equals(message, StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public void ThrowsArgumentExceptionOnIncorrectPath()
    {
        new Action(() =>
            {
                var visualizerParams = new VisualizerParams();
                visualizerParams.PathToFile = "\0/dir";
            })
            .Should()
            .ThrowExactly<ArgumentException>()
            .Where(e => e.Message.Equals("Path \0/dir is invalid", StringComparison.OrdinalIgnoreCase));
    }
    
    [Test]
    public void ThrowsArgumentExceptionOnIncorrectFileName()
    {
        new Action(() =>
            {
                var visualizerParams = new VisualizerParams();
                visualizerParams.FileName = "/a.png";
            })
            .Should()
            .ThrowExactly<ArgumentException>()
            .Where(e => e.Message.Equals("Name /a.png is invalid", StringComparison.OrdinalIgnoreCase));
    }
}