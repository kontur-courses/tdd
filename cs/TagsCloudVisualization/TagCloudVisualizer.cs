namespace TagsCloudVisualization;

public class TagCloudVisualizer(CircularCloudLayouter circularCloudLayouter,
        ImageGenerator imageGenerator, WordsDataSet wordsDataSet)
{
    public void GenerateTagCloud()
    {
        foreach (var kvp in wordsDataSet.CreateFrequencyDict())
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(imageGenerator.GetOuterRectangle(kvp.Key, kvp.Value));
            imageGenerator.DrawWord(kvp.Key, kvp.Value, rectangle);
        }
        imageGenerator.Save();
    }
    
    public void ShowTagCloudLayout()
    {
        foreach (var kvp in wordsDataSet.CreateFrequencyDict())
        {
            circularCloudLayouter.PutNextRectangle(imageGenerator.GetOuterRectangle(kvp.Key, kvp.Value));
        }
        imageGenerator.DrawLayout(circularCloudLayouter.PlacedRectangles);
        imageGenerator.Save();
    }
}