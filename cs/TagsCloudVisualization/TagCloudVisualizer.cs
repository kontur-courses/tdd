using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizer(CircularCloudLayouter circularCloudLayouter,
    ImageGenerator imageGenerator)
{
    private readonly List<(string word, int frequency, Rectangle outline)> wordsFrequenciesOutline = new();

    public void GenerateTagCloud(WordsDataSet wordsDataSet)
    {
        foreach (var kvp in wordsDataSet.CreateFrequencyDict())
        {
            var rectangle =
                circularCloudLayouter.PutNextRectangle(imageGenerator.GetOuterRectangle(kvp.word, kvp.count));
            wordsFrequenciesOutline.Add((kvp.word, kvp.count, rectangle));
        }

        imageGenerator.DrawTagCloud(wordsFrequenciesOutline);
    }
}