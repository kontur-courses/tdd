using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

public class CircularCloudLayouter_Should
{
    #region Exceptions

    public static IEnumerable<TestCaseData> ExceptionsSource()
    {
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) =>
                {
                    layouter.PutNextRectangle(new Size(0, 0));
                }, 
                "Width and height of rectangle must be positive", 
                new ArgumentException())
            .SetName("CircularCloudLayouter_ThrowsArgumentException_OnPuttingZeroDimensionsRectangle");
        
        yield return new TestCaseData(
                (CircularCloudLayouter layouter) =>
                {
                    layouter.PutNextRectangle(new Size(-1, -1));
                }, 
                "Width and height of rectangle must be positive", 
                new ArgumentException())
            .SetName("CircularCloudLayouter_ThrowsArgumentException_OnPuttingNegativeDimensionsRectangle");
    }

    #endregion
    
    private static CircularCloudLayouter _layouter;

    [SetUp]
    public static void SetUp()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
    }
    
    [TestCaseSource(nameof(ExceptionsSource))]
    public static void CircularCloudLayouter_Throws<T>(Action<CircularCloudLayouter> action, string message, T exception)
        where T: Exception
    {
        new Action(() => action(_layouter))
            .Should()
            .ThrowExactly<T>()
            .Where(e => e.Message.Equals(message));
    }

    [Test]
    public static void CircularCloudLayouter_PutNextRectangle_RectanglesShouldNotIntersect()
    {
        var rectangles = AddRectangles().ToArray();
        
        rectangles
            .Where(r1 => rectangles.Where(r2 => r2 != r1).Any(r1.IntersectsWith))
            .Should()
            .BeEmpty();
    }

    [Test, Timeout(300)]
    public static void CircularCloudLayouter_PutNextRectangle_MustBeEfficient()
    {
        AddRectangles().Count();
    }

    private static IEnumerable<Rectangle> AddRectangles(int count = 100)
    {
        var rnd = new Random(228_666);
        
        for (var i = 0; i < count; i++) 
            yield return _layouter.PutNextRectangle(GetNextRandomSize(rnd));
    }

    private static Size GetNextRandomSize(Random rnd)
    {
        return new Size(rnd.Next(1, 10), rnd.Next(1, 10));
    }
}