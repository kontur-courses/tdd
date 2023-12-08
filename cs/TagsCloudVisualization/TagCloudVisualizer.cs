namespace TagsCloudVisualization;

public class TagCloudVisualizer : IDisposable
{
    private CircularCloudLayouter circularCloudLayouter;
    private ImageGenerator imageGenerator;

    public TagCloudVisualizer(CircularCloudLayouter circularCloudLayouter, ImageGenerator imageGenerator)
    {
        this.circularCloudLayouter = circularCloudLayouter;
        this.imageGenerator = imageGenerator;
    }

    public void GenerateTagCloud(string wordsFileName = "words")
    {
        var freqDict = new WordsDataSet().CreateFrequencyDict(wordsFileName);

        foreach (var kvp in freqDict)
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(imageGenerator.GetOutward(kvp.Key, kvp.Value));
            imageGenerator.DrawWord(kvp.Key, kvp.Value, rectangle);
        }
    }

    public void Dispose()
    {
        imageGenerator.Dispose();
    }
}