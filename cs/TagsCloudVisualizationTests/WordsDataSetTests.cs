using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class WordsDataSetTests
{
    [Test]
    public void FreqDict_CorrectWordCount()
    {
        const string testString = "One, Two, Three, Two, Three, Three";

        var expected = new Dictionary<string, int>
        {
            { "Three", 3 },
            { "Two", 2 },
            { "One", 1 }
        };

        var actual = new WordsDataSet(testString).CreateFrequencyDict();
        
        actual.Should().Equal(expected);
    }
}