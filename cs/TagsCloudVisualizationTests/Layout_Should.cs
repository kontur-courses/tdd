namespace TagsCloudVisualizationTests;

public static class Layout_Should
{
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
    
    #region CanPutRectangle
    
    private static IEnumerable<TestCaseData> CanPutRectangleSource()
    {
        yield return new TestCaseData((Layout layout) => { }, new Rectangle(0, 0, 1, 1), true)
            .SetName("Layout_CanPutRectangle_ReturnsTrueWhenLayoutIsEmpty");
        
        yield return new TestCaseData(
                (Layout layout) =>
                {
                    layout.PutRectangle(new Rectangle(0, 0, 1, 1));
                }, 
                new Rectangle(2, 2, 1, 1), 
                true)
            .SetName("Layout_CanPutRectangle_ReturnsTrueOnPuttingRectangleToEmptyPlace");
        
        yield return new TestCaseData(
                (Layout layout) =>
                {
                    layout.PutRectangle(new Rectangle(0, 0, 1, 1));
                }, 
                new Rectangle(0, 0, 1, 1), 
                false)
            .SetName("Layout_CanPutRectangle_ReturnsFalseOnPuttingRectangleToOccupiedPlace");
    }

    #endregion
    
    private static Layout _layout;
    
    [SetUp]
    public static void SetUp()
    {
        _layout = new Layout(new Point(0, 0));
    }
    
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

    [TestCaseSource(nameof(CanPutRectangleSource))]
    public static void Layout_CanPutRectangle_Returns(Action<Layout> action, Rectangle rectangle, bool expected)
    {
        action(_layout);

        _layout.CanPutRectangle(rectangle)
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