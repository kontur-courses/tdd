using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizer(CircularCloudLayouter circularCloudLayouter,
        ImageGenerator imageGenerator)
{
    private readonly List<((string, int), Rectangle)> wordsFrequenciesOutline = new();
    
    public TagCloudVisualizer GenerateLayout(WordsDataSet wordsDataSet)
    {
        foreach (var kvp in wordsDataSet.CreateFrequencyDict())
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(imageGenerator.GetOuterRectangle(kvp.Item1, kvp.Item2));
            var item = (kvp.Item1, kvp.Item2);
            wordsFrequenciesOutline.Add((item, rectangle));
        }

        return this;
    }
    
    public void DrawTagCloud()
    {
        imageGenerator.DrawTagCloud(wordsFrequenciesOutline);
    }
    
    public void DrawLayout()
    {
        imageGenerator.DrawLayout(circularCloudLayouter.PlacedRectangles);
    }
}