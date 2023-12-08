namespace TagsCloudVisualization;

public class TagCloudVisualizer
{
    public void GenerateTagCloud(CircularCloudLayouter circularCloudLayouter, 
        string wordsFileName = "words", string outputName = "rectangles")
    {
        var freqDict = new WordsDataSet().CreateFrequencyDict(wordsFileName);

        using var imageGenerator = new ImageGenerator(outputName);
        foreach (var kvp in freqDict)
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(imageGenerator.GetOutward(kvp.Key, kvp.Value));
            imageGenerator.DrawWord(kvp.Key, kvp.Value, rectangle);
        }
    }
}