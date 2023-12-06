using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class WordsDataSetTests
{
    [Test]
    public void FreqDictIs_Three3Two2One1()
    {
        var expected = new Dictionary<string, int>
        {
            { "Three", 3 },
            { "Two", 2 },
            { "One", 1 }
        };

        var actual = new WordsDataSet().CreateFrequencyDict(
            "../../../../TagsCloudVisualization/src/testNumberWords.txt"
        );
        
        actual.Should().Equal(expected);
    }
}