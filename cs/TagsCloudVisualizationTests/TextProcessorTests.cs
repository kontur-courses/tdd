using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class TextProcessorTests
{
    private const string testFilePath1 = "C:\\Users\\HP\\Desktop\\911\\tdd\\cs\\TagsCloudVisualizationTests\\resurses\\test_words.txt";
    private const string testFilePath2 = "C:\\Users\\HP\\Desktop\\911\\tdd\\cs\\TagsCloudVisualizationTests\\resurses\\test_words2.txt";
    
    [Test]
    public void Process_CalculatesCorrectFontSize_BasedOnFrequency()
    {
        var settings = new AppSettings(
            minFontSize: 10, 
            maxFontSize: 20,
            wordsFilePath: testFilePath1
        );
        var processor = new TextProcessor(settings);
        
        processor.Process();
        var catData = processor.ProcessWords.Single(w => w.Word.Equals("cat"));
        var dogData = processor.ProcessWords.Single(w => w.Word.Equals("dog"));
        var appleData = processor.ProcessWords.Single(w => w.Word.Equals("apple"));
        
        Assert.That(appleData.WordFont.Size, Is.EqualTo(10));
        Assert.That(dogData.WordFont.Size, Is.EqualTo(15));
        Assert.That(catData.WordFont.Size, Is.EqualTo(20));
    }

    [Test]
    public void Process_CalculatesCorrectFontSize_WhenDameDensity()
    {
        var settings = new AppSettings(
        minFontSize: 10, 
        maxFontSize: 20,
        wordsFilePath: testFilePath2
        );
        var processor = new TextProcessor(settings);
        
        processor.Process();
        var catData = processor.ProcessWords.Single(w => w.Word.Equals("cat"));
        var dogData = processor.ProcessWords.Single(w => w.Word.Equals("dog"));
        var appleData = processor.ProcessWords.Single(w => w.Word.Equals("apple"));
        
        Assert.That(appleData.WordFont.Size, Is.EqualTo(15));
        Assert.That(dogData.WordFont.Size, Is.EqualTo(15));
        Assert.That(catData.WordFont.Size, Is.EqualTo(15));
    }
    
}