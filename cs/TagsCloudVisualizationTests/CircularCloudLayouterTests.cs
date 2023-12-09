using System.Reflection;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    [SetUp]
    public void SetUp()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
    }
    
    private CircularCloudLayouter _layouter;
    
    [TestCase(0, 0, TestName = "ZeroDimensions")]
    [TestCase(-1, -1, TestName = "NegativeDimensions")]
    public void PutNextRectangle_ThrowsArgumentExceptionOn(int width, int height)
    {
        new Action(() => _layouter.PutNextRectangle(new Size(width, height)))
            .Should()
            .ThrowExactly<ArgumentException>()
            .Where(e => e.Message.Equals("Width and height of rectangle must be positive"));
    }

    [Test]
    public void PutNextRectangle_RectanglesShouldNotIntersect()
    {
        var rectangles = AddRectangles(_layouter).ToArray();
        
        rectangles
            .Where(r1 => rectangles.Where(r2 => r2 != r1).Any(r1.IntersectsWith))
            .Should()
            .BeEmpty();
    }

    [Test, Timeout(500)]
    public void PutNextRectangle_MustBeEfficient()
    {
        AddRectangles(_layouter).Count();
    }

    private static IEnumerable<Rectangle> AddRectangles(CircularCloudLayouter layouter, int count = 100)
    {
        var rnd = new Random(228_666);
        
        for (var i = 0; i < count; i++) 
            yield return layouter.PutNextRectangle(GetNextRandomSize(rnd));
    }

    private static Size GetNextRandomSize(Random rnd)
    {
        return new Size(rnd.Next(1, 10), rnd.Next(1, 10));
    }

    [TestCase(0, TestName = "WhenEmpty")]
    [TestCase(1, TestName = "WhenOneRectangleGiven")]
    [TestCase(6, TestName = "WhenMultipleRectanglesGiven")]
    public void HasCenterInInitCoords(int rectanglesToAdd)
    {
        for (var i = 0; i < rectanglesToAdd; i++)
            _layouter.PutNextRectangle(new Size(1, 1));

        _layouter.Center
            .Should()
            .Be(new Point(0, 0));
    }
    
    [Test]
    public void CanPutRectangle_ReturnsFalseOnPuttingRectangleToOccupiedPlace()
    {
        var type = _layouter.GetType();
        
        type.GetAndInvokeMethod(
            "PutRectangle", 
            BindingFlags.NonPublic | BindingFlags.Instance,
            _layouter, 
            new Rectangle(0, 0, 1, 1));
        
        type.GetAndInvokeMethod(
                "CanPutRectangle", 
                BindingFlags.NonPublic | BindingFlags.Instance,
                _layouter, 
                new Rectangle(0, 0, 1, 1))
            .Should()
            .Be(false);
    }
    
    [Test]
    public void CanPutRectangle_ReturnsTrueOnPuttingRectangleToEmptyPlace()
    {
        var type = _layouter.GetType();
        
        type.GetAndInvokeMethod(
            "PutRectangle", 
            BindingFlags.NonPublic | BindingFlags.Instance,
            _layouter, 
            new Rectangle(0, 0, 1, 1));
        
        type.GetAndInvokeMethod(
                "CanPutRectangle", 
                BindingFlags.NonPublic | BindingFlags.Instance,
                _layouter, 
                new Rectangle(2, 2, 1, 1))
            .Should()
            .Be(true);
    }
}

public static class TypeExtensions
{
    public static object? GetAndInvokeMethod(this Type type, string methodName, BindingFlags flags, object obj, params object[] parameters)
    {
        return type
            .GetMethod(methodName, flags)!
            .Invoke(obj, parameters);
    }
}