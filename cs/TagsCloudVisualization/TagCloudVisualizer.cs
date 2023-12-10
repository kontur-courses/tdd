using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudVisualizer(CircularCloudLayouter circularCloudLayouter,
        ImageGenerator imageGenerator)
{
    public void GenerateTagCloud(WordsDataSet wordsDataSet)
    {
        foreach (var kvp in wordsDataSet.CreateFrequencyDict())
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(imageGenerator.GetOuterRectangle(kvp.Key, kvp.Value));
            imageGenerator.DrawWord(kvp.Key, kvp.Value, rectangle);
        }
        imageGenerator.Save();
    }
    
    public void GenerateLayout(List<Size> rectangleSizes)
    {
        foreach (var size in rectangleSizes)
            circularCloudLayouter.PutNextRectangle(size);
        imageGenerator.DrawLayout(circularCloudLayouter.PlacedRectangles);
        imageGenerator.Save();
    }
}