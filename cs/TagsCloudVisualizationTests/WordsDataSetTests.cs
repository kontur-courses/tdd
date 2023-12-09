using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class WordsDataSetTests
{
    [Test]
    public void FreqDictIs_Three3Two2One1()
    {
        // Test-data:
        // One, Two, Three, Two, Three, Three
        var expected = new Dictionary<string, int>
        {
            { "Three", 3 },
            { "Two", 2 },
            { "One", 1 }
        };

        var actual = new WordsDataSet("testNumberWords").CreateFrequencyDict();
        
        actual.Should().Equal(expected);
    }
}