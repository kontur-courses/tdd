using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter circularCloudLayouter;
        
    [OneTimeSetUp]
    public void CircularCloudLayouterSetUp()
    {
        var point = new Point(0, 0); 
        
        // TODO fix absolute path
        var dict = WordsDataSet.CreateFrequencyDict(
            "/Users/draginsky/RiderProjects/tdd/cs/TagsCloudVisualizationTests/testNumberWords.txt"
        );
            
        circularCloudLayouter = new CircularCloudLayouter(point, dict);
    }
        
    [Test]
    public void PutNextRectangle_Return_Rectangle()
    {
        circularCloudLayouter.PutNextRectangle().GetType().Should().Be(typeof(Rectangle));
    }
}