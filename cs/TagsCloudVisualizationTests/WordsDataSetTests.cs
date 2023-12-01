using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class WordsDataSetTests
{
    [Test]
    public void CreateFrequencyDict_Works_Fine()
    {
        // TODO fix absolute path
        var actual = WordsDataSet.CreateFrequencyDict("/Users/draginsky/RiderProjects/tdd/cs/TagsCloudVisualizationTests/testNumberWords.txt");
        var expected = new Dictionary<string, int>{
            { "Three", 3 },
            { "Two", 2 },
            { "One", 1 }
        };
        
        actual.Should().Equal(expected);
    }
}