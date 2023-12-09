namespace TagsCloudVisualization;

public class TagCloudVisualizer : IDisposable
{
    private readonly CircularCloudLayouter circularCloudLayouter;
    private readonly ImageGenerator imageGenerator;
    private readonly WordsDataSet wordsDataSet;

    public TagCloudVisualizer(CircularCloudLayouter circularCloudLayouter, 
        ImageGenerator imageGenerator, WordsDataSet wordsDataSet)
    {
        this.circularCloudLayouter = circularCloudLayouter;
        this.imageGenerator = imageGenerator;
        this.wordsDataSet = wordsDataSet;
    }

    public void GenerateTagCloud()
    {
        foreach (var kvp in wordsDataSet.CreateFrequencyDict())
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(imageGenerator.GetOuterRectangle(kvp.Key, kvp.Value));
            imageGenerator.DrawWord(kvp.Key, kvp.Value, rectangle);
        }
        imageGenerator.Save();
    }

    public void Dispose()
    {
        imageGenerator.Dispose();
    }
}