using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

public class CircularCloudLayouter_Should
{
    #region Exceptions

    public static IEnumerable<TestCaseData> NameExceptionSource()
    {
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) =>
                {
                    layouter.SetName(new Size(1, 1));
                    layouter.SetName(new Size(1, 1));
                }, 
                "Name can be set once", 
                new InvalidOperationException())
            .SetName("CircularCloudLayouter_ThrowsInvalidOperationException_OnSettingMultipleNames");
        
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) =>
                {
                    layouter.PutNextRectangle(new Size(1, 1));
                }, 
                "Cannot add tags to unnamed layout", 
                new InvalidOperationException())
            .SetName("CircularCloudLayouter_ThrowsInvalidOperationException_OnAddingRectanglesToUnnamedLayout");
        
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) =>
                {
                    layouter.SetName(new Size(0, 0));
                }, 
                "Width and height of rectangle must be positive", 
                new ArgumentException())
            .SetName("CircularCloudLayouter_ThrowsArgumentException_OnSettingZeroDimensionsRectangle");
        
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) =>
                {
                    layouter.SetName(new Size(-1, -1));
                }, 
                "Width and height of rectangle must be positive", 
                new ArgumentException())
            .SetName("CircularCloudLayouter_ThrowsArgumentException_OnSettingNegativeDimensionsRectangle");
    }

    #endregion

    #region Center

    public static IEnumerable<TestCaseData> CenterInSource()
    {
        yield return new TestCaseData((CircularCloudLayouter r) => {}, new Point(0, 0))
            .SetName("CircularCloudLayouter_HasCenterInInitCoords_WhenEmpty");
        
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) => 
                {
                    layouter.SetName(new Size(40, 80));
                },
                new Point(0, 0))
            .SetName("CircularCloudLayouter_HasCenterInInitCoords_WhenOnlyNameGiven");
        
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) => 
                {
                    layouter.SetName(new Size(41, 81));
                },
                new Point(0, 0))
            .SetName("CircularCloudLayouter_HasCenterInInitCoords_WhenNameAndRectanglesGiven");
    }

    #endregion

    #region Name

    public static IEnumerable<TestCaseData> NameCoordsSource()
    {
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) => 
                {
                    var rect = layouter.SetName(new Size(40, 80));
                    return rect;
                },
                new Rectangle(-20, -40, 40, 80))
            .SetName("CircularCloudLayouter_NameCoords_HaveCenterInItsCenter");
        
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) => 
                {
                    var rect = layouter.SetName(new Size(41, 81));
                    return rect;
                },
                new Rectangle(-21, -41, 41, 81))
            .SetName("CircularCloudLayouter_NameCoords_HaveGreaterHalfBeforeItsCenter");
    }

    #endregion
    
    private static CircularCloudLayouter _layouter;

    [SetUp]
    public static void SetUp()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
    }

    [TestCaseSource(nameof(NameExceptionSource))]
    public static void CircularCloudLayouter_Throws<T>(Action<CircularCloudLayouter> action, string message, T exception)
    where T: Exception
    {
        new Action(() => action(_layouter))
            .Should()
            .ThrowExactly<T>()
            .Where(e => e.Message.Equals(message));
    }
    
    [TestCaseSource(nameof(CenterInSource))]
    public static void CircularCloudLayouter_HasCenterIn(Action<CircularCloudLayouter> action, Point expected)
    {
        action(_layouter);
        
        _layouter.Center
            .Should()
            .Be(expected);
    }

    [TestCaseSource(nameof(NameCoordsSource))]
    public static void CircularCloudLayouter_NameCoords(Func<CircularCloudLayouter, Rectangle> getRectangle, Rectangle expected)
    {
        getRectangle(_layouter)
            .Should()
            .Be(expected);
    }

    [Test]
    public static void CircularCloudLayouter_RectanglesShouldNotIntercept()
    {
        AddRectangles();

        var rectangles = _layouter.GetRectangles;

        rectangles
            .Where(r1 => rectangles.Any(r2 => AreRectanglesIntersecting(r1, r2)))
            .Should()
            .BeEmpty();
    }

    [Test, Timeout(2000)]
    public static void CircularCloudLayouter_MustBeEfficient()
    {
        AddRectangles();
    }

    private static bool AreRectanglesIntersecting(Rectangle r1, Rectangle r2)
    {
        return IsPointInRectangle(new Point(r1.Left, r1.Top), r2)
               || IsPointInRectangle(new Point(r1.Right, r1.Top), r2)
               || IsPointInRectangle(new Point(r1.Right, r1.Bottom), r2)
               || IsPointInRectangle(new Point(r1.Left, r1.Bottom), r2);
    }

    private static bool IsPointInRectangle(Point p, Rectangle r)
    {
        return p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom;
    }

    private static void AddRectangles(int count = 100)
    {
        var rnd = new Random(228_666);

        _layouter.SetName(GetNextRandomSize(rnd));
        
        for (var i = 0; i < count; i++) 
            _layouter.PutNextRectangle(GetNextRandomSize(rnd));
    }

    private static Size GetNextRandomSize(Random rnd)
    {
        return new Size(rnd.Next(1, 20), rnd.Next(1, 20));
    }
}