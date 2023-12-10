namespace TagsCloudVisualizationTests;

public class LayoutTests
{
    [Test]
    public void GetNextCoord_GivesLayerByLayerCoords()
    {
        var coords = Layout.GetNextCoord(new Point(0, 0)).Take(10);

        coords
            .Should()
            .BeEquivalentTo(new[]
            {
                new Point(0, 0),
                new Point(-1, -1),
                new Point(-1, 0),
                new Point(-1, 1),
                new Point(0, -1),
                new Point(0, 1),
                new Point(1, -1),
                new Point(1, 0),
                new Point(1, 1),
                new Point(-2, -2),
            });
    }
    
    [Test, Timeout(50)]
    public void GetNextCoord_MustBeEfficient()
    {
        Layout.GetNextCoord(new Point(0, 0))
            .Take(30_000)
            .ToArray();
    }
}