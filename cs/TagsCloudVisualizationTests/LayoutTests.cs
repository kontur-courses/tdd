namespace TagsCloudVisualizationTests;

public static class LayoutTests
{
    [SetUp]
    public static void SetUp()
    {
        _layout = new Layout(new Point(0, 0));
    }
    
    #region Center

    private static IEnumerable<TestCaseData> CenterInSource()
    {
        yield return new TestCaseData((Layout l) => {}, new Point(0, 0))
            .SetName("Layout_HasCenterInInitCoords_WhenEmpty");
        
        yield return new TestCaseData(
                (Layout layout) => 
                {
                    layout.PutRectangle(new Rectangle());
                },
                new Point(0, 0))
            .SetName("Layout_HasCenterInInitCoords_WhenOneRectangleGiven");
        
        yield return new TestCaseData(
                (Layout layout) => 
                {
                    layout.PutRectangle(new Rectangle(0, 0, 1, 1));
                    layout.PutRectangle(new Rectangle(2, 2, 1, 1));
                    layout.PutRectangle(new Rectangle(4, 4, 1, 1));
                },
                new Point(0, 0))
            .SetName("Layout_HasCenterInInitCoords_WhenMultipleRectanglesGiven");
    }

    #endregion
    
    private static Layout _layout;
    
    [TestCaseSource(nameof(CenterInSource))]
    public static void Layout_HasCenterInInitCoords(Action<Layout> action, Point expected)
    {
        action(_layout);
        
        _layout.Center
            .Should()
            .Be(expected);
    }

    [Test]
    public static void Layout_GetNextCoord_GivesLayerByLayerCoords()
    {
        var coords = _layout.GetNextCoord().Take(10);

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
    public static void Layout_GetNextCoord_MustBeEfficient()
    {
        _layout.GetNextCoord()
            .Take(30_000)
            .ToArray();
    }

    [TestCase(
        new []{0, 0, 1, 1}, 
        new [] {2, 2, 1, 1}, 
        true,
        TestName = "Layout_CanPutRectangle_ReturnsTrueOnPuttingRectangleToEmptyPlace")]
    [TestCase(
        new []{0, 0, 1, 1}, 
        new [] {0, 0, 1, 1}, 
        false,
        TestName = "Layout_CanPutRectangle_ReturnsFalseOnPuttingRectangleToOccupiedPlace")]
    public static void Layout_CanPutRectangle_Returns(int[] r1, int[] r2, bool expected)
    {
        _layout.PutRectangle(new Rectangle(r1[0], r1[1], r1[2], r1[3]));

        _layout.CanPutRectangle(new Rectangle(r2[0], r2[1], r2[2], r2[3]))
            .Should()
            .Be(expected);
    }

    [Test, Timeout(20)]
    public static void Layout_PutRectangle_MustBeEfficient()
    {
        var count = 10_000;

        for (var i = 0; i < count; i++)
            _layout.PutRectangle(new Rectangle(0, 0, 1, 1));
    }
}